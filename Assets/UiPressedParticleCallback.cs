using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiPressedParticleCallback : MonoBehaviour
{
    public void OnParticleSystemStopped()
    {
        gameObject.SetActive(false);
    }
}
