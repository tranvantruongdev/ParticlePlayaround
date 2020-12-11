using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
	public Transform Target;

	public ParticleSystem system;

	public Transform PosTest;
    private static ParticleSystem.Particle[] particles = new ParticleSystem.Particle[100];

    int count;

	void Start()
	{
		if (system == null)
			system = GetComponent<ParticleSystem>();

		if (system == null)
		{
			this.enabled = false;
		}
		else
		{
			system.Play();
		}
	}
	[ContextMenu("GetTargetPos")]
	public void GetTargetPos()
    {
		Debug.Log("PosTest.transform.position: " + PosTest.transform.position);
    }
	void Update()
	{

		count = system.GetParticles(particles);

		for (int i = 0; i < count; i++)
		{
			ParticleSystem.Particle particle = particles[i];

			Vector3 v1 =system.transform.TransformPoint(particle.position);
            //Vector3 v1 =system.transform.TransformPoint(particle.position);
            Vector3 v2 = Target.transform.position;
            //Vector3 v2 = Target.transform.position;


            Vector3 tarPosi = (v2 - v1) * (particle.remainingLifetime / particle.startLifetime);
			Debug.Log("v2-v1: " + (v2 - v1));
			particle.position = system.transform.InverseTransformPoint(v2 - tarPosi);
			particles[i] = particle;
			
    //        if (v1==v2)
    //        {
				////particle.remainingLifetime = -1f;
				//system.Pause();
    //            return;
    //        }
    //        else
    //        {
				system.SetParticles(particles, count);
			//}
            //Debug.Log("tarPosi: "+ tarPosi);
			
		}

        
    }
}
