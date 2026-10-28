# Front-view toothbrushing close-up

Built-in OpenAI image generation, one transparent 2 × 2 sheet, using `unified-v1/plain_0.png` as the character reference. Prompt: same boy facing front, brushing with the same hand, two subtle frames per row, without backpack on top and with backpack straps on bottom; waist-up and isolated with wide gutters. The two poses differ only slightly, so the animation is intentionally restrained.

`sheet.png` is 1246 × 1263; each column is 623 px wide, row boundary y=630. Crop rectangles and names are in `slice.py`; subject bounds do not touch crop edges. `plain_0`–`1` and `bag_0`–`1` are PNG and color-quantized SVG path traces, normalized to a 300 × 460 canvas with bottom-center pivot; SVG viewBox 150 × 230. Pixels per unit is not applicable to web. These are traced paths rather than separately editable body parts. The first generated attempt was discarded because hair/torso touched the cell edge and one pose changed hands.
