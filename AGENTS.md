# rimworld-mods

This repo holds local RimWorld 1.6 fixes for Workshop mods we still play. The original authors keep their mods and their Workshop pages. We copy the source in, fix what is broken, and load our copy instead of the Workshop download.

Do not publish over an author's Workshop item. Do not put their `PublishedFileId.txt` back. Do not change our package id back to theirs. Two copies with the same package id will both load.

Our package ids:

| Folder | Package id | Workshop id to leave unsubscribed | Upstream |
| --- | --- | --- | --- |
| `SimpleStockpilePresets` | `horton.simplestockpilepresets` | `2189006791` (`Mlie.SimpleStockpilePresets`) | https://github.com/emipa606/SimpleStockpilePresets `cb97be637ca7e6ffa8ae973b9c4325ee5fad3592` |
| `WorkbenchConnect` | `horton.workbenchconnect` | `3524727262` (`jungooji.workbenchconnect`) | https://github.com/zzzz465/WorkbenchConnect `0882b03af07dc66b32d2a4159533a293438cdb5c` |
| `PrepareCarefully` | `horton.preparecarefully` | `735106432` (`EdB.PrepareCarefully`) | https://github.com/edbmods/EdBPrepareCarefully `f0e273d9dc001ee96fddb2a53082a368b2e48791` (tag `v1.6.2`) |

The game is the Steam install at `C:\Program Files (x86)\Steam\steamapps\common\RimWorld`. Each mod folder is junctioned into that install's `Mods` directory. Harmony (`brrainz.harmony`) stays subscribed. SteamCMD is not installed, and it cannot unsubscribe Workshop items. Unsubscribe in the Steam client or on the Workshop page while logged in as PoppingPopper, then confirm the Workshop content folder for that id is gone.

Build from the repo root:

```powershell
dotnet build SimpleStockpilePresets\Source\SimpleStockpilePresets\SimpleStockpilePresets.csproj -c Release
dotnet build WorkbenchConnect\Source\WorkbenchConnect\WorkbenchConnect.csproj -c Release
dotnet build PrepareCarefully\EdBPrepareCarefully.csproj -c Release
```

References come from the `Krafs.Rimworld.Ref` and `Lib.Harmony.Ref` packages. Commit the rebuilt DLL. It is what RimWorld loads.

## Local fixes to keep

When upstream code comes in, keep these. They are the reason the copy exists.

- **Simple Stockpile Presets.** `Select preset` is on the Storage tab and first on the selection bar for stockpile zones and shelves. Preset lookups fail soft when a def is missing.
- **Workbench Connect.** Linked benches copy the "do X times" and "until you have" counts, including when Nice Bill Tab draws the bill row. The assembly name and the `WorkbenchConnect.*` class names stay as upstream published them, so existing links in a save still load.
- **Prepare Carefully.** Full randomize does not force the Baseliner xenotype, so skin and the other cosmetic genes can roll. Appearance randomize copies those cosmetic genes from a freshly rolled colonist. Pawn ids come from `PawnIds.Create`, not `Guid.NewGuid()`. A preset whose pawns already share one id drops those relationships instead of attaching them all to one person. A hidden child in a parent group is stored as a child.

Credit the original author in `About\About.xml`. Keep their license file when they shipped one.

## Check upstream

Do this before editing a mod, and when asked to see if the author has updated.

1. Read the table above. The commit is the upstream revision already folded into this repo.
2. If the Upstream cell says there is no public source, stop. Say so. Workshop-only mods have nothing to fetch.
3. Ask the remote what it points at now:

```powershell
git ls-remote https://github.com/emipa606/SimpleStockpilePresets.git HEAD
git ls-remote https://github.com/zzzz465/WorkbenchConnect.git HEAD
git ls-remote https://github.com/edbmods/EdBPrepareCarefully.git HEAD
```

Use the tag listed in the table when the table names one. `HEAD` is the default branch.

4. If the sha matches the table, there is nothing new. Say that and do not copy files.
5. If the sha differs, clone that repo into a temp directory outside this one. Do not add it as a remote of `rimworld-mods`, and do not commit the upstream `.git` folder.

```powershell
git clone --depth 1 https://github.com/ORG/REPO.git $env:TEMP\rw-upstream-NAME
```

6. Diff that tree against our mod folder. Ignore `Assemblies`, `bin`, `obj`, and `.git`. Read the diff before copying. Bring in upstream changes that do not undo the local fixes listed above. Where a hunk touches a local fix, keep the fix and take the rest of the upstream change.
7. Rebuild that mod and run whatever check fits the change. For a behavior fix, the check is the broken action in game, not only a successful compile.
8. Update the commit in the table above to the sha you actually merged. Leave the package id and the Workshop id alone.

A mod with no `About\About.xml` url and no row in the table has no upstream until someone finds a public source and adds the row.
