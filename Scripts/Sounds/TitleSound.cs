using System.Collections;
using UnityEngine;

public class TitleSound : MonoBehaviour
{
    private AudioSource loopBGMSource;
    private void Awake()
    {
        loopBGMSource = transform.Find("LoopBGM").GetComponent<AudioSource>();
        StartCoroutine(LoopBGM());
    }

    private IEnumerator LoopBGM()
    {
        yield return new WaitForSeconds(6f);
        loopBGMSource.Play();
    }
}
