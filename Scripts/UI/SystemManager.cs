using UnityEngine;

public class SystemManager : MonoBehaviour
{
    public static SystemManager Instance;

    public SkillManager skillManager { get; private set; }
    public ActiveBar activeBar { get; private set; }
    public ActivePanel activePanel { get; private set; }
    public Tooltip tooltip{ get; private set; }
    public TurnPanel turnPanel { get; private set; }
    public PauseSystem pauseSystem { get; private set; }
    public EndingSystem endingSystem { get; private set; }
    public ResultPanel resultPanel { get; private set; }


    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        skillManager = GetComponentInChildren<SkillManager>();
        activeBar = GetComponentInChildren<ActiveBar>();
        activePanel = transform.parent.GetChild(0).GetComponentInChildren<ActivePanel>();
        tooltip = GetComponentInChildren<Tooltip>();
        turnPanel = GetComponentInChildren<TurnPanel>();
        pauseSystem = GetComponentInChildren<PauseSystem>();
        endingSystem = GetComponentInChildren<EndingSystem>();
        resultPanel = GetComponentInChildren<ResultPanel>();
    }
}
