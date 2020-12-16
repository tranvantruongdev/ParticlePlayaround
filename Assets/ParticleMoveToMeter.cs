using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//suppress null default warning
#pragma warning disable 0649
public class ParticleMoveToMeter : MonoBehaviour
{
    [SerializeField] private GameObject uiParticle;
    [SerializeField] private Button[] buttons;
    [SerializeField] private GameObject[] target;
    [SerializeField] private Ease ease = Ease.OutQuad;
    [Range(1, 100)]
    [SerializeField] private int numberParticle = 5;
    [SerializeField] private Material[] particleMaterial;
    [SerializeField] private float duration = 1f;

    private readonly Queue<GameObject> uiParticlePool = new Queue<GameObject>();

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
        //create some UiParticle if dont have enough
        if (!CheckUiParticle())
        {
            AddUiParticle(target.Length - uiParticlePool.Count);
        }

        for (int j = 0; j < target.Length; j++)
        {
            if (uiParticlePool.Count > 0)
            {
                //get closure value from for loop and pass to anonymous lamda function
                int x = j;

                GameObject particle = uiParticlePool.Dequeue();
                particle.SetActive(true);

                particle.transform.position = btn.transform.position;

                //set number particle to emit, material display and play it
                if (particle.TryGetComponent(out Coffee.UIExtensions.UIParticle uIParticle))
                {
                    List<ParticleSystem> listUiParticles = uIParticle.particles;

                    ParticleSystem.MainModule uiParticleMain = listUiParticles[0].main;
                    uiParticleMain.maxParticles = numberParticle;

                    if (listUiParticles[0].TryGetComponent(out ParticleSystemRenderer particleSystemRenderer))
                        particleSystemRenderer.material = particleMaterial[x];

                    uIParticle.Play();
                }

                //move uiParticle object to target position within duration time with ease type
                particle.transform.DOMove(target[j].transform.position, duration)
                .SetEase(ease)
                .OnComplete(() =>
                {
                    if (target[x].TryGetComponent(out Animator animator))
                        animator.SetTrigger("pop");

                    particle.SetActive(false);
                    uiParticlePool.Enqueue(particle);
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