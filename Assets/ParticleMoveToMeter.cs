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
        uiParticleStruct.UiPressedParticlePool = new Queue<GameObject>();

        //prewarm some uiParticle
        AddUiParticle(uiParticleStruct.NumberPrewarmParticle);

        //prewarm some uiPressedParticle
        AddUiPressedParticle(uiParticleStruct.NumberPrewarmParticle);

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
        GameObject uiParticle;
        int loopTimes = uiParticleStruct.UiParticlePool.Count;
        for (int i = 1; i < loopTimes; i++)
        {
            uiParticle = uiParticleStruct.UiParticlePool.Dequeue();
            if (!uiParticle.activeInHierarchy)
            {
                Destroy(uiParticle);
            }
        }

        int loopTimes2 = uiParticleStruct.UiPressedParticlePool.Count;
        for (int i = 1; i < loopTimes2; i++)
        {
            uiParticle = uiParticleStruct.UiPressedParticlePool.Dequeue();
            if (!uiParticle.activeInHierarchy)
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

        if (uiParticleStruct.UiPressedParticlePool.Count < 1)
        {
            AddUiPressedParticle(1);
        }

        GameObject pressParticle = uiParticleStruct.UiPressedParticlePool.Dequeue();
        pressParticle.SetActive(true);

        pressParticle.transform.position = btn.transform.position;

        //set number particle to emit, material display and play it
        if (pressParticle.TryGetComponent(out Coffee.UIExtensions.UIParticle uIPressedParticle))
            uIPressedParticle.Play();

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

                    ParticleSystem.ShapeModule uiParticleShape = listUiParticles[0].shape;
                    uiParticleShape.radius = uiParticleStruct.BaseRadius
                        + uiParticleStruct.Spread * 0.5f;

                    ParticleSystem.VelocityOverLifetimeModule uiParticleVelocityOverLifetime
                        = listUiParticles[0].velocityOverLifetime;
                    uiParticleVelocityOverLifetime.radialMultiplier
                        = uiParticleStruct.BaseRadialmultipler - uiParticleStruct.Spread;

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

    private void AddUiPressedParticle(int amout)
    {
        for (int i = 0; i < amout; i++)
        {
            GameObject particle;
            particle = Instantiate(uiParticleStruct.UiPressedParticle, transform);
            particle.SetActive(false);
            uiParticleStruct.UiPressedParticlePool.Enqueue(particle);
        }
    }

    private bool CheckUiParticle()
    {
        return uiParticleStruct.UiParticlePool.Count >= uiParticleStruct.Target.Length;
    }
}