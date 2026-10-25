# Room 404 visual batch v2

Built-in ImageGen; one 4-column x 2-row batch generated for all eight assets.
`PROMPT.md` is the exact generation prompt. Reference images: the existing bed,
room backdrop and Luon idle sprite. No API/CLI fallback was used.

The delivered source is 1774 x 887 (requested 4096 x 2048 was not returned).
Read cells left-to-right then top-to-bottom: hallway, sitting, enter1, enter2,
enter3, lying A, breathing B, sleeping. White gutters requested at 8%; actual
content is checked by `slice.py` and recorded in `manifest.json`.

A single whole-sheet background-extraction edit produced `alpha-sheet.png`.
Characters and their white T-shirts retain generated alpha. The opaque hallway
is cropped from `source.png`. No per-character image generation was used.

Extraction prompt:
> Background extraction ONLY. Preserve the exact 4-column 2-row sprite sheet layout and all eight cells, canvas size and character pixels. Make pure white exterior background and gutters actually transparent, including the white between limbs. Preserve the characters' white T-shirts opaque. Preserve the entire rectangular hallway panel at row1 column1 opaque. Do NOT redraw or reposition or scale any character or hallway. Keep all frame registration, no added shadows, no labels. This is one combined batch alpha extraction for Unity sprites.

Run `slice.py` with Pillow to reproduce all outputs. This does only batch
slicing, common-scale normalization and hip registration. `manifest.json`
records exact source rectangles, hip anchors, scale, paste positions, resulting
alpha bounds, pivots and PPU. `sliced-review.png` is a QA contact sheet packed
from the extracted images, not another generated batch.

Unity settings: Sprite Single, PPU 100, Point, no mipmaps, Uncompressed.
All seven character frames use a 256 x 256 canvas and center hip pivot.
Hallway is 128 x 230 with bottom-left pivot, matching the original door crop.

Runtime animation is owned by LuonCharacterVisual, not an Animator. Entry plays
enter1 → enter2 → enter3 → lying A at 4 fps; exit reverses it. Idle alternates
registered A/B frames at 0.9 seconds, sleeping uses the closed-eye pose. The
right bed mirrors the same batch. Existing older assets remain available but
are not assigned to the Room404_MVP player's runtime sprite arrays.
