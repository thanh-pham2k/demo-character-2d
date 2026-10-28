# Lighting action atlas

Built-in OpenAI image generation produced one transparent 4 × 2 sheet, using `unified-v1/bag_0.png` as the character reference. Prompt: same boy viewed from behind, columns 1–2 reaching up to a wall switch, columns 3–4 reaching forward to a desk lamp, first row without backpack and second row with backpack, no room or props.

`sheet.png` is 1774 × 887. `slice.py` records all eight crop rectangles (four columns, two rows; variable margins) and generates `plain_switch_0`–`1`, `plain_lamp_0`–`1`, `bag_switch_0`–`1`, and `bag_lamp_0`–`1` in PNG and quantized SVG. All alpha bounds remain inside their crop rectangles. Each has a 300 × 460 normalized canvas, bottom-center pivot and SVG viewBox 150 × 230. Pixels per unit: not applicable to web. SVG paths are raster traces rather than independent editable parts.
