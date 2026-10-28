# Backpack pickup and putdown atlas

Built-in OpenAI image generation produced one transparent 2 × 2 sheet with `unified-v1/bag_0.png` as the character reference. Prompt: same boy and blue backpack; top row crouching to pick up and lifting the bag, bottom row removing its strap and setting it down; no room or furniture.

`sheet.png` is 1226 × 1283; row crops use y=630 to avoid clipping the first row. Crop rectangles are in `slice.py`. The first-row right sprite leaves only three pixels at the crop's bottom, and the second-row left sprite has seven at its top; alpha bounds remain within all cells. Outputs `pickup_0`–`1` and `putdown_0`–`1` are PNG and quantized SVG traces. Normalized canvas 300 × 460, bottom-center pivot and SVG viewBox 150 × 230; pixels per unit is not applicable on web. Shapes are traced, not separated editable body parts.
