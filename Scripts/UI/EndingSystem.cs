using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EndingSystem : MonoBehaviour
{
    [SerializeField] private Transform endingCanvas;
    [SerializeField] private Sprite gameClear, gameOver;

    private Image background;
    private Image endingImage;
    private SFXController endingSFX;

    private void Awake()
    {
        background = endingCanvas.Find("Background").GetComponent<Image>();
        endingImage = endingCanvas.Find("EndingImage").GetComponent<Image>();
        endingSFX = endingImage.GetComponentInChildren<SFXController>(true);
        background.gameObject.SetActive(false);
        endingImage.gameObject.SetActive(false);
    }

    private void Update()
    {

    }

    public void GameClear()
    {
        endingImage.sprite = gameClear;
        EndingAnimation();
        endingSFX.SettingAudioClip(0);
    }
    public void GameOver()
    {
        endingImage.sprite = gameOver;
        EndingAnimation();
        endingSFX.SettingAudioClip(1);
    }

    private void EndingAnimation()
    {
        background.gameObject.SetActive(true);
        endingImage.gameObject.SetActive(true);

        Sequence seq = DOTween.Sequence();
        seq.Append(background.DOFade(0.7f, 0.5f));
        seq.Join(endingImage.rectTransform.DOScale(1f, 1f));
        seq.AppendInterval(0.5f);
        seq.OnComplete(() => SystemManager.Instance.resultPanel.OpenResultPanel());
    }

}
