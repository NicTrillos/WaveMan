using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField]
    private GameObject victoryTextPrefab;
    private VictoryMessage victoryText;

    [ServerCallback]
    private void Start()
    {
        //RpcCreateText();
    }

    [ClientRpc]
    private void RpcCreateText(GameObject gameObj)
    {
        //if (!isLocalPlayer) return;

        if (victoryText == null)
        {
            var go = Instantiate(victoryTextPrefab);
            victoryText = go.GetComponent<VictoryMessage>();
        }
        if (gameObj == gameObject)
        {
            victoryText.SetVictoryMessage("You Lose");
            Destroy(gameObj);
        }
        else
        {
            victoryText.SetVictoryMessage("You Win");
        }
    }

    [Server]
    public void ShowVictoryMessage()
    {
        RpcShowVictoryMessage();
    }

    //[ServerCallback]
    //private void OnDisable()
    //{
    //    RpcShowVictoryMessage();
    //}

    [Command]
    private void CmdShowVictoryMessage()
    {
        RpcShowVictoryMessage();
    }

    [ServerCallback]
    private void OnParticleCollision(GameObject collision)
    {
        RpcCreateText(gameObject);
    }

    [Command]
    private void CmdDestroyPlayer(GameObject go)
    {
        GetComponent<PlayerHealth>().ShowVictoryMessage();
    }

    [ClientRpc]
    private void RpcShowVictoryMessage()
    {
        if (victoryText == null) return;

        if (isLocalPlayer)
            victoryText.SetVictoryMessage("You Lose");
        else
            victoryText.SetVictoryMessage("You Win");

        Destroy(gameObject);
    }

    [ClientRpc]
    private void RpcDestroyPlayer(GameObject go)
    {
        if (gameObject == go)
        {
            victoryText.SetVictoryMessage("You Lose");
            Destroy(gameObject);
        }
        else
        {
            victoryText.SetVictoryMessage("You Win");
        }
    }
}
