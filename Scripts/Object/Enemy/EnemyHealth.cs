using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IHealth
{
    public int Health { get; private set; }
    public bool IsDead { get; private set; } = false;
    public Action<int> OnHealthChanged;

    private Enemy owner;
    private SFXController enemySFX;
    private ParticlePlayer particlePlayer;
    [SerializeField] private HealthBar bar;

    public void SetOwner(Enemy enemy)
    {
        owner = enemy;
        if (bar == null)
            bar = GetComponentInChildren<HealthBar>();
    }

    private void Start()
    {
        enemySFX = GetComponentInChildren<SFXController>();
        particlePlayer = GetComponent<ParticlePlayer>();
        Health = owner.Info.Health;
    }

    public void ModifyHealth(int value)
    {
        if (IsDead)
            return;
        Health += value;
        bar.Set(Health / (float)owner.Info.Health * 100 / 100);
        OnHealthChanged?.Invoke(value);
        enemySFX?.SettingAudioClip(1);
        particlePlayer?.Play();
        if (Health <= 0)
        {
            Dead();
        }
    }

    public void Dead()
    {
        IsDead = true;
        Debug.Log("Dead");
        Destroy(gameObject);
        owner.position.SetBlock(null);
        EnemyManager.Instance.enemies.Remove(owner);
    }
}