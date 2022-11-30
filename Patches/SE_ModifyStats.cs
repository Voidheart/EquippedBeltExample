using UnityEngine;

namespace EquippedBeltExample.Patches;

public class SE_ModifyStats : StatusEffect
{
    public float Health;
    public float Stamina;
    public float Eitr;
    private float _orgEitr;

    private float _origHealth;
    private float _origStamina;

    public override void Setup(Character character)
    {
        //We use Base stats because GetTotalFoodValue will overwrite if we use API like SetMaxHealth() etc in Update calls
        if (character is Player player)
        {
            _origHealth = player.m_baseHP;
            _origStamina = player.m_baseStamina;
            _orgEitr = player.m_maxEitr;

            player.m_baseHP += Health;
            player.m_baseStamina += Stamina;
            player.m_maxEitr += Eitr;
        }

        base.Setup(character);
    }


    public override void Stop()
    {
        if (m_character is Player player)
        {
            player.m_baseHP = Mathf.Clamp(player.m_baseHP, _origHealth, player.m_baseHP - Health);
            player.m_baseStamina = Mathf.Clamp(player.m_baseStamina, _origStamina, player.m_baseStamina - Stamina);
            player.m_maxEitr = Mathf.Clamp(player.m_maxEitr, _orgEitr, player.m_maxEitr - Eitr);
        }

        base.Stop();
    }
}