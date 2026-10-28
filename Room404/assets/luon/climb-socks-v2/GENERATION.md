# Climbing with and without backpack

Built-in OpenAI image generation, one transparent 2 × 2 sheet referencing `climb-socks-v1/sheet.png`. The prompt requested alternating raised knees in each row, without backpack in the top row and with backpack in the bottom row, in socks. The generated image repeats the same raised knee in both columns. `slice.py` therefore extracts only the first column in each row and creates a horizontally mirrored SVG for the opposite pose. The second column remains in the source sheet but is not used. Mirroring also flips asymmetric details.

`sheet.png` is 1278 × 1230. Crops are (0, 0, 639, 615) and (0, 615, 639, 1230); alpha bounds do not touch cell edges. `plain_0` and `bag_0` are PNG plus traced SVG; `plain_1.svg` and `bag_1.svg` are mirrored SVG variants. Normalized canvas 300 × 460 with bottom-center pivot; SVG viewBox 150 × 230; pixels per unit not applicable to web. SVG paths are quantized traces, not separate editable body parts.
