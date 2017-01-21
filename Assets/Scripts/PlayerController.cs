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
    public float bombIncreaseRate;
    public float bombBasicSize;
    public float bombBasicTime;
    public float bombTimeIncreaseRate;
    public GameObject bombEffect;
    public Slider staminaBar;
    public Text victoryText;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public GameObject winText;
    public GameObject loseText;
    public GameObject tryButton;
    public bool isGameEnd;

    [SyncVar]
    private bool isCharging;
    [SyncVar]
    public float bombIncrease = 0.0f;
    [SyncVar]
    public float bombIncreaseTime = 0.0f;

    private void Reset()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    private void Awake()
    {
        if (!isLocalPlayer) return;
        direction = -1;
        isGameEnd = false;
    }

    [ServerCallback]
	void Start () {
        currentStamina = maxStamina;
        isCharging = false;
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

            if (Input.GetKeyDown("space") && currentStamina > 0.0f)
            {
                CmdSetIsCharging(true);
            }

            if (isCharging)
            {
                if (currentStamina <= 0.0f)
                {
                    CmdActivateBomb(transform.position);
                }
                else
                {
                    CmdIncreaseBomb();
                    CmdDecreaseStamina();
                }
            }

            if (Input.GetKeyUp("space") && isCharging)
            {
                CmdActivateBomb(transform.position);
            }

            if (staminaBar != null)
            {                
                staminaBar.value = currentStamina / maxStamina;
            }
        }        
    }
        
    [Command]
    private void CmdSetIsCharging(bool v)
    {
        isCharging = v;
    }

    [Command]
    private void CmdIncreaseBomb()
    {
        bombIncrease += bombIncreaseRate * Time.deltaTime;
        bombIncreaseTime += bombTimeIncreaseRate * Time.deltaTime;
    }

    [Command]
    private void CmdActivateBomb(Vector3 position)
    {
        isCharging = false;
        RpcActivateBomb(position, bombIncrease, bombIncreaseTime);
        bombIncrease = 0.0f;
        bombIncreaseTime = 0f;
    }

    [Command]
    private void CmdDecreaseStamina()
    {
        currentStamina -= staminaDecrease * Time.deltaTime;
    }

    [ServerCallback]
    private void OnParticleCollision(GameObject collision)
    {
        RpcActivateDeath(gameObject);
    }

    [ClientRpc]
    private void RpcActivateDeath(GameObject go)
    {
        isGameEnd = true;
        GetComponent<Collider2D>().enabled = false;
        if(gameObject == go)
        {
            animator.SetTrigger("Lose");
            if (loseText != null)
            {
                loseText.SetActive(true);
            }
        }
        else
        {
            animator.SetTrigger("Win");
            if (winText != null)
            {
                winText.SetActive(true);
            }
        }

        
        if(tryButton != null)
        {
            tryButton.SetActive(true);
        }
    }

    [ClientRpc]
    private void RpcActivateBomb(Vector3 position, float bombIncrease, float bombIncreaseTime)
    {
        GameObject bomb = GameObject.Instantiate(bombEffect);
        bomb.transform.position = position;
        BombController bombController = bomb.GetComponent<BombController>();
        bombController.bombEffect.startSize = bombBasicSize + bombIncrease;
        bombController.timeToExplode = bombBasicTime + bombIncreaseTime;
    }
}
