# Room 404 commute review — 2026-10-25

## Implemented behavior

- The wall hook beside the door is available before the story bed claim. E
  equips the existing backpack sprite set and leaves an empty hook. Leaving
  without it shows the requested Vietnamese reminder.
- Both chairs reuse the existing interaction state and desk menu. Study loops
  three pen-writing frames, plays paper audio and advances StudyProgress.
  Computer loops two typing frames, plays typing audio, sets laptop intensity
  to 1.2 and shows a blue screen overlay/reflection tint. Standing stops all
  activity animation/audio and restores movement.
- Room → hallway → WC → stairs → campus sidewalk → bus stop is playable.
  Stairs fade with footsteps; campus movement and camera following remain
  controllable. The lobby also permits returning upstairs.
- Bus 33 approaches and parks. E boards only after arrival, charges exactly
  7,000 VND once (35,000 → 28,000), hides the boarded actor, then drives away.
  The completion card displays fare and balance. Its return button restores
  the room, actor visibility, physics and control.

## Architecture and integration

Created runtime components: CampusCommuteController, BusStopInteractable,
CampusLobbyInteractable. Created editor integration: CommuteSceneSetup.
Updated DormJourneyController, BackpackInteractable, LuonCharacterVisual,
DeskActionModal, DormSound, DormStairsInteractable, DormJourneySceneSetup,
RoomInteractionSceneSetup and PlayModeInteractionSuite. Room404_MVP stores all
references, generated sprites, native desk layer crops and new campus objects.

GameObjects include Backpack_Hook_Prop/Hook_Visual, player Desk_UpperBody,
Laptop_Prop/Activity_Screen_Glow, Campus_Zone with arrival/return/boarding
markers, Bus_Stop_33, Bus_33, and Canvas_DormJourney/Commute_Completed.
No manual Inspector assignment or missing asset remains for this flow.
The generic detector and MVPFlowController progression are preserved.
KTX404 retains the single Build Room 404 Scene command; setup reuses named
objects/markers. Audio is procedural. Asset provenance is in
assets/mvp_sprites/commute/GENERATION.md.

## Verification

Actual Play Mode report finished 2026-10-25T05:59:39.6596250Z:
**142 passed, 0 errors**. It covers both chairs and beds, door sweep/blocker,
movement/physics locks, mutually exclusive interaction states, generated
activity frames changing over time, audio, light/glow, backpack gate, WC,
stairs return/re-entry, sidewalk walking, bus arrival, duplicate-fare rejection,
departure, completion and room restoration. No runtime Error/Exception/Assert.
MVPSceneTester passes all four story stages plus ending without forcing stages.
A subsequent fresh Play session starts with zero Console errors/warnings.
Source whitespace check passes. Curated screenshots and reports are retained
in assets/mvp_sprites/commute/review/; study, typing, bus-stop and completion
captures were visually inspected.

## Scope boundary / future work

This goal ends at the requested bus departure/completion notification. H6
classroom, teacher/Đạm dialogue and Mission 06 logic puzzle remain future work.
Mission 07 deadlines/missed bus and persistent academic/economy saves also
remain future work. Current balance and StudyProgress are session-local.
The bus-stop's generic bottom prompt has lower contrast than the arrival
notification; a future UI polish can unify them. Outdoor character proportions
follow the existing chibi art convention rather than realistic bus scale.
