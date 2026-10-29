# GitHub Copilot Instructions for Yayo's Caravan (Continued)

## Mod Overview and Purpose

**Yayo's Caravan (Continued)** is an update to the original YAYO's mod, enhancing the visual representation of caravans on the world map in the game **RimWorld**. The mod introduces various customization options for caravan visuals and settings, providing players with more control over how caravans are displayed during gameplay.

## Key Features and Systems

- **Visual Customization**: Users can modify the visual aspects of caravans, including options for zooming, animations, and the appearance of pawns.
- **Settings Panel**: The mod includes a settings panel where players can adjust various parameters such as zoom mode, animation toggle, showing/hiding animals, maximum pawn count, pawn scale, and spacing between pawns.
- **Harmony Patching**: The mod uses Harmony to patch vanilla game methods to integrate its custom visual changes seamlessly.
- **XML Integration**: The mod utilizes XML files to define its dependencies and metadata.

## Coding Patterns and Conventions

- **C# Files**: The mod is primarily written in C#, with a focus on creating components, handling harmony patches, and managing game logic.
- **XML Files**: XML is used for defining mod metadata and dependencies.
- **Class and Method Naming**: Classes and methods are named descriptively to reflect their functionality, such as `CaravanComponent` and `PlaySettings_DoWorldViewControls`.
- **Code Organization**: The code is organized into logical directories and files, with each file containing a specific component or patch.

## XML Integration

The mod uses XML files to define its metadata and dependencies. The primary XML file is `About.xml`, which includes:

xml
<ModMetaData>
    <Name>Yayo's Caravan (Continued)</Name>
    <Author>YAYO</Author>
    <PackageId>Mlie.YayosCaravan</PackageId>
    <Version>1.0.0</Version>
    <Description>Update of YAYO's mod with improved caravan visuals.</Description>
    <Dependencies>
        <Dependency>UnlimitedHugs.HugsLib</Dependency>
        <Dependency>brrainz.harmony</Dependency>
    </Dependencies>
    <Tags>animation, world map</Tags>
</ModMetaData>


## Harmony Patching

The mod uses Harmony to patch vanilla game methods to achieve the desired visual effects. Key harmony patches include:

- **PlaySettings_DoWorldViewControls.cs**: Patches the `DoWorldViewControls` method to add custom controls for caravan visual settings.
- **WorldDynamicDrawManager_DrawDynamicWorldObjects.cs**: Patches the `DrawDynamicWorldObjects` method to customize the drawing of dynamic world objects.
- **WorldObject_Draw.cs**: Patches the `Draw` method of `WorldObject` to apply custom visual changes to caravans.

## Suggestions for Copilot

### General Guidelines

- **Follow Existing Patterns**: When generating code, adhere to the existing coding patterns and conventions used in the mod, such as naming conventions and code organization.
- **Use XML for Metadata**: When creating or modifying XML files, ensure they conform to the structure and conventions used in `About.xml`.
- **Implement Harmony Patches Carefully**: When implementing new harmony patches, ensure they are tested thoroughly to avoid conflicts with other mods or vanilla game mechanics.

### Specific Features

- **Visual Customization**: When adding or modifying visual features, consider the impact on performance and ensure that changes are optimized for smooth gameplay.
- **Settings Panel**: When adding or modifying settings in the settings panel, ensure that new options are intuitive and well-documented for users.

### Example Code Snippets

- **CaravanComponent.cs**:
    csharp
    public class CaravanComponent : WorldComponent
    {
        public CaravanComponent(World world) : base(world) { }

        public void DrawQuadTangentialToPlanet(Vector3 center, float radius, Color color)
        {
            // Custom drawing logic
        }

        public void GetRot(Vector3 center, out float rot)
        {
            // Calculation logic for rotation
        }

        public void update()
        {
            // Update logic for caravan component
        }

        public override void WorldComponentTick()
        {
            base.WorldComponentTick();
            update();
        }
    }
    

- **PlaySettings_DoWorldViewControls.cs**:
    csharp
    [HarmonyPatch(typeof(PlaySettings), "DoWorldViewControls")]
    public static class PlaySettings_DoWorldViewControls
    {
        public static void Postfix()
        {
            // Custom UI logic for caravan settings
        }
    }
    

### Testing and Debugging

- **Test in Isolation**: When testing new features or patches, ensure that they are tested in isolation to identify any conflicts or issues.
- **Use Log Uploader**: When reporting errors or issues, use the RimWorld log uploader to share detailed logs for analysis.

By following these guidelines and suggestions, you can contribute effectively to the Yayo's Caravan (Continued) mod, ensuring that new features and improvements are well-integrated and enhance the overall player experience.

---

*Note: Always refer to the official RimWorld documentation and community guidelines for best practices and additional information.*

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

