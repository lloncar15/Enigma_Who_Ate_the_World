using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("PlayerSettings")]

    [Header("Movement")]
    [SerializeField] private float m_enginePower = 10f;
    [SerializeField] private KeyCode m_leftEngineKey = KeyCode.A;
    [SerializeField] private KeyCode m_rightEngineKey = KeyCode.D;

    private Rigidbody2D m_rigidbody;

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // check if the left engine key is pressed
        if (Input.GetKey(m_leftEngineKey))
        {
            // apply force to the left
            m_rigidbody.AddForce(Vector2.left * m_enginePower, ForceMode2D.Impulse);
        }

        // check if the right engine key is pressed
        if (Input.GetKey(m_rightEngineKey))
        {
            // apply force to the right
            m_rigidbody.AddForce(Vector2.right * m_enginePower, ForceMode2D.Impulse);
        }
    }
}
