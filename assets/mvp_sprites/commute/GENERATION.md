# Commute asset generation — 2026-10-25

Two generated batch canvases; each received one whole-sheet refinement. No
individual asset generation. Existing room, furniture, backpack walk sprites,
white overlay texture and UI are reused.

1. Desk: 1774×887, 4 columns × 2 rows, row-major: full hook, empty hook,
   writing 0, writing 1, writing 2, typing 0, typing 1, notebook. The refinement
   registers the character inside cell margins and preserves the hip anchor.
2. Outdoor: 1536×1024, 3 columns × 2 rows. Campus spans the top row; bus spans
   the first two bottom cells; bus-stop sign occupies the last bottom cell.
   The refinement gives the lower props real transparent alpha.

Exact prompts and original/final sheets are retained in `desk/` and `outdoor/`.
`slice.py` reproduces all 11 runtime PNGs in
`mobile-2d/Assets/Sprites/MVP/commute/`. `manifest.json` records exact crop
rectangles, normalization, pivots and anchors. All use PPU 100, Point filtering,
Uncompressed, no mipmaps. The script checks alpha bounds and unique outputs.
Upper/lower desk Sprite assets are native crops of these same PNGs, used to
place hands above the table while the seated body remains behind furniture.

`review/` contains actual Play Mode captures and the 142-check test report.
Writing/typing audio, concrete footsteps and engine audio are procedural clips
from DormSound, with no external audio dependency.
