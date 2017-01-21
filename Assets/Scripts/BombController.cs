using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour {

	public float timeToExplode;
    public bool hasExploded;
    public ParticleSystem bombEffect;

    private float currentTimeToExplode;    
    
    // Use this for initialization
	void Start () {
        currentTimeToExplode = timeToExplode;
        hasExploded = false;
	}
	
	// Update is called once per frame
	void Update () {
        currentTimeToExplode -= Time.deltaTime;
        if(currentTimeToExplode <= 0.0f && !hasExploded)
        {
            hasExploded = true;
            GetComponent<SpriteRenderer>().enabled = false;
            bombEffect.Play();
        }
	}
}
