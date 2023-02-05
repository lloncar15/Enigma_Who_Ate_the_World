using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "ScriptableObjects/Wave")]
public class BlackHoleWave : ScriptableObject
{
    public IndexList[] wavePositionIndexes;
    public float waveSpeed;
}

[Serializable]
public class IndexList
{
    public int[] indexes;
}
