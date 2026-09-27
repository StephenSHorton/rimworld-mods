using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace SimpleStockpilePresets;

public static class PresetMenu
{
    public static List<FloatMenuOption> Options(StorageSettings settings, IStoreSettingsParent parent)
    {
        var list = new List<FloatMenuOption>();
        if (settings?.filter == null)
        {
            return list;
        }

        void Add(string key, Action<ThingFilter> apply)
        {
            list.Add(new FloatMenuOption(key.Translate(), () => Apply(settings, parent, apply)));
        }

        Add("CSSP_GeneralFreezer", filter =>
        {
            filter.SetDisallowAll();
            Allow(filter, Cat("Foods"), true);
            Allow(filter, Thing("Ambrosia"), true);
            Allow(filter, Thing("Beer"), true);
            Allow(filter, Thing("Wort"), true);
            Allow(filter, Thing("MedicineHerbal"), true);
            Allow(filter, Cat("PlantMatter"), true);
            Allow(filter, Cat("CorpsesHumanlike"), true);
            Allow(filter, Cat("CorpsesAnimal"), true);
            Allow(filter, Cat("EggsFertilized"), false);
            Allow(filter, Special("AllowRotten"), false);
        });
        Add("CSSP_FreezerNoMeals", filter =>
        {
            filter.SetDisallowAll();
            Allow(filter, Cat("Foods"), true);
            Allow(filter, Cat("FoodMeals"), false);
            Allow(filter, Thing("Ambrosia"), true);
            Allow(filter, Thing("Beer"), true);
            Allow(filter, Thing("Wort"), true);
            Allow(filter, Thing("MedicineHerbal"), true);
            Allow(filter, Cat("PlantMatter"), true);
            Allow(filter, Cat("CorpsesHumanlike"), true);
            Allow(filter, Cat("CorpsesAnimal"), true);
            Allow(filter, Cat("EggsFertilized"), false);
            Allow(filter, Special("AllowRotten"), false);
        });
        Add("CSSP_Meals", filter =>
        {
            filter.SetDisallowAll();
            Allow(filter, Cat("FoodMeals"), true);
            Allow(filter, Special("AllowRotten"), false);
        });
        Add("CSSP_GeneralStorage", filter =>
        {
            filter.SetDisallowAll();
            Allow(filter, Cat("Manufactured"), true);
            Allow(filter, Thing("Ambrosia"), false);
            Allow(filter, Thing("Beer"), false);
            Allow(filter, Thing("Wort"), false);
            Allow(filter, Cat("ResourcesRaw"), true);
            Allow(filter, Cat("PlantMatter"), false);
            Allow(filter, Cat("EggsFertilized"), true);
            Allow(filter, Cat("Items"), true);
            Allow(filter, Cat("Weapons"), true);
            Allow(filter, Cat("Apparel"), true);
            Allow(filter, Cat("Buildings"), true);
            Allow(filter, Cat("CorpsesMechanoid"), true);
        });
        Add("CSSP_GeneralMedical", filter =>
        {
            filter.SetDisallowAll();
            Allow(filter, Cat("Medicine"), true);
            Allow(filter, Cat("Drugs"), true);
            Allow(filter, Thing("Ambrosia"), false);
            Allow(filter, Thing("Beer"), false);
            Allow(filter, Thing("Neutroamine"), true);
            Allow(filter, Cat("BodyParts"), true);
        });
        Add("CSSP_Drugs", filter =>
        {
            filter.SetDisallowAll();
            Allow(filter, Cat("Drugs"), true);
            Allow(filter, Thing("Ambrosia"), false);
            Allow(filter, Thing("Beer"), false);
            Allow(filter, Thing("Neutroamine"), true);
        });
        Add("CSSP_MaterialsOutside", filter =>
        {
            filter.SetDisallowAll();
            Allow(filter, Cat("StoneBlocks"), true);
            Allow(filter, Thing("Plasteel"), true);
            Allow(filter, Thing("Silver"), true);
            Allow(filter, Thing("Steel"), true);
            Allow(filter, Thing("Uranium"), true);
        });
        Add("CSSP_Materials", filter =>
        {
            filter.SetDisallowAll();
            Allow(filter, Cat("ResourcesRaw"), true);
            Allow(filter, Cat("PlantMatter"), false);
        });
        Add("CSSP_VariousOthers", filter =>
        {
            filter.SetDisallowAll();
            Allow(filter, Cat("Manufactured"), true);
            Allow(filter, Cat("Medicine"), false);
            Allow(filter, Cat("Drugs"), false);
            Allow(filter, Thing("Neutroamine"), false);
            Allow(filter, Thing("Wort"), false);
            Allow(filter, Cat("Items"), true);
            Allow(filter, Cat("BodyParts"), false);
            Allow(filter, Cat("Weapons"), true);
            Allow(filter, Cat("Apparel"), true);
            Allow(filter, Cat("Buildings"), true);
            Allow(filter, Cat("CorpsesMechanoid"), true);
            Allow(filter, Special("AllowDeadmansApparel"), false);
        });
        Add("CSSP_Shells", filter =>
        {
            filter.SetDisallowAll();
            Allow(filter, Cat("MortarShells"), true);
        });
        Add("CSSP_Weapons", filter =>
        {
            filter.SetDisallowAll();
            Allow(filter, Cat("Weapons"), true);
        });
        Add("CSSP_CleanApparel", filter =>
        {
            filter.SetDisallowAll();
            Allow(filter, Cat("Apparel"), true);
            Allow(filter, Special("AllowDeadmansApparel"), false);
        });
        Add("CSSP_TaintedApparel", filter =>
        {
            filter.SetDisallowAll();
            Allow(filter, Cat("Apparel"), true);
            Allow(filter, Special("AllowNonDeadmansApparel"), false);
        });
        Add("CSSP_Chunks", filter =>
        {
            filter.SetDisallowAll();
            Allow(filter, Cat("Chunks"), true);
        });
        Add("CSSP_Rotting", filter =>
        {
            filter.SetDisallowAll();
            Allow(filter, Cat("Apparel"), true);
            Allow(filter, Cat("CorpsesHumanlike"), true);
            Allow(filter, Cat("CorpsesAnimal"), true);
            Allow(filter, Special("AllowFresh"), false);
            Allow(filter, Special("AllowNonDeadmansApparel"), false);
        });
        return list;
    }

    public static void Open(StorageSettings settings, IStoreSettingsParent parent)
    {
        var options = Options(settings, parent);
        if (options.Count == 0)
        {
            return;
        }

        Find.WindowStack.Add(new FloatMenu(options));
    }

    private static void Apply(StorageSettings settings, IStoreSettingsParent parent, Action<ThingFilter> apply)
    {
        apply(settings.filter);
        parent?.Notify_SettingsChanged();
    }

    private static void Allow(ThingFilter filter, ThingCategoryDef def, bool allow)
    {
        if (def != null)
        {
            filter.SetAllow(def, allow);
        }
    }

    private static void Allow(ThingFilter filter, ThingDef def, bool allow)
    {
        if (def != null)
        {
            filter.SetAllow(def, allow);
        }
    }

    private static void Allow(ThingFilter filter, SpecialThingFilterDef def, bool allow)
    {
        if (def != null)
        {
            filter.SetAllow(def, allow);
        }
    }

    private static ThingCategoryDef Cat(string defName) => DefDatabase<ThingCategoryDef>.GetNamedSilentFail(defName);

    private static ThingDef Thing(string defName) => DefDatabase<ThingDef>.GetNamedSilentFail(defName);

    private static SpecialThingFilterDef Special(string defName) => DefDatabase<SpecialThingFilterDef>.GetNamedSilentFail(defName);
}
