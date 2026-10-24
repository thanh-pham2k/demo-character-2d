# Interaction foundation audit

## Inspected baseline

The active room is `Assets/Scenes/Room404_MVP.unity`. Its player is a scene object, not a prefab. The only prefab under `Assets/Prefabs` is `SlashVFX.prefab`.

```text
Player_Luon (Default layer)
  Rigidbody2D: Dynamic, gravity 0, rotation frozen
  CapsuleCollider2D: size (0.5, 0.7), offset (0, -0.3)
  PlayerMovement
  PlayerInteractionDetector
  ContactShadow: SpriteRenderer
  Visual: SpriteRenderer + LuonCharacterVisual
```

- `PlayerMovement` actually reads keyboard, `VirtualJoystick.Instance.InputVector`, and `SetSimulatedInput`; it assigns `Rigidbody2D.linearVelocity` in `FixedUpdate` and exposes `TryDodge`, `IsDodging` and `LastMoveDirection`. It had no interaction movement lock.
- `LuonCharacterVisual` uses the parent rigidbody velocity for walk animation. Its original `SetSitting(bool)` only changed a visual flag/sprite and did not stop movement. There is a sitting sprite but no lying sprite/API.
- `MVPFlowController.AwakeningSequenceRoutine` originally teleported the player to `Desk_Seat_Marker` and called `SetSitting(true)`. `OnUninstallCompleted` called `SetSitting(false)`. Its `Update` handles the story uninstall minigame, not furniture interaction.
- `Desk_Seat_Marker` is a standalone transform at `(-0.95, -0.19, 0)`. It represents the player root pose, not a separate feet pivot. No chair seat/exit markers or bed lie/exit markers exist. The chairs are pixels in `study_desk_chairs.png`, not separate interactable scene objects.
- `Room_Door` has `HingedRoomDoor` and a non-trigger floor collider. Its root is the right-hand hinge at `(2.615, -0.095, 0)`; `Door_Leaf` is a child. There is no independent door player pose marker.
- The left bunk has `InteractableProp` for the existing claim-bed story event, not sleeping. The right bunk has a collider but no interaction. Trash, laptop and light switch use `InteractableProp` with story-controlled availability and UnityEvents. There were exactly two `IInteractable` implementers: `InteractableProp` and `HingedRoomDoor`.
- All these interaction colliders are on `Default`. Bed/door colliders are physical; trash/laptop/light-switch colliders are triggers. Physics2D trigger queries are enabled. No named Interaction layer exists.
- The detector's saved radius is 2. Its saved layer mask is 0 and was unused. `promptCanvas` and `promptText` are unassigned; objective/story UI provides the existing guidance. The detector previously hard-cast to `InteractableProp` to inspect availability.
- Movement originally picked the first child SpriteRenderer, which could be `ContactShadow`. It now prefers the renderer on the actual `LuonCharacterVisual`, with the generic renderer lookup retained as fallback.

## Minimal architecture

```text
PlayerInteractionDetector
  -> eligible nearest IInteractable
  -> CanInteract + Interact()
  -> the object owns physical behaviour / existing UnityEvent

PlayerInteractionStateController
  -> Standing | Sitting | Lying
  -> movement/dodge lock, temporary physics suspension, visual pose

MVPFlowController
  -> story progression and story-time pose requests through the controller
```

`IInteractable` retains the existing parameterless `Interact()`, `InteractionPrompt` and `GetTransform()`. A common `CanInteract` property replaces the detector's concrete prop check. `InteractionTargetUtility` rejects null, destroyed Unity objects, disabled/inactive components, missing transforms and unavailable objects.

`InteractableProp.onInteract` now has a `UnityEvent` initializer. A runtime-created prop previously had a null event, unlike the serialized scene instances; the initial Play Mode probe exposed this case and the final run verifies it is fixed.

The detector honors explicit collider layer masks. Legacy mask 0 uses `Physics2D.DefaultRaycastLayers`, preserving the existing scene instead of making every target unreachable. Collider queries keep the existing root-transform distance ranking and support parent interaction components. `TryInteract()` is the single dispatch entry point for keyboard or a future UI button.

The state controller has no story stages or singleton dependency:

- `TryEnterState(state, poseMarker = null, owner = null)` accepts Sitting or Lying only from Standing. Ordinary interaction is blocked while dodging or while posed.
- A single enum prevents Sitting and Lying at the same time. Movement velocity/input/dodge are cleared on lock and exit.
- A supplied marker places the player root and records its entry position. Physics simulation is temporarily suspended so furniture colliders cannot push the posed player away. Exit restores the previous simulation setting, not an assumed default.
- `ExitInteraction(exitMarker = null)` restores Standing and either the supplied exit marker or the recorded entry point. It never enables an externally disabled movement component.
- An optional active owner remains the sole eligible target while posed, even if its occupied collider is disabled or the pose marker moves the player outside its ordinary detection range. This lets a future world component handle its own exit through the same `Interact()` contract. No chair/bed/door type checks are needed.
- Losing/disabling an owner or disabling the state controller releases the pose and movement lock.

Existing saved players obtain the controller at runtime through detector/flow binding; the current scene does not need to be rewritten. The latest builder explicitly adds it for future builds.

`LuonCharacterVisual.SetSitting(bool)` remains callable and delegates to the controller when one exists. Standalone visual use still works. Lying currently freezes on the idle fallback; it does not introduce a sleeping behaviour, sprite or animation.

`MVPFlowController` keeps its current story events, UI and stage transitions. It requests the seated pose at the awakening beat and releases its own request on completion/disable; positioning and movement control no longer live in its story coroutine.

## Files

Modified:

- `Assets/Scripts/IInteractable.cs`
- `Assets/Scripts/PlayerInteractionDetector.cs`
- `Assets/Scripts/InteractableProp.cs`
- `Assets/Scripts/HingedRoomDoor.cs` — availability guard only; hinge/swing behaviour retained
- `Assets/Scripts/PlayerMovement.cs`
- `Assets/Scripts/LuonCharacterVisual.cs`
- `Assets/Scripts/MVPFlowController.cs`
- `Assets/Editor/MVPSceneBuilderFinal.cs` — explicitly add the controller in future builds

Added:

- `Assets/Scripts/PlayerInteractionState.cs`
- `Assets/Scripts/PlayerInteractionStateController.cs`
- `Assets/Editor/InteractionFoundationChecks.cs`
- This audit document

Unchanged: saved scenes/prefabs, markers, colliders/layers, textures/sprites/materials/shaders, project settings, player controls while Standing, the public `SetSitting(bool)` compatibility entry point, story event names and stage progression, Door animation, and the single existing KTX404 build menu entry. No Chair/Bed component is implemented.

## Verification

The opt-in checks enter fresh Play Mode in the current room, create transient probe objects, and leave Play Mode afterward. They never build or save a scene. No new menu entry is added.

To request a run, create `Temp/InteractionFoundationChecks.run` and recompile `InteractionFoundationChecks.cs`. The report is written to `Temp/InteractionFoundationChecks/report.json` (ignored). Checks cover actual movement/dodge, state exclusion, owner exit/lifetime, null references, parent colliders, layer filtering including Ignore Raycast, old prop/Door dispatch, legacy visual calls and the existing claim-bed/cleanup/story-seating flow. The report records executed checks and runtime errors.

The story script still expects its existing required UI references; this task does not redesign those references. Future Chair/Bed implementations should own their behaviour/markers and request this state API, without adding furniture branches to the detector or story Update.

Verified on 2026-10-24 in Unity 6000.6.7f1: compilation passed, **45 Play Mode checks passed with 0 runtime errors**. Normal movement/dodge, pose locks/exits, null/disabled/destroyed targets, explicit and legacy masks, existing prop/Door interactions and existing story seating/completion passed. A hash comparison found **zero changes to pre-existing non-code assets or project settings**; only source files, new script import metadata and this audit document were added/changed.
