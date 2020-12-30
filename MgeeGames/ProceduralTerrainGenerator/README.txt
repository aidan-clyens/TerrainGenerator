##########################################
###    Procedural Terrain Generator    ###
##########################################
This is a Procedural Terrain Generator for use in Unity projects. Using random noise algorithms, physics simulation, random object placement, custom materials, textures, and colours an endless variety of worlds can be created using this tool. This tool can create a terrain mesh, water mesh, and randomly place objects among a scene. The following images are scenes created based upon terrains created using this Procedural Terrain Generator.

## Generating Terrain ##
1. Add TerrainGenerator prefab into scene.
2. Configure settings and click "Generate" to create terrain.
3. Save terrain by entering a name and clicking "Save".
4. Saved terrains can be loading by selecting a name and clicking "Load".

## Terrain Generator ##
- Generator Settings:
    - Customize seed
    - Set terrain size and coordinates
    - Attach a viewer object (this is used to generate nearby objects)
    - Set view range of viewer object

- Terrain Settings:
    - Attach terrain material
    - Set terrain material colour gradient
    - Choose to generate forest or water

- Water Settings:
    - Set water material
    - Set water level

- Randomize:
    - Randomize terrain settings (seed, water level, and Height Map Generator settings)

- Generate:
    - Generate new terrain chunk

- Clear:
    - Delete existing terrain chunk

- Save Level:
    - Save current terrain settings

- Load Level:
    - Load terrain settings from a file

## Height Map Generator ##
- Noise Map Parameters:
    - Scale, octaves, persistence, lacunarity
    - Map depth
    - Noise redistribution factor

- Normalize Local:
    - Normalize the height map using the minimum and maximum height of the current terrain chunk

- Falloff:
    - Enable falloff to create islands

- Hydraulic Erosion:
    - Enable hydraulic erosion to create for realistic looking height maps

## Hydraulic Erosion ##
- Simulation Parameters:
    - Number of iterations
    - Intertia, gravity, minimum slope
    - Capacity factor, deposition factor, erosion factor, evapouration factor
    - Erosion radius, deposition radius
    - Droplet lifetime

## Forest Generator ##
- Parameters:
    - Select tree prefabs to generate
    - Density of trees
    - Slope threshold for tree placement
    - Vertical offset for tree prefabs

## Author ##
Aidan Clyens
