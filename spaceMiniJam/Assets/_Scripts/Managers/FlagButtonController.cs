using UnityEngine.UI;
using UnityEngine;
using System;

public class FlagButtonController : MonoBehaviour
{
    public Image button;

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
