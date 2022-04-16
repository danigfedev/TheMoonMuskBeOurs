# TheMoonMustBeOurs

<p align="center">
<img src="https://user-images.githubusercontent.com/37219448/163653226-0e3eabcb-808f-473e-b634-9718ea336206.png" width="400" height="400">
</p>

https://user-images.githubusercontent.com/37219448/163608110-344af2c1-2ff6-44d9-b57c-ed29f5cce4c6.mp4

<!--
<p align="right">
    <video src='https://user-images.githubusercontent.com/37219448/163608110-344af2c1-2ff6-44d9-b57c-ed29f5cce4c6.mp4
    '/>
</p>
-->

## Introduction to the project

The Moon Must Be Ours is a game I developed as technical assessment for Outfit7's recruitment process. The repository stores the Unity project, including all the content created by me, as well as all its dependencies it relies on in order to help streamline the review process.

**Development time:** 10 days

## Pitch

It is 2021. You, Alon Must, are the leader of the 21st Century space race. To ratify this position, there is still one objective to complete: reaching the Moon for the first time in the last 50 years. You have invested everything on this quest and success is guaranteed, but you are not the only participant in this competition...

## Assessment requirements

The requirements for this assessment were:

- **Spaceship:** The element under player’s control. Among its main features are: motion based on player’s input, automatic shooting and upgrade by power-up pickup. It can be damaged by enemy shot and enemy or obstacle collision.

<p align="center">
<img src="https://user-images.githubusercontent.com/37219448/163653265-a272ca49-4299-44d0-bfcd-5aa3b8f1e70f.png">
<img src="https://user-images.githubusercontent.com/37219448/163653357-55c8ccb1-27ef-4689-9f2c-9fe3692f5537.png">
<br> 
   <i>Player´s ship and projectile</i>
</p>

- **Enemies with behaviours (paths, formations):** There are two types of enemy in the game. Vans and Destructors. Vans spawn in groups of four and follow a circular pattern. They drop boxes as their projectiles. Destructors, on their part, appear in groups of three, in V formation and with a linear movement. They shoot smiles. Both can be damaged by player’s rockets.

<p align="center">
<img src="https://user-images.githubusercontent.com/37219448/163654216-6806e9ae-0ffc-4aa8-ae19-53475b266265.png">
  <br> 
   <i>Enemies´ 3D models</i>
</p>

<p align="center">
<img src="https://user-images.githubusercontent.com/37219448/163654242-3b5430c1-58c0-4535-80e2-438f4f3efff8.png">
<br> 
   <i>Enemies´ projectiles</i>
</p>


<p align="center">
<img src="https://user-images.githubusercontent.com/37219448/163654276-92d50955-17d5-4e03-93ad-44029faead59.png">
 <br> 
 <i>Vans´circular pattern</i>
</p>

<p align="center">
<img src="https://user-images.githubusercontent.com/37219448/163654671-38fe6b8a-03ae-4bbb-a8c3-3744dd0f8eda.png">
 <br> 
 <i>Three destructors in V formation</i>
</p>


- **Obstacles:** In your journey from the Earth to the Moon you’ll traverse the different atmosphere’s layers, each with its own type of obstacle. On the Troposphere you’ll find Clouds, which don’t cause damage, just blur your vision. Then you’ll move to the Exosphere, where satellites orbit the Earth. These obstacles, following a sinusoidal movement, cause damage if you hit them.

<p align="center">
<img src="https://user-images.githubusercontent.com/37219448/163654826-697af204-f080-47c6-9dfe-7c8e515524e3.png">
  <img src="https://user-images.githubusercontent.com/37219448/163654839-1c130c96-186e-42dc-8b44-dc61ab5b9e12.png">
  <img src="https://user-images.githubusercontent.com/37219448/163654900-4b3389d3-9244-4897-a856-179008f155c9.png">
<br>  
<i>Clouds and satellites overview</i>
</p>

- **Powerups:** There are three types of power up: gun (increases amount of bullets shot), shield (provides a few seconds of protection) and health (increases player’s health).
- 
<p align="center">
<img src="https://user-images.githubusercontent.com/37219448/163654965-0e3d036b-89a6-4494-a565-4098caeb42de.png">
 <br> 
 <i>The three types of power up</i>
</p>


- **Background environment:** The background environment has two main parts: the skybox and the spawned objects. The skybox uses a custom procedural shader that changes depending on the atmosphere layer (aka stage of the game) the player is. It evolves from sunset to starry sky. About the background objects, they are obscured and scaled versions of the obstacles.

<p align="center">
<img src="https://user-images.githubusercontent.com/37219448/163655120-dfc5f69a-7a74-43fd-a7fc-ff2c48edc6c6.png">
 <br> 
 <i>Skybox evolution</i>
</p>

- **Scoring:** The scoring of the game is formed by two elements. In the first place, we have the enemy kill count. On the other side, we have a progress bar that shows the stage tha player is now.


<p align="center">
<img src="https://user-images.githubusercontent.com/37219448/163655169-dc546c53-76d7-4139-a21d-41ab835fe6df.png">
 <br> 
 <i>Game UI: enemy count at the top. Progress bar on the left margin.</i>
</p>


## Tools

- **Engine:** Unity 2021.1.3 + Shader Graph
- **3D Modeling:** Blender
- **2D:** GIMP

## Dependencies

**Note:** All these packages are included in the project

* Text Mesh Pro 3.0.6

