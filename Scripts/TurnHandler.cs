using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class TurnHandler : MonoBehaviour
{
    public abstract UniTask<bool> HandleTurn();
}