<img width="400" height="380" alt="Githubskypogo" src="https://github.com/user-attachments/assets/7c577007-60f4-4487-a6ab-bc46ff6355ea" />

Endless 3D Jumper in Godot Mono implemented in C#

## Gameplay

SkyPogo is a "pogo-stick" style arcade game. Your character automatically bounces on every platform she lands on — your job is to steer her toward the next one.

- **Move** — press `Up` to push forward in the direction you're facing
- **Rotate** — press `Left` / `Right` to spin and aim for the next platform
- **Climb** — each platform you land on triggers a new one to spawn above and to the side
- **Stay alive** — platforms vanish shortly after you land on them. Miss your next landing and you fall

The higher you climb, the better your score. Your best height is saved locally.

## Controls

| Key | Action |
|-----|--------|
| `↑` | Move forward |
| `←` | Rotate left |
| `→` | Rotate right |
| `R` | Restart (after game over) |

## Project Structure

```
SkyPogo/
├── assets/               # 3D models, textures, fonts, audio, skybox
│   ├── audio/            # Sound effects and background music
│   ├── fonts/            # Titan One typeface
│   └── models/
│       ├── platforms/    # Platform model variants
│       └── player/       # Character model (Kenney Mini Characters)
├── globals/              # Autoload singletons
│   ├── SignalHub.cs      # Central event bus (signals)
│   └── ScoreManager.cs   # High score tracking
├── resources/            # Shared Godot resources (themes, etc.)
├── scenes/
│   ├── game/             # Main Game scene
│   ├── GameUI/           # HUD, score display, game-over overlay
│   ├── platform/         # Platform prefabs and logic
│   ├── player/           # Player character and physics
│   ├── playerCam/        # Following camera
│   └── spawner/          # Procedural platform spawner
├── icon.svg              # App icon
└── project.godot         # Godot project config
```

## Architecture

The game is built around a lightweight **signal-based architecture**:

- **`SignalHub`** (autoload) — a global event bus. Components communicate through signals (`OnNewPlatform`, `OnNewHeight`, `OnGameOver`) rather than direct references.
- **`ScoreManager`** (autoload) — tracks the all-time high score.
- **`Player`** — physics-driven `CharacterBody3D` that auto-jumps on floor contact. Handles gravity, rotation, movement, animations, and fall detection.
- **`Platform`** — each platform starts a vanish timer when the player lands. On timeout, it plays a vanish animation and frees itself. It also emits `OnNewPlatform` so the spawner creates the next one.
- **`Spawner`** — listens for `OnNewPlatform` and instantiates a random platform variant at a semi-random offset from the current one.
- **`PlayerCam`** — `Camera3D` that smoothly lerps upward to follow the player's progress.
- **`GameUi`** — displays current height and best score, handles game-over overlay, and manages encrypted high-score persistence.
