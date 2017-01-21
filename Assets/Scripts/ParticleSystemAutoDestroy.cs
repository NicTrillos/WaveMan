using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemAutoDestroy : MonoBehaviour
{
    private ParticleSystem ps;
    private BombController bomb;


    public void Start()
    {
        ps = GetComponent<ParticleSystem>();
        bomb = transform.parent.gameObject.GetComponent<BombController>();
    }

    public void Update()
    {
         if (ps && bomb && bomb.hasExploded)
        {
            if (!ps.IsAlive())
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }
}