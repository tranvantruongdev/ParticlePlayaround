using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class MoveToAnotherUI : MonoBehaviour
{
    public ParticleSystem ParticleSystem;
    [SerializeField]
    private int maxCoins = 5;
    [SerializeField]
    RectTransform newPos;
    public GameObject animatedCoinPrefab;
    public Queue<GameObject> coinsQueue = new Queue<GameObject>();
    [SerializeField]
    private GameObject collectButton;
    [SerializeField] Ease easeType;

    private void Awake()
    {
        //PrepareCoins();
    }

    void PrepareCoins()
    {
        GameObject coin;
        for (int i = 0; i < maxCoins; i++)
        {
            coin = Instantiate(animatedCoinPrefab, Vector3.zero, Quaternion.identity, transform);
            coin.SetActive(false);
            coinsQueue.Enqueue(coin);
        }
    }

    public void Animate()
    {
        //check if there's coins in the pool
        if (coinsQueue.Count > 0)
        {
            //extract a coin from the pool
            GameObject coin = coinsQueue.Dequeue();
            coin.SetActive(true);

            coin.transform.position = collectButton.transform.position;

            coin.transform.DOMove(newPos.position, 2).SetEase(easeType).OnComplete(() => {
                coin.SetActive(false);
                coinsQueue.Enqueue(coin);
                });
        }
    }
}
