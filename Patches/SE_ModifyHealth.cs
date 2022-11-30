namespace EquippedBeltExample.Patches;

public class SE_ModifyHealth : StatusEffect
{
    public float Health;
    private float _origHealth;

    public override void Setup(Character character)
    {
        if (character is Player player)
        {
            _origHealth = player.m_baseHP;
            player.m_baseHP += Health;
        }

        base.Setup(character);
    }

    public override void Stop()
    {
        if (m_character is Player player)
        {
            var formula = player.GetMaxHealth() - Health;
            if (formula < 0)
                formula = _origHealth;
            player.m_baseHP = formula;
        }

        base.Stop();
    }
}