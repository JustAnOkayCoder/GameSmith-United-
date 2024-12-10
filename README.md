<a id="readme-top"></a>
# Tower Defense Game

This is a simple Tower Defense Game created using Unity and C#. In this game, players defend their base by strategically placing towers to destroy waves of enemies. This is an agile development project for Frostburg University's COSC625 class. 


## Installation

In order to re-create/edit this game, you'll need to have C# (via VS Code) and Unity installed. 

[VS Code Download](https://code.visualstudio.com/Download)

[Unity Download](https://unity.com/download)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Files

To access imports and designs within the game, you can visit [Assets](TowerDefense-main/Assets/Imports). These files display attributions within the game from enemies and towers. 

The main Unity file can be located [here](TowerDefense-main/Assets/TowerDefenseScene.unity). This links to our scene file which defines the game environment's layout, objects, and configurations. 

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Enemy.cs
This file handles the basic movement pattern of the enemies using a invisible node system hidden under the actual track on the board.
There is also a damage counter under tick that allows dot style of damage to effect the enemy.

## EntitySummoner.cs
This file deals with adding and removing enemies from the scene and uses a queue system to keep track of how many enemies are in the scene,
the way we have the script wrote you should be able to add multiple enemy types to the game as long as you keep the reference to the enemies in 
order.

## GameLoopManager.cs
This file is probably the most important to the game. It handles the start up of the game by spawning enemies, damage effects to the enemies, here you will also find
the wave system implementation and the current wave setup is 3 waves of 5 enemies which you can change on your own. And the victory screen is displayed when all enemies are defeated
and its connected to Unity through the game object victoryScreen. 

## Player Folder
Under the player folder you will find 3 .cs files, PlayerMovement.cs which allows the player to do basic movement like walk and jump, PlayerStats which is where the money system is implemented allowing the player to 
start the wave with a set amount of money and keeps track of the amount of money the player earns, TowerPlacement.cs handles the placement off the towers and only allows the player to place a tower on the allowed
surfaces.

## Tower Folder
This folder  contains TowerBehavior.cs which sets the base damage for the towers and the calls the damage tick once per frame, TowerTargeting.cs handles the targeting for the towers and simply put it make the towers track the 
closest enemy to the tower and focuses on that enemy until it is defeated which it will then move to the next closest enemy using Vector3.distance through Unity.

## Top contributors:

<a href="https://github.com/dopeskyy/GameSmith-United-/graphs/contributors">
  <img src="https://contrib.rocks/image?repo=dopeskyy/GameSmith-United-" alt="contrib.rocks image" />
</a>

<p align="right">(<a href="#readme-top">back to top</a>)</p>
