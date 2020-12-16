using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct UiParticleStruct
{
    private Ease ease;
    [Range(1, 100)]
    private int numberParticle;
    private float duration;
    private float cleaningInterval;

    public Ease Ease { get => ease; set => ease = value; }
    public int NumberParticle { get => numberParticle; set => numberParticle = value; }
    public float Duration { get => duration; set => duration = value; }
    public float CleaningInterval { get => cleaningInterval; set => cleaningInterval = value; }
    public Queue<GameObject> UiParticlePool { get; set; }

    public UiParticleStruct(Ease ease, int numberParticle, float duration, float cleaningInterval, Queue<GameObject> uiParticlePool) : this()
    {
        Ease = ease;
        NumberParticle = numberParticle;
        Duration = duration;
        CleaningInterval = cleaningInterval;
        this.UiParticlePool = uiParticlePool;
    }
}
