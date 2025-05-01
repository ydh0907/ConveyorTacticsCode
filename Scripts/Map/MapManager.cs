using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class MapManager : TurnHandler
{
    public static MapManager Instance;

    public List<StageSO> stages;
    public List<Line> current = new List<Line>();

    public int conveyorCount = 0;
    public int ConveyorCount => conveyorCount;
    public int nextStageCount = 3;

    [Header("Map Size")]
    public int maxLine = 12;
    public int maxBlock = 9;
    public int safeZone = 5;
    [SerializeField] private float lineDistance = 1.1f;

    private void Awake()
    {
        Instance = this;
    }

    private async void Start()
    {
        await SetUpMap();
    }

    private async UniTask SetUpMap()
    {
        for (int i = current.Count; i < maxLine; i++)
        {
            await NextLine();
        }
        GameManager.Instance.SpawnCharacter();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
            NextLine().Forget();
    }

    public Block GetBlock(int x, int y)
    {
        if (x >= 0 && x < maxLine && y >= 0 && y < maxBlock)
            return current[x][y];
        return null;
    }
    public Block GetBlock(Vector2Int pos) => GetBlock(pos.x, pos.y);

    public Vector2Int GetIdx(Block block)
    {
        for (int x = 0; x < current.Count; x++)
            for (int y = 0; y < current[x].blocks.Count; y++)
                if (current[x][y] == block)
                    return new Vector2Int(x, y);
        return -Vector2Int.one;
    }

    private async UniTask MakeLine(int idx)
    {
        Line line = Instantiate(stages[idx].line, new Vector3(current.Count * lineDistance, 1, 0), Quaternion.identity, transform);
        line.LineIndex = current.Count;
        current.Add(line);

        for (int i = 0; i < maxBlock; i++)
            line.blocks.Add(stages[idx].block);
        line.MakeBlocks();

        if (current.Count > safeZone)
            SetObject(line, idx);

        await line.transform.DOMoveY(0, 0.2f).SetEase(Ease.OutBounce).ToUniTask();
    }

    private async UniTask PullLine()
    {
        Func<UniTask> actions = null;
        foreach (var line in current)
        {
            actions += () => MoveLine(line);
        }

        if (actions != null)
            await actions.Invoke();

        await Delete();
    }

    private async UniTask Delete()
    {
        Destroy(current[0].gameObject);
        current.RemoveAt(0);
        current[0]?.destroyNext?.Invoke();

        conveyorCount++;
        await UniTask.Delay(500);
    }

    private async UniTask MoveLine(Line line)
    {
        await line.transform.DOMoveX(lineDistance * --line.LineIndex, 0.3f).ToUniTask();
    }

    public async UniTask NextLine()
    {
        if (conveyorCount / nextStageCount >= stages.Count)
            return;
        await MakeLine(conveyorCount / nextStageCount);
        if (current.Count > maxLine)
            await PullLine();
    }

    public void SetObject(Line line, int idx)
    {
        float ec = stages[idx].enemyPerLine;
        float oc = stages[idx].obstaclePerLine;
        for (int i = 0; i < line.blocks.Count; i++)
        {
            if (CheckSpawn(ec, line.blocks.Count))
            {
                Enemy enemy = Instantiate(stages[idx].enemys[Random.Range(0, stages[idx].enemys.Count)]);
                enemy.Set(line.blocks[i]);
                ec--;
            }
            else if (CheckSpawn(oc, line.blocks.Count))
            {
                Obstacle obstacle = Instantiate(stages[idx].obstacles[Random.Range(0, stages[idx].obstacles.Count)]);
                obstacle.Set(line.blocks[i]).Forget();
                oc--;
            }
        }
    }

    public bool CheckSpawn(float count, float max)
    {
        return Random.Range(0f, max) < count;
    }

    public override async UniTask<bool> HandleTurn()
    {
        await NextLine();
        return true;
    }
}
