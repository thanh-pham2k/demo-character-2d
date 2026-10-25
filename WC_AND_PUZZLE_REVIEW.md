# WC Nam and easier classroom puzzle

WC Nam now opens its existing door artwork, moves the locked player inside,
closes the door and displays a read-only progress bar for three seconds. The
player is hidden while inside, water audio plays, then the door opens again,
the player exits to ExitPivot and movement/physics restore. Door closes and
progress hides. Repeated E cannot start another visit. Cancellation restores
visibility, door and progress UI. WC Nu refusal is unchanged.

The classroom warm-up now asks 1 + 2 + ... + 10 with choices 45 / 55 / 65.
Answer 55 uses five pairs of 11. Hint, wrong-answer feedback, timeout text,
teacher explanation and Dam reaction were updated consistently. The existing
20-second timer and retry/keyboard/button controls remain.

Changed: RestroomInteractable, DormJourneyController, DormJourneySceneSetup,
CalculusPuzzleModal, YearOneMilestoneController, YearOneSceneSetup,
PlayModeInteractionSuite, Room404_MVP. Existing artwork is reused; the progress
bar is native Unity UI. No new image generation or Inspector assignment needed.

Validation: final actual Play Mode regression **164 passed / 0 errors**, UTC
2026-10-25T08:01:24.2250270Z. MVPSceneTester later stages and ending also pass.
Screenshots visually reviewed for aligned opening, closed-door progress and easy
question. Runtime report/captures: assets/reviews/wc_easy_puzzle/. Source
whitespace check passes. Scene is saved and the runner returns to Edit Mode.
