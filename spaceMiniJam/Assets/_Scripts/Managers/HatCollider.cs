using System;
using UnityEngine;

public class HatCollider : MonoBehaviour
{
    public static event Action OnPlanetProximityEntered;
    public static event Action OnPlanetProximityExited;

    public Player player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Planet")
        {
            player.closestPlanet = collision.gameObject;
            OnPlanetProximityEntered?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Planet")
        {
            player.closestPlanet = null;
            OnPlanetProximityExited?.Invoke();
        }
    }
}
