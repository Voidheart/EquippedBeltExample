using System;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using EquippedBeltExample.Patches;
using HarmonyLib;
using UnityEngine;

namespace EquippedBeltExample;

[BepInPlugin(ModGUID, ModName, ModVersion)]
public class EquippedBeltExamplePlugin : BaseUnityPlugin
{
    internal const string ModName = "EquippedBeltExample";
    internal const string ModVersion = "1.0.0";
    internal const string Author = "azumatt";
    private const string ModGUID = Author + "." + ModName;

    internal static string ConnectionError = "";

    public static readonly ManualLogSource LogSource =
        BepInEx.Logging.Logger.CreateLogSource(ModName);

    private readonly Harmony _harmony = new(ModGUID);

    public void Awake()
    {
        Item beltHealthUpgrade = new("AssetFileNameGoesHere", "belthealthupgrade");

        var prefab = beltHealthUpgrade.Prefab;
        if (prefab == null)
        {
            LogSource.LogError("Prefab is null");
            return;
        }

        //Some useful values if anyone was interested
        //StaffSkeleton requires 100 Eitr for 1 summon
        //StaffFireball requires 35 Eitr for 1 attack
        //StaffIceShards requires 5 Eitr for 1 attack
        //StaffShield requires 60 Eitr for 1 attack
        SetupBelt_ModifyStatsBy(prefab, 500f, 200f, 150f);

        var assembly = Assembly.GetExecutingAssembly();
        _harmony.PatchAll(assembly);
    }

    //Creates Scriptable Object for the Belt
    public static void SetupBelt_ModifyStatsBy(GameObject prefab, float health = 0.0f, float stamina = 0.0f,
        float eitr = 0.0f)
    {
        var mItemData = prefab.GetComponent<ItemDrop>()?.m_itemData ?? throw new ArgumentNullException(nameof(prefab));

        var healthUpgrade = ScriptableObject.CreateInstance<SE_ModifyStats>();
        if (healthUpgrade == null) return;

        healthUpgrade.name = "BeltModifyStats";
        healthUpgrade.m_name = "$item_belthealthupgrade";
        healthUpgrade.m_icon = mItemData.GetIcon();
        healthUpgrade.m_tooltip = "$item_belthealthupgrade_tooltip";
        healthUpgrade.m_startMessage = "$item_belthealthupgrade_startmessage";
        healthUpgrade.m_stopMessage = "$item_belthealthupgrade_stopmessage";

        healthUpgrade.Health = health;
        healthUpgrade.Stamina = stamina;
        healthUpgrade.Eitr = eitr;
        mItemData.m_shared.m_equipStatusEffect = healthUpgrade;
    }
}
