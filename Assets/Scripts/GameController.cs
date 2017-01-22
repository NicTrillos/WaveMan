using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private BoardManager board;
    
    private void Start()
    {
        board.SetupScene(1);
    }

    public void RestartScene()
    {
        NetworkManager.singleton.ServerChangeScene("Main");
    }
}
