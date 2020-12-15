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
    [SerializeField] private int maxParticle;

    private Queue<GameObject> uiParticlePool = new Queue<GameObject>();

    private void Awake()
    {
        PrepareParticle();
    }

    private void PrepareParticle()
    {
        for (int i = 0; i < maxParticle; i++)
        {
            GameObject particle;
            particle = Instantiate(uiParticle, transform);
            particle.SetActive(false);
            uiParticlePool.Enqueue(particle);
        }
    }

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
        for (int j = 0; j < target.Length; j++)
        {
            if (uiParticlePool.Count > 0)
            {
                int x = j;
                GameObject particle = uiParticlePool.Dequeue();
                particle.SetActive(true);

                Coffee.UIExtensions.UIParticle uIParticles = particle.GetComponent<Coffee.UIExtensions.UIParticle>();
                uIParticles.Play();
                if (target[j].TryGetComponent(out ParticleSystemForceField particleSystemForceField))
                {
                    particle.GetComponentInChildren<ParticleSystem>()
                        .externalForces.AddInfluence(particleSystemForceField);
                }
                particle.transform.position = btn.transform.position;
                particle.transform.DOMove(target[j].transform.position, 1).SetEase(Ease.OutQuad)
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
}
