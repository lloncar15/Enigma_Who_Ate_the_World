using UnityEngine.UI;
using UnityEngine;
using System;

public class FlagButtonController : MonoBehaviour
{
    public Image button;
    public Transform playerTransform;
    public float xOffset = 0.5f;
    public float yOffset = 0.5f;


    private void Update()
    {
        Vector3 vector3pos = playerTransform.position;
        transform.position = new Vector3(vector3pos.x + xOffset, vector3pos.y + yOffset, vector3pos.z);
    }

    private void OnEnable()
    {
        HatCollider.OnPlanetProximityEntered += showButton;
        Player.OnHatPut += hideButton;
        HatCollider.OnPlanetProximityExited += hideButton;
    }

    private void OnDisable()
    {
        HatCollider.OnPlanetProximityEntered -= showButton;
        Player.OnHatPut -= hideButton;
        HatCollider.OnPlanetProximityExited -= hideButton;
    }

    public void showButton()
    {
        button.enabled = true;
    }

    public void hideButton()
    {
        Console.WriteLine("BEBE");
        button.enabled = false;
    }
}
