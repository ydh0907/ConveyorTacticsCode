using UnityEngine;
using UnityEngine.EventSystems;

public class ActiveButton : MonoBehaviour, IPointerEnterHandler
{
    private ActivePanel activePanel;

    private void Awake()
    {
        activePanel = GetComponentInParent<ActivePanel>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(SystemManager.Instance.skillManager.GetPlayerInfo() != null) 
        activePanel.ShowActivePanel(true);
    }
}
