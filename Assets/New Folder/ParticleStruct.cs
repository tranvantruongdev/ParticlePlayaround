using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct ParticleStruct
{
    public Button[] Btn;

    public Transform StartPos;
    public Transform EndPos;

    public GameObject ParObj;

    public bool RunBool;

    [Range(1f, 5.0f)]
    public float time;
    [HideInInspector]
    public float speed;

    public GameObject ParticlePrefab;
    public const float radialMul = 6f;
}
