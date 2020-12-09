﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingCoin : MonoBehaviour
{
	public Transform Target;

	private ParticleSystem system;

	private static ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1000];
	int count;

	void Start()
	{
		if (system == null)
			system = GetComponent<ParticleSystem>();

		if (system == null)
		{
			enabled = false;
		}
	}

	void LateUpdate()
	{
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
            }
            particle.position = system.transform.InverseTransformPoint((v2 - tarPosi) );
			particles[i] = particle;
		}
		system.SetParticles(particles, count);
	}

	private void OnEnable()
    {
        //particleSystem.Play();
    }

    private void OnDisable()
    {
        //particleSystem.Clear();
    }
}
