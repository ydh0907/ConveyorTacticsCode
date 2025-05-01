using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private Image tooltip;

    private TextMeshProUGUI descriptionText;

    private bool isTooltip;

    private void Awake()
    {
        descriptionText = tooltip.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        if(isTooltip)
        {
            if (Screen.width/2 < tooltip.rectTransform.anchoredPosition.x + tooltip.rectTransform.sizeDelta.x)
            {
                tooltip.rectTransform.pivot = new Vector2(1, 1);
            }
            else
            {
                tooltip.rectTransform.pivot = new Vector2(0, 1);
            }

            tooltip.rectTransform.position = Input.mousePosition;

        }
    }

    public void ShowTooltip(bool isTrue, SkillType type)
    {
        PlayerInfoSO info = SystemManager.Instance.skillManager.GetPlayerInfo();

        isTooltip = isTrue;
        tooltip.gameObject.SetActive(isTrue);
        switch(type)
        {
            case SkillType.skill_1:
                descriptionText.text = info.skill_1.skillDescription;
                break;
            case SkillType.skill_2:
                descriptionText.text = info.skill_2.skillDescription;
                break;
        }
    }
}
