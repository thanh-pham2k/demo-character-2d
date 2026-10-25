# Audio handoff — 25/10/2026

Source stable for coordinator review. No shared/core/scene/chapter/DormSound/mission/plan/ProjectSettings files changed. No imagegen, download, Unity refresh, Play, Temp request, or paid operation. No messages sent to other chats. No campaign session/wallet created.

## Delivered

- `mobile-2d/Assets/Scripts/ProductionAudio/ProductionAudioDirector.cs`: reusable composition-owned MonoBehaviour, 2 ambience and 2 music voices for crossfade, 4 owner loops, 8 bounded event voices. Fixed pool drops saturated one-shots instead of cutting an audible cue. Clip cache, ownership cleanup, unscaled fades, independent pause owners, background duck owners, scene profiles, settings/mute and explicit PlayerPrefs persistence. One director per composition root; no singleton or automatic scene changes.
- `ProductionAudioCatalog.cs`: Resources catalog loading and validation.
- `ProductionAudioSynthesis.cs`: reuse all 7 DormSound factories without editing them, plus original deterministic foley/ambience synthesis.
- `mobile-2d/Assets/Resources/ProductionAudio/catalog.json`: 73 cues (7 existing procedural, 55 new procedural approximations, 11 **pending BGM**, no completed music), 19 scene profiles, exact mapping for all 212 mission/step keys across 45 missions.
- `mobile-2d/Assets/Editor/ProductionAudio/ProductionAudioChecks.cs`: opt-in menu validation only; no import/startup hook. Coordinator may run Tools → Production Audio → Validate Catalog and Synthesis (explicit). Checks PCM finite/peak/RMS/duration, missing clips, exact event keys, non-loop commit cues. Writes `review/parallel/audio/unity-synthesis-report.json` only when run.
- Evidence under `review/parallel/audio`: INVENTORY.md (source/rights/duration/loop/gain/status), event-map.csv, deterministic catalog builder, host validator and host-report.json. Corresponding Unity metadata included.

## Integration API (coordinator edits shared code)

Create the director with `root.AddComponent<ProductionAudio.ProductionAudioDirector>()`; keep it alongside the injected ProductionChapterContext at the composition layer. Audio never needs a second session. Call `LoadSettings()` explicitly at boot; bind UI to copied `Settings`, then `ApplySettings(copy, true)` on user changes. No save file/model changes needed.

After **successful** `context.Commit(mission, step, choice, out error)`, call `audio.OnCommittedEvent(mission, step)` exactly once in the composition/event adapter. Do not call while replaying saved steps, repainting UI, on a failed save, or before a physical action. The method returns false for unavailable/saturated cues; audio failure must never roll back gameplay or trigger another commit. Mapping is a presentation map, not quest completion logic. Many reading/dialogue steps deliberately use a quiet shared UI confirmation; 212 mappings do not mean 212 unique sounds.

Physical continuous actions, separately from commit sounds:

```csharp
// At actual action start; idempotent when owner + cue are unchanged.
audio.StartLoop("desk", "typing"); // writing, footsteps, bus_engine, hotpot, water also available
// At physical stop/cancel/unload:
audio.ReleaseOwner("desk");
// Per contact frame rather than a generic repeating step if animation timing is available:
audio.PlayCue("step"); // stairs_step or patrol_step
// Branch and transient cues BEFORE their later commit, at real UI/interaction start:
audio.PlayCue("alarm");
audio.PlayCue("snooze"); // only on actual snooze choice
// Guard timer ticks: PlayCue("timer") per actual countdown tick, no frame polling.
```

Call `SetScene(profileId, fadeSeconds)` when destination is accepted. Returns false for an unknown profile; same profile is idempotent. Scene changes fade out all owned foley loops so an old fan/typing/bus does not leak; restore active action loops explicitly after load. Profiles: Station, Reception, RoomDay, RoomNight, Restroom, Classroom, Canteen, Lab, Street, Lake, Farewell, Recovery, Thesis, Defense, Ceremony, BinhDong, Credits, GradesSilence, PhoneSilence. These are **audio profile names**, not new Unity scenes or forced destination enums. Restroom has no perpetual water; start water only during washing. For Cafe use Canteen until a separate authored ambience is available. RoomNight is gaming context; use Farewell/Recovery/Thesis as the chapter requires.

For grade F reveal use `SetScene("GradesSilence")`; phone emotional beat may use `PhoneSilence` or `SetDuck("phone", .15f)`. On exit call `ReleaseDuck("phone")` and restore the accepted profile. Duck applies only to ambience/music, preserving UI/foley. Repeated owner updates replace a gain; multiple owners use the lowest gain. Pause with `SetPaused("menu", true/false)` and a separate owner for transition/modal. Each owner must release itself. Reserved owner `application` is used by OnApplicationPause. Fades and playheads freeze while paused, including at timeScale zero; listener-global pause is intentionally bypassed on these sources, so all production pause routes must call this director. Mute changes gain without resetting playheads. Disabling stops voices and clears scene/owners; re-enable requires restoring profile/actions. Clip cache is freed on destroy; no DontDestroyOnLoad assumption.

Before enabling this director in Production_Opening, coordinator must route/stop the existing separate DormJourneyController, CampusCommuteController, DeskActionModal and YearOneMilestoneController sources (or choose one owner at a time). Those sources are untouched and the director cannot pause/duck them. Otherwise duplicate playback and legacy pause leaks are possible. Scene/chapter code must release loop owners on cancellation as well as successful completion. No automatic observer polls or mutates campaign progress.

BGM integration: all 11 slots currently return no clip and crossfade previous music to silence. **There is no complete BGM soundtrack.** Once rights-cleared music is available, set the cue source to `resource:ProductionAudio/Music/<name>`, its actual duration/loop/gain and status `licensed_music` (record source/license in rights), then rerun checks. No placeholder melody is falsely presented as acoustic credits.

## Validation and remaining review

Host `python3 review/parallel/audio/validate_catalog.py` passed: 45 missions, exactly 212 event keys, all 9 required groups, cue references/ranges/loop semantics, existing factory/synthesis recipe references, no automatic editor/session mutation markers. `host-report.json` records the exact missions.json hash. Rebuild mapping only deliberately with `build_catalog.py`; do not overwrite later hand-tuned catalog edits casually.

**Unity compile, synthesis menu check, Play Mode and listening QA have NOT run**, per coordinator Unity queue. Host checks are data/static checks, not C# compilation or audible quality certification. No test results from earlier 819 checks are claimed for this code.

Coordinator acceptance sequence: compile → explicit synthesis validator → listen on headphones/speaker (short loops, guard distinguishability, alarm comfort, placeholder crowd/wind/applause realism) → local Play fixture for simultaneous pause owners and nested duck release, action start/cancel/reload, rapid scene crossfades, timeScale zero, app background/foreground, mute/unmute, settings restart, pool saturation, scene unload/re-enable. Verify no double legacy audio, no repeated commit sound from Refresh/load, and no stuck typing/hotpot after scene transition. Full 45-mission audio route, mobile audio latency/performance and authored music remain pending.
