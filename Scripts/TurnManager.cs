using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;

    public List<TurnHandler> TurnHandlers;
    public int TurnCount { get; private set; } = 0;
    public CancellationTokenSource Stop = new();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        TurnStart().Forget();
    }

    public async UniTaskVoid TurnStart()
    {
        while (true)
            foreach (var turnHandler in TurnHandlers)
            {
                bool active = await turnHandler.HandleTurn();
                if (!active || Stop.IsCancellationRequested)
                    return;
            }
    }
}
