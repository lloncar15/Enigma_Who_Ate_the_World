using System;
using UnityEngine;

public class ConsumableController : MonoBehaviour
{
    public static event Action OnConsumableCollected;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player.OnFuelCollected += RunCollectibleAnimation;
            OnConsumableCollected?.Invoke();
        }
    }

    private void RunCollectibleAnimation(bool isCollected)
    {
        Player.OnFuelCollected -= RunCollectibleAnimation;

        if (!isCollected)
            return;
        //TODO: run the collect and death animation
        Destroy(gameObject);
    }
}
