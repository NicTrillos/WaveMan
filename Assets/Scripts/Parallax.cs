using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Parallax : NetworkBehaviour
{
    [SerializeField]
    private float speed = .4f;

    private void Start()
    {
        
    }

    private void Update ()
    {
        
        if (!hasAuthority) return;

        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            var moveX = -Input.GetAxisRaw("Horizontal");
            var moveY = -Input.GetAxisRaw("Vertical");

            transform.Translate(moveX * speed * Time.deltaTime, moveY * speed * Time.deltaTime, 0f);
        }
	}
}
