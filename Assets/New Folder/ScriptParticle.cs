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
}

public class ScriptParticle : MonoBehaviour
{
    public ParticleStruct[] ParStruct2;

    public GameObject ParticlePrefab;
    public GameObject ParticleIdlePrefab;

    public GameObject CanvasPar;

    List<ParticleStruct> ListObjPar = new List<ParticleStruct>();
    List<ParticleStruct> ListObjAvail = new List<ParticleStruct>();

    List<ParticleStruct> ListIdleObjPar = new List<ParticleStruct>();
    List<ParticleStruct> ListIdleObjAvail = new List<ParticleStruct>();

    private void Start()
    {
        foreach (var item in ParStruct2)
        {
            item.Btn.onClick.AddListener(() =>
            {
                Click(item);
            });
        }
        InvokeRepeating(nameof(CleanUiParticleOvertime), ParStruct2[0].CleaningInterval,
            ParStruct2[0].CleaningInterval);
    }

    private void Update()
    {
        for (int i = 0; i < ListObjPar.Count; i++)
        {
            if (ListObjPar[i].RunBool && ListObjPar[i].ParObj != null)
            {
                ListObjPar[i].ParObj.transform.position = Vector3.MoveTowards(
                    ListObjPar[i].ParObj.transform.position,
                    ListObjPar[i].EndPos.position,
                    ListObjPar[i].time * Time.deltaTime * 10);

                if (ListObjPar[i].ParObj.transform.position == ListObjPar[i].EndPos.position)
                {
                    ParticleStruct item = ListObjPar[i];
                    item.RunBool = false;
                    item.ParObj.SetActive(false);
                    ListObjPar[i] = item;

                    ListObjAvail.Add(item);
                    ListObjPar.Remove(ListObjPar[i]);
                }
            }
        }
    }

    public void Click(ParticleStruct item)
    {
        if (ListIdleObjAvail.Count == 0)
        {
            GameObject go2 = Instantiate(ParticleIdlePrefab, CanvasPar.transform, false);
            go2.transform.position = item.StartPos.position;
            go2.SetActive(true);
            item.ParObj = go2;
            ListIdleObjPar.Add(item);
        }
        else
        {
            ParticleStruct itemAvail2 = ListIdleObjAvail[0];
            itemAvail2.ParObj.SetActive(true);

            itemAvail2.ParObj.transform.position = item.StartPos.position;

            ListObjPar.Add(itemAvail2);
            ListObjAvail.Remove(ListObjAvail[0]);
        }

        if (ListObjAvail.Count == 0)
        {
            GameObject go = Instantiate(ParticlePrefab, CanvasPar.transform, false);
            go.transform.position = item.StartPos.position;
            item.ParObj = go;
            item.RunBool = true;
            ListObjPar.Add(item);
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

            ListObjPar.Add(itemAvail);
            ListObjAvail.Remove(ListObjAvail[0]);
        }
    }

    private void CleanUiParticleOvertime()
    {
        if (ListObjAvail.Count > 1)
        {
            for (int i = 1; i < ListObjAvail.Count; i++)
            {
                if (ListObjAvail[i].ParObj != null && ListObjAvail[i].ParObj.activeSelf == false && ListObjAvail[i].RunBool == false)
                {
                    Destroy(ListObjAvail[i].ParObj.gameObject);
                    ListObjAvail.Remove(ListObjAvail[i]);
                }
            }
        }
    }
}
