using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class UiParticleManager : MonoBehaviour
{
#pragma warning disable 0649
    public int maxParticle = 2;

    [SerializeField] private GameObject uniformParticle;

    private void Awake()
    {
        PrepareParticle();
    }

    private void PrepareParticle()
    {
        for (int i = 0; i < maxParticle; i++)
        {
            LeanPool.Spawn(uniformParticle, transform.position,
                Quaternion.identity, gameObject.transform);
        }
    }
}
