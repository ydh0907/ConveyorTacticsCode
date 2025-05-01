using System;
using UnityEngine;
using UnityEngine.Events;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager Instance { get; private set; }
    private int _currentTurn;
    public int MaxEnergy = 100;
    public int MaxTurn; 
    public int MaxEnemyCount, KillEnemyCount;
    public int ActiveCount; // 행동한 횟수
    public int MaxPlayer = 3, DeadPlayer;

    public UnityEvent<int> OnTurnChanged;

    public int CurrentTurn
    {
        get => _currentTurn;
        set
        {
            _currentTurn = value;
            OnTurnChanged?.Invoke(_currentTurn);
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    public char GetScore()
    {
        DeadPlayer = AnimalManager.Instance.Animals.Count;

        int value = 30 + Mathf.Min(MaxTurn - CurrentTurn, 0) * 2 + KillEnemyCount - DeadPlayer * 10;

        if (value >= 50) return 'A';
        else if (value >= 40) return 'B';
        else if (value >= 30) return 'C';
        else if (value >= 20) return 'D';
        else if (value >= 10) return 'E';
        else return 'F';
    } 
}
