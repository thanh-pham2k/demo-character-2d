# Door action poses

Built-in OpenAI image generation produced one transparent 2 × 2 sheet using `journey-v1/sheet.png` as character reference. Prompt: same boy viewed from behind, first column reaching toward the handle, second leaning forward and pushing the door; top row without backpack, bottom row with blue backpack; no door or background.

`sheet.png` is 1304 × 1206. Each column is 652 px; row boundary is y=618, chosen to leave the sprites clear of cell edges. Exact crop boxes and output names are in `slice.py`; `plain_0`–`1` and `bag_0`–`1` are PNG and SVG path traces. Canvas 300 × 460, bottom-center pivot, SVG viewBox 150 × 230; pixels per unit not applicable to web. SVGs are color-quantized traces rather than separately editable body parts.
