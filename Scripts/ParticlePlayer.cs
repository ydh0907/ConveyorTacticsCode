using UnityEngine;

public class ParticlePlayer : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _particleSystems;
    [SerializeField] private ParticleSystem[] _particleSystems2;
    
    public void Play()
    {
        foreach (var p in _particleSystems)
        {
            p.Stop();
        }
        foreach (var particleSystem in _particleSystems)
        {
            particleSystem.Play();
        }
    }
    
    public void Play2()
    {
        foreach (var p in _particleSystems2)
        {
            p.Stop();
        }
        foreach (var particleSystem in _particleSystems2)
        {
            particleSystem.Play();
        }
    }
}