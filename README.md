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

<img width="300" alt="image" src="https://github.com/user-attachments/assets/3d29c658-0748-4dae-9bfb-05eacb799b77" />

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
  - ~~Implement different modes of random enemy generation~~
- ~~Weapon logic~~
  - ~~Implement player's second weapon~~
  - ~~Allow player to select weapon before game~~
- ~~Home Scene~~
  - ~~Create a basic scene representing player's home~~
  - ~~Transition from battle field back to home after a game~~

### Project Checkpoint 3: Visual Effects (Ch 11, 12, 13)

- ~~Home Scene~~
  - ~~Replace weapon selection checkbox with Player Object Movement and Action~~
  - ~~Build Simple Home Scene~~
- ~~Battle Field~~
  - ~~Change battle field ground and wall effects~~
  - ~~Add collision sound / effects~~
- ~~Weapon~~
  - ~~Replace sword~~
- ~~Scene Transition~~
  - ~~Improve transition between scenes that are currently done by minimal buttons~~

### Project Checkpoint 3-4: Sound, UI, and Animation (Ch 14, 15, 17)

- ~~Player & Enemy Animation~~
  - ~~Implement proper movement animations for player and enemies~~
  - ~~Add audio effect at necessary points (i.e. ememy death, attacking)~~
- ~~Finalize Game Play Parameters~~
  - ~~Design the difficulty upgrade floor by floor, implement the random factor and hints~~
  - ~~Finalize the gem system (weapon upgrade and potion power) parameter setting~~
- ~~Implement Boss Fight~~
  - ~~Design boss character with special attacks and arena~~
  - ~~Create boss health visualization system~~
- ~~Other Fixes~~

  - ~~Consider adding collision detection between player and enemey, letting player take damage on these (we think without the damage works better since sword already has close attack range, this is skipped)~~
  - ~~Improve animation glitches (i.e. limit movement of player and bullets within the field)~~

  **Additions**

  - ~~Added hints in the right panel before each game~~
  - ~~Added more particle effects in the Home Scene, the fireplace and the potion pot.~~
  - ~~Slightly adjusted the light in the main battlefield.~~
  - ~~Incorporated last checkpoint feedback and enabled open/close panel in Home scene with keys G and P~~

### Project Part 4: Finishing Touches (Ch 18, 19)

- ~~Improve Win/Lose Screen (with win/lose audio tracks)~~
- ~~Adjust and test with weapon upgrading / player's upgrading parameters to the right difficulty level~~
- ~~Fix bugs~~
  - ~~endless mode~~
- ~~Build and Release~~

  **Additions**

  - ~~Incorporated feedback from last checkpoint:~~
    - ~~added sprites to the cost in Home Scene~~
    - ~~added the increase of upgrading cost as the difficulty improves~~
  - ~~added camera shake when player takes damage~~

### Final Project Submission

- ~~Try to find a better asset and enable animation of the Home Scene character~~
- ~~Add particle effects when enemy dies~~
- ~~Add sound of sword/staff shooting as necessary~~
- ~~Solve UI Display Bug (seems like the web rendered is not in correct ratio)~~

  **Additions**
  (No additional implementation)

## Development

### Project Checkpoint 1-2:

Our deliverables for this iteration consists of a basic game in which the player can move around using WASD and the closest enemy within attack range takes damage automatically, and the enemies are randomly generated and tend to move towards the player. If the player survives with given health level after destroying all enemies, the player wins. If the player dies before clearing the field, the player loses.

<img width="300" alt="Game Screen Example" src="https://github.com/user-attachments/assets/1968ff43-fdbd-40be-80c7-cdb87f628d53" />

<img width="150" alt="Win Screen" src="https://github.com/user-attachments/assets/01c5aec9-ae3b-4da5-b4a7-eb0072b1b424" /> <img width="150" alt="Lose Screen" src="https://github.com/user-attachments/assets/800c7112-b3a6-4c01-a118-03498778c802" />

### Project Checkpoint 2:

Our deliverables for Checkpoint 2 includes a Home Scene, Game Scene, and a Lose/Win Scene which are now connected through button navigations. We added a melee weapon and now the player can play with either or both weapons, the enemy types and generations are also diversified. The main objects (player and enemies) are replaced with 3D models, and the bullet sizes and game parameters are adjusted based on last iteration feedback.

The model reference:

- https://poly.pizza/bundle/Toon-Shooter-Game-Kit-qraiSXoAru
- https://poly.pizza/m/TX8r9WBXpe

  <img width="300" alt="Start Screen" src="https://github.com/user-attachments/assets/627004cf-7473-4868-9c1a-1cf9c984b9b8" />
  <img width="300" alt="Game Screen" src="https://github.com/user-attachments/assets/39c051ff-3777-47ea-9a8a-d374d6cfc0e9" />

### Project Part 3: Visual Effects

In part 3, (i) we added a **Main** scene, showing the game title and entry to start the game; (ii) we improved the **Home** scene with a 3D setting and the actual functionality of crafting weapons and purchasing potions; (iii) and we replaced the basic shapes in the **Game** scene with castle, towel, and cloud and sky effect. (iv) we also introduced background audio to our game.

We successfully implemented the continous game logic of finishing a floor -> returning to home -> preparing for battle -> next battle. Both weapons can be upgraded using available gems or degraded for refund, and the player could also buy potion of different usages with available mushrooms. Gems and mushrooms are collected (i.e. computed) during the game, reflected at the top right corner. Usage of the potions during the game would be reflected at the top left corner below the health bar. Available temporary potions are shown at the buttom 2 corners.

Asset we used:
[Cloud Effect](https://assetstore.unity.com/packages/tools/particles-effects/cloudstoy-35559), [Sky](https://assetstore.unity.com/packages/2d/textures-materials/sky/free-night-sky-79066), [Medivel Castle](https://assetstore.unity.com/packages/3d/environments/fantasy/medieval-castle-modular-282498).

Quick Look of the Game:

<img width="400" alt="Main" src="https://github.com/user-attachments/assets/ef9abe6b-352c-4145-b065-d6fd2622263c" />
<img width="400" alt="HomeScene" src="https://github.com/user-attachments/assets/e166a61c-9d8e-4d1a-b3a9-2d7cef31ca74" />
<img width="400" alt="GemCrafting" src="https://github.com/user-attachments/assets/bbe114aa-eb15-4ea8-aabe-fa344b054950" />
<img width="400" alt="Game" src="https://github.com/user-attachments/assets/18a1174c-e433-4856-865a-022549faed83" />

### Project Checkpoint 3-4: Sound, UI, and Animation

We added increasing difficulty floor-by-floor and randomized game modifiers on each floor, and implemented the boss battle in this iteration. Following feedback from last iteration, we added more particle effects in the Home Scene and made possible to close the panel using the G/P keys as well. Audio effects of player's taking damage and enemy death are also added in this checkpoint. We also adjusted the animation of the sword attack, and added movement (up/down) animations to the boss.

<img width="400" alt="Screenshot 2025-04-16 at 8 41 40 PM" src="https://github.com/user-attachments/assets/b562e855-c575-4df1-b85b-93cd1947935f" />
<img width="400" alt="Screenshot 2025-04-16 at 8 42 37 PM" src="https://github.com/user-attachments/assets/3a958654-33bb-49c5-a2c3-7ddb09768f15" />

Resources we used:

Fire
https://assetstore.unity.com/packages/vfx/particles/fire-explosions/free-fire-vfx-urp-266226

Monster Death Sound
https://freesound.org/people/RedRoxPeterPepper/sounds/420250/

Player Damage Sound
https://freesound.org/people/deleted_user_3330286/sounds/188549/

Background tracks
http://epidemicsound.com/track/cKvr2xxYss/
https://www.epidemicsound.com/track/wOJUGbdCra/

### Project Part 4: Finishing Touches

We mainly focused on adjusting game parameters and debugging the endless mode during this iteration. We also incorporated feedback from last checkpoint and implemented the increase of the cost of upgrading as game progresses, and in the panel there's also sprites next to the cost indicating what materials the player's using. We added camera shake when player takes the damage and improved the win/lose screen with proper fonts and sounds.

<img width="400" alt="Screenshot 2025-04-24 at 7 43 52 PM" src="https://github.com/user-attachments/assets/fee10b0e-3570-4c51-a786-6d45f06e2d89" />
<img width="400" alt="Screenshot 2025-04-24 at 7 43 37 PM" src="https://github.com/user-attachments/assets/3403619b-ceed-4cbb-9270-19de6fd0d1a9" />
<img width="400" alt="Screenshot 2025-04-24 at 7 44 12 PM" src="https://github.com/user-attachments/assets/28b8ab8b-28bc-4450-b628-c3220fdb3597" />

Resources we used:

Sound tracks for lose/win
https://pixabay.com/sound-effects/game-bonus-144751/
https://pixabay.com/sound-effects/game-over-38511/

### Final Project Submission

We updated animation for the player character in the Home Scene and Game Scene. Added death particles. Also fixed the UI scaling issues.

<img width="400" alt="Screenshot 2025-05-06 at 11 54 37 AM" src="https://github.com/user-attachments/assets/45808d80-31b5-43b4-9006-aaea20d60276" />
<img width="400" alt="Screenshot 2025-05-06 at 11 53 53 AM" src="https://github.com/user-attachments/assets/b90ee29e-20e9-41c1-b16f-af198ed13b2f" />
<img width="400" alt="Screenshot 2025-05-06 at 11 53 02 AM" src="https://github.com/user-attachments/assets/a7ac10e7-ef14-4bcd-8fd0-9d7665a78d66" />


Resources we used:
https://assetstore.unity.com/packages/3d/characters/humanoids/fantasy/rpg-hero-pbr-hp-polyart-121480

## Instructions on How to Play and Test the Game

#### Getting Started

- Load the **Main** scene to play the game, click "Start Game" to start, and you'll be directed to the **Home** scene
- Select your weapon by checking sword or staff toggle.
- [!! Not applicable for first floor] Start gem crafting by pressing G or potion purchase by pressing P. Close button of the pop-up windows are on upper left.
- Click "Start Battle" to begin, the battlefield is created on **Game** scene.

#### Combat & Controls

- WASD to move
- Character automatically faces the nearest enemy
- Sword: melee weapon, attacks at close range
- Staff: ranged weapon, shoots blue projectiles
- Avoid orange enemy projectiles
- Q: Use health potion
- E: Use speed potion
- Enemy drops random amounts of Gemdust and Mushrooms

#### Upgrading Weapons

- Spend Gem Dust for Damage, Attack Speed, and Range upgrades
- Reset buttons refund 80% of spent Gem Dust

#### Potions

- Craft potions with Mushrooms in the Home scene
- Permanent potions provide lasting stat increases
- Temporally Potion can be used during battle

#### Game Progression

- "Return to Main Menu" after winning or losing

#### Debug Commands

- J: add 50 Gem Dust (Home scene)
- K: add 50 Mushrooms (Home scene)
- L: skip to the 9th floor (Home scene)

## Demo

Check out a play-through showcase of the game:
[Demo Video](https://livejohnshopkins-my.sharepoint.com/personal/jcui19_jh_edu/_layouts/15/stream.aspx?id=%2Fpersonal%2Fjcui19%5Fjh%5Fedu%2FDocuments%2FTower%20Of%20The%20Betrayer%20Demo%2Emov&nav=eyJyZWZlcnJhbEluZm8iOnsicmVmZXJyYWxBcHAiOiJPbmVEcml2ZUZvckJ1c2luZXNzIiwicmVmZXJyYWxBcHBQbGF0Zm9ybSI6IldlYiIsInJlZmVycmFsTW9kZSI6InZpZXciLCJyZWZlcnJhbFZpZXciOiJNeUZpbGVzTGlua0NvcHkifX0&referrer=StreamWebApp%2EWeb&referrerScenario=AddressBarCopied%2Eview%2Ee9edb5e7%2D3f52%2D45b2%2Db192%2Dbdf3c12ab195&ga=1&LOF=1)

Download:
[WebGL](https://jjjcff.itch.io/tower-of-the-betrayer)

## Future Work

Below are some high-level changes we could make given longer development time:

#### More Playable Characters

- Unlockable heroes with unique abilities
- Each character has a different starting loadout and skill tree

#### Expanded Weapon System

- Add more weapon types (bows, daggers, shields) with new gem slots
- Introduce weapon fusions or ultimate forms after upgrades
- Improve attack animation and effects

#### Smarter Enemies & Bosses

- Improve AI to dodge or surround players
- Add mini-bosses on higher floors with unique powers

#### Dynamic Floor

- Add traps, moving platforms, or destructible walls etc
- Weather or lighting effects that affect gameplay

## Contribution

- **Jeff Cui**

  - Designed and implemented core gameplay logic, including weapon upgrade systems and difficulty scaling
  - Developed the boss battle mechanics and effects
  - Built UI panel in Home Scene
  - UI Scaling fixes

- **Elaine Zhao**

  - Created and animated the Home Scene with UI interactions
  - Adapted and fitted 3D models into Game Scene environment for polished visuals
  - Integrated particle effects and sound design across scenes
  - Minor bug fixes in game logic

- **Collaboration**

  Overall Jeff built the core gameplay skeleton and system back-end (e.g. upgrade mechanics, floor logic, boss control) and Elaine adjusted and integrated UI elements, visual feedback, and animations on top of the gameplay systems.
  An example of the collaborative process is the Home Scene development: Jeff implemented the weapon upgrade backend with inventory management and computation and a basic full-size UI panel. Elaine introduced the player character movement in the 3D scene and the pop-up effects of a precise section of the panel according to which weapon/material the player's working on.
