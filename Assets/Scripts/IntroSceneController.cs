using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSceneController : MonoBehaviour {

    public Animator creditsAnimator;

    public void Start()
    {
        creditsAnimator.SetBool("Show", false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void ShowCredits()
    {
        creditsAnimator.SetBool("Show", true);
    }

    public void HideCredits()
    {
        creditsAnimator.SetBool("Show", false);
    }
}
