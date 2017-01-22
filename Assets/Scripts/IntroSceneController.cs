using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSceneController : MonoBehaviour {

    public Animator creditsAnimator;
    public AudioSource buttonClick;

    public void StartGame()
    {
        buttonClick.Play();
        SceneManager.LoadScene("Main");
    }

    public void GoBack()
    {
        buttonClick.Play();
        SceneManager.LoadScene("StartScene");
    }

    public void ExitGame()
    {
        buttonClick.Play();
        Application.Quit();
    }

    public void ShowCredits()
    {
        creditsAnimator.SetBool("Hide", false);
        creditsAnimator.SetBool("Show", true);        
    }

    public void HideCredits()
    {
        creditsAnimator.SetBool("Show", false);
        creditsAnimator.SetBool("Hide", true);
    }
}
