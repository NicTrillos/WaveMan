using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class IntroSceneController : NetworkBehaviour {

    public Animator creditsAnimator;
    public Animator howToPlayAnimator;
    public AudioSource buttonClick;

    public void StartGame()
    {
        buttonClick.Play();
        SceneManager.LoadScene("Main");
    }

    public void GoBack()
    {
        buttonClick.Play();
        if(isServer)
        {
            NetworkManager.singleton.StopHost();
        }
        else if(isClient)
        {
            NetworkManager.singleton.StopClient();
        }
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

    public void ShowHowToPlay()
    {
        howToPlayAnimator.SetBool("Hide", false);
        howToPlayAnimator.SetBool("Show", true);
    }

    public void HideHowToPlay()
    {
        howToPlayAnimator.SetBool("Show", false);
        howToPlayAnimator.SetBool("Hide", true);
    }
}
