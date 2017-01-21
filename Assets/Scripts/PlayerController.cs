﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour {

    [SerializeField]
    private Rigidbody2D rigidbody2d;

    public float charSpeed;
    public float horizontalLimit;
    public float verticalLimit;
    public float maxStamina;
    public float currentStamina;
    public float staminaDecrease;
    public float bombIncreaseRate;
    public GameObject bombEffect;
    public Slider staminaBar;

    public bool isCharging;
    public float bombIncrease = 0.0f;

    private void Reset()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

	// Use this for initialization
	void Start () {
        currentStamina = maxStamina;
        isCharging = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!isLocalPlayer) return;

        if(!isCharging)
        {
            var moveX = Input.GetAxisRaw("Horizontal");
            var moveY = Input.GetAxisRaw("Vertical");

            rigidbody2d.velocity = new Vector2(moveX, moveY) * charSpeed;

            if (this.transform.position.x > horizontalLimit)
            {
                this.transform.position = new Vector3(horizontalLimit, this.transform.position.y, 0);
            }
            if (this.transform.position.x < -horizontalLimit)
            {
                this.transform.position = new Vector3(-horizontalLimit, this.transform.position.y, 0);
            }
            if (this.transform.position.y > verticalLimit)
            {
                this.transform.position = new Vector3(this.transform.position.x, verticalLimit, 0);
            }
            if (this.transform.position.y < -verticalLimit)
            {
                this.transform.position = new Vector3(this.transform.position.x, -verticalLimit, 0);
            }
        }

        if(Input.GetKeyDown("space") && currentStamina > 0.0f)
        {
            isCharging = true;
        }

        if (isCharging)
        {
            if(currentStamina <= 0.0f)
            {
                ActivateBomb();
            }
            else
            {
                bombIncrease += bombIncreaseRate * Time.deltaTime;
                currentStamina -= staminaDecrease * Time.deltaTime;
            }
        }        

        if(Input.GetKeyUp("space") && isCharging)
        {
            ActivateBomb();
        }

        if(staminaBar != null)
        {
            staminaBar.value = currentStamina / maxStamina;
        }
    }

    private void ActivateBomb()
    {
        isCharging = false;
        GameObject bomb = GameObject.Instantiate(bombEffect);
        bomb.transform.position = this.transform.position;
        bombIncrease = 0.0f;
    }
}
