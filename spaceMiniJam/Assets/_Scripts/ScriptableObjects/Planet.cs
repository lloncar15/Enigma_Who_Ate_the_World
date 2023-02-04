using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Planet", menuName = "ScriptableObjects/Planet")]
public class Planet : ScriptableObject
{
    [SerializeField] public float gravitationalForce = 9.8f;
    [SerializeField] public float gravityRadius = 10f;
}
