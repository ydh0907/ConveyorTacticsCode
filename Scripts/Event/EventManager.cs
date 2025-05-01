using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }
    [SerializeField] private EventListSO EventListSO;
    [SerializeField] public int _turnCnt;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    

    private void Start()
    {
        EventListSO.Init();
        EnergyManager.Instance.OnTurnChanged.AddListener(StartRandomEvent);
    }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         StartRandomEvent(1);
    //     }
    // }

    public void StartRandomEvent(int turn)
    {
        if (turn % _turnCnt == 0)
        {
            var index = Random.Range(0, EventListSO.Events.Count);
            EventListSO.StartEvent(index);
        }
    }
}
