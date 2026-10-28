# Stair ascent/descent atlas

Built-in OpenAI image generation, one transparent 4 × 2 sheet using `journey-v1/sheet.png` as identity and style reference. Prompt: the same boy facing right, first two columns lifting a foot for ascent, next two reaching down for descent; first row without backpack, second row with blue backpack; isolated figures without stairs or scenery. The paired frames differ only subtly; the character translates along the illustrated steps in the preview.

`sheet.png` is 1536 × 1024. Each cell is 384 × 512; exact rectangles and output names are in `slice.py`; alpha bounds do not touch cell boundaries. Outputs `plain_up_0`–`1`, `plain_down_0`–`1`, `bag_up_0`–`1`, `bag_down_0`–`1` are PNG and quantized SVG traces. Normalized canvas 300 × 460; bottom-center pivot; SVG viewBox 150 × 230. Pixels per unit: not applicable to web. SVG paths are raster traces, not separately editable character parts.
