using System;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public List<Block> blocks;
    public Action destroyNext = null;

    private int lineIndex = 0;
    public int LineIndex
    {
        get => lineIndex;
        set => lineIndex = value;
    }

    [SerializeField] private float blockDistance = 1.1f;

    public Block this[int idx]
    {
        get => blocks[idx];
    }

    public void MakeBlocks()
    {
        for (int i = 0; i < blocks.Count; i++)
        {
            blocks[i] = Instantiate(blocks[i], new Vector3(transform.position.x, transform.position.y, i * blockDistance), Quaternion.identity, transform);
            destroyNext += blocks[i].destroyNext;
        }
    }
}
