using UnityEngine;

public interface IHealthUpdateReceiver
{
    void GetHurt(int currentHealth, int maxHealth);
    void GetHealed(int currentHealth, int maxHealth);
    void GetKilled();
}
