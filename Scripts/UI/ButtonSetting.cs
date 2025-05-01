using UnityEngine;
using UnityEngine.EventSystems;

public enum SkillType
{
    skill_1,
    skill_2,
}
public class ButtonSetting : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public SkillType skillType;
    public void OnPointerEnter(PointerEventData eventData)
    {
        SystemManager.Instance.tooltip.ShowTooltip(true, skillType);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SystemManager.Instance.tooltip.ShowTooltip(false, skillType);
    }
}
