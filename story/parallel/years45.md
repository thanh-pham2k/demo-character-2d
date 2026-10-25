# Years45 handoff — M24–M36

Source stable 2026-10-25. **Not integrated into the shared scene. No Unity refresh, Unity compilation, or Play Mode was requested or run.** Offline Roslyn checks succeeded; native scene/device/presentation acceptance remains pending coordinator QA. No image generation, shared core edits, second session/wallet, ProjectSettings or Temp request writes.

## Files / entrypoint

- `Assets/Scripts/ProductionChapters/Years45/Years45Activities.cs`: explicit authored contracts for all **54 events** from the current `missions.json`; no fallback completion. Per-step ordered inputs, wrong-action rejection, room occupants derived from checkpoint facts, course gates.
- `Assets/Scripts/ProductionChapters/Years45/Years45ChapterModule.cs`: `ProductionChapters.Years45.Years45ChapterModule : IProductionChapterModule, IInteractable`. Inject existing `ProductionChapterContext` with `Configure(context)` at M24. Uses only `context.Progress` and `context.Commit`; no save/session construction. `Refresh()` activates/reconstructs its owned UI/staging; `Suspend()` closes UI and releases only its own player lock. Disable/unload paths do the same.
- `Assets/Editor/ProductionChapters/Years45/Years45ChapterBuilder.cs`: opt-in `Tools > Production Chapters > Years45 > Create owned staging under selection`. No import hook. Select composition-owned world parent before invoking. Creates only an owned staging hierarchy, serialized existing sprite references, six configurable action anchors, three independent room-slot renderers and three separate visitor renderers.
- `Assets/Editor/ProductionChapters/Years45/Years45ContractChecks.cs`: opt-in authored/model checks, not invoked in this task.
- Corresponding `.meta` files included.

## Wiring needed by coordinator

1. Instantiate/create the module under the shared composition root; only one chapter active. Call `Configure` with the **existing** session, player, camera and Canvas RectTransform. Existing EventSystem/input module required. `Refresh()` can resume a suspended configured module.
2. Place the six `activityAnchors` in walkable scene positions: room desk, door/travel, bed/alarm, WC, H6 desk, lab/courtyard. E or HUD touch button opens only within 2.2 units. `AnchorIndex` maps current event to the relevant station. These are interaction stations; builder does **not** create complete physical H6/WC/travel routes or call the existing commute service. Shared destination/camera transitions must be connected by composition if continuous free movement between locations is required.
3. Align `roomSlots[0..2]` to Long/Tộ/Đạm's actual bed slots for either player-bed choice. Their indices are semantic roommate slots, not absolute authored bed numbers. Hide shared old-room actors via composition while this staging is shown; module never manipulates another owner's renderers. Keep player slot separate.
4. Module renders room slots from `room:*` facts; each old friend leaves, that slot briefly becomes empty, then that freshman enters. Yard uses three graduation sprites. M34 visitors use separate renderers and never overwrite freshman belongings. On transition to Year6, composition/next module must continue reconstructing all three freshman slots from the same facts.
5. Builder resolves existing Sprite assets under `Assets/Sprites/MVP/remaining`, preserves background aspect and does not alter PNGs. Imported sprite sizing, contact, foreground/bed alignment and sorting require scene review. All text is native runtime UI; no baked image text.

## Interaction coverage

- M24: take correct book, select HCMUS, deliver to Đạm, listen to three research points, return.
- M25: three CV formatting edits; PDF export and filename; recipient/attachment/send; later job email. These are native in-game document/mail cards, not actual filesystem PDF export or external email.
- M26–27: work schedules and notice; single durable application event; rejection 118/136 with missing 18 and CTDL→HĐH; GT2 explicitly already passed. Repeated application rejected by current-step gate.
- M28: all three friends shown in a clipped native photo preview; pan and zoom sliders move/scale the preview and must both be within center tolerance, then focus and shutter; parent answer and return.
- M29–31: Long→Tộ→Đạm, individual goodbye/carry/gift/move-in cards; exact per-slot reconstruction; Đạm leaves dorm without flight fact.
- M32: select game/uninstall, real pointer hold for 2 seconds. Release/exit, focus loss, pause or cancel clears hold. Cancel doesn't erase the committed select event; resuming `hold_uninstall` starts from zero. Alarm requires 06 then 30 then save; recovery plan requires CTDL kỳ9 then HĐH kỳ10.
- M33: wake, WC checklist, timely attendance and three representative study days with food/rest. No repeatable money/credits loop.
- M34: door, supplies, meal, notes, visitors leave, **then** flight message; visitor renderers are separate from freshman room slots.
- M35: inorder CTDL revision and FIFO answer, CTDL pass before HĐH enroll, mutex answer for HĐH pass. Wrong answers/order do not commit. No GT2 retake.
- M36: five passed-course facts checked before 136-credit commit; only then thesis eligibility and appointment. Model supplies the authoritative credits/effects; module never copies/mutates wallet or credits.

These interactions are a reviewable native prototype. CV/checklists/dialogue/exams use authored ordered cards with distractors; box carrying is pick/place cards rather than a physics carry animation, photo framing uses sliders rather than a complete camera scene. Do not claim final animation/art/full-route acceptance from this source.

## Validation actually run

Evidence and reproducible scripts: `review/parallel/years45/`.

- `python3 review/parallel/years45/check_sources.py`: offline Mono/Roslyn compile of owned runtime and editor sources against existing Unity/Assembly-CSharp reference DLLs; **exit 0 / no diagnostics**. Does not start Unity or import/recompile the project. Existing editor assembly was read as a reference only.
- Static exact coverage: **54/54 current definition events**, no missing/extra contracts.
- `python3 review/parallel/years45/check_model.py`: actual current production definition/progress + owned activities compiled into standalone host, native JSON calls bypassed by generated object initialization; **1,122 assertions passed**. Verifies wrong/future/duplicate events, authored-input bounds and invalid action, checkpoint replay after every owned event, independent freshman slot restoration, no early flight/eligibility, GT2 retained, CTDL-before-HĐH, endpoint M37 / 136 credits / not Finished.
- Hashes recorded in `source-report.json`; compiler output `source-compile.txt`, `editor-compile.txt`; host result `model-checks.txt`.

## Coordinator QA pending

Unity project compilation; opt-in editor check; actual M24 activation with injected session; keyboard/touch reachability at all anchors; camera sliders; hold release/pause/cancel/reload; save failure recovery; input lock ownership across scene/module unload and reentrant `changed` callback; both player-bed layouts; three newcomers retained through M34 and onward; all route/location transitions; Vietnamese font and safe-area/readability; actor scale/contact/motion/audio. No shared-contract changes are required for this prototype; physical travel and final room presentation need coordinator composition wiring.
