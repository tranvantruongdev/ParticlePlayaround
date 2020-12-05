using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToAnotherUI : MonoBehaviour
{
    //Declare RectTransform in script
    RectTransform faceButton;
    //The new position of your button
    [SerializeField]
    RectTransform newPos;
    [SerializeField]
    float speed = 15f;
    public ParticleSystem ParticleSystem;

    void Start()
    {
        //Get the RectTransform component
        faceButton = GetComponent<RectTransform>();
        //Debug.Log(faceButton.position.ToString());
        //newPos.position -= new Vector3(0f, 5f, 0f);
        //Debug.Log(newPos.position.ToString());
    }

    void LateUpdate()
    {
        if (faceButton.position == newPos.position)
        {
            ParticleSystem.Clear();
            return;
        }
        //Update the localPosition towards the newPos
        faceButton.position = Vector3.MoveTowards(faceButton.position, newPos.position,
            speed * Time.deltaTime);
    }

}
