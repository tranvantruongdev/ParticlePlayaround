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
    public ParticleSystem ParSys;
    public ParticleSystem PressedParSys;
    public bool RunBool;
    [Range(0.1f, 5.0f)]
    public float time;
    [HideInInspector]
    public float speed;
    public Queue<GameObject> UiParticlePool { get; set; }
    public Queue<GameObject> UiPressedParticlePool { get; set; }
    public float CleaningInterval;
}

public class ScriptParticle : MonoBehaviour
{
    //public ParticleStruct ParStruct = new ParticleStruct();
    public ParticleStruct[] ParStruct2;

    public GameObject ParticlePrefab;
    public GameObject CanvasPar;
    List<ParticleStruct> ListObjPar = new List<ParticleStruct>();
    List<ParticleStruct> ListObjAvail = new List<ParticleStruct>();

    private void Start()
    {
        //ParStruct.Btn.onClick.AddListener(() => Click());
        foreach (var item in ParStruct2)
        {
            item.Btn.onClick.AddListener(() =>
            {
                Click(item);
            });

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
                    ListObjPar[i].time * Time.deltaTime * 10
                    );

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
        if (ListObjAvail.Count == 0)
        {
            GameObject go = Instantiate(ParticlePrefab, CanvasPar.transform, false);
            //set pos, chay run 
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

    private void AddUiParticle(int index, Queue<GameObject> pool, int amout)
    {
        for (int i = 0; i < amout; i++)
        {
            GameObject particle;
            particle = Instantiate(ParStruct2[index].ParSys.gameObject, transform);
            particle.SetActive(false);
            pool.Enqueue(particle);
        }
    }

    private void AddUiPressParticle(int index, Queue<GameObject> pool, int amout)
    {
        for (int i = 0; i < amout; i++)
        {
            GameObject particle;
            particle = Instantiate(ParStruct2[index].PressedParSys.gameObject, transform);
            particle.SetActive(false);
            pool.Enqueue(particle);
        }
    }

    private void CleanUiParticleOvertime()
    {
        for (int j = 0; j < ParStruct2.Length; j++)
        {
            GameObject uiParticle;
            int loopTimes = ParStruct2[j].UiParticlePool.Count;
            for (int i = 1; i < loopTimes; i++)
            {
                uiParticle = ParStruct2[j].UiParticlePool.Dequeue();
                if (!uiParticle.activeInHierarchy)
                {
                    Destroy(uiParticle);
                }
            }

            int loopTimes2 = ParStruct2[j].UiPressedParticlePool.Count;
            for (int i = 1; i < loopTimes2; i++)
            {
                uiParticle = ParStruct2[j].UiPressedParticlePool.Dequeue();
                if (!uiParticle.activeInHierarchy)
                {
                    Destroy(uiParticle);
                }
            }
        }
    }

    IEnumerator PutBackToPool(GameObject gameObjectToPool, Queue<GameObject> pool, float time = 1.0f)
    {
        yield return new WaitForSeconds(time);
        gameObjectToPool.gameObject.SetActive(false);
        pool.Enqueue(gameObjectToPool);
    }

    IEnumerator FlyTo(GameObject objectToMove, Transform end, float speed)
    {
        while (objectToMove.transform.position != end.position)
        {
            objectToMove.transform.position = Vector3.MoveTowards(
                objectToMove.transform.position,
                end.position,
                speed * Time.deltaTime);

            // Wait a frame and move again.
            yield return null;
        }
    }
}
