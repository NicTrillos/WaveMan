using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSceneController : MonoBehaviour {

    public Animator creditsAnimator;
    public AudioSource buttonClick;

    public void Start()
    {
        creditsAnimator.SetBool("Show", false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowCredits()
    {
        creditsAnimator.SetBool("Show", true);
        Debug.Log("Reached here");
    }

    public void HideCredits()
    {
        creditsAnimator.SetBool("Show", false);
        Debug.Log("Reached here again");
    }
}
