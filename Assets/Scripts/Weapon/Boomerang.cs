using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Boomerang : Weapon
{
    // Base property:
    // name/type
    public string _name = "Boomerang";
    public override string Name
    {
        get { return _name; }
        set { _name = value; }
    }
    // stamina
    public int _staminaCost = 1;
    public override int StaminaCost
    {
        get { return _staminaCost; }
        set { _staminaCost = value; }
    }
    //damage
    public int _damage = 1;
    public override int Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }
    // attack range
    public float _range = 3f;
    public override float Range
    {
        get { return _range; }
        set { _range = value; }
    }
    // local position for equip
    public Vector3 _localPosition = new Vector3 (-0.506f, 0.109f, 0);
    public override Vector3 LocalPosition
    {
        get { return _localPosition; }
        set { _localPosition = value; }
    }
    // attack direction = player.movement
    public Vector2 _direcion;
    public override Vector2 Direction
    {
        get { return _direcion; }
        set { _direcion = value; }
    }
    // attack with weapon
    public bool _isFiring = false;
    public override bool IsFiring
     {
        get { return _isFiring; }
        set { _isFiring = value; }
    }
    // throwing weapon
    public bool _isThrowing = false;
    public override bool IsThrowing
    {
        get { return _isThrowing; }
        set { _isThrowing = value; }
    }
    // parent player equip weapon
    public bool _isEquipped = false;
    public override bool IsEquipped
    {
        get { return _isEquipped; }
        set { _isEquipped = value; }
    }
    //set up attack position
    public Transform _attackPoint;
    public override Transform attackPoint
    {
        get { return _attackPoint; }
        set { _attackPoint = value; }
    }
    // __Base property
    // Start is called before the first frame update
    void awake()
    {
    }

    // Update is called once per frame
    //void Update()
    //{
    //    GetParentPlayer();
    //}

    // attacking
    // players be hitted and reduce damage
    public override void MakeDamage()
    {
        target.TakeDamage(_damage);
    }

    

    // draw range
    public void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, _range);
    }


    //// parent player position
    //public Vector3 _parentPlayerPosition;
    //public Vector3 SetParentPlayerPosition(Vector3 position)
    //{
    //    return _parentPlayerPosition;
    //}

    //// attacking
    //// players be hitted and reduce damage
    //public override void MakeDamage()
    //{
    //    target.TakeDamage(_damage);
    //}

    //// boomerang move forward
    //public float moveSpeed = 2f;
    //public Vector3 currentPosition;
    //public void MoveForward(Vector2 direction)
    //{
    //    currentPosition = this.transform.position;
    //    this.transform.position += new Vector3(_direcion.x * moveSpeed * Time.deltaTime, _direcion.y * moveSpeed * Time.deltaTime, 0);
    //}
    //// boomerang move to parent player
    //public void MoveBackToParent(Vector2 direction)
    //{
    //    currentPosition = this.transform.position;
    //    this.transform.position += new Vector3(_direcion.x * moveSpeed * Time.deltaTime, _direcion.y * moveSpeed * Time.deltaTime, 0);
    //}

    //// create a raycast to detect the collider type
    //public override void CollisionDetect(Weapon weapon, Vector2 direction, float range)
    //{
    //    // create a ray cast
    //    RaycastHit2D[] allHits = Physics2D.CircleCastAll(weapon.attackPoint.transform.position, range, direction);
    //    // get the hit
    //    for (int i = 0; i < allHits.Length; i++)
    //    {
    //        // if hit player, cause damage
    //        if (allHits[i].collider.gameObject.CompareTag("Player"))
    //        {
    //            // save target
    //            target = allHits[i].collider.gameObject.GetComponent<PlayerController>();
    //            // make damage
    //            MakeDamage();
    //        } // break breakable obstacles
    //    }
    //    //update the stats of weapon
    //    IsFiring = false;
    //    IsThrowing = false;
    //}


    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    GetParentPlayer();
    //    //if (_isFiring || _isThrowing)
    //    //{
    //    //    MoveForward(Direction);
    //    //    CollisionDetect(this, Direction);

    //    //    if (Vector3.Distance(currentPosition, this.transform.position) > _range && _isFiring)
    //    //    {
    //    //        MoveBackToParent(this.GetComponentInParent<Transform>().position);
    //    //    }
    //    //}
    //}

    //// draw range
    //public void OnDrawGizmosSelected()
    //{
    //    Gizmos.DrawWireSphere(attackPoint.position, _range);
    //}
}
