# GitHub Copilot Instructions for Yayo's Caravan (Continued)

## Mod Overview and Purpose
Yayo's Caravan (Continued) is a mod for the game RimWorld, designed to update and enhance the functionalities of YAYOs mod. The primary focus of this mod is to offer a more immersive experience when interacting with caravans on the world map.

## Key Features and Systems
- **Visual Changes to the Caravan**: The mod changes the visual representation of caravans on the world map.
- **Customizable Settings**: Players can adjust several aspects of the mod through option settings:
  - Zoom out mode options: None, Big Leader, Vanilla.
  - Animation toggle (On/Off).
  - Show or hide animals.
  - Maximum pawn count display.
  - Adjustable pawn scale.
  - Spacing between pawns.
- **Future Features**: Although not yet implemented, there are ideas for sleeping poses and FX, displaying images of campfires or tents during rest, and showing weapons when the caravan stops.

## Coding Patterns and Conventions
- Classes are predominantly named using PascalCase, e.g., `Caravan_ExposeData`, `caravanComponent`.
- Internal mod settings and behavior changes are encapsulated in classes like `caravanVisualMod` and `caravanVisualSettings`.
- Utilization of interfaces such as `IExposable` in `caravanData` for serialization purposes with the `ExposeData()` method.

## XML Integration
- The mod suggests XML settings integration, especially in defining or modifying RimWorld’s default settings for visual and interactive elements on the world map.
- Ensure XML files are well-structured, utilizing the proper keys and values to match the mod's customizable features.

## Harmony Patching
- This mod likely employs Harmony to patch methods in the base game to introduce new features without altering the base game's source code.
- Use Harmony’s `Postfix` and `Prefix` attributes to modify original methods safely.
- Ensure to target specific methods related to world view control and caravan behaviors for patches.

## Suggestions for Copilot
1. **Helper Functions**: Implement utility functions in `dataUtility` to handle common data operations, making the code cleaner and more maintainable.
2. **Consistent Naming**: Use descriptive and consistent naming conventions for class members and methods to maintain clarity.
3. **Expand With New Features**: Use Copilot to explore extending the current feature set, such as completing the 'Incompleted Idea' items.
4. **Efficient XML Parsing**: Suggest Copilot snippets that correctly parse and apply settings defined in XML configuration files.
5. **Robust Error Handling**: Leverage Copilot to implement error handling, possibly using `try-catch` blocks, especially where user inputs can affect settings.
6. **Documentation**: Use Copilot to generate summaries and comments throughout the codebase for better clarity and documentation.

## Additional Notes
- Be aware of compatibility issues, especially with mods that change the planet size.
- The mod encourages community involvement, suggesting modifications and contributions from developers to create variants.
- For troubleshooting, it is advised to isolate the mod with its requirements and use relevant community forums or Discord channels for reporting and resolving issues.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.
