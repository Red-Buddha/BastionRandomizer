# BastionRandomizer
This is a Randomizer for Supergiant Games' Bastion.

## Settings

* Randomize Level Order
  * Randomizes the order of the levels (Wharf District is still the first level and Tazal Terminals is still the last level)  
* Randomize Enemies
  * Randomizes the spawn for each enemy in the game
* Randomize Loot 
  * `Guarantee Weapon` A weapon will always be placed at the hammer location 
  * `Weapons, Abilities, Upgrades, Loot` Adds the specified item pools to the randomized loot pool
* Seed
  * Used for creating a specific randomized version of the game, leave blank for a random seed
  
* Randomize
  * Makes backups of the games data files that will be modified and creates randomized versions
* Unrandomize
  * Removes randomized data files and restores backups
  
* Select Folder
  * Opens file selector so you can provide the path to the folder that contains your Bastion.exe
  
## Things to Note
  
* For now Wharf District is still the first level and Tazal Terminals is still the last level
* The first core and building are required to open the skyway in the Bastion but the skyway is always unlocked after that
* Langston River now sends you back to the Bastion rather than into Prosper Bluff
* Weapon pickup animations often won't work
* When you pick up a melee weapon it will replace the weapon in your melee slot even if your ranged weapon slot is still empty (same goes for picking up ranged weapons)
* Picking up the ram weapon only gives the ram swipe ability
* Picking up the weapon that takes the place of the ram in Tazal will still give you the ram loadout
