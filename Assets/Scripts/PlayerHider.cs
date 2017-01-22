using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerHider : NetworkBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRender;

    private void Reset()
    {
        spriteRender = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (!isLocalPlayer)
        {
            spriteRender.enabled = false;
        }
    }

    public void ShowOther()
    {
        if (!isLocalPlayer)
        {
            spriteRender.enabled = true;
        }
    }
}
