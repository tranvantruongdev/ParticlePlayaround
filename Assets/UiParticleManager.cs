using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UiParticleManager : MonoBehaviour
{
    [SerializeField] private GameObject uniformParticle;
    private Queue<GameObject> uniformParticlePool = new Queue<GameObject>();
    private FlyingCoin flyingCoin;

    private void Awake()
    {
        PrepareParticle();
        flyingCoin = FindObjectOfType<FlyingCoin>();
    }

    private void PrepareParticle()
    {
        GameObject particle = Instantiate(uniformParticle, transform.position,
            Quaternion.identity, gameObject.transform);
        particle.SetActive(false);
        uniformParticlePool.Enqueue(particle);
    }

    public void ClickMe()
    {

    }

    public void Pop(string originTag, string destinationTag)
    {
        if (uniformParticlePool.Count>0)
        {
            GameObject particle = uniformParticlePool.Dequeue();
            particle.transform.position = GameObject.FindGameObjectWithTag(originTag).transform.position;
            flyingCoin.Target = GameObject.FindGameObjectWithTag(destinationTag).transform;
            particle.SetActive(true);
        }
    }
}
