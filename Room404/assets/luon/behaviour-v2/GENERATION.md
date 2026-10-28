# Lươn behaviour sheet — batch 2

- Tool: built-in image generation, with `../luon_idle.png` and the first generated sheet as style references.
- Prompt: one transparent 2×2 pixel-art sheet of the same boy viewed from behind, sitting on a chair with legs hanging, no chair or desk drawn. Four cells: seated still, two writing poses, seated with blue backpack; same dark hair, white T-shirt, olive shorts and green sandals.
- Source: `sheet.png`, 1261 × 1247 pixels. Visual grid: 2 columns × 2 rows. The horizontal grid boundary was moved into the transparent gutter at y=660 to avoid clipping the top-row feet. Crop rectangles (x0, y0, x1, y1): (0,0,630,660), (630,0,1261,660), (0,660,630,1247), (630,660,1261,1247).
- `*_cell.png` files retain raw cell slices; alpha bounds are bottom-centered in 400 × 590 transparent canvases. Pivot: bottom center; pixels per unit: not applicable to this web preview.
- SVGs are traced from the normalized PNGs into editable pixel-aligned paths at 200 × 295, using a 192-color palette and four alpha levels. They are not hand-drawn vectors. `backpack_overlay.svg` clips the backpack frame and is an approximate overlay on study poses.
- The four alpha-bound crops fit within their source cells. Gameplay, animation and placement remain for user checkpoint verification.
