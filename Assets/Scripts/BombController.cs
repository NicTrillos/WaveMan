using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BombController : NetworkBehaviour
{
    //[SyncVar]
	public float timeToExplode;
    public float timeAdjustment;
    //[SyncVar]
    public bool hasExploded;
    public GameObject smallBomb;
    public GameObject largeBomb;
    public float smallBombLimit;
    public ParticleSystem bombEffect;
    public bool bombEffectActivate;
    public ParticleSystem waveEffect;
    public bool waveEffectActive;
    public GameObject warningObject;
    public Text bombText;
    public AudioSource smallBombExplosion;
    public AudioSource largeBombExplosion;

    //[SyncVar]
    private float currentTimeToExplode;

    private bool isSmall;    
    
    //[ServerCallback]
	void Start () {
        currentTimeToExplode = timeToExplode;
        hasExploded = false;
        if (timeToExplode <= smallBombLimit)
        {
            smallBomb.SetActive(true);
            isSmall = true;
        }
        else
        {
            largeBomb.SetActive(true);
            isSmall = false;
        }
    }
	
    //[ServerCallback]
	void Update () {

        if (bombText != null)
        {
            bombText.text = "" + Mathf.Floor(currentTimeToExplode * timeAdjustment);
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
        
        smallBomb.SetActive(false);
        largeBomb.SetActive(false);

        warningObject.SetActive(false);

        if(isSmall)
        {
            smallBombExplosion.Play();
        }
        else
        {
            largeBombExplosion.Play();
        }
        
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
