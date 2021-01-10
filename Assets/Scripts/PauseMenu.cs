using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets._2D;

public class PauseMenu : MonoBehaviour
{
    public static bool m_gameIsPause = false ;
    public GameObject pauseMenuUI;
    public GameObject character;
    // Update is called once per frame
    void Update()
    {
        if ( Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_gameIsPause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public bool gameIsPause
    {
        get { return m_gameIsPause; }
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        character.GetComponent<Platformer2DUserControl>().enabled = true;
        Time.timeScale = 1f;
        m_gameIsPause = false;
    }
    public void Menu()
    {
        
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);

    }
    public void Save()
    {
        SaveLoad.Save();
        Resume();
    }
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        character.GetComponent<Platformer2DUserControl>().enabled=false;
        Time.timeScale = 0f;
        m_gameIsPause = true;
    }


}
