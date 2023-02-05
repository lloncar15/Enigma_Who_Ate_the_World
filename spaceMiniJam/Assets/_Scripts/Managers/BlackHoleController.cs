using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class BlackHoleController : MonoBehaviour
{
    public static event Action OnBlackHoleEntered;

    [SerializeField] private float waveIntervalTime = 30.0f;
    [SerializeField] private float intervalStartDelay = 10.0f;
    public float waveInterval = 5.0f;

    public List<GameObject> wavePrefabs;
    public Vector3[] wavePositions;
    public List<BlackHoleWave> wavesList;

    void Start()
    {
        InvokeRepeating("EmitWaves", intervalStartDelay, waveIntervalTime);
    }

    private void EmitWaves()
    {
        // take one of the waveList at random
        BlackHoleWave wave = wavesList.ElementAt(UnityEngine.Random.Range(0, wavesList.Count));
        IndexList[] wavePositions = wave.wavePositionIndexes;

        //for each of the wave create the waves around it
        for (int i = 0; i < wavePositions.Count(); ++i)
        {
            StartCoroutine(CreateOneWave(wavePositions[i].indexes, waveInterval * i, wave.waveSpeed));
        }
    }

    private IEnumerator CreateOneWave(int[] indexes, float waitTime, float waveSpeed)
    {
        yield return Extensions.GetWait(waitTime);

        // for each of the index get a random wave and instantiate it at the lociational index and add velocity
        foreach (int index in indexes)
        {
            Vector3 waveData = wavePositions[index];
            Vector3 position = new Vector3(waveData.x, waveData.y, 0);
            Vector3 rotation = new Vector3(0, 0, waveData.z);
            GameObject smallWave = Instantiate(getRandomWavePrefab(), position, Quaternion.Euler(rotation));
            smallWave.GetComponent<WaveCollision>().speed = waveSpeed;
        }
    }

    private GameObject getRandomWavePrefab()
    {
        return wavePrefabs.ElementAt(UnityEngine.Random.Range(0, wavePrefabs.Count));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            OnBlackHoleEntered?.Invoke();
        }
    }
}
