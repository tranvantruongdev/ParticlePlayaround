using UnityEngine;
using UnityEngine.UI.Extensions;

public class FlyingCoin : MonoBehaviour
{
    public Transform target;
    public ParticleSystem system;
    private static ParticleSystem.Particle[] particles = new ParticleSystem.Particle[5];
    public UIParticleSystem uiParticleSystem;
    private int count;

    private Animator anim;

    void Start()
    {
        if (system == null)
            system = GetComponent<ParticleSystem>();
        if (TryGetComponent(out UIParticleSystem uiPar))
        {
            uiParticleSystem = uiPar;
        }
    }

    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        anim = target.gameObject.GetComponent<Animator>();
        count = system.GetParticles(particles);

        for (int i = 0; i < count; i++)
        {
            ParticleSystem.Particle particle = particles[i];

            Vector3 v1 = system.transform.TransformPoint(particle.position);
            Vector3 v2 = target.transform.position;

            Vector3 tarPosi = (v2 - v1) * (particle.remainingLifetime / particle.startLifetime);

            if (Vector3.Distance(v1, v2) < 15f)
            {
                //set particle's lifetime to negative => remove particle from system
                particle.remainingLifetime = -1f;
                anim.SetTrigger("pop");
            }

            particle.position = system.transform.InverseTransformPoint((v2 - tarPosi));
            particles[i] = particle;
        }
        system.SetParticles(particles, count);
    }
}
