using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestPar : MonoBehaviour
{
    public ParticleSystem System;
    public FlyingCoin[] Coin;
    public ParticleSystemForceField[] forceField;
    public Transform BeginTransform;
    public Transform[] Target;
    public Material[] particleMaterial;
    [Space]
    public Button[] Btn;

    private static ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1000];
    private int count;
    private Animation anim;
    private ParticleSystem.Particle particle;

    private void LateUpdate()
    {
        for (int k = 0; k < Target.Length; k++)
        {
            if (Target[k].gameObject.TryGetComponent<Animation>(out Animation animation))
            {
                anim = animation;
            }

            count = System.GetParticles(particles);
            for (int i = 0; i < count; i++)
            {
                particle = particles[i];

                Vector3 v1 = System.transform.TransformPoint(particle.position);
                Vector3 v2 = Target[k].transform.position;

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
    }

    private bool ParticleAvailable(int index)
    {
        if (index >= Coin.Length) return false;
        if (!Coin[index].system.isPlaying) return true;
        return false;
    }

    public void Click(string tag)
    {
        switch (tag)
        {
            case "BtnGold":
                //Loop to check for any free particle to avoid particle change destination when playing
                for (int i = 0; i < Coin.Length; i++)
                {
                    for (int k = 0; k < Coin.Length; k++)
                    {
                        if (!ParticleAvailable(i))
                        {
                            if (i >= Coin.Length - 1) return;
                            i += 1;
                        }
                        else break;
                    }

                    Coin[i].transform.position = Btn[1].transform.position;
                    Coin[i].target = Target[1];
                    Coin[i].system.externalForces.AddInfluence(forceField[1]);

                    if (Coin[i].system.TryGetComponent(out Renderer renderCom)) //Prevent allocate fake null value
                        renderCom.material = particleMaterial[1];

                    Coin[i].system.Play();
                    break;
                }
                break;

            case "BtnWolfnGold":
                int j = 0;
                //Loop to check for any free particle to avoid particle change destination when playing
                for (int i = 0; i < Coin.Length; i++)
                {
                    for (int k = 0; k < Coin.Length; k++)
                    {
                        if (!ParticleAvailable(i))
                        {
                            if (i >= Coin.Length) return;
                            i += Target.Length;
                        }
                        else break;
                    }

                    Coin[i].transform.position = Btn[0].transform.position;
                    Coin[i].target = Target[j];
                    Coin[i].system.externalForces.AddInfluence(forceField[j]);

                    if (Coin[i].system.TryGetComponent(out Renderer renderCom)) //Prevent allocate fake null value
                        renderCom.material = particleMaterial[j];

                    Coin[i].system.Play();

                    if (j >= Target.Length - 1) return; //Should stop increasing value greater than index

                    j++;
                }
                break;

            default:
                Debug.Log("There is no tag: " + tag);
                break;
        }
    }
}
