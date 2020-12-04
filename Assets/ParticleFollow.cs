using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFollow : MonoBehaviour
{
    public Camera cam;
    public Transform target;
	public ParticleSystem m_System;
	ParticleSystem.Particle[] m_Particles;
	int index;

	[Tooltip("Cap the maximum speed to prevent particle from being flung too far from the missed target.")]
	public float maxSpeed = 50f;

	private void LateUpdate()
    {
        Vector3 targetPos = cam.ScreenToWorldPoint(new Vector3(target.position.x, 
            target.position.y, target.position.z * -1));

		int numParticlesAlive = m_System.GetParticles(m_Particles);
		for (int i = 0; i < numParticlesAlive; i++)
		{
			float ted = (target.position - this.transform.position).sqrMagnitude + 0.001f;
			Vector3 diff = target.position - m_Particles[i].position;
			float diffsqrm = diff.sqrMagnitude;
			float face = Vector3.Dot(m_Particles[i].velocity.normalized, diff.normalized);
			float f = Mathf.Abs((ted - diffsqrm) / ted) * ted * (face + 1.001f);
			//m_Particles[i].velocity = Vector3.ClampMagnitude(m_Particles[i].velocity + diff * speed * 0.01f * f, maxSpeed);
			float t = 0;
			t += Time.deltaTime;
			m_Particles[i].velocity = Vector3.ClampMagnitude(
				Vector3.Slerp(m_Particles[i].velocity, m_Particles[i].velocity + diff * 0.01f * f, t), 
				maxSpeed);			
		}
		m_System.SetParticles(m_Particles, numParticlesAlive);
	}

    // Start is called before the first frame update
    void Start()
    {
		//m_System = GetComponent<ParticleSystem>();
		m_Particles = new ParticleSystem.Particle[m_System.main.maxParticles];// Since Unity 5.5
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
