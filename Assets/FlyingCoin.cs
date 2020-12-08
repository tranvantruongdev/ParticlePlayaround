using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingCoin : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particleSystem;
    private float currentSpeed = 15f;
    private float minSpeed;
    private float maxSpeed = 50f;
    private float time;
    private int accelerationTime = 1;
    //Declare RectTransform in script
    RectTransform faceButton;
    //The new position of your button
    public RectTransform newPos;

    //private MoveToAnotherUI moveTo;

    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        //moveTo = FindObjectOfType<MoveToAnotherUI>();
        var collectButton = GameObject.FindGameObjectWithTag("ConfirmButton");
        faceButton = collectButton.GetComponent<RectTransform>();
        var coinMetter = GameObject.FindGameObjectWithTag("CoinMeter");
        newPos = coinMetter.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        currentSpeed = Mathf.SmoothStep(minSpeed, maxSpeed, time / accelerationTime);
        //Update the localPosition towards the newPos
        transform.position = Vector3.MoveTowards(faceButton.position, newPos.position,
            currentSpeed * Time.deltaTime);
        time += Time.deltaTime;
    }

    private void OnEnable()
    {
        minSpeed = currentSpeed;
        time = 0f;
        particleSystem.Play();
    }

    private void OnDisable()
    {
        particleSystem.Clear();
        //moveTo.coinsQueue.Enqueue(gameObject);
    }
}
