using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class Weapon : MonoBehaviour
{
    //public string _name;
    public abstract string Name { get; set; }
    //public int _staminaCost;
    public abstract int StaminaCost { get; set; }
    //public int _damage;
    public abstract int Damage { get; set; }
    // attack range
    public abstract float Range { get; set; }
    //public Vector3 _localPosition;
    public abstract Vector3 LocalPosition { get; set; }
    // public Vector2 _direcion;
    public abstract Vector2 Direction { get; set; }
    // public bool _isFiring;
    public abstract bool IsFiring { get; set; }
    // public bool _isThrowing;
    public abstract bool IsThrowing { get; set; }
    // public bool _isEquipped;
    public abstract bool IsEquipped { get; set; }
    public float throwRange = 0.01f;
    //get parent player
    public PlayerController parentPlayer;
    //set up attack position
    public virtual Transform attackPoint { get; set; }
    //public virtual SpriteRenderer attackRange { get; set; }
    public float newAlpha = 0.8f;

    public Vector2 MoveDirection { get; set; }
    public float MoveSpeed;
    public float MoveDuration;
    public float MoveStartTime;
    public bool IsFlying;
    public bool HitEeney;

    private void Awake()
    {
        DisableAttackRangeSprite();
    }

    void Update()
    {
        //if (!IsEquipped)
        //{
        //    parentPlayer = null;
        //}
        //Debug.Log("It is updating");

        if (IsFlying)
        {
            //Debug.Log("It is moving");
            if (Time.time < MoveStartTime + MoveDuration)
            {
                Move();
            }
            else
            {
                MoveEnd();
            }
        }
    }
    public void GetParentPlayer()
    {
        // get parent player when weapon is with player
        if (IsEquipped)
        {
            parentPlayer = this.GetComponentInParent<PlayerController>();
        }
    }
    public void ResetParentPlayer()
    {
        // reset parent player when weapon is un equipped
        if (!IsEquipped)
        {
            parentPlayer = null;
        }
    }


    // attack with weapon
    public PlayerController target;
    public abstract void MakeDamage();
    // detect collision while attacking
    public virtual void CollisionDetect(Weapon weapon, Vector2 direction)
    {
        RaycastHit2D[] allHits = Physics2D.CircleCastAll(weapon.attackPoint.transform.position, Range, direction, 0.03f);
        // get the hit
        for (int i = 0; i < allHits.Length; i++)
        {
            // if hit player, cause damage
            if (allHits[i].collider.gameObject.CompareTag("Player"))
            {
                // save target
                target = allHits[i].collider.gameObject.GetComponent<PlayerController>();
                // make damage
                if (target.name != parentPlayer.name)
                {
                    MakeDamage();
                    HitEeney = true;
                }
            }// stop when hit the wall
            else if (allHits[i].collider.gameObject.CompareTag("Wall"))
            {
                // stop moving
                //parentPlayer.throwSpeed = 0;
                HitEeney = true;
                print("You hit the wall, and speed is: " + parentPlayer.throwSpeed);
            } // break breakable obstacles
        }
        if (HitEeney && !IsFiring)
        {
            MoveEnd();
        }
    }

    // weapon types
    //public enum Weapon { Boomerang, Spear, GoldenSword, MagicWand, MeteorHammer }
    // get parent player current direction
    public void SetDirection(Vector3 direction)
    {
        Direction.Set(direction.x, direction.y);
    }
    // current weapon
    public void Display()
    {
        print("You have euqipped the weapon: " + Name);
    }
    // blow up weapon when crash into unbreakable/undamaged objects
    public void BlowUp(Weapon weapon)
    {
        Destroy(this);
    }
    //Disable Attack Range Sprite
    public void DisableAttackRangeSprite()
    {
        attackPoint.GetComponent<SpriteRenderer>().enabled = false;
    }
    //Enable Attack Range Sprite
    public void EnableAttackRangeSprite()
    {
        attackPoint.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void GetAttackRangeColor(int playerIndex)
    {
        SpriteRenderer attackRangeSprite = attackPoint.GetComponent<SpriteRenderer>();

        if (attackRangeSprite != null && parentPlayer!=null)
        {
            if (parentPlayer.GetPlayerIndex() == 0)
            {
                attackRangeSprite.color = Color.green;
            }
            else if (parentPlayer.GetPlayerIndex() == 1)
            {
                attackRangeSprite.color = Color.red;
            }
            else if (parentPlayer.GetPlayerIndex() == 2)
            {
                attackRangeSprite.color = Color.blue;
            }
            else
            {
                attackRangeSprite.color = Color.yellow;
            }
        }

        // change alpha to 50
        Color tmp = attackRangeSprite.color;
        tmp.a = newAlpha;
        attackRangeSprite.color = tmp;
    }

    public void InitializeWeapon(PlayerController pc)
    {
        IsEquipped = true;
        parentPlayer = pc;
    }

    public void StartThrowing(Vector2 moveDirection )
    {
        MoveDirection = moveDirection;
        MoveStartTime = Time.time;
        IsFlying = true;
        Debug.Log("Start throwing -- " + IsFlying);
    }

    public void Move()
    {
        transform.position += new Vector3(MoveDirection.x, MoveDirection.y, 0) * MoveSpeed * Time.deltaTime;
    }

    public void MoveEnd()
    {
        //IsFiring = false;
        IsEquipped = false;
        IsFlying = false;
        HitEeney = false;
        parentPlayer.ResetThrowStat();
        ResetParentPlayer();
        Debug.Log("End throwing");

    }

    private void OnDestroy()
    {
        if(parentPlayer != null)
            parentPlayer.ResetThrowStat();
    }
}
