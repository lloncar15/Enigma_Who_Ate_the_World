using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObjects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [Header("PlayerSettings")]
    [SerializeField] public float fuelMax = 100f;
    [SerializeField] public float fuelCollectAmount = 20f;
    [SerializeField] public float fuelConsumptionRate = 0.5f;
    [SerializeField] public float invulnerableTime = 3.0f;

    [Header("Movement")]
    [SerializeField] public float speed = 5f;
    [SerializeField] public float maxSpeed = 10f;
    [SerializeField] public float slowDownFactor = 0.95f;
    [SerializeField] public float maxSpeedWithBoost = 10f;
    [SerializeField] public float rotationSpeed = 90f;
    [SerializeField] public float slowingSpeed = 2f;
    [SerializeField] public float boostMultiplier = 1.5f;
}
