using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class UiParticlesController : MonoBehaviour
{
    [Tooltip("Preset particles in scene")]
    public FlyingCoin[] coin;

    [Tooltip("Force Field to gather particle inside")]
    public ParticleSystemForceField[] forceField;

    [Tooltip("Which meter or object to fly into?")]
    public GameObject[] target;

    [Tooltip("Material per target")]
    public Material[] particleMaterial;

    [Tooltip("How many button are we need?")]
    public Button[] btn;

    private void Start()
    {
        int num = 1;
        //cant use normal for to seach for button -> need to figure out
        foreach (Button item in btn)
        {
            item.onClick.AddListener(() =>
            {
                Click(item, num);
            });
            num++;
        }
    }

    private bool ParticleAvailable(int index)
    {
        if (index >= coin.Length) return false;
        if (!coin[index].system.isPlaying) return true;
        return false;
    }

    void Click(Button btn, int numberOfTarget)
    {
        int j = 0;
        //Loop to check for any free particle to avoid particle change destination when playing
        for (int i = 0; i < coin.Length; i++)
        {
            for (int k = 0; k < coin.Length; k++)
            {
                if (!ParticleAvailable(i))
                {
                    if (i >= coin.Length) return;
                    i += numberOfTarget;
                }
                else break;
            }

            coin[i].transform.position = btn.transform.position;
            coin[i].target = target[j].transform;
            coin[i].system.externalForces.AddInfluence(forceField[j]);

            if (coin[i].TryGetComponent(out UIParticleSystem renderCom)) //Prevent allocate fake null value
                renderCom.material = particleMaterial[j];

            
            coin[i].uiParticleSystem.StartParticleEmission();

            if (j >= target.Length - 1) return; //Should stop increasing value greater than index

            j++;
        }
    }
}
