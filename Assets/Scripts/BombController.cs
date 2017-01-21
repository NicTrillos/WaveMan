using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombController : MonoBehaviour {

	public float timeToExplode;
    public bool hasExploded;
    public ParticleSystem bombEffect;
    public Text bombText;

    private float currentTimeToExplode;    
    
    // Use this for initialization
	void Start () {
        currentTimeToExplode = timeToExplode;
        hasExploded = false;
	}
	
	// Update is called once per frame
	void Update () {
        if(bombText != null)
        {
            bombText.text = "" + Mathf.Ceil(currentTimeToExplode);
        }
        currentTimeToExplode -= Time.deltaTime;
        if(currentTimeToExplode <= 0.0f && !hasExploded)
        {
            hasExploded = true;
            GetComponent<SpriteRenderer>().enabled = false;
            bombText.enabled = false;
            bombEffect.Play();
        }
	}
}
