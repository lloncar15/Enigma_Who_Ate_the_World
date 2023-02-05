using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Slider slider;
    public Player player;
    public int playerHealth = 2;
    public Image[] hearts;

    private void Update()
    {
        slider.value = player.fuel;
    }

    private void OnEnable()
    {
        BlackHoleController.OnBlackHoleEntered += BlackHoleEntered;
    }

    private void OnDisable()
    {
        BlackHoleController.OnBlackHoleEntered -= BlackHoleEntered;
    }

    private void BlackHoleEntered()
    {
        if (playerHealth < 0)
            return;

        //TODO: Heart animation
        hearts[playerHealth].GetComponent<Image>().color = Color.black;
        --playerHealth;
    }
}
