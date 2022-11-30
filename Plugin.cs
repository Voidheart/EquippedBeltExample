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
        Item beltHealthUpgrade = new("mybeltasset", "belthealthupgrade");
        // beltHealthUpgrade.Snapshot();

        var prefab = beltHealthUpgrade.Prefab;
        if (prefab == null)
        {
            LogSource.LogError("Prefab is null");
            return;
        }

        SetupBelt_IncreaseHealthBy(prefab, 500f);

        var assembly = Assembly.GetExecutingAssembly();
        _harmony.PatchAll(assembly);
    }

    //Creates Scriptable Object for the Belt
    //Had to use custom SE because SE_HealthUpgrade would reset, possibly ZDO related
    public static void SetupBelt_IncreaseHealthBy(GameObject prefab, float health = 0.0f)
    {
        var mItemData = prefab.GetComponent<ItemDrop>()?.m_itemData ?? throw new ArgumentNullException(nameof(prefab));

        var healthUpgrade = ScriptableObject.CreateInstance<SE_ModifyHealth>();
        if (healthUpgrade != null)
        {
            healthUpgrade.name = "SE_BeltHealthUpgrade";
            healthUpgrade.m_name = "$item_belthealthupgrade";
            healthUpgrade.m_icon = mItemData.GetIcon();
            healthUpgrade.m_tooltip = "$item_belthealthupgrade_tooltip";
            healthUpgrade.m_startMessage = "$item_belthealthupgrade_startmessage";
            healthUpgrade.m_stopMessage = "$item_belthealthupgrade_stopmessage";

            healthUpgrade.Health = health;
            mItemData.m_shared.m_equipStatusEffect = healthUpgrade;
        }
    }
}