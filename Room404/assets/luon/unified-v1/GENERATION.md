# Lươn unified sprites

Three transparent sheets generated with OpenAI image generation from the existing Lươn character and supplied atlas. The prompts specified the same boy, dark hair, dinosaur T-shirt, olive shorts, green footwear and blue backpack, with isolated frames and consistent style.

- `no-backpack-sheet.png`: 2 × 2, idle and three walking frames, without backpack.
- `with-backpack-sheet.png`: 4 × 2; only the second row is used, idle and three walking frames with backpack. The first row did not consistently omit the backpack.
- `seated-sheet.png`: 4 × 2, seated facing away, two studying poses and tablet, without backpack on the first row and with backpack on the second. The studying poses contain a tablet.

Run `python3 slice.py` to slice the sheets using the exact boxes in the script. It crops each frame to its alpha bounds, removes faint background haze, and places it on a transparent 300 × 460 canvas with a bottom-center pivot. SVGs are generated as quantized colored paths on a 150 × 230 viewBox; they are traced from the generated raster artwork, not hand-authored editable character layers. Pixels per unit is not applicable to the web preview.
