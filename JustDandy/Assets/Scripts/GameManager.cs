using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public PlayerController player;
    public Image healthBar;
    public TextMeshProUGUI stageText;
    public TextMeshProUGUI pollenText;

    // Start is called before the first frame update
    void Start()
    {
        

        if (SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 4)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            healthBar = GameObject.Find("HealthBar").GetComponent<Image>();
            stageText = GameObject.Find("StageNumber").GetComponent<TextMeshProUGUI>();
            pollenText = GameObject.Find("PollenNumber").GetComponent<TextMeshProUGUI>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 4)
        {
            healthBar.fillAmount = player.hp / player.maxhp;
            stageText.text = "Stage: " + player.Stage.ToString();
            pollenText.text = "Pollen: " + player.Pollen.ToString();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadLevel(int levelNumber)
    {
        SceneManager.LoadScene(levelNumber);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
