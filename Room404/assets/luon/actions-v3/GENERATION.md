# Action atlas v3

Built-in OpenAI image generation, one 4 × 2 transparent batch. Reference: `unified-v1/plain_0.png`. Prompt: same boy, palette and 2D game style; row 1: two ladder-climbing poses facing away, then two side-lying sleeping poses; row 2: two raised-arm switch poses, then two door-handle poses; no furniture or background. The generated poses do not include a backpack.

`sheet.png` is 1774 × 887. `slice.py` records exact crop rectangles and outputs `climb_0`–`climb_1`, `sleep_0`–`sleep_1`, `switch_0`–`switch_1`, and `door_0`–`door_1` as PNG and traced SVG. Crops have variable margins and no subject touches crop edges. Each output is on a 300 × 460 canvas with a bottom-center pivot; SVG viewBox is 150 × 230. Pixels per unit: not applicable to web. SVGs are quantized traces rather than independently editable character parts.

The `sleep_bed_0` and `sleep_bed_1` SVGs reuse the sleep traces with a tighter viewBox (0 157 150 73) so the horizontal character fills its bed-sized display box. Right-bed rendering mirrors the sprite so the head faces its pillow.
