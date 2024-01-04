using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bomb : Tool
{
    // name/type
    public string _name = "Bomb";
    public override string Name
    {
        get { return _name; }
        set { _name = value; }
    }
    // local position for equip
    public Vector3 _localPosition = new Vector3(-0.41f, -0.03f, 0);
    public override Vector3 LocalPosition
    {
        get { return _localPosition; }
        set { _localPosition = value; }
    }
    // throwing tool
    public bool _isThrowing = false;
    public override bool IsThrowing
    {
        get { return _isThrowing; }
        set { _isThrowing = value; }
    }
    // parent player pick up tool
    public bool _isPicked = false;
    public override bool IsPicked
    {
        get { return _isPicked; }
        set { _isPicked = value; }
    }

    // public Vector2 _direcion;
    public Vector2 _direcion;
    public override Vector2 Direction
    {
        get { return _direcion; }
        set { _direcion = value; }
    }

    // Bomb
    // explode
    public float explosionTime = 0f;
    public float range = 1f;
    public int _damage = 1;
    private bool isTriggered= false;
    // range sprite
    public GameObject explodeRangeSprite;

    void Awake()
    {
        // disable the sprite
        explodeRangeSprite.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // explode in 3 seconds after picked by player
        if (!isTriggered && IsPicked && explosionTime < Time.time)
        {
            // record current time
            explosionTime = 3 + Time.time;
            //print("explostion time" + explosionTime); 
            // bomb is triggered
            isTriggered = true;
            // enable explode range
            explodeRangeSprite.SetActive(true);
        }

        // after time gap bomb explode
        if (isTriggered && Time.time >= explosionTime)
        {
            // make damage to players around
            CollisionDetect(this, range);

            if(transform.parent != null)
            {
                parentPlayer.ResetToolStat();
            }

            // reset bool
            isTriggered = false;
            IsPicked = false;
            // then destroy the game object
            Destroy(this.gameObject);
            
        }

        if(IsThrowing)
        {
            if(Time.time < MoveStartTime + MoveDuration)
                Move();
            else
            {
                MoveEnd();
            }
        }
    }

   

    public void CollisionDetect(Bomb bomb, float range)
    {
        // create a ray cast
        RaycastHit2D[] allHits = Physics2D.CircleCastAll(bomb.transform.position, range, new Vector2(0,0));
        // get the hit
        for (int i = 0; i < allHits.Length; i++)
        {
            // if hit player, cause damage
            if (allHits[i].collider.gameObject.CompareTag("Player"))
            {
                // save target
                target = allHits[i].collider.gameObject.GetComponent<PlayerController>();
                // make damage
                MakeDamage();
            } // break breakable obstacles
        }
        //update the stats of weapon
    }

    public void MakeDamage()
    {
        target.TakeDamage(_damage);
    }

    // draw range
    public void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(this.transform.position, range);
    }

    
}
