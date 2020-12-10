using System.Collections.Generic;
using UnityEngine;

public class UiParticleManager : MonoBehaviour
{
    public int maxParticle = 2;

    [SerializeField] private GameObject uniformParticle;
    private Queue<GameObject> uniformParticlePool = new Queue<GameObject>();

    private void Awake()
    {
        PrepareParticle();
    }

    private void PrepareParticle()
    {
        GameObject particle;
        for (int i = 0; i < maxParticle; i++)
        {
            particle = Instantiate(uniformParticle, transform.position,
                Quaternion.identity, gameObject.transform);
            uniformParticlePool.Enqueue(particle);
        }
    }
}
