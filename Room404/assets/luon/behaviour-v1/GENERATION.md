# Lươn behaviour sheet — batch 1

- Tool: built-in image generation; input style reference: `../luon_idle.png`.
- Prompt: Create a transparent 2×2 sheet of the same pixel-art Lươn viewed from behind, with equal isolated cells: seated idle, two writing poses, seated with blue backpack. Preserve dark hair, white shirt, olive shorts and green sandals; no chair, desk, labels or borders.
- Sheet: `sheet.png`, 1261 × 1247 pixels, 2 columns × 2 rows, no grid lines.
- Crop rectangles, x0/y0/x1/y1: idle (0,0,630,623); study 1 (630,0,1261,623); study 2 (0,623,630,1247); backpack (630,623,1261,1247).
- The `*_cell.png` files are the untrimmed cells. `slice.py` uses the alpha bounds of each character and bottom-centers it in a 400 × 520 transparent canvas. Pivot: bottom center. Pixels per unit: not applicable to this web preview.
- `*.svg` files trace normalized frames as pixel-aligned paths with a 192-color palette and four alpha levels; they are editable vectors but not clean hand-drawn shape layers.
- `backpack_overlay.svg` is clipped from the generated backpack frame for use with the study frames. The inferred clipping is approximate. The frame shows a cross-legged sitting pose; it is an approximation for the chair and may need further adjustment.
- Slicing observed no opaque content at cell edges; gameplay, placement and animation are user checkpoints.
