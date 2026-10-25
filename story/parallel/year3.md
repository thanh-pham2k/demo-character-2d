# Year3 M16–M23 handoff

Source stable for coordinator review; **not Unity compiled or Play verified**. No refresh/Play/Temp/ProjectSettings/scene edits/imagegen. Only Year3 owned source/meta, this handoff and review/parallel/year3 evidence were written.

## Files and integration

- `mobile-2d/Assets/Scripts/ProductionChapters/Year3/Year3ChapterModule.cs`: `ProductionChapters.Year3.Year3ChapterModule`, implements injected `IProductionChapterModule` and `IInteractable`.
- `Year3Activities.cs`: 37 scratch activity sequences; mother choice is handled separately (38 authored events total).
- `mobile-2d/Assets/Editor/ProductionChapters/Year3/Year3ModuleBuilder.cs`: explicit menu `Tools/Production Chapters/Year3/Create selected module`; no import hooks. Binds existing C02/P02/P03/E01/E02 sprites. Menu `Check definition coverage` checks current JSON against adapters.
- Composition: create the component with builder or serialize its sprite fields, call `Configure(existingProductionChapterContext)` when entering M16. Call `Refresh` on shared progression and `Suspend` before switching away. Supply WorldRoot, UI root, existing Player and Camera. Enable only this chapter during M16–23. Keyboard E opens the current activity; existing touch interaction can invoke `Interact()`. No new session, wallet or save is created.

## Implemented behavior

- M16 honest/help choices have distinct maternal responses, then converge through ending call and receiving job message. Ask-help does not invent a financial grant outside model.
- M17 consent → simulated backup/checksum → UEFI USB → preserve data/install/drivers/test → customer acceptance. Reward only through the authored 50k handover event.
- M18 service sign placement sequence and job/class schedule.
- M19 two representative jobs: network diagnosis and safe cleaning, consent/backup/testing/handover. Each rewards 50k through its own event once. Rest before next job. Four-week ledger reconciles 1.2m net separately from three representative jobs; 900k fee from shared wallet. KTLT/VL1 quizzes and KTLT→CTDL→HĐH explanation/gates follow model.
- M20 consent/backup, scan/quarantine, restore/test/return, waive labor only, cafe message.
- M21 borrow jacket, cafe/boyfriend dialogue, full second-machine checklist, return jacket, friends invite.
- M22 four C02 seated friends, tea/food sharing and untimed conversation, home/sleep.
- M23 portal GT2 pass, CTDL fail/HĐH locked, 78 credits and lab message.
- Activity UI presents uncompleted tasks in reverse order plus unsafe distractors where relevant. Clicking out of order shows retry feedback without event; correct order accumulates only scratch state. Final save is gated by the full sequence. Cancel/Escape/disable/unload discards scratch state and releases only this module's player lock. Camera returns after cafe/lake staging. Reload resumes model's current step, redoing only its incomplete local sequence. Failed save leaves safe retry UI and does not grant local money.

## Verification and limits

`python3 review/parallel/year3/check_source.py` passes: all 38 current event IDs covered, no duplicate activity IDs. Definition projection M01–23 yields 78 credits, GT2 pass, CTDL fail, jacket returned; sample first-choice route money 479k. Projection is Python validation of authored contracts, not execution of C# model or UI. Source hashes in `review/parallel/year3/source-check.json`.

Coordinator must compile and Play before acceptance: both mother branches, all unsafe/reordered choices, cancel/reopen each step, disk failure retry, save/load, suspend while open, native UI safe area/font/input, reward duplication attempts, prerequisites and 78-credit terminal state. No host C# compiler was available via PATH.

Native staging is a first implementation: sprite background/camera and props exist under transient owned root; player stays locked during activity rather than walking a full cafe/lake route. Final visual contact, actor spawn/walk bounds/foreground occlusion, repair hand alignment, service-sign runtime world lettering and device layout still need composition/art pass. No claim of final motion or full route acceptance. Shared composition wiring and mobile interaction affordance remain coordinator-owned; shared contracts need no changes for current implementation.
