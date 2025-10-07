using UnityEngine;

public class CharacterHealth : MonoBehaviour, iFactionControlReciever
{
    public int maxHealth = 50;
    public int startingHealth = -1; //If we choose -1 it will use max health as starting health
    int currentHealth = 0;
    bool dead = false;

    IHealthUpdateReceiver[] healthUpdateReceivers;

    [Header("GUI")]
    public HealthBarControl healthBarGUI;

    FactionManager FactionManager;
    public void InjectFactionControl(FactionManager factionManager)
    {
        this.FactionManager = factionManager;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (startingHealth == -1)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth = startingHealth;
        }

        healthUpdateReceivers = GetComponents<IHealthUpdateReceiver>();

        UpdateHealthGUI();
    }

    public bool IsAlive()
    {
        return dead == false;
    }

    public bool CanReceiveDamage(Faction attackingFaction)
    {
        return FactionManager == null
            || attackingFaction != FactionManager.faction 
            || FactionManager.faction == Faction.None;
    }

    void UpdateHealthGUI()
    {
        if (healthBarGUI != null)
        {
            healthBarGUI.UpdateHealthBar(currentHealth, maxHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        print($"{gameObject.name} took {damage} points of damage");
        ChangeHealth(-damage);

    }

    public void Heal(int healing)
    {
        print($"{gameObject.name} healed {healing} amount of points");
        ChangeHealth(healing);
    }

    void ChangeHealth(int value)
    {
        if (dead)
        {
            return;
        }

        currentHealth += value;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthGUI();
        
        if (healthUpdateReceivers.Length > 0)
        {
            ProcessHealthUpdateRecivers(value);
        }


    }

    void ProcessHealthUpdateRecivers(int changeValue)
    {
        if (currentHealth <= 0)
        {
            SendDeath();
        }
        else if (changeValue < 0)
        {
            SendDamage();
        }
        else if (changeValue > 0)
        {
            SendHeal();
        }
    }

    void SendDeath()
    {
        dead = true;
        for (int i = 0; i < healthUpdateReceivers.Length; i++)
        {
            healthUpdateReceivers[i].GetKilled();
        }

    }

    void SendDamage()
    {
        for (int i = 0; i < healthUpdateReceivers.Length; i++)
        {
            healthUpdateReceivers[i].GetHurt(currentHealth, maxHealth);
        }
    }

    void SendHeal()
    {
        for (int i = 0; i < healthUpdateReceivers.Length; i++)
        {
            healthUpdateReceivers[i].GetHealed(currentHealth, maxHealth);
        }
    }


}
