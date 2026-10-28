# Journey action atlas

Built-in OpenAI image generation used one transparent 4 × 2 batch; reference: `unified-v1/bag_0.png`. Prompt: the same boy, top row without backpack and bottom row with backpack; columns brushing teeth, washing face, stepping down, and reaching for a door, isolated against transparent background with no furniture. The generated stair pose is a generic step, and each action has one illustrated pose; the web preview adds a small motion to that pose rather than a frame-by-frame animation.

`sheet.png` is 1536 × 1024, with 384 × 512 cells. Crop coordinates and output names are in `slice.py`; no alpha bounds touch a cell edge. Files `plain_brush`, `plain_wash`, `plain_stairs`, `plain_door` and corresponding `bag_…` are PNG and color-quantized SVG traces. Normalized canvas 300 × 460, bottom-center pivot, SVG viewBox 150 × 230. Pixels per unit is not applicable to web. SVGs are traces, not separately editable body layers.
