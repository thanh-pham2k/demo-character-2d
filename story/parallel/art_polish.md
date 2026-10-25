# Art polish handoff — 25/10/2026

Source stable; coordinator integration required. No Unity refresh, compile, Play, scene save, ProjectSettings, Temp request, imagegen or primary PNG modification performed. No shared model/store/context/player/opening edits. Baseline 45 missions retained; this is not a new runtime pass.

## Files and entrypoints

- `mobile-2d/Assets/Editor/ProductionArtPolish/ProductionArtPolishBuilder.cs`: opt-in asset builder + native registration/contact helpers.
- `mobile-2d/Assets/AssetProduction/ParallelArt/README.md`: output ownership and coordinator instructions.
- `review/parallel/art_polish/source-audit.json`: inspected source hashes, room dimensions and outline. The two contact sheets are existing previews, not newly generated batches.

## Observed findings and status

| Target | Observed from inspected image | Static result / runtime gate |
|---|---|---|
| M10 room matte | 1037×627 room texture has outside upper corner triangles; whole texture tint retains a bright rectangle against dark camera clear | Native silhouette mesh builder implemented; generated asset and night/day visual acceptance pending coordinator |
| AN01 backpack back | Back row has frame-to-frame head/backpack height changes; feet remain on common canvas. Backpack is baked into torso, not separate rig | Pending motion judgement. Do not flatten all frames or move player root to hide bob. Offset clip helper supports measured visual-child registration only; no speculative offsets applied |
| AN05 departure | All three outfits have alternating wide contact / narrower passing silhouettes; faces and torsos remain 3/4 rather than strict profile | Static contact/passing reuse supported; strict side silhouette pending. Transform/mirror cannot invent a true profile. No squeeze applied and no new art requested |
| M08 Tộ / glass | Tộ source visibly holds bowl in left hand and chopsticks/food in right; SharedDrink is independently placed on floor by current builder | Pending. Glass placement is not hand contact or toast animation. AttachProp helper alone cannot erase bowl. Keep eating pose until a separate, inspected neutral-hand candidate is approved |

## Concrete coordinator integration patch

Run `ProductionArtPolishBuilder.BuildRoomSilhouette()` explicitly once, then in shared room builder immediately after cloning `Room_Backdrop`:

```csharp
if (sourceName == "Room_Backdrop")
    clone.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(
        ProductionArtPolish.ProductionArtPolishBuilder.Output);
```

Do not change clone scale/rotation/position: mesh copies original normalized pivot and PPU. Mesh outline uses bottom-left pixels `(0,0) (1037,0) (1037,415) (891,627) (195,627) (0,415)`, triangulated fan, same texture UVs. Original exterior white is discarded geometrically; indoor window remains daylight and needs a separately staged native night treatment. This patch removes outside matte only, not window lighting. Check actual cloned sprite uses the inspected source before replacing; if source differs, stop and measure its outline instead.

AN01 helper `CreateVisualOffsetClip(name, "Visual", fourOffsets, fps)` returns an unsaved looping transform clip with stepped offsets and closing key. It must be merged with the existing sprite-frame track by coordinator; playing it as an exclusive animation would suppress frame animation. Offsets are local units on a dedicated visual child; root collider, feet pivot and motion must remain unchanged. Do not use until measured frame anchors are available.

`AttachProp(handAnchor, glassSprite, gripPixels, uniformScale, sortingOrder)` places a prop by `(gripPixels - sprite.pivot)/PPU` from bottom-left pixels; caller owns hand anchor, facing, sorting layer, visibility, cleanup and action timing. No approved Tộ hand anchor is claimed: baked bowl blocks approval. Current floor glass stays valid as staged prop only.

## Validation / remaining queue

Read current missions.json (45 missions), ownership contract, image rules, flow baseline, remaining inventory, registration review and shared builders. Visually inspected AN01/AN05 existing preview sheets, Tộ floor PNG and room PNG. Audit includes source hashes. Source inspection confirms no automatic editor callback; no scene or runtime ownership changes.

Coordinator must compile this editor source, build derived sprite, verify asset persistence after reload, confirm source pivot/PPU retained, compare day/night corners at real camera including both claimed beds, and check window treatment. Then check AN01 four-frame loop at runtime speed/feet registration, AN05 departure behind luggage/occluders, and M08 eating versus toast contact. No Unity compile or Play result claimed. No screenshots of an unrun fix supplied as evidence.
