using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BombController : NetworkBehaviour
{
	public float timeToExplode;
    [SyncVar]
    public bool hasExploded;
    public ParticleSystem bombEffect;
    public Text bombText;

    [SyncVar]
    private float currentTimeToExplode;    
    
    [ServerCallback]
	void Start () {
        currentTimeToExplode = timeToExplode;
        hasExploded = false;
	}
	
	[ServerCallback]
	void Update () {

        if (bombText != null)
        {
            bombText.text = "" + Mathf.Floor(currentTimeToExplode * 20f);
        }

        CmdDecreaseCurrentTimeToExplode();
        if(currentTimeToExplode <= 0.0f && !hasExploded)
        {
            CmdExplode(gameObject);
        }
	}

    [Command]
    private void CmdDecreaseCurrentTimeToExplode()
    {
        currentTimeToExplode -= Time.deltaTime;
    }

    [Command]
    private void CmdExplode(GameObject go)
    {
        hasExploded = true;
        RpcExplode(go);
    }

    [RPC]
    private void RpcExplode(GameObject go)
    {
        if (bombText != null)
        {
            bombText.enabled = false;
        }
        go.GetComponent<SpriteRenderer>().enabled = false;
        go.GetComponentInChildren<ParticleSystem>().Play();

    }
}
