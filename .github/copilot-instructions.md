# GitHub Copilot Instructions for RimWorld Modding: Callouts (Continued)

## Mod Overview and Purpose
The "Callouts (Continued)" mod is an updated iteration of Captain Muscles' original mod, continuing from Profligates' version. It enhances the gameplay experience by adding dynamic text callouts above pawns' heads in RimWorld when specific in-game events occur. This feature adds depth to interactions by using callouts similar to log entries, allowing for immersive storytelling and interaction feedback. The mod aims to deliver an engaging user experience by reflecting the personalities and conditions of pawns during gameplay scenarios.

## Key Features and Systems
- **Dynamic Callouts**: Text callouts appear above pawns' heads during specific game interactions, such as combat or animal interactions.
- **Event Triggers**: Callouts are activated by various events such as melee attacks, animal taming, and lethal hediffs progression.
- **Grammar Integration**: Utilizing RimWorld's grammar rules, callouts are customizable and varied, reflecting different pawn properties.
- **Trait-Based Variations**: The nature of callouts can be "mild" or "spicy" based on pawns' traits like 'Kind', 'Wimp', 'Abrasive', 'Bloodlust', or 'Psychopath'.
- **Settings Configuration**: Players can configure callout frequency, visibility in combat, animal interactions, and more.
- **Visual Feedback**: Options to queue text motes, attach text to pawns, and display body parts wounded enhance the visual storytelling aspect.

## Coding Patterns and Conventions
- **Class Structure**: Each logical unit within the mod's codebase is encapsulated in classes, most of which follow a static structure to accommodate the RimWorld framework.
- **Harmony Patches**: Harmony is extensively used to patch existing RimWorld methods to introduce new behaviors without altering the game's base code.
- **Consistent Naming**: Classes and methods are named to reflect their purpose and functionality clearly, such as `InteractionWorker_Nuzzle_Patches` or `CalloutModSettings`.
- **File Organization**: The mod is organized across multiple C# files, each focusing on different aspects of callout functionalities and maintaining readability.

## XML Integration
- **Definition Management**: RimWorld mods extensively use XML for definitions. This mod includes several definition classes such as `CalloutConstantByTraitDef` and `CalloutDefOf` to manage game objects referenced by callouts.
- **Rule Packs**: Grammar and callout rules are defined using XML structures, enabling flexible and localized strings.

## Harmony Patching
- **Purpose**: Harmony is employed to inject new behaviors into existing game functions seamlessly, allowing the mod to interact with internal game mechanics.
- **Patch Examples**:
  - `Verb_MeleeAttack_CreateCombatLog` for enhancing combat log information with callout text.
  - `JobDriver_InteractAnimal_StartFeedAnimal` for integrating callouts during animal interactions.
  - Patches are applied using static classes with nested static methods to ensure efficiency and maintain code separation.

## Suggestions for Copilot
- **Event Handling**: Suggest efficient ways to identify and manage event triggers for callouts.
- **Callout Customization**: Propose methods to expand grammar structures and rule packs for more varied and context-specific callouts.
- **Settings Optimization**: Ideas for improving mod settings UI, enhancing user experience when configuring callout features.
- **Performance Enhancement**: Tips on optimizing the execution of Harmony patches to reduce performance overhead during gameplay.
- **Debugging Assistance**: Tools or scripts to streamline debugging of callouts and ensure compatibility across different game versions.

## Future Enhancements
The mod is slated for future improvements including a broader range of callouts, additional interaction events, and expanded trait-based variations. Community feedback and contributions are encouraged to drive ongoing development. 

Please share any suggestions for additional features or improvements in the comments.

## Credits
- **Concept**: Tallas
- **Original Coding**: Captain Muscles
- **Inspiration**: Inspired by the taunt feature from the "Combat Extended" mod.

For more contributions or to view the mod's source code, visit the project's GitHub repository.
