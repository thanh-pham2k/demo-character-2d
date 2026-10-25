# Remaining production assets — execution log

Updated 2026-10-25. Scope: art production/import evidence; full gameplay remains in progress.

## Inventory and provenance

24 composite sheets sliced into 178 primary outputs: 46 character poses, 46 props, 12 backgrounds and 74 animation frames. Existing sprites are reused separately, never relabeled as generated batches.

Every batch directory retains the planning brief (`prompt.txt`), exact initial tool prompt (`generation-prompt-v1.txt`), selected `source.png`, planned manifest, actual `manifest.json`, crop geometry and slice preview. Actual dimensions/crops override the requested grid. Runtime outputs are under `mobile-2d/Assets/Sprites/MVP/remaining/<batch>/`.

Generation used built-in imagegen. Refinements affected whole composite sheets, not isolated output calls. Source and output SHA256 evidence is stored in manifests.

| Batch | Grid | Outputs | Source |
|---|---|---:|---|
| C00 | 4×2 | 8 | `C00/source.png` |
| C01 | 4×2 | 8 | `C01/source.png` |
| C02 | 4×2 | 8 | `C02/source.png` |
| C03 | 4×2 | 8 | `C03/source.png` |
| C04 | 4×2 | 8 | `C04/source.png` |
| C05 | 3×2 | 6 | `C05/source.png` |
| P00 | 4×2 | 8 | `P00/source.png` |
| P01 | 4×2 | 8 | `P01/source.png` |
| P02 | 4×2 | 8 | `P02/source.png` |
| P03 | 4×2 | 8 | `P03/source.png` |
| P04 | 4×2 | 8 | `P04/source.png` |
| P05 | 3×2 | 6 | `P05/source.png` |
| E00 | 1×2 | 2 | `E00/source.png` |
| E01 | 1×2 | 2 | `E01/source.png` |
| E02 | 1×2 | 2 | `E02/source.png` |
| E03 | 1×2 | 2 | `E03/source.png` |
| E04 | 1×2 | 2 | `E04/source.png` |
| E05 | 1×2 | 2 | `E05/source.png` |
| AN00 | 5×2 | 10 | `AN00/source.png` |
| AN01 | 5×2 | 10 | `AN01/source.png` |
| AN02 | 5×3 | 15 | `AN02/source.png` |
| AN03 | 5×3 | 15 | `AN03/source.png` |
| AN04 | 4×3 | 12 | `AN04/source.png` |
| AN05 | 4×3 | 12 | `AN05/source.png` |

## Extraction and registration

- `slice_batch.py` measures actual crop geometry and records normalization. Actors share feet baselines; animation scale is shared per sheet, preserving source bob.
- C00 and C02 retain original sources and whole-sheet padding refinements.
- C05 and P05 retain rejected halo attempts and white-matte sources. Edge-connected matte extraction scripts and metadata preserve white costume interiors; the selected alpha source remains reproducible.
- C05 reunion outputs use 384×320 canvases with body/shoulder anchors; `reunion-fit-preview.jpg` demonstrates contact staging. Other standing actors generally use 256×320.
- Wardrobe open/closed share a registered canvas and foot pivot. Nồi remains a separate reused overlay.
- AN04/AN05 retain first versions, gait-guide references and whole-sheet gait refinement prompts.
- E03/E04 panel boundary is the midpoint627, not a ceiling edge. E05 is extra-wide1774×887: preserve aspect at8 world-unit height and camera crop.

## Verification and limitations

`review/report.json` records current Unity import settings, pivot/FullRect checks, source-output hashes and native clip sampling. `blockout/report.json` covers spatial staging only. These reports do not prove full mission gameplay.

`review_animation.py` produces16 cycle previews and registration/phase metrics. All cycles still require motion approval in gameplay. Known review flags include backpack-back body bob and similar side contact/passing silhouettes; do not treat four unique PNG hashes as gait approval.

The separate `AssetProduction_Blockout` and `AssetProduction_Gallery` scenes preserve the existing Room404_MVP flow. Native clips are reusable catalog entries; no additional sprite writer is attached to the gameplay player.

## Remaining gates

1. Review motion and correct confirmed cycle issues using the batch/refinement plan.
2. Validate scene camera, foreground, seat/carry/contact anchors and room-state continuity.
3. Integrate assets into all45 missions, finish UI/data/audio and transition/save/economy ownership.
4. Run complete branch/failure/save/load/touch playthrough, ending and build checks.

See `story/ASSET_PRODUCTION_CHECKLIST.md`, `story/REMAINING_ASSET_INVENTORY.md` and `story/PRODUCTION_FLOW_BASELINE.md` for complete scope.

## H6 production integration

C00 `npc_h6_classmate_writing` now replaces the former Dam classroom identity in the saved Room404_MVP scene and both scene authoring paths. The NPC sprite writer is configured with the same classmate reference so Update cannot restore old Dam frames. Teacher gestures remain configured separately. Classroom dialogue/prompt now says “Bạn cùng lớp”; dorm conversations still use Đạm. Serialized action ordinal and historical GameObject names are preserved for existing references.

`review/classmate-integration.txt` proves saved scene and writer references. This is not a full Play Mode test. The return-to-404 reaction beat is implemented with a one-time checkpoint flag and a journey input lock; the fresh Play Mode regression passed506checks with0errors, including the home reaction and one-time commit (see `review/h6-playmode-report.json`). Complete M06/07 production transition remains pending.

## AN02 gait refinement

Refined the complete5×3 sheet with the saved `AN02/refinement-gait-prompt.txt` using original AN02 and the code-native gait diagram as references. Selected output is `source-gait-v2.png`, copied to `source.png`; previous artifacts remain in `before-gait-refinement/`. Actual1619×971 source was remeasured and sliced to15 registered outputs. Static preview shows alternating wide contact/narrow passing poses; the former silhouette warning is cleared by the metric. This does not grant full motion/gameplay approval. Unity report must be refreshed against the new15 hashes after Play Mode tests finish.

## AN03 gait refinement

Same whole-sheet5×3 correction for robe walk using saved prompt and original source plus gait diagram. Source1619×971, measured transparent gutters,15outputs at common scale/270px nominal full height. Previous source/crops/manifest remain in `AN03/before-gait-refinement/`. Static preview confirms alternating contact/passing feet with intact mortarboard/robe; metric warning clears. Full motion/gameplay approval is pending.

## Production campaign model

Implemented a separate45mission/199event definition and ordered event/economy/inventory/academic/checkpoint model. Current EditMode report passes28911assertions across16choice combinations and matches the current definition SHA256. See `review/production-campaign-model.json` and `story/PRODUCTION_CAMPAIGN_IMPLEMENTATION.md`. This is a data-model gate; it is not connected to the scene and does not prove minigame/physical action completion or full45mission playthrough. Legacy missed-bus walking fallback remains a required runtime correction.

## Production opening integration

Separate `Production_Opening.unity` now runs the ten physical M01–03 actions using already-generated C00/E00/P00 and existing room/bus/props. No imagegen calls in this phase. Nine existing side sprites were registered by the saved `derived-existing/normalize_existing.py`; provenance manifest explicitly marks them as derived, outside178primary outputs. Optional directional sets use the existing Luon sprite writer; no second Animator.

Fresh PlayMode report passes125checks for both bed choices, real movement/E, doc drag/drop, fare/idempotency, scene transitions, cancel/hold/release/background pause, checkpoint resume and directional/outfit/feet binding. Captures reviewed after correcting floating bus, prop scale, modal order and hold-fill geometry. Current source/scene/report hashes are recorded in `review/opening-runtime-evidence.json`. This is an opening prototype; M04–45, device/portrait/audio/detail animation and fullroute QA remain pending.

## M04–M05 production integration

Extended the same scene through supplies purchase, real four-spot cleaning with60s timer/retry, sequential roommate introductions, canteen travel, six priced meal variants, chair presentation, phone support and return/sleep toM06. No imagegen calls. Three standing friend sprites normalized from existing individual sources; derived-existing manifest now contains12outputs, outside178primary generated cells. Native counter foregroundV2 and tray transform remove occlusion; room walking/cleaning anchors now stay within the visible central floor. Crop/transform provenance is retained.

Current PlayMode:271checks pass,0errors, both bed routes throughM05; includes accelerated timeout path, pause/reload and no duplicate kit/meal/support. Current model report:346785assertions across192choice combinations with explicit old<=M04prefix save compatibility. Both reports retain their limited scope; M06–45runtime/full release gates remain pending. See `story/PRODUCTION_HOUSEKEEPING_REVIEW.md`.

## 2026-10-25 — School adapters; no image generation

Implemented reusable production bus progress host (one shared wallet, direction/mission/choice gates, commit before destination unlock, retry on failed save), durable WC completion callback and explicit room camera configuration. Legacy missed-bus objective shows wait/board-next-bus and hides expired countdown.

Evidence: `review/production-commute-host.json`91 model assertions; `review/commute-playmode/report.json`59 physical adapter checks; `review/year-one-progress-next-bus.json`117 data/presentation assertions. Hashed scope: `review/production-school-adapter-evidence.json`. Full legacy Year1 suite previously completed514 checks at12:55UTC; that prior result is not proof of the later production route.

Fixture checks real vehicle arrival/boarding/departure, rollback with no fare on save failure, one successful fare, checkpoint reload, visible next bus, interrupted boarding, locked WC commit/retry and actor renderer/physics restoration. Fixture uses existing art and does not save scene changes. Not accepted yet: authored production M06–M08 route and M01→M09 playthrough. No batch/source/crop/output changed.

## 2026-10-25 — M06 physical production route; no image generation

Integrated backpack from selected bed, physical hallway/WC, real bus33 boarding/departure both directions at7k each, H6/classroom, teacher/classmate dialogue, retry/cancel warmup55, lesson end and separate Dam reaction in404. Preserves0year credits; opensM07instead of demo gaming. Corrected M06 checkpoint zones and explicitly allowed unchanged old history throughM05only; archived previous definition.

Fixed reused WC collider offset and hallway floor alignment, classroom center aisle bounds/spawn, standing foreground order and seated study pose. No new raster art or slicing. Return journey currently presents KTX/stairs/room as one locked transition; free-control stairs remain outside this prototype acceptance.

Final full opening run finished2026-10-25T13:40:09.3674090Z:390checks,0errors, both bed choices throughM01–M06. Model346807assertions/192choice routes passes current definition. Reviewed WC/bus/aisle/seated captures; recorded149hashed files in`review/opening-runtime-evidence.json`. LaterM07–M45, motion artistry/audio/touch/device/build/fullgame gates remain pending. Details:`story/PRODUCTION_SCHOOL_REVIEW.md`.

## 2026-10-25 — M07/M08 gameplay prototype; no image generation

Added persistent paused morning clock, queue/WC, actual next-bus recovery (including departure after parking), attendance reaction, reserved9fares/63k, six distinct lesson/exam pairs and durable6/11/16/21/26/32credits. Full definition now45missions/212events; old source archived with explicit unchanged prefix throughM06. Year1remainsfictional curriculum; no prior pass of Year2GT2/VL1/KTLT.

Integrated cooker pickup,1second hold power connection, contributed ingredients, four existing floor poses, separate toast and sleep→M09registration. Reused pot/cups baked in poses, moved fan/cord/socket via Unity transforms; no new raster outputs. Added native HotpotCord.mat only. Four existing prop attachments remain pending: toothbrush_cup,towel_folded,dried_buffalo_meat,drinking_glass; bind or document reuse elimination before final art acceptance.

Firstfullruns exposed destroyed-actor lifecycle/update exceptions. Fixed both and kept forced-despawn checks. Final fullM01–M08finished2026-10-25T14:43:28.1057670Z:739checks/0errors. Separateyear1fixture438checks; lifecycle52checks; timer/curriculum42assertions; model369301assertions/192routes/212events currentSHA. Snapshot261files inreview/opening-runtime-evidence.json. M09–M45/audio/artistic motion/device/build/fullending pending;24sheets/178outputs unchanged.

## 2026-10-25 — Existing props and M09 registration; no image generation

Bound P01brush-cup/towel to WC progress, P04dried buffalo gift to M08ingredients, P05glass beside floor group. Three floor poses bake cups; To still holds bowl, so hand/contact animation remains pending. Reused typing0/1 and laptop for physical registration portal. F5before/after opening, full09:00section, three required classes, draft cancel/reload, committed schedule review and locked weeks bridge toM10. No new model events, charges, credits or rasters.

FullM01–M09finished2026-10-25T15:21:45.4415010Z:819checks/0errors. Focusedregistration87checks; snapshot292source/scene/data/art/meta/report/capturehashes. M10night entry matte/window polish plus M10–M45/artistic contact-motion/audio/device/build/fullending pending.24sheets/178outputs unchanged. Verification policy: focused checks for local changes; full route at milestone/shared-flow boundaries.
