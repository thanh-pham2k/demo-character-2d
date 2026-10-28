# Walking sheet v2

Built-in OpenAI image generation used one transparent 4 × 2 batch, with `unified-v1/plain_0.png` and `bag_0.png` as references. Prompt requested the same boy walking right, four phases per row, without and with backpack, including alternating forward feet and passing poses. The generated sheet has passing poses but does not clearly alternate the forward foot, so the resulting cycle is an approximation.

`sheet.png` is 1774 × 887. `slice.py` records the exact grid cell rectangles and outputs `plain_0`–`plain_3` and `bag_0`–`bag_3` as transparent PNG and SVG traces. Margins vary; no alpha bounds reach a crop edge, although `plain_1` has only two pixels at the bottom. Output pivot: bottom center of 300 × 460 canvas; SVG viewBox 150 × 230; pixels per unit not applicable to web. SVGs are quantized traces, not layered vectors.
