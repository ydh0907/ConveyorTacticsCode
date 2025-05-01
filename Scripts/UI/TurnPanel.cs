using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnPanel : MonoBehaviour
{
    [SerializeField] private RectTransform turnPanel;
    [SerializeField] private Button turnNextButton;

    private TextMeshProUGUI turnCountText;
    private TextMeshProUGUI turnOwnerText;

    private bool isPlayerTurn = false;

    private void Awake()
    {
        turnCountText = turnPanel.Find("TurnCountTxt").GetComponent<TextMeshProUGUI>();
        turnOwnerText = turnPanel.Find("TurnOwnerTxt").GetComponent<TextMeshProUGUI>();
        turnNextButton.onClick.AddListener(ChangeTurn);
    }   

    public void ChangeTurn()
    {
        AnimalManager manager = AnimalManager.Instance;
        if (manager.Current && manager.Current.IsActing || manager.Skill && manager.Skill.OnSkill)
            return;

        string turnOwner = string.Empty;
        isPlayerTurn = !isPlayerTurn;
        
        if (isPlayerTurn)
        {
            EnergyManager.Instance.CurrentTurn++;
            turnNextButton.gameObject.SetActive(true);
            turnOwner = "MY TURN";
        }
        else
        {
            turnNextButton.gameObject.SetActive(false);
            turnOwner = "ENEMY TURN";
            SystemManager.Instance.activeBar.ResetActiveBar();
        }

        turnCountText.text = EnergyManager.Instance.CurrentTurn + "/" + EnergyManager.Instance.MaxTurn;
        turnOwnerText.text = turnOwner;
    }
}
