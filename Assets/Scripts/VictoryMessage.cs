using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class VictoryMessage : MonoBehaviour
{
    [SerializeField]
    private Text victoryText;

    public void SetVictoryMessage(string message)
    {
        victoryText.text = message;
    }
}
