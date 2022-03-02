##########################################
###    Procedural Terrain Generator    ###
##########################################
This is a Procedural Terrain Generator for use in Unity projects. Using random noise algorithms, physics simulation, random object placement, custom materials, textures, and colours an endless variety of worlds can be created using this tool. This tool can create a terrain mesh, water mesh, and randomly place objects among a scene. The following images are scenes created based upon terrains created using this Procedural Terrain Generator.

## Getting Started ##
- To use the included sample materials and shaders, install the Universal Render Pipeline and ShaderGraph
- Create a new Universal Render Pipeline Asset with Depth Texture and Opaque Texture enabled
- Go to Project Settings > Graphics and update the Scriptable Render Pipeline Settings

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
    - Set wave speed
    - Set wave strength

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
    - 
- Average Map Depth
- Height Map Settings List:
    - Height Map Settings:
        - Map depth
        - Noise type (Perlin or Simplex)
        - Scale, octaves, persistence, lacunarity
        - Noise redistribution factor
        - Use falloff to create islands

- Biome Height Map Settings:
    - Biome noise scale factor:
        - Biome noise scale is a factor of noise scale
    - Biome depth factor:
        - Biome depth is a factor of map depth

## Hydraulic Erosion ##
- Simulation Parameters:
    - Enable or disable Hydraulic Erosion
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

## Change Log ##
v1.0
- Initial release.

v1.1
- Fixed sample models imported from Blender.
- Added WaterManager script, which is attached to the Water mesh and used to synchronize waves produced using the sample Water shader. The WaterManager provides the time offset to the Water shader so that the same wave equation may be accessed by script.

v1.2
- Add option to change terrain chunk width at powers of 2
- Add option to generate terrain chunks in a grid
- Add options for more non-uniform terrain generation
- Fix viewer for multi-chunk generation
- Add view range for terrain chunk objects as well as forest objects
- Remove "Normalize Local" option and just normalize all terrain chunks globally
- Add OpenSimplex noise as well as Perlin noise for terrain generation
- Add multiple layers of noise in Height Map Generator

v1.2.1
- Edited README
- Do not allow chunk width of 0

## Author ##
Aidan Clyens
