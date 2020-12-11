using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestPar2 : MonoBehaviour
{
    public FlyingCoin Coin;
    public ParticleSystemForceField forceField;
    public GameObject Target;
    //public Material[] particleMaterial;
    [Space]
    public Button[] Btn;

    private void Start()
    {
        foreach (Button item in Btn)
        {
            item.onClick.AddListener(() =>
            {
                click(item);
            });
        }
    }

    void click(Button btn)
    {
        Coin.transform.position = btn.transform.position;
        Coin.target = Target.transform;
        Debug.Log(Coin.target.transform.position.ToString());
        //forceField = target.gameObject.GetComponentInChildren<ParticleSystemForceField>();
        //Coin.system.externalForces.AddInfluence(forceField);
        Coin.system.Play();
    }
}
