using System.Collections;
using UnityEngine;

public class Particle : MonoBehaviour, IPoolable
{
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    public void Initialize()
    {
        _particleSystem.Play();
    }
}
