using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "SO/Event/BombEvent")]
public class BombEvent : EventSO
{
    public int damage;
    public ParticleSystem _bombParticle;
    public Material _blockMaterial;
    private List<List<Block>> blocks;
    private List<IMoveable> entities;
    private int x;
    private int y;

    public override void Init()
    {
        x = MapManager.Instance.maxLine;
        y = MapManager.Instance.maxBlock;
        blocks = new List<List<Block>>();
        entities = new List<IMoveable>();
        EnergyManager.Instance.OnTurnChanged.AddListener(EndEventHandler);
    }

    private void EndEventHandler(int obj)
    {
        entities = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).Select(x => x as IMoveable).ToList();
        foreach (var entity in entities)
        {
            if (ReferenceEquals(entity, null))
            {
                Debug.Log("Entity is null");
                continue;
            }
            if (entity.IsBlockMove)
            {
                entity.IsBlockMove = false;
            }
        }
        
        foreach (var block in blocks)
        {
            block.RemoveAll(x => x is null || x.Equals(null));
            foreach (var b in block.Where(b => b is not null))
            {
                if (b.Equals(null)) continue;
                b?.ChangeMaterial(b.DefaultMat);
                b?.SetBlocked(false);
            }
        }
        blocks.Clear();
    }

    public override void StartEvent()
    {
        int centerX = Random.Range(0, x);
        int centerY = Random.Range(0, y);

        blocks.Clear();

        for (int i = centerX - 2; i <= centerX + 2; i++)
        {
            for (int j = centerY - 2; j <= centerY + 2; j++)
            {
                if (i < 0 || i >= x || j < 0 || j >= y)
                {
                    continue;
                }
                blocks.Add(new List<Block> {MapManager.Instance.GetBlock(i, j)});
            }
        }
        
        foreach (var block in blocks)
        {
            foreach (var b in block)
            {
                var particle = Instantiate(_bombParticle, b.transform.position, Quaternion.identity);
                particle.Play();
                b?.ChangeMaterial(_blockMaterial);
                b?.SetBlocked(true);
                EntityTakeDamage(b);
            }
        }
    }

    private void EntityTakeDamage(Block b)
    {
        if (b.Current is null) return;
        var entity = b.Current as MonoBehaviour;
        entity.GetComponent<IHealth>().ModifyHealth(-damage);
        entity.GetComponent<IMoveable>().IsBlockMove = true;
        PushEntity(entity as IMoveable, b);
    }

    private void PushEntity(IMoveable moveable, Block block)
    {
        Vector2Int dir = Vector2Int.right;
        Block target = MapManager.Instance.GetBlock(block.Idx + dir);
        if (target is null || target.Current is not null)
        {
            return;
        }
        moveable.Move(target).Forget();
    }
}