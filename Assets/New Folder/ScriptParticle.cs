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
    [Range(0.1f, 5.0f)]
    public float time;
    [HideInInspector]
    public float speed;
    public Queue<GameObject> UiParticlePool { get; set; }
    public Queue<GameObject> UiPressedParticlePool { get; set; }
}

public class ScriptParticle : MonoBehaviour
{
    //public ParticleStruct ParStruct = new ParticleStruct();
    public ParticleStruct[] ParStruct2;
    bool run = false;
    int currentIndex = 0;

    private void Start()
    {
        //ParStruct.Btn.onClick.AddListener(() => Click());
        for (int i = 0; i < ParStruct2.Length; i++)
        {
            int x = i;
            ParStruct2[i].Btn.onClick.AddListener(() => Click(x));
        }

        //allocate some pool
        ParStruct2[0].UiParticlePool = new Queue<GameObject>();
        ParStruct2[0].UiPressedParticlePool = new Queue<GameObject>();
    }

    private void AddUiParticle(int amout)
    {
        for (int i = 0; i < amout; i++)
        {
            GameObject particle;
            particle = Instantiate(ParStruct2[0].ParSys.gameObject, transform);
            particle.SetActive(false);
            ParStruct2[0].UiParticlePool.Enqueue(particle);
        }
    }

    public void Click(int index)
    {
        currentIndex = index;
        if (ParStruct2[index].ParSys.isPlaying)
        {
            ParStruct2[index].ParSys.Clear();
            ParStruct2[index].ParSys.Stop();
        }

        ParStruct2[index].ParSys.transform.position = ParStruct2[index].StartPos.position;
        var particleMain = ParStruct2[index].ParSys.main;
        particleMain.startLifetime = ParStruct2[index].time;
        ParStruct2[index].speed = Vector3.Distance(ParStruct2[index].StartPos.position,
            ParStruct2[index].EndPos.position) / (ParStruct2[index].time + Time.deltaTime);
        ParStruct2[index].ParSys.Play();
        run = true;
    }

    private void Update()
    {
        if (ParStruct2[currentIndex].ParSys.transform.position == ParStruct2[currentIndex].EndPos.position)
        {
            run = false;
        }

        if (run == true)
        {
            ParStruct2[currentIndex].ParSys.transform.position =
                Vector3.MoveTowards(ParStruct2[currentIndex].ParSys.transform.position,
                ParStruct2[currentIndex].EndPos.position, ParStruct2[currentIndex].speed * Time.deltaTime);
        }
    }

    #region Single Version
    //public void Click()
    //{
    //    if (ParStruct.ParSys.isPlaying)
    //    {
    //        ParStruct.ParSys.Clear();
    //        ParStruct.ParSys.Stop();
    //    }

    //    ParStruct.ParSys.transform.position = ParStruct.StartPos.position;
    //    var particleMain = ParStruct.ParSys.main;
    //    particleMain.startLifetime = ParStruct.time;
    //    ParStruct.speed = Vector3.Distance(ParStruct.StartPos.position,
    //        ParStruct.EndPos.position) / (ParStruct.time + Time.deltaTime);
    //    ParStruct.ParSys.Play();
    //    run = true;
    //}

    //private void Update()
    //{
    //    if (ParStruct.ParSys.transform.position == ParStruct.EndPos.position)
    //    {
    //        run = false;
    //    }

    //    if (run == true)
    //    {
    //        ParStruct.ParSys.transform.position =
    //            Vector3.MoveTowards(ParStruct.ParSys.transform.position,
    //            ParStruct.EndPos.position, ParStruct.speed * Time.deltaTime);
    //    }
    //}
    #endregion
}
