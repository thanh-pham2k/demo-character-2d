# Year 1 campaign asset batches

Generated through the built-in image generation tool, 25 October 2026.
Lươn's existing design was the reference for proportion, outline and palette.
Existing room, beds, H6 architecture and Lươn bed animation were reused.

| Batch | Planned content | Actual source layout | Runtime use |
|---|---|---|---|
| desk | Six typing/mouse/rest poses for Lươn and Long | 1448 × 1086, 6 columns × 2 rows | Twelve registered desk frames |
| npc | Long bed entry/sleep/exit; Đạm writing/reaction; teacher gestures | 1254 × 1254, five packed rows | Sixteen isolated poses |
| environments | Classroom, computer lab, student canteen | 966 × 1629, three horizontal panels | Three backgrounds; native desk foreground crops |
| long_motion | Four walk and two stand-up poses | 1536 × 1024, 3 × 2 | Six feet-registered frames |

Exact requests are preserved in each batch's `PROMPT.md`; unmodified RGBA
sources in `source.png`. The generator changed the requested desk/NPC grid,
so measured rectangles are explicit in `slice.py`, not inferred equal cells.
`manifest.json` records all 37 output crops, scale and anchors. NPC extraction
keeps the main connected alpha component to remove adjacent-cell fragments.

Reproduce slicing with a Python environment containing Pillow:

    python3 assets/mvp_sprites/year1_campaign/slice.py

Unity imports: 100 pixels/unit, Point filtering, no compression. Runtime crops
are authored in YearOneCampaignSceneSetup; camera assets are not regenerated.
Desk upper/lower layers reuse the existing desk foreground convention. The
upper body's authored registration rises 0.45 room units during typing, while
legs remain at SeatPivot. Laptop-facing is explicit, independent of the NPC's
last walking direction. Long's front/overhead sleep pose is registered by Sleeping_Pose_Registration
(identity rotation, .30 scale, +.14 vertical alignment) to the room bed axis.
These registrations are serialized scene authoring data, not world-position
branches in the player detector.

Video records and the final review are in assets/reviews/year1_campaign/.

A fifth targeted batch `long_bed_v2` replaced only nine Long bed poses after
actual video review found that rotating side-on art did not match Luon's
camera. Source 1254 × 1254, exact 3 × 3 cells of 418px. Common .9 scale and
head registration; transparent 384 × 384 exports. Sleeping pose registration
now uses identity rotation, .30 scale and +.14 vertical alignment. The manifest retains superseded crop
provenance separately; there are still 37 distinct runtime PNGs.

Long walk/rise PNGs retain a common feet registration within each canvas; Unity imports use centre pivots, matching the existing NPC actor origin and room markers. Stand-up returns to the cached standing scale before walking. Actual pose-registration-review.mp4 verifies this transition.
