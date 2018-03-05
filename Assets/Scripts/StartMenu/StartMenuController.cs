using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuController : MonoBehaviour {

	public void NewGameButton()
    {
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }
    public void LoadGameButton()
    {

    }
    public void SettingsButton()
    {

    }
    public void EndGameButton()
    {
        Application.Quit();
    }
}
