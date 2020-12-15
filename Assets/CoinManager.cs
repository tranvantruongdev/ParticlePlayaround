using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
#pragma warning disable 0649 //disable no default value warning
    //References
    [Header("UI references")]
    [SerializeField] GameObject animatedCoinPrefab;
    [SerializeField] GameObject[] target;
    [SerializeField] Button[] btn;


    [Space]
    [Header("Available coins : (coins to pool)")]
    [SerializeField] int maxCoins;
    [SerializeField] int numCoins;
    readonly Queue<GameObject> coinsQueue = new Queue<GameObject>();


    [Space]
    [Header("Animation settings")]
    [SerializeField] [Range(0.5f, 0.9f)] float minAnimDuration;
    [SerializeField] [Range(0.9f, 2f)] float maxAnimDuration;
    [SerializeField] Animator[] anim;

    [SerializeField] Ease easeType;
    [SerializeField] float spread;

    private int checkIntpar = 0;
    void Awake()
    {
        //prepare pool
        //PrepareCoins();
        //for (int i = 0; i < btn.Length; i++)
        //{
        //    checkIntpar = i;
        //    btn[i].onClick.AddListener(checkInt);
        //}
        //foreach (var item in btn)
        //{
        //    item.onClick.AddListener(() => Animate(item.transform.position, numCoins));
        //}
        //StartCoroutine("IEPre");
        //foreach (var item in btn)
        //{
        //    StartCoroutine(CheckCor(item));
        //    //item.onClick.AddListener(() => Animate(item.transform.position, numCoins));
        //}
    }
    IEnumerator IEPre()
    {
        GameObject coin;
        for (int i = 0; i < maxCoins; i++)
        {
            coin = Instantiate(animatedCoinPrefab, transform);
            coin.SetActive(false);
            coinsQueue.Enqueue(coin);
        }
        yield return new WaitForSeconds(0);

    }
    IEnumerator CheckCor(Button btn)
    {
        btn.onClick.AddListener(() => Animate(btn.transform.position, numCoins));
        yield return new WaitForSeconds(0);
    }
    void checkInt()
    {
        Debug.Log("checkIntpar: "+checkIntpar);
    }
    private void OnDestroy()
    {
        foreach (var item in btn)
        {
            item.onClick.RemoveListener(() => Animate(item.transform.position, numCoins));
        }
    }

    void PrepareCoins()
    {
        GameObject coin;
        for (int i = 0; i < maxCoins; i++)
        {
            coin = Instantiate(animatedCoinPrefab, transform);
            coin.SetActive(false);
            coinsQueue.Enqueue(coin);
        }
    }

    void Animate(Vector3 startPosition, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            //check if there's coins in the pool
            if (coinsQueue.Count > 0)
            {
                //extract a coin from the pool
                GameObject coin = coinsQueue.Dequeue();
                coin.SetActive(true);

                //move coin to the collected coin pos
                coin.transform.position = startPosition
                    + new Vector3(Random.Range(-spread, spread), 0f, 0f);

                //animate coin to target position
                float duration = Random.Range(minAnimDuration, maxAnimDuration);
                coin.transform.DOMove(target[0].transform.position, duration)
                .SetEase(easeType)
                .OnComplete(() =>
                {
                    //executes whenever coin reach target position
                    anim[0].SetTrigger("pop");
                    coin.SetActive(false);
                    coinsQueue.Enqueue(coin);
                });
            }
        }
    }
}
