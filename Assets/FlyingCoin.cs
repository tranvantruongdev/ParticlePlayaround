using UnityEngine;

public class FlyingCoin : MonoBehaviour
{
    public Transform Target;

    public ParticleSystem system;

    private static ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1000];

    private int count;

    private Animation anim;

    void Start()
    {
        if (system == null)
            system = GetComponent<ParticleSystem>();
    }

    void LateUpdate()
    {
        anim = Target.gameObject.GetComponent<Animation>();
        count = system.GetParticles(particles);

        for (int i = 0; i < count; i++)
        {
            ParticleSystem.Particle particle = particles[i];

            Vector3 v1 = system.transform.TransformPoint(particle.position);
            Vector3 v2 = Target.transform.position;

            Vector3 tarPosi = (v2 - v1) * (particle.remainingLifetime / particle.startLifetime);

            if (tarPosi.y < .1f)
            {
                //set particle's lifetime to negative => remove particle from system
                particle.remainingLifetime = -1f;
                anim.Play();
            }
            particle.position = system.transform.InverseTransformPoint((v2 - tarPosi));
            particles[i] = particle;
        }
        system.SetParticles(particles, count);
    }
}
