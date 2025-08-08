# GitHub Copilot Instructions for Yayo's Caravan (Continued)

## Mod Overview and Purpose
**Yayo's Caravan (Continued)** is an update of the original YAYO's mod designed to enhance the visual representation of caravans on the world map in RimWorld. This mod provides customization options for players to modify how caravans appear and interact with the game world, allowing for a more tailored and immersive gameplay experience.

## Key Features and Systems
- **Visual Enhancements**: Changes the appearance of caravans on the world map, offering multiple stylistic options such as "Big Leader" and "Vanilla".
- **Customizable Settings**: Players can adjust numerous visual and functional settings like zoom-out mode, animation toggling, max pawn count, pawn scale, and spacing.
- **Planned Features**: Future enhancements include sleeping poses, campfire/tent displays for resting caravans, and weapon displays.
- **Compatibility**: The mod is compatible with Yayo's Animation but may conflict with mods that alter planet size.

## Coding Patterns and Conventions
- Classes are generally defined as `public` or `internal` depending on their accessibility requirements.
- Method names follow a clear and descriptive naming convention to denote their functionality.
- The mod uses standard C# coding practices, complying with .NET Framework v4.8 and v4.8.1 conventions.

## XML Integration
The mod likely interacts with XML for data-related operations such as defining caravan appearance settings and storing mod configurations. Ensure that XML files are structured correctly and correspond with C# classes responsible for reading and applying these settings.

## Harmony Patching
For modifying game behavior without altering original game files, Harmony patches are employed. This might involve:
- Modifying or extending existing RimWorld caravan object methods to change visual representation.
- Adding new functionalities through Harmony Postfix or Prefix methods.
- Ensure compatibility with other mods by carefully patching only the necessary components.

## Suggestions for GitHub Copilot
- **Enhanced Autocomplete**: Utilize GitHub Copilot to autocomplete repetitive coding patterns, especially for method signatures defined in the mod's classes.
- **Code Suggestions**: Copilot can assist in generating Harmony patches by providing suggestions on where and how to insert prefix or postfix functions.
- **XML Data Handling**: Employ Copilot to write boilerplate code for XML data parsing and handling, ensuring smooth interaction with RimWorld's data structures.
- **Refactoring Assistance**: Leverage Copilot to propose refactoring options for cumbersome or outdated code within the mod, making it more efficient and maintainable.
- **Error Checking**: Use Copilot’s capabilities to identify potential errors or bad practices in existing code and suggest improvements.

By following these guidelines, contributions to the Yayo's Caravan (Continued) mod can be optimized, streamlined, and integrated seamlessly into the broader RimWorld modding ecosystem.

This instruction file should help guide contributors to the project, ensuring they have a good understanding of the mod's goals and the best practices for maintaining and enhancing it.
