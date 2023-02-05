using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class BlackHoleController : MonoBehaviour
{
    public static event Action OnBlackHoleEntered;

    public GameObject positionsChild;

    [SerializeField] private float waveIntervalTime = 30.0f;
    [SerializeField] private float intervalStartDelay = 10.0f;
    public float waveInterval = 5.0f;
    public float expandRate = 0.02f;

    public List<GameObject> wavePrefabs;
    public Vector3[] wavePositions;
    public List<BlackHoleWave> wavesList;

    public Animator animator;
    public GameObject squiggle;

    void Start()
    {
        InvokeRepeating("EmitWaves", intervalStartDelay, waveIntervalTime);
    }

    private void Update()
    {
        Vector3 localScale = transform.localScale;
        transform.localScale = new Vector3(localScale.x + expandRate, localScale.y + expandRate, localScale.z);

        squiggle.transform.Rotate(new Vector3(0, 0, 1));
    }

    private void EmitWaves()
    {
        // take one of the waveList at random
        BlackHoleWave wave = wavesList.ElementAt(UnityEngine.Random.Range(0, wavesList.Count));
        IndexList[] wavePositions = wave.wavePositionIndexes;

        //for each of the wave create the waves around it
        for (int i = 0; i < wavePositions.Count(); ++i)
        {
            StartCoroutine(CreateOneWave(wavePositions[i].indexes, waveInterval * i, wave.waveSpeed, i == wavePositions.Count() - 1));
        }
    }

    private IEnumerator CreateOneWave(int[] indexes, float waitTime, float waveSpeed, bool changeAngry)
    {
        animator.SetBool("IsAngry", true);
        yield return Extensions.GetWait(waitTime);

        // for each of the index get a random wave and instantiate it at the lociational index and add velocity
        foreach (int index in indexes)
        {
            Vector3 waveData = wavePositions[index];
            Vector3 position = positionsChild.transform.GetChild(index).transform.position;
            Vector3 rotation = new Vector3(0, 0, waveData.z);
            GameObject smallWave = Instantiate(getRandomWavePrefab(), position, Quaternion.Euler(rotation));
            float blackHoleScale = transform.localScale.x;
            smallWave.GetComponent<WaveCollision>().speed = waveSpeed;
            smallWave.GetComponent<WaveCollision>().maximumScale *= blackHoleScale;
            Vector3 smallWaveTransform = smallWave.transform.localScale;
            smallWave.transform.localScale = new Vector3(smallWaveTransform.x * blackHoleScale, smallWaveTransform.y * blackHoleScale, smallWaveTransform.z);
        }

        if (changeAngry)
        {
            yield return Extensions.GetWait(0.5f);
            animator.SetBool("IsAngry", false);
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
