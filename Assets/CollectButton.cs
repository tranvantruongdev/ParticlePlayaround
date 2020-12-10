using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectButton : MonoBehaviour
{
    public Transform Target;
    public ParticleSystemForceField forceField;
    public Material material;
    private FlyingCoin coin;

    // Start is called before the first frame update
    void Start()
    {
        coin = FindObjectOfType<FlyingCoin>();
    }

    public void Click()
    {
        coin.transform.position = transform.position;
        coin.Target = Target;
        coin.system.externalForces.AddInfluence(forceField);
        coin.system.GetComponent<Renderer>().material = material;
        coin.system.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
