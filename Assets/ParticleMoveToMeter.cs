using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//suppress null default warning
#pragma warning disable 0649
public class ParticleMoveToMeter : MonoBehaviour
{
    [Tooltip("Struct holds infomation about uiParticle")]
    [SerializeField]
    private UiParticleStruct uiParticleStruct = new UiParticleStruct();

    private void Start()
    {
        uiParticleStruct.UiParticlePool = new Queue<GameObject>();

        for (int i = 0; i < uiParticleStruct.Buttons.Length; i++)
        {
            //get closure value from for loop and pass to anonymous lamda function
            int x = i;
            uiParticleStruct.Buttons[x].onClick.AddListener(() => ClickEmitParticle(uiParticleStruct.Buttons[x]));
        }

        InvokeRepeating(nameof(CleanUiParticleOvertime),
            uiParticleStruct.CleaningInterval, uiParticleStruct.CleaningInterval);
    }

    private void CleanUiParticleOvertime()
    {
        for (int i = 0; i < uiParticleStruct.UiParticlePool.Count; i++)
        {
            var uiParticle = uiParticleStruct.UiParticlePool.Dequeue();
            if (uiParticle.activeSelf == false)
            {
                Destroy(uiParticle);
            }
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < uiParticleStruct.Buttons.Length; i++)
        {
            //get closure value from for loop and pass to anonymous lamda function
            int x = i;
            uiParticleStruct.Buttons[x].onClick.RemoveListener(() => ClickEmitParticle(uiParticleStruct.Buttons[x]));
        }
    }

    public void ClickEmitParticle(Button btn)
    {
        //create some UiParticle if dont have enough
        if (!CheckUiParticle())
        {
            AddUiParticle(uiParticleStruct.Target.Length - uiParticleStruct.UiParticlePool.Count);
        }

        for (int j = 0; j < uiParticleStruct.Target.Length; j++)
        {
            if (uiParticleStruct.UiParticlePool.Count > 0)
            {
                //get closure value from for loop and pass to anonymous lamda function
                int x = j;

                GameObject particle = uiParticleStruct.UiParticlePool.Dequeue();
                particle.SetActive(true);

                particle.transform.position = btn.transform.position;

                //set number particle to emit, material display and play it
                if (particle.TryGetComponent(out Coffee.UIExtensions.UIParticle uIParticle))
                {
                    List<ParticleSystem> listUiParticles = uIParticle.particles;

                    ParticleSystem.MainModule uiParticleMain = listUiParticles[0].main;
                    uiParticleMain.maxParticles = uiParticleStruct.NumberParticle;

                    if (listUiParticles[0].TryGetComponent(out ParticleSystemRenderer particleSystemRenderer))
                        particleSystemRenderer.material = uiParticleStruct.ParticleMaterial[x];

                    uIParticle.Play();
                }

                //move uiParticle object to target position within duration time with ease type
                particle.transform.DOMove(uiParticleStruct.Target[j].transform.position, uiParticleStruct.Duration)
                .SetEase(uiParticleStruct.Ease)
                .OnComplete(() =>
                {
                    if (uiParticleStruct.Target[x].TryGetComponent(out Animation animation))
                        animation.Play();

                    particle.SetActive(false);
                    uiParticleStruct.UiParticlePool.Enqueue(particle);
                });
            }
        }
    }

    private void AddUiParticle(int amout)
    {
        for (int i = 0; i < amout; i++)
        {
            GameObject particle;
            particle = Instantiate(uiParticleStruct.UiParticle, transform);
            particle.SetActive(false);
            uiParticleStruct.UiParticlePool.Enqueue(particle);
        }
    }

    private bool CheckUiParticle()
    {
        return uiParticleStruct.UiParticlePool.Count >= uiParticleStruct.Target.Length;
    }
}