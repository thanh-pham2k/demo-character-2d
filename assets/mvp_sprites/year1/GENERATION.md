# Year 1 milestone asset batches

Built-in imagegen; two planned batch sheets, no standalone asset calls.

- H6 sheet: 1536×1024, requested 2×2. Generated divider is measured at y=468,
  with x=768. Top: H6 gate and classroom backdrops. Bottom: writing Đạm and
  lecturer with actual alpha transparency. Exact crop bounds in manifest.json.
- Hotpot sheet: 1536×1024, 3×2, 512px cells. Two steam frames, Lươn, Đạm,
  Long and Tộ seated on the floor. One whole-sheet margin refinement retained.

Original sheets, final sheet and exact prompts are in h6/ and hotpot/.
Run slice.py using Python/Pillow to reproduce ten runtime PNGs. Import uses
PPU 100, Point, Uncompressed, no mipmaps. Cropped transparent subjects are
normalized with bottom-center anchors; backdrops use center pivots. Native
H6_DeskForeground.asset reuses a rectangular crop of the classroom texture.
The existing Room404, portraits and player walking/studying frames are reused.

Status: integrated and visually reviewed. Final Play Mode suite passed 160
checks with zero errors. Curated runtime captures and reports are in review/.
