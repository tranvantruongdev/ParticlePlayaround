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
        //int num = 1;
        ////Cant use normal for to seach for button -> need to figure out
        //foreach (Button item in btn)
        //{
        //    item.onClick.AddListener(() => Click(item, num));
        //    num++;
        //}

        foreach (Button item in btn)
        {
            item.onClick.AddListener(() => Click(item));
        }
    }

    private void OnDestroy()
    {
        //int num = 1;
        ////Remove listener on each button
        //foreach (Button item in btn)
        //{
        //    item.onClick.RemoveListener(() => Click(item, num));
        //    num++;
        //}

        foreach (Button item in btn)
        {
            item.onClick.RemoveListener(() => Click(item));
        }
    }

    private bool ParticleAvailable(int index)
    {
        if (index >= coin.Length) return false;
        if (!coin[index].system.isPlaying) return true;
        return false;
    }

    void Click(Button btn)
    {
        int j = 0;
        int emitedParticle = 0;

        for (int i = 0; i < coin.Length; i++)
        {
            //Enough particle aldready
            //if (emitedParticle >= numberOfTarget)
            //{
            //    return;
            //}

            //Loop to check for any free particle to avoid particle change destination when playing
            //for (int k = 0; k < coin.Length; k++)
            //{
            //    if (!ParticleAvailable(i))
            //    {
            //        if (i >= coin.Length) return;
            //        //i += numberOfTarget;
            //        i += target.Length;
            //    }
            //    else break;
            //}

            coin[i].transform.position = btn.transform.position;
            coin[i].target = target[j].transform;
            coin[i].system.externalForces.AddInfluence(forceField[j]);

            //Prevent allocate fake null value
            if (coin[i].TryGetComponent(out UIParticleSystem renderCom))
                renderCom.material = particleMaterial[j];

            //Add count value to track number of particle so that wont spam greater number particle than target number
            emitedParticle++;

            coin[i].uiParticleSystem.StartParticleEmission();

            //Should stop increasing value greater than index
            if (j >= target.Length - 1) return;

            j++;
        }
    }
}
