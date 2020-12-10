﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectButton : MonoBehaviour
{
    public Transform[] Target;
    public ParticleSystemForceField[] forceField;
    public Material[] material;
    private FlyingCoin[] coin;

    // Start is called before the first frame update
    void Start()
    {
        coin = FindObjectsOfType<FlyingCoin>();
    }

    private bool ParticleAvailable(int index)
    {
        if (index >= coin.Length)
        {
            return false;
        }

        if (!coin[index].system.isPlaying)
        {
            return true;
        }

        return false;
    }

    public void Click()
    {
        int j = 0;

        for (int i = 0; i < coin.Length; i++)
        {
            for (int k = 0; k < coin.Length; k++)
            {
                if (!ParticleAvailable(i))
                {
                    if (i >= coin.Length)
                    {
                        return;
                    }
                    i += Target.Length;
                }
                else
                {
                    break;
                }
            }

            coin[i].transform.position = transform.position;
            coin[i].Target = Target[j];
            coin[i].system.externalForces.AddInfluence(forceField[j]);
            coin[i].system.GetComponent<Renderer>().material = material[j];
            coin[i].system.Play();

            if (j >= Target.Length -1)
            {
                return;
            }

            j++;
        }
    }
}
