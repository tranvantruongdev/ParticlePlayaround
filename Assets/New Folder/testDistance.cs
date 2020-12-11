using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testDistance : MonoBehaviour
{
    public Transform Start;
    public Transform End;

    [ContextMenu("Do Distance")]
    public void Distance()
    {
        float distance = Vector3.Distance(Start.position, End.position);
        Debug.Log("distance: " + distance);
    }


}
