# Year 1 milestone — H6 / calculus / hotpot

## Behavior

Bus 33 departure now fades to the H6 university entrance, restores the visible
player and movement, and preserves the single 7,000 VND fare. Walk to the H6
door and press E; walk to the seat beside Đạm and press E. A seated writing
pose opens lecture dialogue. The teacher calls Lươn; an actual locked movement
coroutine takes him to Blackboard_PlayerPivot.

CalculusPuzzleModal presents the integral of x⁴/[x⁴+(1−x)⁴] from 0 to 1.
Three choices, keyboard 1/2/3 or mouse buttons, twenty-second timer. Incorrect
answers or timeout show retry (R/button); they do not progress story. Correct
symmetry f(x)+f(1−x)=1 gives I=1/2 and starts teacher/Đạm/Lươn dialogue.

A fade/time skip opens the academic card: Year 1 semesters 1–2, 32/150 credits,
normal status and no debt. Space or Continue returns to Room404 at dusk. Four
separate floor-seated friends surround the electric hotpot with animated steam,
vegetables/noodles/bowls and cable. Tộ discusses the hidden stove, Long games,
Đạm warns about harder subjects, and Lươn says the required boast. Procedural
boiling and glass-clink audio play. The conversation is placed above the feast.

After the toast, the feast disappears, actor/control restore and the existing
Stage2_Year2_Temptation transition runs: 23:40, dark room, laptop light and
Long's invitation. Room artwork receives a temporary blue night tint because
the existing SpriteRenderers are unlit; tint restores after Stage 2.

## Files and architecture

New runtime: YearOneMilestoneController.cs, YearOneInteractable.cs,
CalculusPuzzleModal.cs. New editor: YearOneSceneSetup.cs.

Modified runtime: CampusCommuteController (arrival handoff and actor restore),
DormJourneyController (University area and presentation ownership), DormSound
(original procedural boiling/clink), MVPFlowController (small opt-in extended
Year1 gate and public guarded completion). Roommates' initial introduction no
longer skips directly to Year2 when the extension is configured. Legacy scenes
without the extension preserve their original transition.

Modified editor: CommuteSceneSetup calls idempotent YearOne setup;
PlayModeInteractionSuite exercises the complete route; MVPSceneTester accepts
that genuinely completed Year1 and continues the remaining original stages.
Standalone legacy simulation can still start from Stage1. It never assigns a
story stage directly.

Scene additions: YearOne_Milestone, H6_Gate_Zone, H6_Classroom_Zone,
Room404_Hotpot, Canvas_YearOne. Door/seat triggers use IPlayerInteractable and
actual Transform markers. The detector remains generic. H6 desk foreground is
a native crop of its backdrop; no duplicate room layout or extra menu entries.
All references are serialized in Room404_MVP; no manual assignments are needed.

## Assets

Ten runtime PNGs from two planned generated batches. Exact prompts, source
sheets, crop/pivot manifest and slicing script are retained in
assets/mvp_sprites/year1/. H6 gate/classroom/teacher/Đạm plus two hotpot steam
frames and four floor sprites. Existing room, walking/studying sprites and UI
are reused. PPU100, Point, Uncompressed, no mipmaps.

## Verification status

Final complete Play Mode run finished **2026-10-25T06:34:55.3936210Z**:
**160 checks passed / 0 errors**. This covers previous furniture, hallway/WC/bus
regression, H6 walking and seat locks, teacher walk-to-board, wrong answer,
twenty-second timeout and retry, correct symmetry answer and surprise emote,
academic card, four-person feast, boiling audio, actual Year2/night tint, later
story stages and ending. MVPSceneTester succeeds without forcing stage values.

The final H6 gate, seated writing, puzzle, academic card, hotpot and Year2
captures were visually reviewed; gate feet align to pavement, writing sprites
sit behind the desk foreground, feast dialogue leaves all four friends visible,
and Year2 year/time labels no longer overlap. Reports/screenshots are archived
in assets/mvp_sprites/year1/review/. New serialized controller references were
checked for null fields; ten PNG imports were checked at PPU100/Point.
A fresh Play session after the suite starts with zero Console errors/warnings;
the scene is left in Edit Mode. Unity compilation and source whitespace checks pass. No manual assignments or
missing runtime assets remain.

The authored goal ends at entry into Year2. Mission07 missed-bus scheduling,
full persistent academic systems and later Year2 missions are outside this task.

## Latest H6 visual revision

Building and classroom backdrops were regenerated as one two-panel batch from
user photographic references, with one whole-sheet format refinement. Scene
uses the v2 blue-white modern H6 building and rear-to-front classroom with
students, green chalkboard, projection screen, projector, fans and central aisle.
Camera, character scale, seat/door/board markers and desk foreground now follow
that perspective. Final 160-check regression passes with zero errors; see
assets/mvp_sprites/h6_reference_v2/review/ for latest visual evidence and reports.
