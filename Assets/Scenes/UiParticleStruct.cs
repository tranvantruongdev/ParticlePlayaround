using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct UiParticleStruct
{
    #region Cache reference
    [Tooltip("Buttons that emit uiParticle")]
    [SerializeField]
    private Button[] buttons;
    [Tooltip("uiParticle in the pool")]
    [SerializeField]
    private GameObject uiParticle;
    [Tooltip("uiParticle spawm at the moment button pressed")]
    [SerializeField]
    private GameObject uiPressedParticle;
    [Tooltip("Targets uiParticle fly into")]
    [SerializeField]
    private GameObject[] target;
    [Tooltip("Materials of uiParticle")]
    [SerializeField]
    private Material[] particleMaterial;
    #endregion

    #region Variables
    [Tooltip("Moving animation")]
    [SerializeField]
    private Ease ease;
    [Range(1, 100)]
    [Tooltip("Number of uiParticle to emit at once")]
    [SerializeField]
    private int numberParticle;
    [Tooltip("Number of uiParticle prewarm in pool")]
    [SerializeField]
    private int numberPrewarmParticle;
    [Tooltip("How long does it take to move uiParticle from start to finish")]
    [SerializeField]
    private float duration;
    [Tooltip("How long between each time clean inactive uiParticle")]
    [SerializeField]
    private float cleaningInterval;
    [Range(-3, 3)]
    [Tooltip("How spread of uiParticle")]
    [SerializeField]
    private float spread;
    #endregion

    private const float baseRadius = 3.0f;
    private const float baseRadialmultipler = -6.0f;

    #region Getter and Setter
    public Ease Ease { get => ease; set => ease = value; }
    public int NumberParticle { get => numberParticle; set => numberParticle = value; }
    public float Duration { get => duration; set => duration = value; }
    public float CleaningInterval { get => cleaningInterval; set => cleaningInterval = value; }
    public Queue<GameObject> UiParticlePool { get; set; }
    public Queue<GameObject> UiPressedParticlePool { get; set; }
    public Button[] Buttons { get => buttons; set => buttons = value; }
    public GameObject UiParticle { get => uiParticle; set => uiParticle = value; }
    public GameObject[] Target { get => target; set => target = value; }
    public Material[] ParticleMaterial { get => particleMaterial; set => particleMaterial = value; }
    public int NumberPrewarmParticle { get => numberPrewarmParticle; set => numberPrewarmParticle = value; }
    public float Spread { get => spread; set => spread = value; }
    public float BaseRadius { get => baseRadius; }
    public float BaseRadialmultipler { get => baseRadialmultipler; }
    public GameObject UiPressedParticle { get => uiPressedParticle; set => uiPressedParticle = value; }
    #endregion
}
