using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingCoin : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particleSystem;

    private void OnEnable()
    {
        particleSystem.Play();
    }

    private void OnDisable()
    {
        particleSystem.Clear();
    }
}
