using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct UiParticleStruct
{
    [Tooltip("Buttons that emit uiParticle")]
    [SerializeField]
    private Button[] buttons;
    [Tooltip("uiParticle in the pool")]
    [SerializeField]
    private GameObject uiParticle;
    [Tooltip("Targets uiParticle fly into")]
    [SerializeField]
    private GameObject[] target;
    [Tooltip("Materials of uiParticle")]
    [SerializeField]
    private Material[] particleMaterial;

    [Tooltip("Moving animation")]
    [SerializeField]
    private Ease ease;
    [Range(1, 100)]
    [Tooltip("Number of uiParticle to emit at once")]
    [SerializeField]
    private int numberParticle;
    [Tooltip("How long does it take to move uiParticle from start to finish")]
    [SerializeField]
    private float duration;
    [Tooltip("How long between each time clean inactive uiParticle")]
    [SerializeField]
    private float cleaningInterval;

    public Ease Ease { get => ease; set => ease = value; }
    public int NumberParticle { get => numberParticle; set => numberParticle = value; }
    public float Duration { get => duration; set => duration = value; }
    public float CleaningInterval { get => cleaningInterval; set => cleaningInterval = value; }
    public Queue<GameObject> UiParticlePool { get; set; }
    public Button[] Buttons { get => buttons; set => buttons = value; }
    public GameObject UiParticle { get => uiParticle; set => uiParticle = value; }
    public GameObject[] Target { get => target; set => target = value; }
    public Material[] ParticleMaterial { get => particleMaterial; set => particleMaterial = value; }
}
