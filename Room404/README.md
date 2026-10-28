# Room 404 preview

From this directory, run `python3 -m http.server 8765` and open <http://localhost:8765/>. The JSON is loaded over HTTP, so opening `index.html` directly may not work.

Object IDs, normalized hit areas, stacking order, labels and action metadata are in `layout.json`. Objects are fixed; actions are labels only. The preview currently displays the original reference as a single image with transparent interaction regions. Reference controls show the source image or overlay it on the preview.

## Assets

The original checkout contained one SVG wrapping the complete raster image, without separate object vectors. The SVG layers in `assets/objects` and `assets/room-vector.svg` are automatically derived from the reference at 768 × 512, using a 256-color palette and vector paths. They preserve the original composition, but have pixel-level edges and baked-in occlusion; they are not clean hand-drawn object outlines and are best kept at their current positions. The PNG is retained for reference comparison.
