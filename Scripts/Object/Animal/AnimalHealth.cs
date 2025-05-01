using UnityEngine;

public class AnimalHealth : MonoBehaviour, IHealth
{
    [field: SerializeField] public int Maxhealth { get; private set; } = 100;
    [field: SerializeField] public int health { get; private set; }

    [SerializeField] private HealthBar healthBar;
    private SFXController animalSFX;

    private Animal owner;

    private void Awake()
    {
        animalSFX = transform.GetChild(0).GetComponentInChildren<SFXController>();

        health = Maxhealth;
        owner = GetComponent<Animal>();
        if (!healthBar)
            healthBar = GetComponentInChildren<HealthBar>();
    }

    public void Dead()
    {
        animalSFX.SettingAudioClip(5);
        owner.Dead();
    }

    public void ModifyHealth(int value)
    {
        health = Mathf.Clamp(health + value, 0, Maxhealth);
        healthBar?.Set((float)health / Maxhealth);
        animalSFX.SettingAudioClip(4);
        if (health <= 0)
        {
            Dead();
        }
    }
}
