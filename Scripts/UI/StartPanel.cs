using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartPanel : MonoBehaviour
{
    [SerializeField] private Transform startCanvas;

    private Button startButton;
    private Button exitButton;
    private Image fadeImage;

    private Lever lever;
    private bool isStart = false;

    private void Awake()
    {
        Transform buttonBox = startCanvas.Find("Box");

        startButton = buttonBox.Find("StartBtn").GetComponent<Button>();
        exitButton = buttonBox.Find("ExitBtn").GetComponent<Button>();
        fadeImage = startCanvas.Find("Fade").GetComponent<Image>();
        lever = startCanvas.Find("Lever").GetComponent<Lever>();

        ButtonSettings();
    }

    private void ButtonSettings()
    {
        startButton.onClick.AddListener(OnClickStartButton);
        exitButton.onClick.AddListener(OnClickExitButton);
    }

    private void OnClickStartButton()
    {
        if (isStart) return;
        isStart = true;
        Sequence seq = DOTween.Sequence();
        seq.Append(lever.LeftMoveLever());
        seq.Append(fadeImage.rectTransform.DOAnchorPos(Vector2.zero, 1f));
        seq.OnComplete(() => SceneManager.LoadScene("GameScene"));
    }
    private void OnClickExitButton()
    {
        if(isStart) return;
        isStart = true;
        Sequence seq = DOTween.Sequence();
        seq.Append(lever.RightMoveLever());
        seq.OnComplete(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });
    }

}
