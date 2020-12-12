using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    //References
    [Header("UI references")]
    [SerializeField] GameObject animatedCoinPrefab;
    [SerializeField] Transform target;
    [SerializeField] Button btn;


    [Space]
    [Header("Available coins : (coins to pool)")]
    [SerializeField] int maxCoins;
    readonly Queue<GameObject> coinsQueue = new Queue<GameObject>();


    [Space]
    [Header("Animation settings")]
    [SerializeField] [Range(0.5f, 0.9f)] float minAnimDuration;
    [SerializeField] [Range(0.9f, 2f)] float maxAnimDuration;
    [SerializeField] Animator anim;

    [SerializeField] Ease easeType;
    [SerializeField] float spread;

    void Awake()
    {
        //prepare pool
        PrepareCoins();
        btn.onClick.AddListener(() => Animate(btn.transform.position, maxCoins));

    }

    private void OnDestroy()
    {
        btn.onClick.RemoveListener(() => Animate(btn.transform.position, maxCoins));
    }

    void PrepareCoins()
    {
        GameObject coin;
        for (int i = 0; i < maxCoins; i++)
        {
            coin = Instantiate(animatedCoinPrefab);
            coin.transform.parent = transform;
            coin.SetActive(false);
            coinsQueue.Enqueue(coin);
        }
    }

    void Animate(Vector3 collectedCoinPosition, int amount)
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
                coin.transform.position = collectedCoinPosition 
                    + new Vector3(Random.Range(-spread, spread), 0f, 0f);

                //animate coin to target position
                float duration = Random.Range(minAnimDuration, maxAnimDuration);
                coin.transform.DOMove(target.position, duration)
                .SetEase(easeType)
                .OnComplete(() =>
                {
                    //executes whenever coin reach target position
                    anim.SetTrigger("pop");
                    coin.SetActive(false);
                    coinsQueue.Enqueue(coin);
                });
            }
        }
    }
}
