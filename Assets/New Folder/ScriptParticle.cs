using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct ParticleStruct
{
    public Button Btn;
    public Transform StartPos;
    public Transform EndPos;
    public ParticleSystem ParSys;
    public float time;
    public float speed;
}

public class ScriptParticle : MonoBehaviour
{
    public ParticleStruct ParStruct = new ParticleStruct();
    bool run = false;

    private void Start()
    {
        ParStruct.Btn.onClick.AddListener(() => Click());
    }

    public void Click()
    {
        if (ParStruct.ParSys.isPlaying)
        {
            ParStruct.ParSys.Clear();
            ParStruct.ParSys.Stop();
        }

        ParStruct.ParSys.transform.position = ParStruct.StartPos.position;
        var particleMain = ParStruct.ParSys.main;
        particleMain.startLifetime = ParStruct.time;
        ParStruct.speed = Vector3.Distance(ParStruct.StartPos.position, 
            ParStruct.EndPos.position) / (ParStruct.time + Time.deltaTime);
        ParStruct.ParSys.Play();
        run = true;
    }

    private void Update()
    {
        if (ParStruct.ParSys.transform.position == ParStruct.EndPos.position)
        {
            run = false;
        }

        if (run == true)
        {
            ParStruct.ParSys.transform.position =
                Vector3.MoveTowards(ParStruct.ParSys.transform.position,
                ParStruct.EndPos.position, ParStruct.speed * Time.deltaTime);
        }
    }
}
