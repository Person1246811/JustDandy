using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (SceneManager.GetActiveScene().buildIndex != 0)
            healthBar.fillAmount = player.hp / player.maxhp;*/
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
