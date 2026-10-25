# Dorm journey batch

Built-in ImageGen, one planned 3-column x 3-row sheet. The continuous top row
spans three cells for a wide balcony corridor; lower cells contain WC Nam,
WC Nu, backpack idle and three walking poses. White gutters/margins requested
at 8%. Style/identity references are existing room_404_backdrop, luon_idle_0
and luon_walk_0. `PROMPT.md` retains the exact first prompt.

One whole-sheet edit made the lower-cell backgrounds transparent and corrected
the walk cycle (passing pose / opposite contact). It did not generate assets
individually. `ALPHA_PROMPT.md` is the exact edit prompt. The original and edited
sheets are `source.png` / `alpha-sheet.png`.

The tool delivered 1254 x 1254, with uneven actual row boundaries. `manifest.json`
records the measured crop rectangles rather than assuming equal thirds. The
corridor is cropped from the opaque original. Doors are sliced from alpha and
normalized to 128 x 250. Four character frames share source scale .62, registered
foot baselines and 180 x 260 canvases. White shirts retain generated opacity.

Run `slice.py` with Pillow to reproduce the seven runtime PNGs. The script checks
visible character bounds and unique output hashes. Runtime folder:
`mobile-2d/Assets/Sprites/MVP/dorm_journey/`. All are Sprite Single, PPU 100,
Point filtering, Uncompressed, no mipmaps. The balcony foreground is a native
Sprite rect on the same corridor texture, not another generated/duplicate PNG.
Existing backpack prop artwork and chair/bed sprites are reused.

Book icon and progress bar are ordinary Unity UI Images/text, not generated
bitmap assets. Keyboard and water foley are original procedural AudioClips from
DormSound.cs (no external recordings or third-party audio dependency).
