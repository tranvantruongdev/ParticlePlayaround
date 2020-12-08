using System.Collections;
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

    private void Awake()
    {
        PrepareCoins();
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

    public void Animate()
    {
        //check if there's coins in the pool
        if (coinsQueue.Count > 0)
        {
            //extract a coin from the pool
            GameObject coin = coinsQueue.Dequeue();
            coin.SetActive(true);

            if (coin.transform.position == newPos.position)
            {
                //executes whenever coin reach target position
                coin.SetActive(false);
                coin.transform.position = transform.position;
                coinsQueue.Enqueue(coin);
                return;
            }
        }
    }

    void Start()
    {
    }
}
