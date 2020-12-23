using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct ParticleStruct
{
    public Button[] Btn;

    public Transform StartPos;
    public Transform EndPos;

    public GameObject ParObj;

    public bool RunBool;

    [Range(0.1f, 5.0f)]
    public float time;
    [HideInInspector]
    public float speed;

    public GameObject ParticlePrefab;
}

[System.Serializable]
public class AnimationController
{
    public string animationName;
    public Animation[] anim;
    //[Space]
    
    //public AnimationClip[] clip;
}

public class ScriptParticle : MonoBehaviour
{
    public ParticleStruct[] ParStruct2;
    public GameObject CanvasPar;
    public float GCTime = 5;
    readonly List<ParticleStruct> ListObjPar = new List<ParticleStruct>();
    readonly List<ParticleStruct> ListObjAvail = new List<ParticleStruct>();

    public AnimationController[] animController;
    private void Start()
    {
        foreach (var item in ParStruct2)
        {
            foreach (var item2 in item.Btn)
            {
                item2.onClick.AddListener(() =>
                {
                    Click(item);
                });
            }
        }

        InvokeRepeating(nameof(ClearingPar), 2, GCTime);
    }

    public void ClearingPar()
    {
        for (int i = 0; i < ListObjAvail.Count; i++)
        {
            Destroy(ListObjAvail[i].ParObj);
            ListObjAvail.RemoveAt(i);
        }
    }

    private void Update()
    {
        for (int i = 0; i < ListObjPar.Count; i++)
        {
            if (ListObjPar[i].RunBool)
            {
                ListObjPar[i].ParObj.transform.position = Vector3.MoveTowards(
                    ListObjPar[i].ParObj.transform.position,
                    ListObjPar[i].EndPos.position,
                    ListObjPar[i].speed * Time.deltaTime);

                if (ListObjPar[i].ParObj.TryGetComponent(out ParticleSystem parSys))
                {
                    if (!parSys.isPlaying)
                    {
                        //if (ListObjPar[i].EndPos.gameObject.TryGetComponent(out Animation animation))
                        //        animation.Play();

                        //for (int j = 0; j < animController.anim.Length; j++)
                        //{
                        //    //if (ListObjPar[i].EndPos != ListObjPar[i].StartPos)
                        //    //{
                        //        if (animController.anim[j].gameObject.name == ListObjPar[i].EndPos.gameObject.name)
                        //        {

                        //            animController.anim[j].Play();
                        //        }
                        //    //}
                        //    //else
                        //    //{
                        //    //    animController.anim[0].Play();
                        //    //}
                        //}

                        ListObjPar[i].ParObj.SetActive(false);
                        ListObjAvail.Add(ListObjPar[i]);
                        ListObjPar.Remove(ListObjPar[i]);
                    }
                }
            }
        }
    }

    public void Click(ParticleStruct item)
    {
        if (ListObjAvail.Count == 0)
        {
            GameObject go = Instantiate(item.ParticlePrefab, CanvasPar.transform, false);

            go.transform.position = item.StartPos.position;
            item.ParObj = go;
            item.speed = SetUpSpeedParticle(item);
            item.RunBool = true;
            if (go.TryGetComponent(out ParticleSystem par))
            {
                var particleMain = par.main;
                particleMain.startLifetime = item.time;
                particleMain.duration = item.time;
                par.Play();
            }
            ParticleStruct addItem = item;
            ListObjPar.Add(addItem);
        }
        else
        {
            ParticleStruct itemAvail = ListObjAvail[0];
            Destroy(itemAvail.ParObj);

            GameObject go = Instantiate(item.ParticlePrefab, CanvasPar.transform, false);

            go.transform.position = item.StartPos.position;
            item.ParObj = go;
            item.speed = SetUpSpeedParticle(item);
            item.RunBool = true;

            if (go.TryGetComponent(out ParticleSystem par))
            {
                var particleMain = par.main;
                particleMain.startLifetime = item.time;
                particleMain.duration = item.time;
                par.Play();
            }
            ParticleStruct addItem = item;
            ListObjPar.Add(addItem);

            ListObjAvail.Remove(ListObjAvail[0]);
        }
    }

    public float SetUpSpeedParticle(ParticleStruct par)
    {
        return Vector3.Distance(par.StartPos.position, par.EndPos.position) / (par.time + Time.deltaTime);
    }
}
