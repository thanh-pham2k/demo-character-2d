# Opposing walk poses

Built-in OpenAI image generation made one transparent 2 × 2 sheet using `unified-v1/plain_0.png` as character reference. Prompt: boy facing right, column one foreground leg behind, column two foreground leg in front, opposite arm swings, no backpack in top row and blue backpack in bottom row. The two poses have distinct arm and limb overlap; visual quality of the loop should be checked during gameplay.

`sheet.png` is 1149 × 1368. `slice.py` records the cell coordinates, with variable margins; no character touches a crop edge. Output files `plain_0`, `plain_1`, `bag_0`, `bag_1` are PNG and quantized SVG traces. Each is placed on a 300 × 460 canvas with bottom-center pivot; SVG viewBox is 150 × 230; pixels per unit is not applicable to web. The web cycle interleaves these poses with passing poses from `walk-v2`.
