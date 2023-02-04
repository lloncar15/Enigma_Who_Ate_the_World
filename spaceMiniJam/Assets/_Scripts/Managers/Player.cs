using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("PlayerSettings")]
    [SerializeField] private int playerHealth = 3;
    [SerializeField] private float fuel = 100f;
    [SerializeField] private float fuelMax = 100f;
    [SerializeField] private float fuelCollect = 20f;
    [SerializeField] public float fuelConsumptionRate = 0.5f;
    private GameObject closestPlanet = null;


    [SerializeField] public float invulnerableTime = 3.0f;
    private float invulnerableDeltaTime = 0.0f;
    private bool invulnerable = false;


    [Header("Movement")]
    [SerializeField] public float speed = 5f;
    [SerializeField] public float maxSpeed = 10f;
    [SerializeField] public float slowDownFactor = 0.95f;
    [SerializeField] public float maxSpeedWithBoost = 10f;
    [SerializeField] public float rotationSpeed = 90f;
    [SerializeField] public float slowingSpeed = 2f;
    [SerializeField] public float boostMultiplier = 1.5f;

    public static event Action OnHealthLost;
    public static event Action OnGameOver;
    public static event Action OnHatPut;

    private Rigidbody2D rigidBody;

    private void OnEnable()
    {
        BlackHoleController.OnBlackHoleEntered += BlackHoleEntered;
    }

    private void OnDisable()
    {
        BlackHoleController.OnBlackHoleEntered -= BlackHoleEntered;
    }

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // get input for moving forward
        float forwardInput = Input.GetAxis("Vertical");

        // get input for rotating left and right
        float rotationInput = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;

        // check if slowing key is pressed
        if (Input.GetKey(KeyCode.S))
        {
            forwardInput = Mathf.Max(forwardInput - slowingSpeed * Time.deltaTime, 0);
        }

        // check if boost key is pressed
        bool spedUp = false;
        if (Input.GetKey(KeyCode.Space) && fuel > 0)
        {
            forwardInput *= boostMultiplier;
            fuel -= fuelConsumptionRate * Time.deltaTime;
            spedUp = true;
        }

        // apply force to move the ship forward
        rigidBody.AddRelativeForce(new Vector2(0, forwardInput * speed));
        rigidBody.velocity *= slowDownFactor;

        float localMaxSpeed = spedUp ? maxSpeedWithBoost : maxSpeed;
        // limit the speed of the ship
        if (rigidBody.velocity.magnitude > localMaxSpeed)
        {
            rigidBody.velocity = rigidBody.velocity.normalized * localMaxSpeed;
        }

        // rotate the ship based on the horizontal input
        transform.Rotate(0, 0, -rotationInput);


        if (invulnerableDeltaTime > 0)
        {
            invulnerableDeltaTime -= Time.deltaTime;
            if (invulnerableDeltaTime <= 0)
            {
                invulnerableDeltaTime = 0;
                invulnerable = false;
            }
        }

        if (Input.GetKey(KeyCode.E))
        {
            PutHatOnClosestPlanet();
        }
    }

    private void CollectFuel()
    {
        // skip collecting fuel if you have max fuel
        if (fuel >= fuelMax)
        {
            return;
        }

        fuel = Mathf.Clamp(fuel + fuelCollect, 0, fuelMax);
    }

    private void BlackHoleEntered()
    {
        if (invulnerable)
            return;
        LoseHealth();
        invulnerable = true;
        invulnerableDeltaTime = invulnerableTime;
        // TODO: ACTIVATE THE SHADER ANIMATION
    }

    private void LoseHealth()
    {
        --playerHealth;
        if (playerHealth == 0)
        {
            OnGameOver?.Invoke();
        }
    }

    private void PutHatOnClosestPlanet()
    {
        if (closestPlanet == null)
        {
            return;
        }
        closestPlanet.GetComponent<PlanetController>().putHat();
        OnHatPut?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Planet")
        {
            closestPlanet = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Planet")
        {
            closestPlanet = null;
        }
    }
}

