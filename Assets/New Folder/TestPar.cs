using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestPar : MonoBehaviour
{
    public ParticleSystem System;
    public FlyingCoin Coin;
    public ParticleSystemForceField forceField;
    public Transform BeginTransform;
    public Transform Target;
    [Space]
    public Button Btn;

    private static ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1000];
    private int count;
    private Animation anim;

    private void Awake()
    {
        forceField = Target.GetComponentInChildren<ParticleSystemForceField>();
    }
    private void Start()
    {
        Btn.onClick.AddListener(()=> {
            Click();
        });

    }
    private void LateUpdate()
    {
        if(Target.gameObject.TryGetComponent<Animation>(out Animation animation)){
            anim = animation;
        }
        count = System.GetParticles(particles);
        for (int i = 0; i < count; i++)
        {
            ParticleSystem.Particle particle = particles[i];

            Vector3 v1 = System.transform.TransformPoint(particle.position);
            Vector3 v2 = Target.transform.position;

            Vector3 tarPosi = (v2 - v1) * (particle.remainingLifetime / particle.startLifetime);

            if (tarPosi.y < .1f)
            {
                //set particle's lifetime to negative => remove particle from system
                particle.remainingLifetime = -1f;
                anim.Play();
            }
            particle.position = System.transform.InverseTransformPoint((v2 - tarPosi));
            particles[i] = particle;
        }
        System.SetParticles(particles, count);
    }
    public void Click()
    {

        Coin.Target = Target;
        Coin.system.externalForces.AddInfluence(forceField);
        Coin.system.Play();
        //if (Coin.system.TryGetComponent(out Renderer renderCom)) //Prevent allocate fake null value
        //    renderCom.material = material;

        //    coin[i].system.Play();
        //int j = 0;

        ////Loop to check for any free particle to avoid particle change destination when playing
        //for (int i = 0; i < coin.Length; i++)
        //{
        //    for (int k = 0; k < coin.Length; k++)
        //    {
        //        if (!ParticleAvailable(i))
        //        {
        //            if (i >= coin.Length) return;
        //            i += Target.Length;
        //        }
        //        else break;
        //    }

        //    coin[i].transform.position = transform.position;
        //    coin[i].Target = Target[j];
        //    coin[i].system.externalForces.AddInfluence(forceField[j]);

        //    if (coin[i].system.TryGetComponent(out Renderer renderCom)) //Prevent allocate fake null value
        //        renderCom.material = material[j];

        //    coin[i].system.Play();

        //    if (j >= Target.Length - 1) return; //Should stop increasing value greater than index

        //    j++;
        //}
    }
}
