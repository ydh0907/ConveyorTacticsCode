using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Event/SwapEvent")]
public class SwapEvent : EventSO
{
    private List<IMoveable> animals;
    private List<Block> blocks;

    public override void Init()
    {
        animals = new List<IMoveable>();
        blocks = new List<Block>();
    }

    public override void StartEvent()
    {
        animals.Clear();
        blocks.Clear();
        
        animals.AddRange(AnimalManager.Instance.Animals);
        animals.AddRange(EnemyManager.Instance.enemies);

        foreach (var animal in AnimalManager.Instance.Animals)
        {
            blocks.Add(animal.Position);
        }
        
        foreach (var enemy in EnemyManager.Instance.enemies)
        {
            blocks.Add(enemy.position);
        }
        
        
        foreach (var t in animals)
        {
            int random = Random.Range(0, blocks.Count);
            t.Move(blocks[random]);
            (t as ISetable)?.Set(blocks[random]);
            blocks.Remove(blocks[random]);
        }
    }
}