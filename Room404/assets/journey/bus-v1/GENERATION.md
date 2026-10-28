# Route 33 bus visual variants

Built-in OpenAI image generation used `Room404/assets/journey/bus-stop.png` as the style reference and produced one transparent 2 × 2 sheet. Prompt: same Vietnamese city bus, white and green, route display 33; left column passenger doors closed, right column doors open; day on the first row and dusk on the second; isolated, no people or street.

`sheet.png` is 1774 × 887. Each crop is 887 px wide, row boundary y=443; exact rectangles and names in `slice.py`. Visible alpha bounds stay within cells, though the vehicles approach the side edges. `day_closed`, `day_open`, `dusk_closed`, `dusk_open` are PNG and quantized SVG traces; the `_tight.svg` variants use a horizontal viewBox. Default normalized canvas is 300 × 460, bottom-center pivot, and tight viewBox 0 180 150 50. Pixels per unit not applicable to web. SVG paths are traces rather than separately editable vehicle parts. Window reflections are baked into each bus sprite. These are asset drafts, not evidence that the bus schedule, boarding or H6 journey is implemented.

The slicer also exports `_native.png` tight crops at original pixel resolution. The web trạm xe scene displays the daytime closed-door sprite as a static preview; bus arrival, boarding, timetable and travel are not implemented by this asset.
