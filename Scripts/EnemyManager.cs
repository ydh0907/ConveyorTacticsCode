using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : TurnHandler
{
    public static EnemyManager Instance;

    public List<Enemy> enemies;

    private void Awake()
    {
        Instance = this;
    }

    public override async UniTask<bool> HandleTurn()
    {
        if (AnimalManager.Instance.IsGameOver())
        {
            Debug.Log("Game Over");
            SystemManager.Instance.endingSystem.GameOver();
            return false;
        }
        if (IsGameClear())
        {
            Debug.Log("Game Clear");
            SystemManager.Instance.endingSystem.GameClear();
            return false;
        }
        foreach (var enemy in enemies)
        {
            await enemy.HandleTurn();
        }
        return true;
    }

    public bool IsGameClear()
    {
        return enemies.Count == 0 && GameManager.Instance.isInitEnd &&
               EnergyManager.Instance.CurrentTurn >= EnergyManager.Instance.MaxTurn;
    }
}
