using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;
    private float fuel = 100f;
    private int playerHealth = 3;
    private GameObject closestPlanet = null;

    private float invulnerableDeltaTime = 0.0f;
    private bool invulnerable = false;

    public static event Action OnHealthLost;
    public static event Action OnGameOver;
    public static event Action OnHatPut;
    public static event Action<bool> OnFuelCollected;

    private Rigidbody2D rigidBody;

    private void OnEnable()
    {
        BlackHoleController.OnBlackHoleEntered += BlackHoleEntered;
        ConsumableController.OnConsumableCollected += CollectFuel;
    }

    private void OnDisable()
    {
        BlackHoleController.OnBlackHoleEntered -= BlackHoleEntered;
        ConsumableController.OnConsumableCollected -= CollectFuel;
    }

    private void Start()
    {
        fuel = stats.fuelMax;
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // get input for moving forward
        float forwardInput = Input.GetAxis("Vertical");

        // get input for rotating left and right
        float rotationInput = Input.GetAxis("Horizontal") * stats.rotationSpeed * Time.deltaTime;

        // check if slowing key is pressed
        if (Input.GetKey(KeyCode.S))
        {
            forwardInput = Mathf.Max(forwardInput - stats.slowingSpeed * Time.deltaTime, 0);
        }

        // check if boost key is pressed
        bool spedUp = false;
        if (Input.GetKey(KeyCode.Space) && fuel > 0)
        {
            forwardInput *= stats.boostMultiplier;
            fuel -= stats.fuelConsumptionRate * Time.deltaTime;
            spedUp = true;
        }

        // apply force to move the ship forward
        rigidBody.AddRelativeForce(new Vector2(0, forwardInput * stats.speed));
        rigidBody.velocity *= stats.slowDownFactor;

        float localMaxSpeed = spedUp ? stats.maxSpeedWithBoost : stats.maxSpeed;
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
        if (fuel >= stats.fuelMax)
        {
            OnFuelCollected?.Invoke(false);
            return;
        }

        fuel = Mathf.Clamp(fuel + stats.fuelCollectAmount, 0, stats.fuelMax);
        OnFuelCollected?.Invoke(true);
    }

    private void BlackHoleEntered()
    {
        if (invulnerable)
            return;
        LoseHealth();
        invulnerable = true;
        invulnerableDeltaTime = stats.invulnerableTime;
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

