# rimworld-mods

Local RimWorld 1.6 fixes. These are not official updates. The original authors still own their mods. This repo exists so a broken Workshop copy can be fixed without waiting on an update.

Disable the Workshop version of a mod before enabling the copy here. The package ids are different, so leaving both on loads both.

Steam Workshop copies are not replaced. Nothing here publishes over those Workshop items.

## Simple Stockpile Presets

- Original by Lanilor, continued by Mlie
- Workshop: https://steamcommunity.com/sharedfiles/filedetails/?id=2189006791
- Source: https://github.com/emipa606/SimpleStockpilePresets
- License: MIT (see `SimpleStockpilePresets/LICENSE.md`). French translation by Imprécation.
- Package id: `horton.simplestockpilepresets`
- Disable Workshop package `Mlie.SimpleStockpilePresets`

The old mod added a "Select preset" gizmo by replacing the result of `Zone_Stockpile.GetGizmos`. That button stopped showing up on the selection bar. This copy puts **Select preset** on the Storage tab, and also puts it first on the selection bar for stockpile zones and shelves. The preset list is the same one the original mod used.

## Workbench Connect

- Original by jungooji
- Workshop: https://steamcommunity.com/sharedfiles/filedetails/?id=3524727262
- Source: https://github.com/zzzz465/WorkbenchConnect
- Package id: `horton.workbenchconnect`
- Disable Workshop package `jungooji.workbenchconnect`

Linked benches already copied the recipe list. The "do this many times" and "until you have" counts did not. The original mod only noticed those edits inside `Bill.DoInterface` and the vanilla details dialog. Nice Bill Tab draws the bill queue itself and never calls `Bill.DoInterface`, so a count change on that row never synced. This copy compares linked bills a few times a second and copies the bill that changed, including those counts.

The assembly and class names are unchanged, so workbench links already in a save still load. Unlink benches before removing the mod.

## EdB Prepare Carefully

- Original by EdB (edbmods)
- Workshop: https://steamcommunity.com/sharedfiles/filedetails/?id=735106432
- Source: https://github.com/edbmods/EdBPrepareCarefully
- License: MIT (see `PrepareCarefully/LICENSE`)
- Package id: `horton.preparecarefully`
- Disable Workshop package `EdB.PrepareCarefully`

Randomize was keeping the Baseliner xenotype forced, and that skips the genes that choose skin color, hair color, hair, beard, head, and body. Skin stayed the default light color. Appearance randomize now rolls a new colonist and copies those cosmetic genes. A full randomize no longer forces Baseliner, so a new xenotype roll includes them too. A xenotype you picked on purpose is still kept.

Preset files stored one id on every colonist (`Guid.NewGuid()` came back identical), so every relationship loaded as the same person. Saving now gives each pawn their own id. A preset that already has the shared id cannot tell the relationships apart, so those links are dropped instead of being glued to one colonist. Set them again and save a new preset.

## Build

Each mod builds with the .NET SDK. RimWorld and Harmony come from the `Krafs.Rimworld.Ref` and `Lib.Harmony.Ref` packages.

```powershell
dotnet build SimpleStockpilePresets\Source\SimpleStockpilePresets\SimpleStockpilePresets.csproj -c Release
dotnet build WorkbenchConnect\Source\WorkbenchConnect\WorkbenchConnect.csproj -c Release
dotnet build PrepareCarefully\EdBPrepareCarefully.csproj -c Release
```

The DLLs land in each mod's `Assemblies` folder. RimWorld loads a folder that contains `About\About.xml`. Point it at these two folders, or junction them into `RimWorld\Mods`.
