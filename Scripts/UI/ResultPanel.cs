using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
    [SerializeField] private Transform resultCanvas;

    private Image leftShutter, rightShutter;
    private TextMeshProUGUI goTitleText;

    private TextMeshProUGUI turnText;
    private TextMeshProUGUI killText;
    private TextMeshProUGUI deadText;
    private TextMeshProUGUI scoreText;

    private SFXController resultSFX;

    private bool isOpen;

    private void Awake()
    {
        leftShutter = resultCanvas.Find("Shutter_Left").GetComponent<Image>();
        rightShutter = resultCanvas.Find("Shutter_Right").GetComponent<Image>();
        goTitleText = resultCanvas.Find("GoTitleTxt").GetComponent<TextMeshProUGUI>();

        turnText = leftShutter.transform.GetChild(0).Find("TurnPanel").GetChild(1).GetComponent<TextMeshProUGUI>();
        killText = leftShutter.transform.GetChild(0).Find("KillPanel").GetChild(1).GetComponent<TextMeshProUGUI>();
        deadText = rightShutter.transform.GetChild(0).Find("DeadPanel").GetChild(1).GetComponent<TextMeshProUGUI>();
        scoreText = rightShutter.transform.GetChild(0).Find("ScorePanel").GetChild(1).GetComponent<TextMeshProUGUI>();

        resultSFX = resultCanvas.GetComponent<SFXController>();
    }

    private void Update()
    {
        if (!isOpen) return;
        if (Input.GetMouseButtonDown(0))
            SceneManager.LoadScene("TitleScene");
    }

    public void OpenResultPanel()
    {
        UpdateResult();
        Sequence seq = DOTween.Sequence();

        seq.Append(leftShutter.rectTransform.DOAnchorPosX(0, 1f));
        seq.Join(rightShutter.rectTransform.DOAnchorPosX(0, 1f));
        seq.AppendCallback(() => resultSFX.SettingAudioClip(0));
        seq.Join(goTitleText.DOFade(0.5f, 0.5f));
        seq.OnComplete(() => isOpen = true);
    }

    private void UpdateResult()
    {
        EnergyManager manager = EnergyManager.Instance;

        turnText.text = manager.CurrentTurn + "/" + manager.MaxTurn + "턴";
        killText.text = manager.KillEnemyCount + "마리";
        deadText.text = manager.DeadPlayer + "/" + manager.MaxPlayer + "마리";
        scoreText.text = manager.GetScore() + "";
    }
}
