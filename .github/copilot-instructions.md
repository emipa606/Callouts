# GitHub Copilot Instructions for Callouts (Continued) Mod

## Mod Overview and Purpose

The Callouts (Continued) mod is an update to Captain Muscles' Callouts mod, which adds callouts in text form that occasionally appear above a pawn's head when certain in-game events occur. The mod uses the same grammar rules and concepts as log entries, allowing for "Mad-libs" style callouts. It also generates text motes for wounds and potentially lethal hediffs advancing in severity. Pawns are more likely to use certain callouts depending on their traits.

## Key Features and Systems

### Callouts
- **Combat Events**: Ranged attack, Melee attack, Ranged attack hitting, Melee attack hitting, Taking damage.
- **Animal Interaction Events**: Being nuzzled, Feeding an animal, Successfully taming/training an animal, Tending to a wounded animal, Slaughtering an animal.
- **Other Events**: Lethal hediff advancing to next stage, Being drafted.
- **Referenceable Properties**: Various properties of the initiator and recipient of the action, properties of the initiator and recipient's weapons, cover used by the initiator and recipient.

### Settings
- **Combat callouts enabled**: Enables/disables all combat callouts.
- **Animal interaction callouts enabled**: Enables/disables all animal interaction callouts.
- **Queue text motes**: Queues text motes to release over time to avoid overlap.
- **Attach callout text to caller**: Makes callouts stay directly over the caller's head until they expire.
- **Draw label background for text motes**: Turns on/off drawing a grey label behind callouts and motes for easier readability.
- **Show wounds**: Turns on/off text motes displaying body parts that sustained wounds.
- **Base callout frequency**: Base chance that any given event can fire a callout.
- **Call out when targeting animals**: Enables/disables showing callouts when the target is not humanlike.
- **Animals call out**: Allows animals to use callouts.

## Coding Patterns and Conventions

1. **Naming Conventions**:
   - Use `PascalCase` for class names.
   - Use `camelCase` for method and variable names.
   - Use `CONSTANT_CASE` for constants.

2. **File Organization**:
   - C# files are located in the `Source` directory.
   - XML files are located in the `Defs` directory.

3. **Code Comments**:
   - Provide detailed comments for methods and complex logic.
   - Use XML comments for public APIs.

4. **Error Handling**:
   - Use `try-catch` blocks where appropriate.
   - Log errors using the RimWorld logging system.

5. **Testing**:
   - Write unit tests for new features.
   - Use the RimWorld modding framework for integration testing.

## XML Integration

The mod uses XML files to define various game elements such as callout rules, effects, and rule packs. Here is an example of how to define a `ThingDef` in `Mote_Special.xml`:

xml
<ThingDef>
    <defName>Mote_Callout</defName>
    <label>Mote Callout</label>
    <description>A mote that displays a callout text.</description>
    <category>Mote</category>
    <comps>
        <li Class="Mote_Callout"/>
    </comps>
</ThingDef>


To integrate new XML definitions, follow these steps:

1. Create a new XML file in the `Defs` directory.
2. Define the necessary game elements using the appropriate XML tags.
3. Ensure that the new XML file is included in the mod's `ModMetaData.xml` file.

## Harmony Patching

The mod uses Harmony patching to inject custom code into the game's existing methods. Here is an example of a Harmony patch:

csharp
using HarmonyLib;
using Verse;

namespace Callouts.Patches
{
    [HarmonyPatch(typeof(Pawn), "TakeDamage")]
    public static class Pawn_TakeDamage_Patch
    {
        public static void Postfix(Pawn __instance, DamageInfo dinfo)
        {
            // Custom code to handle callouts on taking damage
        }
    }
}


To create a new Harmony patch, follow these steps:

1. Identify the method you want to patch.
2. Create a new class in the `Patches` namespace.
3. Use the `[HarmonyPatch]` attribute to specify the target method.
4. Implement the necessary prefix, postfix, or transpiler method.
5. Register the patch in the mod's initialization code.

## Suggestions for Copilot

- **Code Generation**:
  - Generate new C# classes and methods based on existing patterns.
  - Generate XML definitions for new game elements.
  - Generate Harmony patches for specific game methods.

- **Code Completion**:
  - Complete method implementations based on existing code.
  - Complete XML attributes and tags based on existing definitions.
  - Complete Harmony patch methods based on target methods.

- **Code Refactoring**:
  - Refactor existing code to improve readability and maintainability.
  - Rename variables and methods to follow naming conventions.
  - Extract common code into reusable methods.

- **Code Analysis**:
  - Analyze code for potential errors or issues.
  - Suggest optimizations for performance improvements.
  - Provide code quality suggestions based on best practices.

- **Documentation**:
  - Generate XML comments for public APIs.
  - Provide detailed comments for complex logic.
  - Create documentation for new features and settings.

By following these guidelines, you can ensure that the Callouts (Continued) mod remains high-quality and maintainable.

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

