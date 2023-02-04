using System;
using UnityEngine;

public class BlackHoleController : MonoBehaviour
{
    public static event Action OnBlackHoleEntered;

    [SerializeField] private float waveIntervalTime = 10.0f;
    [SerializeField] private float intervalStartDelay = 10.0f;
    private float waveInterval = 0.0f;

    void Start()
    {
        InvokeRepeating("EmitWave", intervalStartDelay, waveIntervalTime);
    }

    private void EmitWave()
    {
        // TODO: create a wave
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            OnBlackHoleEntered?.Invoke();
        }
    }
}
