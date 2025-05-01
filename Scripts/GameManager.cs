using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Vector2Int spawnPos;
    [SerializeField] private Animal test1;
    [SerializeField] private Animal test2;
    [SerializeField] private Animal test3;
    public bool isInitEnd;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnCharacter()
    {
        isInitEnd = false;
        Block block1 = MapManager.Instance.GetBlock(spawnPos);
        Animal inst1 = Instantiate(test1, block1.transform.position + Vector3.up, Quaternion.Euler(0, 90, 0));
        inst1.Set(block1);

        spawnPos.x += 1;
        Block block2 = MapManager.Instance.GetBlock(spawnPos);
        Animal inst2 = Instantiate(test2, block2.transform.position + Vector3.up, Quaternion.Euler(0, 90, 0));
        inst2.Set(block2);
        
        spawnPos.x += 1;
        Block block3 = MapManager.Instance.GetBlock(spawnPos);
        Animal inst3 = Instantiate(test3, block3.transform.position + Vector3.up, Quaternion.Euler(0, 90, 0));
        inst3.Set(block3);
        isInitEnd = true;
        Debug.Log("Init End");
    }

    public void HandleAnimalDead()
    {

    }
}
