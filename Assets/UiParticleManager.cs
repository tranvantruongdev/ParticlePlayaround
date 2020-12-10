using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class UiParticleManager : MonoBehaviour
{
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
