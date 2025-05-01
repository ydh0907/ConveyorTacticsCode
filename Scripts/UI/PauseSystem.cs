using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseSystem : MonoBehaviour
{
    [SerializeField] private RectTransform pausePanel;

    private bool isPanel = false;

    private void Awake()
    {
        Button resumeButton = pausePanel.Find("ResumeBtn").GetComponent<Button>();
        Button restartButton = pausePanel.Find("RestartBtn").GetComponent<Button>();
        Button titleButton = pausePanel.Find("TitleBtn").GetComponent<Button>();

        resumeButton.onClick.AddListener(OnClickResumeButton);
        restartButton.onClick.AddListener(OnClickRestartButton);
        titleButton.onClick.AddListener(OnClickTitleButton);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SetPausePanel(!isPanel);
        }
    }

    private void OnClickResumeButton()
    {
        SetPausePanel(false);
    }

    private void OnClickRestartButton()
    {
        // Scene Reload
        Debug.Log("Scene Reload");
    }

    private void OnClickTitleButton()
    {
        SceneManager.LoadScene("TitleScene");
    }

    private void SetPausePanel(bool isTrue)
    {
        isPanel = isTrue;

        int moveY = 0;
        if (!isTrue) moveY = 1080;

        pausePanel.DOAnchorPosY(moveY, 0.2f).SetEase(Ease.Linear);
    }
}
