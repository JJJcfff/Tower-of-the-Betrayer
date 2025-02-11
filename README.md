# Tower of the Betrayer
## Team Members
Jeff Cui: https://github.com/JJJcfff
Elaine Zhao: https://github.com/ElaineZhao1210
Michelle Wang: https://github.com/michellewww

## Game Summary
Tower of the Betrayer is a roguelike strategy game where players assume the role of a warrior fighting to liberate their enslaved village from a tyrannical devil. The devil challenges adventurers to conquer a 10-floor monster tower, promising freedom in exchange. Players battle through procedurally generated floors, each with escalating enemy threats and randomized modifiers, while strategically upgrading weapons and crafting items. Upon reaching the final floor, the devil betrays the player, triggering a boss battle. Survivors can test their limits in Endless Mode, where delaying the final fight increases the boss’s strength.

## Genres
* Roguelike
* Strategy
* Crafting
* Action RPG

## Inspiration
### [Brotato]
Brotato’s fast-paced runs, auto-attack combat, and build diversity inspired our approach to accessible yet strategic gameplay. Its use of randomized items and character-specific traits influenced our gem and modifier systems. The "survive waves, upgrade between rounds" loop directly shaped our floor progression and material-driven upgrades.

<img width="982" alt="image" src="https://github.com/user-attachments/assets/3d29c658-0748-4dae-9bfb-05eacb799b77" />

https://store.steampowered.com/app/1942280/Brotato/

### [Low Poly Style Games]
The visual design of our game draws inspiration from low poly art styles, which are both visually appealing and efficient for development. The low poly aesthetic allows for a focus on clean, stylized environments and characters, enhancing the fantasy theme of the game while maintaining performance. Examples include games like "Totally Accurate Battle Simulator" and "For The King," which feature vibrant, minimalist designs that effectively convey their worlds.


## Gameplay
In this game, players take on the role of the most skilled warrior from a village that has been enslaved by a devil. Bored with his control over the humans, the devil constructs a 10-floor monster tower and declares that if any adventurer can conquer it, he will grant the village its freedom. As the village’s last hope, the player fights through the tower, growing stronger with each battle. However, upon reaching the final floor, the devil refuses to honor his promise, forcing the player into a final showdown against him.

**Core Mechanics**
* Weapon Choice: Players start with two weapons—a sword and a staff. At the beginning of each floor, they must choose between melee (sword) or ranged (staff) combat.
* Gem System: Each weapon has a limited number of gem slots that can be used for enhancements. Gems can be inserted, removed, and transferred between weapons by spending materials.
* Material Collection: After clearing each floor, players receive materials that can be used to strengthen their character or upgrade weapons.
* Item Crafting: Materials come in various types, some of which can be used to synthesize upgrade gems for weapons, while others can be crafted into potions that enhance abilities or restore health during battle.
* Combat System: Players control movement and consumable usage, while attacks are automated, targeting the nearest enemy.

**Enemy Variations & Modifiers**
* Each floor features different enemy types with randomized modifiers (e.g., increased damage, movement speed boosts).
* Each floor can have 0-3 random modifiers, which are hinted at before the battle, allowing players to prepare strategically.

**Progression & Challenge**
* Players must defeat all enemies on a floor to proceed.
* If the player dies, all upgrades are lost, and they must start over from the beginning.

**Endless Mode & Final Battle**
* After clearing all 10 floors, players can choose to enter Endless Mode, where they continue fighting increasingly difficult enemies.
* At the end of each additional floor in Endless Mode, players have the option to initiate the final boss fight.
* However, the longer they wait, the more difficult the final boss battle becomes.



## Development Plan
### Project Checkpoint 1-2: Basic Mechanics and Scripting (Ch 5-9)
* Implement basic movement of the character and the enemies
  * random generation of enemies
  * movement of the enemies towards the character
  * movement of the character controlled by WASD
* Weapon logic
  * fire animation
  * towards the closest enemy
* Basic layouts
  * Basic backgrounds
  * Health bar and floor display
