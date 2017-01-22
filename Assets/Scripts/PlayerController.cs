using System.Collections;
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
    public int direction;
    [SyncVar]
    public float currentStamina;
    public float staminaDecrease;
    public float staminaIncrease;
    public float bombIncreaseRate;
    public float bombBasicSize;
    public float bombIncreaseSpeedRate;
    public float bombBasicSpeed;
    public float bombBasicTime;
    public float bombTimeIncreaseRate;
    public GameObject bombEffect;
    public Slider staminaBar;
    public Text victoryText;
    public Animator animator;
    public Animator victoryAnimator;
    public Animator defeatAnimator;
    public SpriteRenderer spriteRenderer;
    public GameObject playerBomb;
    public AudioSource winSound;
    public AudioSource loseSound;

    [SyncVar]
    public bool isGameEnd;
    [SyncVar]
    public bool isDead;

    private bool isCharging;
    [SyncVar]
    public float bombIncrease = 0.0f;
    [SyncVar]
    public float bombIncreaseTime = 0.0f;
    [SyncVar]
    public float bombIncreaseSpeed = 0.0f;

    private void Reset()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    private void Awake()
    {
        if (!hasAuthority) return;
        direction = -1;
        isGameEnd = false;
        isDead = false;
    }

    [ServerCallback]
	void Start () {
        currentStamina = maxStamina;
        isCharging = false;
	}

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        GetComponent<BoxCollider2D>().enabled = true;
    }

    // Update is called once per frame
    void Update ()
    {
        if (!isLocalPlayer) return;

        if(!isGameEnd)
        {
            if (!isCharging)
            {
                var moveX = Input.GetAxisRaw("Horizontal");
                var moveY = Input.GetAxisRaw("Vertical");

                if (moveX == 0.0f && moveY == 0.0f)
                {
                    direction = -1;
                }

                if (moveY < 0.0f)
                {
                    direction = 0;
                }
                else if (moveY > 0.0f)
                {
                    direction = 2;
                }

                if (moveX > 0.0f)
                {
                    direction = 1;
                    spriteRenderer.flipX = false;
                }
                else if (moveX < 0.0f)
                {
                    direction = 3;
                    spriteRenderer.flipX = true;
                }

                animator.SetInteger("Direction", direction);

                rigidbody2d.velocity = new Vector2(moveX, moveY) * charSpeed;

                if(currentStamina + staminaIncrease*Time.deltaTime > maxStamina)
                {
                    currentStamina = maxStamina;
                }
                else
                {
                    currentStamina += staminaIncrease * Time.deltaTime;
                }
            }

            if (Input.GetKeyDown("space") && currentStamina > 0.0f)
            {
                isCharging = true;
                playerBomb.SetActive(true);                
            }

            if (isCharging)
            {
                rigidbody2d.velocity = new Vector2(0.0f,0.0f);
                if (currentStamina <= 0.0f)
                {
                    isCharging = false;
                    CmdActivateBomb(transform.position);
                }
                else
                {
                    CmdIncreaseBomb();
                    CmdDecreaseStamina();
                }
                direction = -1;
            }

            if (Input.GetKeyUp("space") && isCharging)
            {
                isCharging = false;
                CmdActivateBomb(transform.position);
            }

            if (staminaBar != null)
            {                
                staminaBar.value = currentStamina / maxStamina;
            }

            animator.SetBool("Charging", isCharging);
        }        
    }


    [Command]
    private void CmdIncreaseBomb()
    {
        bombIncrease += bombIncreaseRate * Time.deltaTime;
        bombIncreaseTime += bombTimeIncreaseRate * Time.deltaTime;
        bombIncreaseSpeed += bombIncreaseSpeedRate * Time.deltaTime;
    }

    [Command]
    private void CmdActivateBomb(Vector3 position)
    {        
        RpcActivateBomb(position, bombIncrease, bombIncreaseTime);
        bombIncrease = 0.0f;
        bombIncreaseTime = 0.0f;
        bombIncreaseSpeed = 0.0f;
    }

    [Command]
    private void CmdDecreaseStamina()
    {
        currentStamina -= staminaDecrease * Time.deltaTime;
    }

    private void OnParticleCollision(GameObject collision)
    {
        if (!isLocalPlayer) return;
        isDead = true;
        CmdNotifyDeath();
    }

    [Command]
    private void CmdNotifyDeath()
    {
        RpcActivateEnd();
    }

    [ClientRpc]
    private void RpcActivateEnd()
    {
        GetComponent<PlayerHider>().ShowOther();
        isGameEnd = true;
        isCharging = false;
        GetComponent<Collider2D>().enabled = false;
        staminaBar.transform.Translate(0.0f, -1000.0f, 0.0f);
        if(isDead)
        {
            if(isLocalPlayer)
            {
                animator.SetTrigger("Lose");
            }
            else
            {
                animator.SetTrigger("Win");
            }
            
            defeatAnimator.SetTrigger("Appear");
            loseSound.Play();
        }
        else
        {
            if (isLocalPlayer)
            {
                animator.SetTrigger("Win");
            }
            else
            {
                animator.SetTrigger("Lose");
            }
            victoryAnimator.SetTrigger("Appear");
            winSound.Play();
        }
    }

    [ClientRpc]
    private void RpcActivateBomb(Vector3 position, float bombIncrease, float bombIncreaseTime)
    {
        playerBomb.SetActive(false);
        GameObject bomb = GameObject.Instantiate(bombEffect);
        bomb.transform.position = position;
        BombController bombController = bomb.GetComponent<BombController>();
        bombController.bombEffect.startSpeed = bombBasicSpeed + bombIncreaseSpeed;
        bombController.timeToExplode = bombBasicTime + bombIncreaseTime;
        bombController.waveEffect.startSize = bombBasicSize + bombIncrease;
    }

}
