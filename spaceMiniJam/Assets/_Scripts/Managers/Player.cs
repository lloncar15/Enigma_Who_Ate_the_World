using System;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;
    public float fuel = 100f;
    public int playerHealth = 3;
    public GameObject closestPlanet = null;
    public int hats = 6;

    private float invulnerableDeltaTime = 0.0f;
    private bool invulnerable = false;

    private float reverseControlsDeltaTime = 0.0f;
    private bool reversed = false;

    public static event Action OnHealthLost;
    public static event Action OnHatPut;
    public static event Action<bool> OnFuelCollected;

    public TMP_Text reverseText;

    private Rigidbody2D rigidBody;

    public bool paused = false;

    public UIManager UIManager;

    public Animator anim;

    private void OnEnable()
    {
        BlackHoleController.OnBlackHoleEntered += BlackHoleEntered;
        ConsumableController.OnConsumableCollected += CollectFuel;
        WaveCollision.OnWaveCollision += OnWaveHit;
    }

    private void OnDisable()
    {
        BlackHoleController.OnBlackHoleEntered -= BlackHoleEntered;
        ConsumableController.OnConsumableCollected -= CollectFuel;
        WaveCollision.OnWaveCollision -= OnWaveHit;
    }

    private void Start()
    {
        fuel = stats.fuelMax;
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if (paused)
            return;

        // get input for moving forward
        float forwardInput = reversed ? -Input.GetAxis("Vertical") : Input.GetAxis("Vertical");

        // get input for rotating left and right
        float horizontalInput = reversed ? -Input.GetAxis("Horizontal") : Input.GetAxis("Horizontal");
        float rotationInput = horizontalInput * stats.rotationSpeed * Time.deltaTime;

        // check if slowing key is pressed
        if (Input.GetKey(KeyCode.S))
        {
            // forwardInput = Mathf.Max(forwardInput - stats.slowingSpeed * Time.deltaTime, 0);
        }

        // check if boost key is pressed
        bool spedUp = false;
        if (Input.GetKey(KeyCode.Space) && fuel > 0)
        {
            forwardInput *= stats.boostMultiplier;
            fuel -= stats.fuelConsumptionRate * Time.deltaTime;
            spedUp = true;
            anim.SetBool("IsThrust", true);
        }
        else
        {
            anim.SetBool("IsThrust", false);
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

        if (reverseControlsDeltaTime > 0)
        {
            reverseControlsDeltaTime -= Time.deltaTime;
            if (reverseControlsDeltaTime <= 0)
            {
                reversed = false;
                reverseControlsDeltaTime = 0;
                reverseText.enabled = false;
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
            OnGameOver();
        }
    }

    private void PutHatOnClosestPlanet()
    {
        if (closestPlanet == null)
        {
            return;
        }
        PlanetController con = closestPlanet.GetComponent<PlanetController>();
        if (con.hasHat)
            return;

        con.putHat();
        OnHatPut?.Invoke();
        --hats;
        if (hats == 0)
        {
            Finish();
        }
    }

    private void OnWaveHit()
    {
        reversed = true;
        reverseControlsDeltaTime = stats.reversedTime;
        reverseText.enabled = true;
        // TODO: ACTIVATE REVERSE SHADER ANIMATION
    }

    private void TogglePause()
    {
        if (Time.timeScale > 0)
        {
            Time.timeScale = 0;
            paused = true;
            UIManager.Paused.SetActive(true);
            UIManager.BlackScreen.enabled = true;
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            paused = false;
            UIManager.Paused.SetActive(false);
            UIManager.BlackScreen.enabled = false;
        }
    }

    public void OnGameOver()
    {
        Time.timeScale = 0;
        paused = true;
        UIManager.BlackScreen.enabled = true;
        UIManager.GameOver.SetActive(true);
    }

    public void Finish()
    {
        Time.timeScale = 0;
        paused = true;
        UIManager.BlackScreen.enabled = true;
        UIManager.Win.SetActive(true);
    }
}

