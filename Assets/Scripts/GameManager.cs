using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Canvas inGameUi;

    void Start()
    {
        inGameUi.enabled = false;    
    }
    public void restartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void setGameUI(Canvas gameUI) {
        inGameUi = gameUI;
    }

    public void toggleInGameUI() {
        if (!inGameUi.enabled) {
            PauseGame();
        } else {
            ContinueGame();
        }
    }

    public void changeGameScene(string sceneName) {
        GameObject.FindGameObjectWithTag("YouWin").GetComponent<UnityEngine.UI.Text>().text = "";
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1;
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        inGameUi.enabled = true;
        //Disable scripts that still work while timescale is set to 0
    } 
    private void ContinueGame()
    {
        Time.timeScale = 1;
        inGameUi.enabled = false;
        //enable the scripts again
    }


}
