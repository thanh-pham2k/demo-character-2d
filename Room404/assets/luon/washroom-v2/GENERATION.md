# Rear-view washroom atlas

Generated using the built-in OpenAI image generator in one transparent 4 × 2 batch, referencing `journey-v1/sheet.png`. Prompt: same boy viewed from behind toward the sink and mirror; two brushing frames and two face-washing frames, with no backpack in the first row and a blue backpack in the second; isolated, no furniture or background.

`sheet.png` is 1536 × 1024; `slice.py` records the eight 384 × 512 crop rectangles and outputs `plain_brush_0`–`1`, `plain_wash_0`–`1`, `bag_brush_0`–`1`, and `bag_wash_0`–`1` as PNG and traced SVG paths. No subject's alpha bounds reach a crop edge. Each frame is bottom-aligned on a 300 × 460 canvas, with a bottom-center pivot and 150 × 230 SVG viewBox. Pixels per unit: not applicable on web. The SVG paths are quantized raster traces, not independently editable character parts.
