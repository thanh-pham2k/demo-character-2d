# Year2 M10–M15 handoff

Source stable for coordinator review; **Unity compile / isolated Unity checks / Play Mode have NOT run**. No editor refresh, Play, Temp request, shared scene, definition, model, ProjectSettings, imagegen or cross-chat message was used.

## Files and entrypoint

- `mobile-2d/Assets/Scripts/ProductionChapters/Year2/Year2Module.cs`: `IProductionChapterModule`, Configure(shared ProductionChapterContext), Refresh, Suspend, SetPaused; only injected Session used.
- `Year2WorldAction.cs`: native IPlayerInteractable stations with actor identity, CanInteract and 1.8-unit proximity gates.
- `Year2Challenge.cs`: deterministic transient challenge state and knowledge questions.
- `mobile-2d/Assets/Editor/ProductionChapters/Year2/Year2Tools.cs`: explicit menu builder and isolated challenge checks; no initialize-on-load/import hooks.
- Matching .meta files; source audit: `review/parallel/year2/source-audit.json` (21/21 explicit step cases, injected session, hashes; source audit is not compilation).

Coordinator: select a dedicated chapter root, invoke Tools/Production/Year2/Add module to selected root (opt-in). Place Laptop/Giường/Cửa/Ổ điện/Tủ anchors at the actual room props and H6 at classroom entrance. Assign bedPose, deskPose, pluggedCooker; replace/place the builder's sample pot/trash/guard sprites to align room scale and foreground. Builder reuses existing pot_0, trash_piles and guard_inspecting; no new art. Inject the SAME production context and activate only for M10–M15. Suspend old module first; call SetPaused(true/false) from pause menu and Suspend on unload. Parent composition/root is deliberately untouched.

## Event coverage

- M10 choose_night commits play/sleep; end_night requires 3-second night presentation and uses saved branch. YES gives 25 energy/trash, NO gives 85 energy/early sleep. MorningEnergy and MorningAttendanceMissed expose derived consequences without a second wallet/save.
- M11 choose_alarm wake/snooze; show_morning_consequence has separate 06:30/on-time vs 06:50/missed attendance text, requires 2 seconds, converges to another evening. No exam F from alarm.
- M12 hear_guard starts 10-second active gameplay clock; world stations enforce unplug_cooker → hide_pot → pretend_sleep; persisted step determines pot/plug state. Timeout blocks actions, gives warning/retry with 10 seconds from current durable checkpoint. resolve_inspection waits 2 seconds. Retry never rewinds durable campaign events.
- M13 compile_assignment executes authored simulation and shows ASan overflow log; find_error requires selecting loop; patch_assignment requires i<n, incorrect options do not commit; test_assignment runs authored empty/single/three-element/boundary cases; submit_assignment gated before 23:59:59. Clock starts 23:40 and advances gameplay seconds (1199-second window), no baked clock. Late attempt has explicit retry at durable checkpoint. This is a game simulation, not execution of arbitrary C++ or a real toolchain.
- M14 arrive_exam at H6; answer_known_sections asks 5 actual questions, grades answers 0–100% with no forced20%; then submit_exam. If reloaded at submit without transient score, questions must be done again before submitting. Grade F later is authored semester outcome, explicitly distinguished from this practice score.
- M15 portal → GT2/VL1/KTLT F → KTLT→CTDL→HĐH locks → invoice/call bridge. Credits/effects only come from existing definition commits. Invoice view does not invent a charge/price.

## Lifecycle and persistence

UI acquires player state with the station as owner and releases only its own state. Close/cancel returns renderer/physics via PlayerInteractionStateController; Suspend/disable/destroy close UI. Pause stops clocks and closes owned interaction. Durable commit failure reports error and retains current step. Full reload reconstructs safe checkpoint from injected campaign; transient exam/clock state starts a fresh attempt, never copies campaign/wallet. No auto-complete, generic CompleteMission, or out-of-order step dispatch.

## Validation pending / shared contract limitations

Coordinator must compile, run opt-in isolated checks, wire real scene anchors/poses/plug, test both night and alarm branches, timeout/retry, failed save, partial-step reload, pause/unload ownership, C++ late retry, all-correct/all-wrong exam, portal → M16 and mobile layout. Current builder is an opt-in staging aid, not a validated scene layout. UI uses native Unity Text/Button; Vietnamese font/touch/safe-area need device review.

Shared event-only contract cannot durably store arbitrary minigame elapsed/score. Current policy is explicit safe-attempt retry on full reload. To preserve exact stealth seconds/assignment clock/exam answers across app restart, coordinator needs an owned session-prefix sidecar API; this task did not modify shared stores. Runtime pause/suspend in the same instance retains those values. UI presents time-lapse and branch consequences; richer existing actor sheets and sound cues remain coordinator staging work. No completed scene/full-route claim.
