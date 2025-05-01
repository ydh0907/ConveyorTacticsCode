using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActiveBar : MonoBehaviour
{
    [SerializeField] private RectTransform fillBar;

    private Image realBar;
    private Image fakeBar;
    private TextMeshProUGUI valueText;

    private float maxBarValue;
    private float currenBarValue;
    private float minusValue;

    private Tween fakeTween;

    private void Awake()
    {
        realBar = fillBar.Find("Bar").GetComponent<Image>();
        fakeBar = fillBar.Find("FakeBar").GetComponent<Image>();
        valueText = fillBar.Find("ValueText").GetComponent<TextMeshProUGUI>();

        maxBarValue = (int)realBar.rectTransform.sizeDelta.y;
        currenBarValue = maxBarValue;
        UpdateValueText();
    }

    private void Update()
    {
        if (currenBarValue <= 0 && minusValue == 0)
        {
            SystemManager.Instance.turnPanel.ChangeTurn();
            if (AnimalManager.Instance.Current && !AnimalManager.Instance.Current.IsActing || AnimalManager.Instance.Skill && !AnimalManager.Instance.Skill.OnSkill)
            {
                AnimalManager.Instance.EndTurn();
            }
        }
    }

    public bool CheckMinusActiveValue()
    {
        if (currenBarValue < 0)
        {
            Debug.Log("행동력이 부족합니다.");
            return false;
        }
        return true;
    }

    public void CheckMinusBar(int percent)
    {
        float value = maxBarValue * (percent / 100.0f);

        currenBarValue -= value;
        minusValue += value;

        if (!CheckMinusActiveValue()) return;

        UpdateValueText();

        BarValueSetting(realBar, currenBarValue);

        fakeTween = fakeBar.DOColor(new Color(1, 1, 1, 0.1f), 1f).SetLoops(-1, LoopType.Yoyo);
    }

    public void CancelBarValue()
    {
        fakeTween.Kill();

        currenBarValue += minusValue;
        minusValue = 0;

        UpdateValueText();

        BarValueSetting(realBar, currenBarValue);
    }

    public void CompleteBarValue()
    {
        fakeTween.Kill();

        minusValue = 0;

        BarValueSetting(fakeBar, currenBarValue);
    }

    public void ResetActiveBar()
    {
        currenBarValue = maxBarValue;
        minusValue = 0;
        UpdateValueText();
        BarValueSetting(fakeBar, currenBarValue);
        BarValueSetting(realBar, currenBarValue);
    }

    public void BarValueSetting(Image bar, float value)
    {
        Vector2 barVec = bar.rectTransform.sizeDelta;
        bar.rectTransform.sizeDelta = new Vector2(barVec.x, value);
    }

    public void UpdateValueText()
    {
        valueText.text = (int)(currenBarValue / (float)maxBarValue * 100) + "";
    }
}
