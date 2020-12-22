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
    public GameObject ParObj;
    public bool RunBool;
    [Range(0.1f, 5.0f)]
    public float time;
    [HideInInspector]
    public float speed;
    public float CleaningInterval;
    public GameObject ParticlePrefab;
}

public class ScriptParticle : MonoBehaviour
{
    public ParticleStruct[] ParStruct2;
    public GameObject CanvasPar;
    public float GCTime = 5;

    List<ParticleStruct> ListObjPar = new List<ParticleStruct>();
    List<ParticleStruct> ListObjAvail = new List<ParticleStruct>();

    private void Start()
    {
        foreach (var item in ParStruct2)
        {
            item.Btn.onClick.AddListener(() =>
            {
                Click(item);
            });
        }

        InvokeRepeating(nameof(ClearingPar), 2, GCTime);
    }

    public void ClearingPar()
    {
        for (int i = 1; i < ListObjAvail.Count; i++)
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

            itemAvail.ParObj.SetActive(true);
            itemAvail.StartPos = item.StartPos;
            itemAvail.EndPos = item.EndPos;
            itemAvail.time = item.time;

            itemAvail.ParObj.transform.position = item.StartPos.position;
            itemAvail.RunBool = true;

            if (itemAvail.ParObj.TryGetComponent(out ParticleSystem par))
            {
                var particleMain = par.main;
                particleMain.startLifetime = item.time;
                particleMain.duration = item.time;
                par.Play();
            }

            itemAvail.speed = SetUpSpeedParticle(itemAvail);

            ListObjPar.Add(itemAvail);

            ListObjAvail.Remove(ListObjAvail[0]);
        }
    }

    public float SetUpSpeedParticle(ParticleStruct par)
    {
        return Vector3.Distance(par.StartPos.position, par.EndPos.position) / (par.time + Time.deltaTime);
    }

}
