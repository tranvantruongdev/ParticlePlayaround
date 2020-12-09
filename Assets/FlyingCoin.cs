using UnityEngine;

public class FlyingCoin : MonoBehaviour
{
	public Transform Target;

	private ParticleSystem system;

	private static ParticleSystem.Particle[] particles = new ParticleSystem.Particle[100];
	int count;
	Animation anim;

	void Start()
	{
		if (system == null)
			system = GetComponent<ParticleSystem>();

		if (system == null)
		{
			enabled = false;
		}
		anim = Target.gameObject.GetComponent<Animation>();
	}

	void LateUpdate()
	{
		count = system.GetParticles(particles);
		bool timeToStop = false;

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
				timeToStop = true;
				anim.Play();
            }
			timeToStop = false;
            particle.position = system.transform.InverseTransformPoint((v2 - tarPosi) );
			particles[i] = particle;
		}
        if (timeToStop)
        {
			enabled = false;
        }
		system.SetParticles(particles, count);
	}

	private void OnEnable()
    {
		system.Play();
    }

    private void OnDisable()
    {
		system.Clear();
    }
}
