# Desk and tablet with backpack

Built-in OpenAI image generation, one transparent 2 × 2 sprite sheet. References: `actions-v2/sheet.png` for character and poses and `unified-v1/bag_0.png` for backpack. Prompt: same character with the backpack in all cells; first row two seated rear-view desk poses in a blue chair, second row two standing tablet poses, no desk, laptop or background. The seated frames include a chair.

`sheet.png` is 1024 × 1536. Each column is 512 px; first row crop ends at y=745 and the second starts there to avoid clipping the second row's hair. Exact crop rectangles and output names are in `slice.py`; alpha bounds stay inside each crop. `study_0`–`1` and `tablet_0`–`1` are PNG and quantized SVG traces. Normalized canvas 300 × 460, bottom-center pivot, SVG viewBox 150 × 230, pixels per unit not applicable on web. SVG paths are traces, not separately editable layers.
