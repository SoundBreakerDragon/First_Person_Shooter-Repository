using UnityEngine;

public class DummyControl : MonoBehaviour, IHealthUpdateReceiver
{
    void IHealthUpdateReceiver.GetHealed(int currentHealth, int maxHealth)
    {
        print("Dummy got healed");
    }

    void IHealthUpdateReceiver.GetHurt(int currentHealth, int maxHealth)
    {
        print("Dummy took damage");
    }

    void IHealthUpdateReceiver.GetKilled()
    {
        print("Dummy destroyed");
        Destroy(gameObject);
    }
}
