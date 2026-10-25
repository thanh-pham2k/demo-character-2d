# H6 reference correction

One two-environment batch generated with built-in imagegen from the user's
building/classroom photographic references, followed by one whole-sheet format
refinement. Original references, prompts, first source and final sheet retained.

The final generator canvas is 1182×1330 (requested size was not guaranteed).
Measured split y=717: top H6 building 1182×717, lower classroom 1182×613.
`slice.py` exports h6_gate_v2.png and h6_classroom_v2.png to the existing Unity
year1 sprite folder. manifest.json records exact crops and center pivots.
Import: PPU100, Point, Uncompressed, no mipmaps. Existing character/hotpot
sprites are reused; earlier backgrounds and crop assets are retained.

New architecture: blue/turquoise structural towers, white balcony corridors,
central H6 entrance and landscaped forecourt. Classroom: rear-to-front view,
green board, central projection screen, fans/projector, windows and occupied
desk rows around a central aisle; the two front seats remain available.

Authoring changes fit the camera to the full university backdrop, reposition
door/seat/board markers and native desk foreground crop, and temporarily scale
the player to match the classroom perspective. Room scale restores on return.

Integration complete. Both backgrounds, new foreground crop and calibrated
markers are saved in Room404_MVP. Teacher/player placement was visually checked
against the new room perspective. Existing hotpot and narrative flow retained.

Final actual Play Mode regression: **160 passed / 0 errors**, UTC
2026-10-25T07:33:29.0294170Z. MVPSceneTester later stages/ending also pass.
The first test was interrupted by calling Build Scene during Play; that editor
command is now disabled/refused during Play to prevent the observed exception.
Final reports and actual runtime captures are in review/. Source whitespace
check and imported PPU100/Point references pass. Scene remains in Edit Mode.
