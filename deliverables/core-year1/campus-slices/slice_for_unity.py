#!/usr/bin/env python3
"""Slice campus art for the Unity showcase scenes.

Sources (both are the same art, used differently):
- references/canteen-campus-base-reference.png: 2x2 grid, no labels. Backgrounds are cropped from here.
- source.png: labelled sheet (4 rows x 2 columns). Props are cropped from here.
  The pre-cropped *.png files in this folder are NOT used: their alpha channel is
  broken (almost no fully opaque pixels, quantized RGB) because they were saved
  with a damaged alpha ramp. Cropping from the sheet keeps full colour depth.

Usage:
    python3 slice_for_unity.py --out ../../../mobile-2d/Assets/Showcase/Art
"""

import argparse
import json
import os
from pathlib import Path

import numpy as np
from PIL import Image
from scipy import ndimage

HERE = Path(__file__).resolve().parent
REFERENCE = HERE.parent / "references" / "canteen-campus-base-reference.png"
SHEET = HERE / "source.png"

# Reference canvas is 1672x941 with an 8 px black gutter (x 832..839, y 466..473).
REFERENCE_PANELS = {
    "bg_canteen": (0, 0, 832, 466),
    "bg_bus_stop": (840, 0, 1672, 466),
    "bg_h6_gate": (0, 474, 832, 941),
    "bg_h6_classroom": (840, 474, 1672, 941),
}

# Prop crops measured on source.png with a white-background connected component pass.
# Rect is (x0, y0, x1, y1) in sheet pixels. Clusters that touch in the sheet stay one
# sprite: they are documented as clusters instead of being cut apart by guesswork.
# `punch_holes` removes enclosed sheet-white areas (the see-through gaps between a
# tabletop frame and its legs). It is not applied to assets whose white is artwork
# (clock face, tray plate, bottles).
PROPS = {
    "prop_canteen_table_set": ((789, 61, 1025, 252), True),   # 1 table + 2 chairs, touching
    "prop_canteen_chair": ((817, 142, 890, 257), True),
    "prop_canteen_condiments": ((1183, 208, 1250, 261), False),
    "prop_canteen_meal_tray": ((1260, 218, 1392, 267), False),
    "prop_canteen_cart": ((1541, 185, 1638, 281), True),
    "prop_classroom_desks_left": ((770, 758, 1017, 933), True),
    "prop_classroom_desks_right": ((1007, 758, 1266, 930), True),
    "prop_classroom_clock": ((1454, 840, 1504, 890), False),
}

HOLE_MIN_AREA = 30    # enclosed sheet-white smaller than this is treated as highlight
HOLE_MIN_CHANNEL = 236
WHITE_MIN = 238      # min-channel above this is treated as sheet background
FRINGE_MIN = 225     # fringe band: partially transparent anti-aliasing against white
FRINGE_SPAN = 45     # alpha ramp width for fringe pixels


def load_rgb_on_white(path):
    img = Image.open(path).convert("RGBA")
    flat = Image.new("RGBA", img.size, (255, 255, 255, 255))
    return Image.alpha_composite(flat, img).convert("RGB")


def cut_background(flat, rect, inset=2):
    x0, y0, x1, y1 = rect
    return flat.crop((x0 + inset, y0 + inset, x1 - inset, y1 - inset))


def cut_prop(flat, rect, punch_holes):
    x0, y0, x1, y1 = rect
    crop = np.array(flat.crop((x0, y0, x1, y1))).astype(np.float32)
    h, w, _ = crop.shape
    minch = crop.min(axis=2)

    # Background = near-white pixels connected to the crop border. Interior white
    # areas (paper, steam, highlights) are kept because they are not connected.
    whitish = minch >= WHITE_MIN
    labels, count = ndimage.label(whitish, np.ones((3, 3)))
    border = set(np.unique(np.concatenate([
        labels[0, :], labels[-1, :], labels[:, 0], labels[:, -1],
    ])))
    border.discard(0)
    bg = np.isin(labels, list(border))

    if punch_holes:
        # See-through gaps between a tabletop frame and its legs are enclosed by the
        # drawing, so the border flood fill cannot reach them. Punch out enclosed
        # near-white areas that are big enough to be a gap instead of a highlight.
        enclosed = whitish & ~bg
        hlabels, hcount = ndimage.label(enclosed, np.ones((3, 3)))
        for hid in range(1, hcount + 1):
            region = hlabels == hid
            if region.sum() < HOLE_MIN_AREA:
                continue
            if minch[region].min() >= HOLE_MIN_CHANNEL:
                bg |= region

    # Fringe: pixels just outside the background that are still lightened by it.
    near_bg = ndimage.binary_dilation(bg, np.ones((3, 3)))
    fringe = near_bg & ~bg & (minch >= FRINGE_MIN)

    alpha = np.where(bg, 0.0, 1.0)
    ramp = np.clip((WHITE_MIN - minch) / float(FRINGE_SPAN), 0.0, 1.0)
    alpha = np.where(fringe, ramp, alpha)

    # Un-premultiply against white so the edge loses its white halo.
    safe = np.maximum(alpha, 1e-3)[..., None]
    rgb = np.clip((crop - (1.0 - safe) * 255.0) / safe, 0.0, 255.0)
    rgb = np.where(alpha[..., None] <= 0.0, 0.0, rgb)

    out = np.dstack([rgb, alpha * 255.0]).round().astype(np.uint8)

    # Drop fragments that the crop cut in half: small blobs touching the crop edge
    # belong to a neighbouring item on the sheet, not to this sprite.
    keep = out[..., 3] > 0
    klabels, kcount = ndimage.label(keep, np.ones((3, 3)))
    if kcount > 1:
        areas = ndimage.sum(keep, klabels, range(1, kcount + 1))
        largest = float(areas.max())
        for kid, area in enumerate(areas, start=1):
            region = klabels == kid
            touch = (region[0, :].any() or region[-1, :].any()
                     or region[:, 0].any() or region[:, -1].any())
            if touch and area < largest * 0.2:
                out[region, 3] = 0

    return trim(out)


def trim(rgba, pad=1):
    alpha = rgba[..., 3]
    ys, xs = np.nonzero(alpha)
    if len(ys) == 0:
        return rgba
    y0, y1 = ys.min(), ys.max() + 1
    x0, x1 = xs.min(), xs.max() + 1
    core = rgba[y0:y1, x0:x1]
    h, w, _ = core.shape
    canvas = np.zeros((h + pad * 2, w + pad * 2, 4), dtype=np.uint8)
    canvas[pad:pad + h, pad:pad + w] = core
    return canvas


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--out", required=True)
    ap.add_argument("--preview", default="")
    args = ap.parse_args()
    out_dir = Path(args.out).resolve()
    out_dir.mkdir(parents=True, exist_ok=True)

    ref = load_rgb_on_white(REFERENCE)
    sheet = load_rgb_on_white(SHEET)
    manifest = {"backgrounds": {}, "props": {}}

    for name, rect in REFERENCE_PANELS.items():
        img = cut_background(ref, rect)
        path = out_dir / f"{name}.png"
        img.save(path, optimize=True)
        manifest["backgrounds"][name] = {
            "file": path.name,
            "source": "deliverables/core-year1/references/canteen-campus-base-reference.png",
            "source_rect": list(rect),
            "size": list(img.size),
            "pivot": "center",
            "ppu": 100,
            "processing": "crop 2 px inside the black gutter; opaque RGB, no alpha work",
        }

    for name, (rect, punch_holes) in PROPS.items():
        rgba = cut_prop(sheet, rect, punch_holes)
        img = Image.fromarray(rgba)
        path = out_dir / f"{name}.png"
        img.save(path, optimize=True)
        manifest["props"][name] = {
            "file": path.name,
            "source": "deliverables/core-year1/campus-slices/source.png",
            "source_rect": list(rect),
            "size": list(img.size),
            "pivot": "bottom-center",
            "ppu": 100,
            "processing": (
                "white sheet background removed by border flood fill "
                f"(min channel >= {WHITE_MIN}); anti-aliased fringe un-premultiplied "
                f"with a {FRINGE_SPAN} level ramp from {FRINGE_MIN}; "
                + ("enclosed sheet-white gaps punched out; " if punch_holes else "")
                + "crop-cut fragments dropped; trimmed to alpha bounds with 1 px padding"
            ),
            "punch_holes": punch_holes,
        }

    if args.preview:
        thumbs = []
        for entry in list(manifest["backgrounds"].values()) + list(manifest["props"].values()):
            thumbs.append((Path(out_dir) / entry["file"], entry))
        cell = 300
        cols = 4
        rows = (len(thumbs) + cols - 1) // cols
        board = Image.new("RGBA", (cols * cell, rows * (cell + 18)), (40, 40, 40, 255))
        from PIL import ImageDraw
        draw = ImageDraw.Draw(board)
        for i, (path, entry) in enumerate(thumbs):
            img = Image.open(path).convert("RGBA")
            img.thumbnail((cell - 8, cell - 8), Image.LANCZOS)
            px = (i % cols) * cell
            py = (i // cols) * (cell + 18)
            board.alpha_composite(img, (px + 4, py + 16))
            draw.text((px + 6, py + 2), f"{path.stem} {entry['size'][0]}x{entry['size'][1]}",
                      fill=(255, 255, 255, 255))
        Path(args.preview).parent.mkdir(parents=True, exist_ok=True)
        board.convert("RGB").save(args.preview)

    (out_dir / "campus_manifest.json").write_text(
        json.dumps(manifest, indent=2, ensure_ascii=False) + "\n", encoding="utf-8")
    print(f"wrote {len(manifest['backgrounds'])} backgrounds, "
          f"{len(manifest['props'])} props to {out_dir}")


if __name__ == "__main__":
    main()
