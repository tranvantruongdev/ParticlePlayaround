using UnityEngine;

public class FlyingCoin : MonoBehaviour
{
    public Transform target;

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
        if (target == null)
        {
            return;
        }

        anim = target.gameObject.GetComponent<Animation>();
        count = system.GetParticles(particles);

        for (int i = 0; i < count; i++)
        {
            ParticleSystem.Particle particle = particles[i];

            Vector3 v1 = system.transform.TransformPoint(particle.position);
            Vector3 v2 = target.transform.position;

            Vector3 tarPosi = (v2 - v1) * (particle.remainingLifetime / particle.startLifetime);

            //if (tarPosi.sqrMagnitude < 10)
            //{
            //    //set particle's lifetime to negative => remove particle from system
            //    particle.remainingLifetime = -1f;
            //    anim.Play();
            //}

            particle.position = system.transform.InverseTransformPoint((v2 - tarPosi));
            particles[i] = particle;

            Debug.Log(particle.velocity.ToString());
        }
        system.SetParticles(particles, count);
    }
}
