using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Stage")]
public class StageSO : ScriptableObject
{
    public Line line;
    public Block block;
    public List<Enemy> enemys;
    public List<Obstacle> obstacles;
    public float enemyPerLine;
    public float obstaclePerLine;
}
