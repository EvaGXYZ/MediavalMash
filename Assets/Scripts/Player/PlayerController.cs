using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Sprites;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject body;

    // dust effect
    public ParticleSystem dust;

    [SerializeField]
    private int playerIndex = 0;

    // movement
    public Vector2 movement;
    public float moveSpeed = 2f;
    private float originalMoveSpeed = 2f;
    Vector3 currentScale;
    //public GameObject body;

    // health system
    public int maxHealth = 5;
    public int currentHealth;

    // health bar
    public HealthBar healthBar;

    // stamina system
    public int maxStamina = 3;
    public int currentStamina;
    private float currentTime;
    public float refillTimeGap = 1.5f;

    // stamina bar
    public StaminaBar staminaBar;

    // attack setting
    private bool attacking = false;

    [Header("Player Body")]
    public SpriteRenderer PlayerSprite;
    public Transform weaponSlot;
    public Transform toolSlot;

    // dash
    private Vector2 dashDirection;
    private float dashSpeed;
    private float maxDashSpeed = 10f;
    private float dashSpeedDropMultiplier = 2f;
    private float dashSpeedMinimum = 4f;
    private int dashCost = 1;
    private enum State { Normal, Dashing, }
    private State state;
    private bool dashed = false;

    // interact
    //weapon
    private bool weaponEquiped= false;
    private bool canEquipWeapon = false;
    private Weapon weapon;
    private Vector2 direction;
    

    // tool
    private bool toolPicked = false;
    private bool canPickTool = false;
    private Tool tool;
    
    // throw
    private Vector3 toolCurrentPosition;
    private Vector3 weaponCurrentPosition;
    private bool canThrow = false;
    private bool throwed = false;
    private bool throwingTool = false;
    private bool throwingWeapon = false;
    public int throwSpeed = 2;
    private float throwStartTime;
    private float throwTimeGap = 1.5f;

    // damage
    private int damageOfSpikes = 1;
    // smoke
    private int damageOfSmoke = 1;
    private float withinSmokeStart;
    private float timeGapForSecondDamageOfSmoke = 5f;

    // Speed Up Zone & icy ground
    private float speedUp = 5f;

    // get player sprite
    public CharManager playerSprite;
    //private SpriteRenderer sr;
    private int selectedSprite;
    private Sprite spriteSelected;
    private string savingSprite;

    // death
    public bool playerIsDead = false;

    // score
    public int playerScore;

    // flash
    public SimpleFlash flash;

    // invincible flash
    [SerializeField] private ColoredFlash invincible;
    Color[] colors = { Color.red, Color.blue, Color.gray, Color.cyan, Color.yellow, Color.magenta, Color.green, Color.white};
    [SerializeField] private KeyCode flashKey;
    public bool crownPickedUp = true;
    public bool isInvincible = false;
    private float invincibleStart;
    private float invincibleTime = 6f;
    public Vector2 lastMoveDirection;
    private float lastFlashTime;
    private int lastIndex = 0;

    // sound effect
    public AudioSource audioSource;
    public AudioClip playerDeath;
    private bool playDeathSound= false;

    private void Awake()
    {
        // get sprite renderer
        //sr = GetComponent<SpriteRenderer>();
        savingSprite = "Player" +playerIndex + " SelectedSprite";

        // load previous option
        // if no save info
        if (!PlayerPrefs.HasKey(savingSprite))
        {
            selectedSprite = 0;
        }
        else// get sprite number
        {
            selectedSprite = LoadSprite();
        }

        // assign sprite to player
        spriteSelected = playerSprite.UpdateSprite(selectedSprite);
        PlayerSprite.sprite = spriteSelected;
    }
    private void OnEnable()
    {
        InitializePlayer();
    }
    // reset when open a new scene
    private void InitializePlayer()
    {
        // setup health
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        playerIsDead = false;

        // setup stamina
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);

        // setup state
        state = State.Normal;

        // remove weapons
        if (weapon != null)
        {
            Destroy(this.weapon.gameObject);
            weaponEquiped = false;
            canEquipWeapon = true;
        }
        // remove tools
        if (tool != null)
        {
            Destroy(this.tool.gameObject);
            toolPicked = false;
            canPickTool = true;
        }

        // reset flash
        flash.ResetFlash();

        //reset invincible
        invincible.ResetFlash();
        isInvincible = false;
        invincibleStart = 0f;
        crownPickedUp = false;
        playerIsDead = false;

        // reset death sound
        playDeathSound = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        // setup health
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        // setup stamina
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);

        // setup state
        state = State.Normal;

    }

    // Update is called once per frame
    void Update()
    {
        // invincible effect
        if (crownPickedUp && !isInvincible)
        {
            // record time
            invincibleStart = Time.time;
            // player is invincible now
            isInvincible = true;
        }

        //  trigger the flash effect
        if (isInvincible)
        {
            // within invincibleTime
            if (Time.time < invincibleStart + invincibleTime)
            {
                if(Time.time - lastFlashTime > 0.05f)
                {
                    // flash color
                    if(lastIndex < colors.Length)
                    {
                        // load next color
                        invincible.Flash(colors[lastIndex]);
                    }
                    else
                    {
                        // load first one
                        invincible.Flash(colors[0]);
                        lastIndex = 0;
                    }
                    lastIndex++;
                    lastFlashTime = Time.time;
                }
                
            }else
            {
                // invinvible time runs out
                isInvincible = false;
                crownPickedUp = false;
                invincible.ResetFlash();
                // destroy the crown
                tool.IsPicked = false;
                Destroy(tool.gameObject);
            }
        }

        //move
        if(movement != new Vector2(0,0))
        {
            lastMoveDirection = movement.normalized;
            // play dust
            CreateDust();
        }
        // dash
        switch (state)
        {
            case State.Normal:
                // move
                transform.position += new Vector3 (movement.x * moveSpeed * Time.deltaTime, movement.y * moveSpeed * Time.deltaTime, 0);

                // detect dash
                if (dashed & currentStamina >= 1)
                {
                    // update dash direction
                    dashDirection = lastMoveDirection;
                    // update dash speed and change the state to dashing    
                    dashSpeed = maxDashSpeed;
                    // change to dashing state
                    state = State.Dashing;
                }
                break;
            case State.Dashing:
                // reduce 1 stamina
                if (dashSpeed >= maxDashSpeed)
                {
                    StaminaCost(dashCost);
                }
                // dashing
                transform.position += new Vector3(dashDirection.x * dashSpeed * Time.deltaTime, dashDirection.y * dashSpeed * Time.deltaTime, 0);
                // reduce dash speed
                dashSpeed -= dashSpeed * dashSpeedDropMultiplier * Time.deltaTime;

                // rollspeed lower than minimum, state back to normal
                if (dashSpeed < dashSpeedMinimum)
                {
                    state = State.Normal;
                }
                break;
        }

        DetectDirection();

        // Throw weapon of tool
        // player has tool or weapon
        if (canThrow && throwed && !throwingTool && !throwingWeapon)
        {
            Throw();
        }
        // throwing
        // move tool or weapon after throw it
        if (throwingTool)
        {
            // moving forward
            // destory after timegap
            if (Time.time > throwStartTime + throwTimeGap)
            {
                // can pick up a new tool
                tool = null;
                toolPicked = false;
                throwingTool = false;
            }
            
        }
        else if (throwingWeapon)
        {
            // making damage
            if (weapon.IsFlying)
            {
                // with throw range
                weapon.CollisionDetect(weapon, direction);
                // destory after timegap
                if (Time.time > throwStartTime + throwTimeGap)
                {
                    // can equip a new weapon of player controller
                    weaponEquiped = false;
                    throwingWeapon = false;
                    weapon = null;
                    Debug.Log("<color=green> Throw ends in pc</color>");
                }
            }
            
        }
        // set canThrow false when nothing to throw
        if (tool == null && weapon == null)
        {
            canThrow = false;
        }

        // check health bar
        if (Input.GetKeyDown(KeyCode.Z))
        {
            TakeDamage(1);
        }

        // check stamina bar
        if (Input.GetKeyDown(KeyCode.X))
        {
            StaminaCost(1);
        }
        // refill stamina
        if (Time.time >= (currentTime + refillTimeGap) && currentStamina < maxStamina)
        {
            StaminaRefill();
        }

        // check health status
        if (currentHealth <= 0 && !playDeathSound)
        {
            playDeathSound = true;
            StartCoroutine(Death());
        }

        // Draw weapon attack range
        if (weaponEquiped)
        {
            // enable the attackrange sprite
            weapon.EnableAttackRangeSprite();
            // assign the color
            weapon.GetAttackRangeColor(playerIndex);
        }

        // attacking // make sure there is weapon and stamina available; button pressed; not in throwing
        if (weapon != null && !throwingWeapon && attacking && currentStamina >= weapon.StaminaCost)
        {
            // reduce stamina
            StaminaCost(weapon.StaminaCost);
            // attack with weapon
            Attack();
            attacking = false;
        }
    }

    void FixedUpdate()
    {
        
    }

    public void ResetThrowStat()
    {
        weaponEquiped = false;
        throwingWeapon = false;
        weapon = null;
    }

    // get player index
    public int GetPlayerIndex()
    {
        return playerIndex;
    }

    // load the sprite index
    public int LoadSprite()
    {
        return PlayerPrefs.GetInt(savingSprite);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        attacking = context.action.triggered;
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        dashed = context.action.triggered;
    }

    public void OnThrow(InputAction.CallbackContext context)
    {
        throwed = context.action.triggered;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        canPickTool = context.action.triggered;
    }

    public void OnEquip(InputAction.CallbackContext context)
    {
        canEquipWeapon = context.action.triggered;
    }

    // throw
    public void Throw()
    {
        if (tool != null)
        {
            if(!crownPickedUp)
            {
                // record time
                throwStartTime = Time.time;
                // remove player as parent
                tool.transform.parent = null;
                // record current position
                toolCurrentPosition = tool.transform.position;
                // record direction
                direction = lastMoveDirection;
                throwingTool = true;
                // update bool of tool
                tool.StartThrowing(direction);
            }
        }
        else if (weapon != null)
        {
            // record time
            throwStartTime = Time.time;
            // reset scale
            weapon.transform.localScale = new Vector3(1, 1, 1);
            // remove player as parent
            weapon.transform.parent = null;

            // record direction
            direction = lastMoveDirection;
            throwingWeapon = true;
            // update bool of weapon
            weapon.StartThrowing(direction);
        }
    }

    // change direction of sprite
    public void DetectDirection()
    {
        currentScale = transform.localScale;

        if (movement.x > 0)
        {
            // change direction
            body.transform.localScale = new Vector3(Mathf.Abs(currentScale.x) * -1, currentScale.y, currentScale.z);
            // avoid healthBar and staminaBar flip
            //healthBar.transform.localScale = new Vector3(Mathf.Abs(healthBar.transform.localScale.x) * -1, 1, 1);
            //staminaBar.transform.localScale = new Vector3(Mathf.Abs(staminaBar.transform.localScale.x) * -1, 1, 1);
        }
        else if (movement.x < 0)
        {
            body.transform.localScale = new Vector3(Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
            // avoid healthBar and staminaBar flip
            //healthBar.transform.localScale = new Vector3(Mathf.Abs(healthBar.transform.localScale.x), 1, 1);
            //staminaBar.transform.localScale = new Vector3(Mathf.Abs(staminaBar.transform.localScale.x), 1, 1);
        }
    }

    // calculate health
    public void TakeDamage(int damage)
    {
        // only get hurt when not invincible
        if (!isInvincible)
        {
            // update current health
            currentHealth -= damage;
            // update health bar
            healthBar.SetHealth(currentHealth);
            // blink
            flash.Flash();
        }
    }

    // calculate stamina
    public void StaminaCost(int cost)
    {
        // update current stamina
        currentStamina -= cost;
        // record current time to avoid refill instantly
        currentTime = Time.time;
        // update stamina bar
        staminaBar.SetStamina(currentStamina);
    }

    // refill stamina
    public void StaminaRefill()
    {
        currentStamina ++;
        // record current time
        currentTime = Time.time;
        // update stamina bar
        staminaBar.SetStamina(currentStamina);
    }

    public void Attack()
    {
        // attack animation
        //animator.SetTrigger("Attack");
        if(!weapon.IsFiring)
        {
            weapon.IsFiring = true;
            // with attacking range
            weapon.CollisionDetect(weapon, movement);
            weapon.IsFiring = false;
        }
    }

    // destory player
    private IEnumerator Death()
    {
        // play ah
        audioSource.PlayOneShot(playerDeath, 0.3f);
        yield return new WaitForSeconds(0.4f);
        // disable player
        playerIsDead = true;
        this.gameObject.SetActive(false);
    }

    // equip a weapon
    private void OnCollisionStay2D(Collision2D collision)
    {
        // detect if it is weapon
        if(collision.gameObject.layer == 3 && !weaponEquiped && canEquipWeapon)
        {
            // assign weapon with correct type
            if (collision.gameObject.CompareTag("GoldenSword"))
            {
                weapon = collision.gameObject.GetComponent<GoldenSword>();
            }
            else if (collision.gameObject.CompareTag("Boomerang"))
            {
                weapon = collision.gameObject.GetComponent<Boomerang>();
            }
            else if (collision.gameObject.CompareTag("MagicWand"))
            {
                weapon = collision.gameObject.GetComponent<MagicWand>();
            }
            else if (collision.gameObject.CompareTag("MeteorHammer"))
            {
                weapon = collision.gameObject.GetComponent<MeteorHammer>();
            }
            else if (collision.gameObject.CompareTag("BlackSword"))
            {
                weapon = collision.gameObject.GetComponent<BlackSword>();
            }
            else if (collision.gameObject.CompareTag("Hammer"))
            {
                weapon = collision.gameObject.GetComponent<Hammer>();
            }
            else if (collision.gameObject.CompareTag("Spear"))
            {
                weapon = collision.gameObject.GetComponent<Spear>();
            }

            // pick up the weapon
            if (weapon != null)
            {
                weapon.Display();
                // set as weapon is child
                weapon.transform.parent = weaponSlot.transform;
                // set up position
                if (weapon.Direction.x < 0) // check direction
                {
                    // reset local scale when facing right
                    if(weapon.transform.localScale.x < 0)
                    {
                        weapon.transform.localScale = new Vector3(1, 1, 1);
                    }
                }

                weapon.transform.localPosition = weapon.LocalPosition;

                //update bool of player controller
                weaponEquiped = true;
                canThrow = true;
                // update bool of weapon
                weapon.InitializeWeapon(this);
            }
        }

        // pick a tool
        // detect if it is tool
        if (collision.gameObject.layer == 6 && !toolPicked && canPickTool)
        {
            // assign with tool type
            if (collision.gameObject.CompareTag("Bomb"))
            {
                tool = collision.gameObject.GetComponent<Bomb>();
            }
            else if (collision.gameObject.CompareTag("GoldenCrown"))
            {
                Debug.Log("you are touch golden crown!");
                tool = collision.gameObject.GetComponent<GoldenCrown>();
            }
            // pick up the tool
            if (tool != null)
            {
                // set as weapon is child
                tool.transform.parent = toolSlot.transform;

                // set up position
                if (tool.Direction.x < 0) // check direction
                {
                    // reset local scale when facing right
                    if (tool.transform.localScale.x < 0)
                    {
                        tool.transform.localScale = new Vector3(1, 1, 1);
                    }
                }
                // set up position
                tool.transform.localPosition = tool.LocalPosition;
                //update bool
                toolPicked = true;
                canThrow = true;
                // update bool of tool
                print(tool.name +" is picked, " + tool.IsPicked);
                tool.IsPicked = true;
                tool.InitializeTool(this);

                if (collision.gameObject.CompareTag("GoldenCrown"))
                {
                    crownPickedUp = true;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // get damagae from Spikes
        if (collision.gameObject.CompareTag("Spikes"))
        {
            TakeDamage(damageOfSpikes);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // SpeedUpZone||IcyGround increase speed
        if (collision.gameObject.CompareTag("SpeedUpZone") || collision.gameObject.CompareTag("IcyGround"))
        {
            moveSpeed = speedUp;
        }
        // within the smoke range
        if (collision.gameObject.CompareTag("PosionSmoke"))
        {
            // take damage
            TakeDamage(damageOfSmoke);
            //record tiem
            withinSmokeStart = Time.time;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // get hurt again if stay in the smoke over time gap
        if (collision.gameObject.CompareTag("PosionSmoke"))
        {
            // pass time gap before get hurt again
            if (Time.time >= (withinSmokeStart + timeGapForSecondDamageOfSmoke))
            {
                // take damage
                TakeDamage(damageOfSmoke);
                //record tiem
                withinSmokeStart = Time.time;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // SpeedUpZone||IcyGround set speed back to normal
        if (collision.gameObject.CompareTag("SpeedUpZone") || collision.gameObject.CompareTag("IcyGround"))
        {
            moveSpeed = originalMoveSpeed;
        }
    }

    // play particle effect
    private void CreateDust()
    {
        dust.Play();
    }

    public void ResetToolStat()
    {
        toolPicked = false;
        canPickTool = false;
        tool = null;
    }
}
