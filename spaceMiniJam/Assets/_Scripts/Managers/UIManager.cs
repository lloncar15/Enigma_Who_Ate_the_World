using UnityEngine.UI;
using UnityEngine;
using UnityEditor;

public class UIManager : MonoBehaviour
{
    public Slider slider;
    public Player player;
    public int playerHealth = 2;
    public Image[] hearts;
    public Image[] deadHearts;

    public GameObject GameOver;
    public GameObject Win;
    public GameObject Paused;
    public Image BlackScreen;
    public Canvas canvas;

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

        hearts[playerHealth].GetComponent<Image>().enabled = false;
        deadHearts[playerHealth].GetComponent<Image>().enabled = true;
        --playerHealth;
    }

    public void MainMenuPressed()
    {

    }
}
