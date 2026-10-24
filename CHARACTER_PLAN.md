# Goal

Create a **small 2D action RPG character in Unity for mobile**, using the provided image as the visual scale and gameplay reference.

Focus only on the player character.

Do not build a complete game yet.

## Character Scale

The character must appear **small on the mobile screen**, similar to the reference image.

Important:

- Do not create a large character that fills the screen.
- The player should occupy roughly **7–10% of the screen height** during normal gameplay.
- Keep enough visible world around the character.
- The camera should show a relatively large surrounding area.
- Design for portrait mobile gameplay.
- Reference resolution: `1080 × 1920`.

Target visual proportion:

```text
┌─────────────────────────┐
│                         │
│                         │
│        environment      │
│                         │
│                         │
│            🧍           │
│          PLAYER         │
│                         │
│                         │
│                         │
│                         │
│                         │
└─────────────────────────┘

Player ≈ 7–10% screen height
```

Do not zoom the camera closely onto the player.

## Visual Style

Use a stylized hand-painted 2D RPG character inspired by historical Asian martial arts.

Character:

- small body;
- slightly oversized head for readability;
- simple clothing;
- strong silhouette;
- readable at small scale;
- limited tiny details;
- clear weapon silhouette.

Use a staff or sword as the initial weapon.

Do not copy an existing copyrighted character exactly. Use the reference only for scale, composition, and general gameplay feel.

## Sprite Requirements

If suitable assets are available, use sprite animation.

Otherwise, create a temporary placeholder character first.

Target sprite source size:

`128 × 128 px`

or

`256 × 256 px`

depending on available artwork.

Keep the character visually small through camera and Pixels Per Unit configuration rather than using unnecessarily low-resolution source art.

## Animation

Implement only:

- Idle
- Walk
- Attack
- Dodge
- Hit

Keep animation simple.

The character does not need dozens of frames.

Prioritize good timing and responsiveness.

Target sprite animation:

`8–15 FPS`

Game rendering:

`60 FPS`

Movement must remain smooth and independent from animation frame rate.

## Movement

Create smooth 2D movement.

Support:

- Up
- Down
- Left
- Right
- Diagonal movement

Normalize diagonal velocity.

Movement should feel immediate and responsive.

For Unity Editor testing:

```text
WASD / Arrow Keys = Move
```

For mobile:

```text
Virtual Joystick = Move
```

## Attack

Create one basic melee attack.

Example:

```text
Idle
 ↓
Wind-up
 ↓
Fast Swing
 ↓
Impact
 ↓
Follow-through
 ↓
Recovery
```

Attack duration should be approximately:

`0.3–0.5 seconds`

The attack should feel fast.

## Slash Effect

Create a large slash VFX around the small character.

This contrast is important:

```text
       ╭──────────────╮
    ╱                     ╲
   │       SLASH VFX        │
   │           🧍            │
    ╲                     ╱
       ╰──────────────╯
```

Character:

`small`

Attack effect:

`large and readable`

The slash effect may temporarily occupy roughly `2–4×` the character's visual size.

Keep its lifetime short.

Approximately:

`100–250 ms`

## Combat Feedback

When the attack connects, add subtle:

- hit flash;
- impact VFX;
- enemy knockback;
- very short hit-stop;
- small camera shake.

Do not overdo camera shake on mobile.

## Camera

Use a portrait mobile camera.

Requirements:

- player remains small;
- show a large surrounding gameplay area;
- smooth follow;
- no aggressive zoom;
- no unnecessary camera movement.

The visual composition should resemble:

```text
World
World
NPC / Enemy

        Player

World
Enemy
World

Mobile Controls
```

rather than:

```text
     PLAYER
   █████████
   █████████
   █████████
```

## Mobile Controls

Create minimal controls.

Bottom-left:

`Virtual Joystick`

Bottom-right:

`Attack`

`Dodge`

Example:

```text
┌──────────────────────────┐
│                          │
│        Enemy             │
│                          │
│             ⚔            │
│           🧍             │
│                          │
│                          │
│                          │
│   ◯               ◯  ◯   │
│ MOVE            ATK DODGE│
└──────────────────────────┘
```

Controls must not cover the player.

## Unity Structure

Create:

```text
Player
├── SpriteRenderer
├── Animator
├── Rigidbody2D
├── Collider2D
├── PlayerMovement
├── PlayerCombat
└── PlayerHealth
```

Create:

`Player.prefab`

Keep scripts modular but simple.

Do not introduce unnecessary architecture.

## Test Scene

Create one minimal test scene:

`CharacterTestScene`

Include only:

- simple ground;
- player;
- one training dummy/enemy;
- camera;
- joystick;
- Attack button;
- Dodge button.

The purpose is to evaluate:

1. character size;
2. movement;
3. animation;
4. attack timing;
5. slash VFX;
6. combat feel on a phone.

## Agent Workflow

Do not stop after generating C# code.

Use available Unity Editor tools.

Follow this loop:

```text
Create
  ↓
Compile
  ↓
Play
  ↓
Observe
  ↓
Check Console
  ↓
Fix
  ↓
Play Again
```

After each major implementation:

- save the scene;
- save assets;
- compile;
- enter Play Mode;
- inspect Console errors;
- fix errors before continuing.

## Acceptance Criteria

The prototype is complete when:

- the player looks small like the reference;
- player occupies approximately 7–10% of screen height;
- portrait mobile composition looks natural;
- movement feels smooth;
- diagonal movement has correct speed;
- Idle and Walk work;
- Attack works;
- attack has a visible slash effect;
- Dodge works;
- keyboard controls work in Unity Editor;
- mobile controls work;
- camera follows smoothly;
- game runs at a target of 60 FPS;
- Unity Console has no known errors.

## Important

Do NOT build:

- inventory;
- quests;
- dialogue;
- shops;
- multiplayer;
- backend;
- skill trees;
- procedural maps;
- complex enemy AI.

Build only the **small mobile character combat prototype**.

The first priority is:

**small character + smooth movement + satisfying attack + large readable VFX + good mobile camera scale.**
