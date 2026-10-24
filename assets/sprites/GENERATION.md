# Sprite sheets

Generated with the built-in image generation tool.

## wuxia-hero.png

Use case: stylized-concept. Create a production 2D game character sprite sheet, one image, exactly 4 columns by 2 rows, exactly eight equal square cells, canvas aspect 2:1. Invisible regular grid, no lines, no labels, no text. Solid pure #FFFFFF background, absolutely no floor or cast shadows. Each complete sprite including sword stays inside its cell with generous clean margins, no overlap or clipping. Same stylized Asian wuxia swordsman in all eight cells: slightly oversized head, compact readable RPG body, simple teal martial arts travel robe, dark trousers and boots, headband, dark tied hair, Chinese straight jian sword visibly held in every cell. Clean hand-painted 2D game art, crisp cel shading and sharp dark outlines, uncluttered silhouettes. Camera locked at 35-degree top-down 2.5D isometric angle, character always oriented bottom-right. Lock identity, clothing, colors, proportions, body scale across cells; crouching naturally lowers height. Row 1 left to right: 1 idle ready stance sword steady; 2 idle breathing relaxed subtle weight shift; 3 walk left foot forward; 4 walk right foot forward. Row 2 left to right: 5 attack wind-up sword pulled back high; 6 fast horizontal forward sword slash impact with full arm and blade extension, visible straight sword; 7 dodge dash low crouched forward pose holding sword; 8 hit reaction stagger backward hurt pose still holding sword. Eight distinct purposeful poses, one single character per cell. No extra effects or props.

Frames, row-major: idle ready, idle breathing, left walk step, right walk step, attack wind-up, attack impact, dodge dash, hit reaction. Slice into four equal columns and two equal rows.

## cyan-slash.png

Use case: stylized-concept. Asset: 2D mobile action RPG sword slash VFX sprite sheet ONLY. Single very wide image aspect ratio exactly 4:1, exactly FOUR equal square cells in ONE horizontal row. Invisible grid, no border lines or labels or text. Pure flat solid black #000000 background. Each effect fully contained in its respective equal cell with clean black margins, no overlapping frames. Fixed camera, fixed effect center and consistent slash direction across all four frames. NO character, no human, no weapon object, no scenery. Sharp huge crescent arc / curved blade wave, radiant electric cyan-blue core transitioning into white luminous glow, razor edges, dynamic energy trails with subtle martial arts ink-wash strokes. Clean stylized anime game VFX. Read left to right as sequential animation: cell 1 small crescent blade instantly igniting, small bright flash occupying about 25 percent of cell; cell 2 maximum size sweeping crescent arc occupying 80 percent of cell, massive powerful 3x-character-scale energy wave, brilliant cyan-white blade edge with flowing trail; cell 3 expanding slash wave slightly larger spatial extent but breaking apart into separated sharp angular cyan energy shards, recognizable broken crescent; cell 4 faint sparse cyan sparks and dissolving energy mist, nearly vanished, very low brightness. Strong visual temporal progression from small ignition to maximum power to fragmentation to dissipation. Exactly four isolated effects, no extra arcs outside cells, no additional rows. Flat black background must reach every outer edge and every inter-cell margin.

Final edit: preserve the designs and sequence; center effects in four equal-width cells, shrink to prevent overlap, and retain black gutters between frames. Slice into four equal columns and one row. Cells are rectangular.


## bunk_bed_lengthwise.png — 2026-10-24

- Output: `mobile-2d/Assets/Sprites/MVP/environment/bunk_bed_lengthwise.png` (1068 × 1472 RGBA, transparent background).
- Edit target: the original `mobile-2d/Assets/Sprites/MVP/environment/bunk_bed.png`; the original asset is preserved.
- Perspective reference: `mobile-2d/Assets/Sprites/MVP/environment/room_404_backdrop.png`.
- One shared sprite serves both walls; the right instance mirrors the left. No second image generation is needed.
- Provenance: a single-image perspective edit, **not** a batch-generated sheet. Retained for reuse; future related asset generation must follow `AGENTS.md` and `.agents/rules/image_generation.md` (one planned grid, then slicing).
- Import: single sprite, 100 pixels per unit, point filtering, no mipmaps, no compression.
- Scene: upright posts, near short end facing the camera, length receding toward the back wall; identity Z rotation. Do not rotate the flat sprite 90° to simulate a spatial quarter turn.

Prompt:

> Use case: precise-object-edit. Create ONE transparent isolated game sprite: the EXACT SAME bunk bed from image 1, turned in 3D by a quarter turn so its LENGTH goes away from the camera toward the BACK, for placement LENGTHWISE against the LEFT side wall of the room in image 2. Image 1 is the edit target/design reference; image 2 is ONLY a camera and perspective reference and must NOT appear in output. Preserve the same worn green tubular metal frame, rusty joints, simple rails, narrow ladder, two bare tan scratched wooden sleeping platforms, no mattress or bedding, identical illustrated pixel-textured art style. CHANGE ONLY THE SPATIAL ORIENTATION. Crucial: view from near the FOOT of the bed, looking along its LENGTH toward the far HEAD. The NEAR SHORT END is across the bottom and the FAR SHORT END is across the top. The two LONG edges extend mostly VERTICALLY in the image from lower foreground to upper background, converging slightly toward the upper right. The bed floor footprint's long axis is nearly vertical with only ~12 degrees tilt toward upper right as it recedes; align with left side wall floor direction of image 2. The near foot crossbar is almost HORIZONTAL. All four corner posts and the ladder remain VERTICAL upright (gravity). See the inner/right long side and the near foot end; the outer/left long side will sit against the left wall. Narrow front-view width, substantial visible bed length receding away, like a long vertical strip in floor plan. Elevated camera as in image 2; see both wood decks from above. Keep structure physically coherent with four upright posts, two sleeping levels, do not tip it over, do not rotate the flat image on its canvas. This must look completely different in orientation than image 1: image 1 has a broad diagonal long side facing the viewer, output must have the NARROW SHORT END facing viewer and length leading away. Single bed only, complete uncropped feet, tightly framed with small transparent margin, actual transparent background with alpha. No room, floor, cast shadow, text, labels, diagram, second bed, or other object. Output portrait aspect ratio.

## Room furniture layout and batch rule review — 2026-10-24

Reused the existing `study_desk_chairs.png`, `wall_socket_clean.png`, `fan_mount_stem.png`, and `fan_rotor.png`. No image generation call, new sheet, or slicing was required for this layout update. The desk and its associated props move together; the socket mounts on the back wall; the fan layers now share the correct custom pivot at source pixel (238, 158) measured from the top left of their 510 × 268 canvas.

For future image generation, follow the planned grid and slicing requirements in `AGENTS.md` and `.agents/rules/image_generation.md`. Record the source sheet, exact prompt, canvas/grid sizes, margins, cell order, crop coordinates, output paths and Unity import settings. Do not describe a reused asset, an existing single-image edit, or a repacked atlas as a batch generation.

Placement follow-up: the desk now sits directly below the window, with its laptop, light and seat marker moved together. The fan sits higher at the ceiling at a smaller scale. Its rotor artwork has its vertical foreshortening undone (local Y scale 2) before rotation in a 75-degree tilted plane, keeping its rotation horizontal in the room view. `MVPSceneSnapshot.CaptureFanRotationReview()` renders 24 angles, 15 degrees apart, into the ignored Unity Temp folder; `fan_rotation_demo.gif` previews the resulting revolution. These are rendered verification frames from existing assets, not newly generated image assets.

## Requested final layout and hinged door — 2026-10-24

No AI image generation was used. All existing artwork, camera and room pixels are retained.

- Both bunks retain their sprite, scale and zero Z rotation. Their centers move from `(±3.65, -0.35)` to `(±3.95, -0.95)`: 0.6 units toward the foreground, 15% of their four-unit projected height. The outward shift follows the side-wall convergence, and the two instances remain mirrored.
- The desk reuses `study_desk_chairs.png` unchanged. `Room404/SpriteProjection` applies vertical shear `-60/647` to the sprite vertices while preserving UVs and upright legs, bringing the tabletop edges parallel to the back wall. Its rear edge sits just under the window sill. The red chairs remain part of the original sprite.
- The laptop reuses `laptop_open.png`, at scale 0.10 and a vertical offset of 0.53 above the desk origin. All four keyboard-base corners were checked against the projected tabletop polygon.
- `Assets/Room404Layout/DoorLeaf.asset` is a native Unity sprite slice referencing the original backdrop texture: bottom-left crop rect `(652, 304, 128, 230)` on its 1037 × 627 canvas, 100 PPU, bottom-right pivot `(1, 0)`. The fixed frame and transom remain in the backdrop. This is a deterministic slice, not a generated image.
- A plain one-pixel sprite forms the dark interior behind the opening. `HingedRoomDoor` projects the door around its existing right hinge, updates its floor collider, and supports `[E]` through `IInteractable`. The saved preview is 25 degrees ajar; the interactive open limit is 70 degrees.
- `MVPSceneBuilderFinal.ApplyRequestedRoomLayout()` updates only the requested layout objects. The full scene builder uses the same settings.
- `MVPSceneSnapshot.CaptureDoorSwingReview()` verifies interaction reachability, fixed hinge, full opening and closing; it renders 32 review frames into the ignored Unity Temp folder. `door_swing_demo.gif` is assembled from those rendered frames. The closed door interior was pixel-identical to the previous image over the compared 143 × 246 region.


Editor menu cleanup: `KTX404` exposes only `Build Room 404 Scene`, which builds the latest layout. Incremental layout helpers and review captures are no longer menu entries; their methods remain available for development and verification.


## luon_interaction_sheet.png — 2026-10-25

- Output: `assets/mvp_sprites/interaction/poses-sheet-alpha.png` (1774 × 887 RGBA, transparent background).
- Sliced sub-images:
  1. `mobile-2d/Assets/Sprites/MVP/characters/interaction/luon_sitting_chair.png` (385 × 762, unitHeight 1.9, custom pivot [0.5, 0.22])
  2. `mobile-2d/Assets/Sprites/MVP/characters/interaction/luon_lying.png` (701 × 745, unitHeight 1.8, custom pivot [0.5, 0.5])
- Design & Reference: Character Lươn from `luon_idle_0.png`, `luon_walk_*.png`, `luon_sitting_laptop.png`.
- Batch Layout: 2 columns × 1 row (equal cell width 887, cell height 887). Single image generation with 2 poses, sliced via Python and imported with Point filter, Uncompressed, 100 PPU equivalent scale.
- Adheres strictly to `AGENTS.md` and `.agents/rules/image_generation.md` (no single image generated alone, generated as batch grid and sliced).
