using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ParticleMoveToMeter : MonoBehaviour
{
    [SerializeField] private GameObject uiParticle;
    [SerializeField] private Button[] buttons;
    [SerializeField] private GameObject[] target;
    [SerializeField] private int maxParticleInPool;
    [SerializeField] private Ease ease;
    [Range(1, 100)]
    [SerializeField] private int numberParticle;
    [SerializeField] private Material[] particleMaterial;

    private Queue<GameObject> uiParticlePool = new Queue<GameObject>();

    private void Awake()
    {
        //PrepareParticle();
    }

    //private void PrepareParticle()
    //{
    //    for (int i = 0; i < maxParticleInPool; i++)
    //    {
    //        GameObject particle;
    //        particle = Instantiate(uiParticle, transform);
    //        particle.SetActive(false);
    //        uiParticlePool.Enqueue(particle);
    //    }
    //}

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            //get closure value from for loop and pass to anonymous lamda function
            int x = i;
            buttons[x].onClick.AddListener(() => ClickEmitParticle(buttons[x]));
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            //get closure value from for loop and pass to anonymous lamda function
            int x = i;
            buttons[x].onClick.RemoveListener(() => ClickEmitParticle(buttons[x]));
        }
    }

    public void ClickEmitParticle(Button btn)
    {
        if (!CheckUiParticle())
        {
            AddUiParticle(target.Length - uiParticlePool.Count);
        }

        for (int j = 0; j < target.Length; j++)
        {
            if (uiParticlePool.Count > 0)
            {
                int x = j;
                GameObject particle = uiParticlePool.Dequeue();
                particle.SetActive(true);
                var mainParticle = particle.GetComponentInChildren<ParticleSystem>().main;
                mainParticle.maxParticles = numberParticle;
                particle.transform.position = btn.transform.position;
                if (particle.TryGetComponent(out Coffee.UIExtensions.UIParticle uIParticle))
                {
                    uIParticle.material = particleMaterial[x];
                    uIParticle.Play();
                }
                particle.transform.DOMove(target[j].transform.position, 1, true)
                .SetEase(ease)
                .OnComplete(() =>
                {
                    if (target[x].TryGetComponent(out Animator animator))
                    {
                        animator.SetTrigger("pop");
                        particle.SetActive(false);
                        uiParticlePool.Enqueue(particle);
                    }
                });
            }
        }
    }

    private void AddUiParticle(int amout)
    {
        for (int i = 0; i < amout; i++)
        {
            GameObject particle;
            particle = Instantiate(uiParticle, transform);
            particle.SetActive(false);
            uiParticlePool.Enqueue(particle);
        }
    }

    private bool CheckUiParticle()
    {
        return uiParticlePool.Count >= target.Length;
    }
}
