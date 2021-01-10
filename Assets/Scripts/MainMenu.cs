using Prime31.TransitionKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Playgame ()
    {
        var vertical = new VerticalSlicesTransition()
        {
            nextScene = SceneManager.GetActiveScene().buildIndex+1,
            duration = 1.0f
        };
        TransitionKit.instance.transitionWithDelegate(vertical);
    }

    public void QuitGame ()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
