using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActivePanel : MonoBehaviour, IPointerExitHandler
{
    private Image activePanel;
    private Button skill_1Button;
    private Button skill_2Button;

    private bool isPanel = false;

    private void Start()
    {
        activePanel = GetComponent<Image>();
        skill_1Button = activePanel.transform.Find("Skill_1Btn").GetComponent<Button>();
        skill_2Button = activePanel.transform.Find("Skill_2Btn").GetComponent<Button>();
    }

    public void UpdatePlayerSkill()
    {
        PlayerInfoSO info = SystemManager.Instance.skillManager.GetPlayerInfo();

        SkillButtonSetting(skill_1Button.transform, info.skill_1);
        SkillButtonSetting(skill_2Button.transform, info.skill_2);
    }

    private void SkillButtonSetting(Transform trm, SkillSet skill)
    {
        Image skillIcon = trm.Find("Icon").GetComponent<Image>();
        TextMeshProUGUI nameText = trm.Find("Name").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI activePowerText = trm.Find("ActivePower").GetComponent<TextMeshProUGUI>();

        skillIcon.sprite = skill.skillIcon;
        nameText.text = skill.skillName;
        activePowerText.text = "Çàµ¿·Â -" + skill.activePower;
    }

    public void ShowActivePanel(bool isTrue)
    {
        int moveY = 0;
        if (!isTrue) moveY = 550;

        activePanel.rectTransform.DOAnchorPosX(moveY, 0.2f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ShowActivePanel(false);
    }
}
