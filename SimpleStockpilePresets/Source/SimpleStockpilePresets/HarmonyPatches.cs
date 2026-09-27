using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace SimpleStockpilePresets;

[StaticConstructorOnStartup]
public static class HarmonyPatches
{
    static HarmonyPatches()
    {
        new Harmony("horton.simplestockpilepresets").PatchAll();
    }
}

[HarmonyPatch(typeof(Zone_Stockpile), nameof(Zone_Stockpile.GetGizmos))]
public static class Patch_Zone_Stockpile_GetGizmos
{
    public static void Postfix(Zone_Stockpile __instance, ref IEnumerable<Gizmo> __result)
    {
        __result = WithPreset(__result, __instance?.GetStoreSettings(), __instance);
    }

    private static IEnumerable<Gizmo> WithPreset(IEnumerable<Gizmo> gizmos, StorageSettings settings, IStoreSettingsParent parent)
    {
        if (gizmos != null)
        {
            foreach (var gizmo in gizmos)
            {
                yield return gizmo;
            }
        }

        if (settings != null)
        {
            yield return new Command_StorageSettingsPresets(settings, parent);
        }
    }
}

[HarmonyPatch(typeof(Building_Storage), nameof(Building_Storage.GetGizmos))]
public static class Patch_Building_Storage_GetGizmos
{
    public static void Postfix(Building_Storage __instance, ref IEnumerable<Gizmo> __result)
    {
        var settings = __instance?.GetStoreSettings();
        if (settings == null)
        {
            return;
        }

        var command = new Command_StorageSettingsPresets(settings, __instance);
        __result = (__result ?? Enumerable.Empty<Gizmo>()).Prepend(command);
    }
}

[HarmonyPatch(typeof(InspectTabBase), "UpdateSize")]
public static class Patch_StorageTabWidth
{
    public static void Postfix(InspectTabBase __instance)
    {
        if (__instance is not ITab_Storage)
        {
            return;
        }

        var field = AccessTools.Field(typeof(InspectTabBase), "size");
        var size = (Vector2)field.GetValue(__instance);
        if (size.x < 430f)
        {
            size.x = 430f;
            field.SetValue(__instance, size);
        }
    }
}

[HarmonyPatch(typeof(ITab_Storage), "FillTab")]
public static class Patch_ITab_Storage_FillTab
{
    public static void Postfix(ITab_Storage __instance)
    {
        var parent = AccessTools.Property(typeof(ITab_Storage), "SelStoreSettingsParent")
            ?.GetValue(__instance) as IStoreSettingsParent;
        var settings = parent?.GetStoreSettings();
        if (settings == null)
        {
            return;
        }

        // ITab_Storage draws its filter in a fixed 300px group. The tab window is widened
        // in UpdateSize so this button sits in the spare strip, clear of the close button.
        var button = new Rect(308f, 10f, 110f, 29f);
        if (Widgets.ButtonText(button, "CommandStorageSettingsPresetsLabel".Translate()))
        {
            PresetMenu.Open(settings, parent);
        }
    }
}
