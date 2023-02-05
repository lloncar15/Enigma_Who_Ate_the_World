using System;
using UnityEditor;
using UnityEngine;

public class WaveCollision : MonoBehaviour
{
    public static event Action OnWaveCollision;
    private Rigidbody2D rb;
    public float speed;
    public float expansionsSpeed;
    public float maximumScale;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb.velocity = transform.up * Time.deltaTime * speed;

        Vector3 localScale = transform.localScale;
        if (localScale.x < maximumScale)
        {
            float scaleTransform = localScale.x * expansionsSpeed;
            transform.localScale = new Vector3(scaleTransform, scaleTransform, localScale.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            OnWaveCollision?.Invoke();
        if (collision.gameObject.tag == "CollisionWall")
            Destroy(gameObject);
    }
}
