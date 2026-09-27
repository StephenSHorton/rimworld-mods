using RimWorld;
using UnityEngine;
using Verse;

namespace SimpleStockpilePresets;

public class Command_StorageSettingsPresets : Command
{
    private readonly StorageSettings settings;
    private readonly IStoreSettingsParent parent;

    public Command_StorageSettingsPresets(StorageSettings settings, IStoreSettingsParent parent)
    {
        this.settings = settings;
        this.parent = parent;
        icon = ContentFinder<Texture2D>.Get("UI/Commands/StorageSettingsPresets", false);
        defaultLabel = "CommandStorageSettingsPresetsLabel".Translate();
        defaultDesc = "CommandStorageSettingsPresetsDesc".Translate();
        Order = -100f;
    }

    public override void ProcessInput(Event ev)
    {
        base.ProcessInput(ev);
        PresetMenu.Open(settings, parent);
    }
}
