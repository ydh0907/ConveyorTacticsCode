using UnityEngine;

public class SFXController : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] clips;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void SettingAudioClip(int num)
    {
        audioSource.clip = clips[num];
        audioSource.Play();
    }
}
