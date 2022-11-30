using HarmonyLib;

namespace EquippedBeltExample.Patches;

//Original method zero's out Eitr there is no saved based variable and is itr through food buffs
//Not sure why IronGate would do this? Its as Eitr is solely relying on food only
[HarmonyPatch(typeof(Player), nameof(Player.GetTotalFoodValue))]
public static class GetTotalFoodValuePatch
{
    public static void Postfix(Player __instance, ref float eitr)
    {
        if (!__instance.GetSEMan().HaveStatusEffect("BeltModifyStats"))
            return;
        eitr += __instance.m_maxEitr;
    }
}