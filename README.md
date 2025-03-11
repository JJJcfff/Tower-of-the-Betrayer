# Tower of the Betrayer

## Team Members

- Jeff Cui: https://github.com/JJJcfff
- Elaine Zhao: https://github.com/ElaineZhao1210

## Game Summary

Tower of the Betrayer is a roguelike strategy game where players assume the role of a warrior fighting to liberate their enslaved village from a tyrannical devil. The devil challenges adventurers to conquer a 10-floor monster tower, promising freedom in exchange. Players battle through procedurally generated floors, each with escalating enemy threats and randomized modifiers, while strategically upgrading weapons and crafting items. Upon reaching the final floor, the devil betrays the player, triggering a boss battle. Survivors can test their limits in Endless Mode, where delaying the final fight increases the boss’s strength.

## Genres

- Roguelike
- Strategy
- Crafting
- Action RPG

## Inspiration

### [Brotato]

Brotato’s fast-paced runs, auto-attack combat, and build diversity inspired our approach to accessible yet strategic gameplay. Its use of randomized items and character-specific traits influenced our gem and modifier systems. The "survive waves, upgrade between rounds" loop directly shaped our floor progression and material-driven upgrades.

<img width="982" alt="image" src="https://github.com/user-attachments/assets/3d29c658-0748-4dae-9bfb-05eacb799b77" />

https://store.steampowered.com/app/1942280/Brotato/

### [Low Poly Style Games]

The visual design of our game draws inspiration from low poly art styles, which are both visually appealing and efficient for development. The low poly aesthetic allows for a focus on clean, stylized environments and characters, enhancing the fantasy theme of the game while maintaining performance. Examples include games like "Totally Accurate Battle Simulator" and "For The King," which feature vibrant, minimalist designs that effectively convey their worlds.

![image](https://github.com/user-attachments/assets/8211249f-b3e3-4706-8cfd-44ef93f8ba19)

https://store.steampowered.com/app/508440/Totally_Accurate_Battle_Simulator/

## Gameplay

In this game, players take on the role of the most skilled warrior from a village that has been enslaved by a devil. Bored with his control over the humans, the devil constructs a 10-floor monster tower and declares that if any adventurer can conquer it, he will grant the village its freedom. As the village’s last hope, the player fights through the tower, growing stronger with each battle. However, upon reaching the final floor, the devil refuses to honor his promise, forcing the player into a final showdown against him.

**Core Mechanics**

- Weapon Choice: Players start with two weapons—a sword and a staff. At the beginning of each floor, they must choose between melee (sword) or ranged (staff) combat.
- Gem System: Each weapon has a limited number of gem slots that can be used for enhancements. Gems can be inserted, removed, and transferred between weapons by spending materials.
- Material Collection: After clearing each floor, players receive materials that can be used to strengthen their character or upgrade weapons.
- Item Crafting: Materials come in various types, some of which can be used to synthesize upgrade gems for weapons, while others can be crafted into potions that enhance abilities or restore health during battle.
- Combat System: Players control movement and consumable usage, while attacks are automated, targeting the nearest enemy.

**Enemy Variations & Modifiers**

- Each floor features different enemy types with randomized modifiers (e.g., increased damage, movement speed boosts).
- Each floor can have 0-3 random modifiers, which are hinted at before the battle, allowing players to prepare strategically.

**Progression & Challenge**

- Players must defeat all enemies on a floor to proceed.
- If the player dies, all upgrades are lost, and they must start over from the beginning.

**Endless Mode & Final Battle**

- After clearing all 10 floors, players can choose to enter Endless Mode, where they continue fighting increasingly difficult enemies.
- At the end of each additional floor in Endless Mode, players have the option to initiate the final boss fight.
- However, the longer they wait, the more difficult the final boss battle becomes.

## Development Plan

### Project Checkpoint 1-2: Basic Mechanics and Scripting (Ch 5-9)

- ~~Implement basic movement of the character and the enemies~~
  - ~~random generation of enemies~~
  - ~~movement of the enemies towards the character~~
  - ~~movement of the character controlled by WASD~~
- ~~Weapon logic~~
  - ~~fire animation~~
  - ~~towards the closest enemy~~
- ~~Basic layouts~~
  - ~~Basic backgrounds~~
  - ~~Health bar and floor display~~

### Project Checkpoint 2: 3D Scenes and Models (Ch 3+4, 10)

- ~~Enemy~~
  - ~~Introduce more enemy variants (different movement speed, max health, shooting speed etc.)~~
  - ~~Implement different modes of random enemy generation
- ~~Weapon logic~~
  - ~~Implement player's second weapon~~
  - ~~Allow player to select weapon before game~~
- ~~Home Scene~~
  - ~~Create a basic scene representing player's home~~
  - ~~Transition from battle field back to home after a game~~

### Project Checkpoint 3: Visual Effects (Ch 11, 12, 13)

- Home Scene
  - Replace weapon selection checkbox with Player Object Movement and Action
  - Build Simple Home Scene
- Battle Field
  - Change battle field ground and wall effects
  - Add collision sound / effects
- Weapon
  - Replace sward
- Scene Transition
  - Improve transition between scenes that are currently done by minimal buttons

## Development

### Project Checkpoint 1-2:

Our deliverables for this iteration consists of a basic game in which the player can move around using WASD and the closest enemy within attack range takes damage automatically, and the enemies are randomly generated and tend to move towards the player. If the player survives with given health level after destroying all enemies, the player wins. If the player dies before clearing the field, the player loses.
<img width="600" alt="Game Screen Example" src="https://github.com/user-attachments/assets/1968ff43-fdbd-40be-80c7-cdb87f628d53" />

<img width="300" alt="Win Screen" src="https://github.com/user-attachments/assets/01c5aec9-ae3b-4da5-b4a7-eb0072b1b424" /> <img width="300" alt="Lose Screen" src="https://github.com/user-attachments/assets/800c7112-b3a6-4c01-a118-03498778c802" />

### Project Checkpoint 2:

Our deliverables for Checkpoint 2 includes a Home Scene, Game Scene, and a Lose/Win Scene which are now connected through button navigations. We added a melee weapon and now the player can play with either or both weapons, the enemy types and generations are also diversified. The main objects (player and enemies) are replaced with 3D models, and the bullet sizes and game parameters are adjusted based on last iteration feedback.

The model reference:

- https://poly.pizza/bundle/Toon-Shooter-Game-Kit-qraiSXoAru
- https://poly.pizza/m/TX8r9WBXpe
<img width="800" alt="Start Screen" src="https://github.com/user-attachments/assets/627004cf-7473-4868-9c1a-1cf9c984b9b8" />
<img width="800" alt="Game Screen" src="https://github.com/user-attachments/assets/39c051ff-3777-47ea-9a8a-d374d6cfc0e9" />

## How to Play

1. Load the **Home** scene to start the game play
2. You can check/uncheck staff or sword to select 1 or 2 weapons together to play in the game
3. Click "Start Game" to start once you finalize weapon selection
4. During the game, use keyboard up/down/left/right or WASD to move the player object. Sword is a melee weapon (shown as a yellow object) and Staff is a ranged weapon, and both are automatically handled by the game. The player's bullets are blue and the enemy bullets are in orange.
5. After win or lose, hit "Return Home" to go back to Home Screen.
