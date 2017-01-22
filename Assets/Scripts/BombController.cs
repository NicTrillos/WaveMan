using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BombController : NetworkBehaviour
{
    //[SyncVar]
	public float timeToExplode;
    //[SyncVar]
    public bool hasExploded;
    public ParticleSystem bombEffect;
    public bool bombEffectActivate;
    public ParticleSystem waveEffect;
    public bool waveEffectActive;
    public GameObject warningObject;
    public Text bombText;

    //[SyncVar]
    private float currentTimeToExplode;    
    
    //[ServerCallback]
	void Start () {
        currentTimeToExplode = timeToExplode;
        hasExploded = false;
	}
	
    //[ServerCallback]
	void Update () {

        if (bombText != null)
        {
            bombText.text = "" + Mathf.Floor(currentTimeToExplode * 20f);
        }
        
        currentTimeToExplode -= Time.deltaTime;
        if (currentTimeToExplode <= 0.0f && !hasExploded)
        {
            if (bombText != null)
            {
                bombText.enabled = false;
            }
            RpcExplode(gameObject);
        }
	}

    //[ClientRpc]
    private void RpcExplode(GameObject go)
    {
        hasExploded = true;
        if (bombText != null)
        {
            bombText.enabled = false;
        }
        go.GetComponent<SpriteRenderer>().enabled = false;
        warningObject.SetActive(false);
        if(bombEffectActivate && bombEffect != null)
        {
            bombEffect.Play();
        }
        if (waveEffectActive && waveEffect != null)
        {
            waveEffect.Play();
        }
    }
}
