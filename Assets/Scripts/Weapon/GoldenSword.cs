using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GoldenSword : Weapon
{
    // Base property:
    // name/type
    public string _name = "GoldenSword";
    public override string Name
    {
        get { return _name; }
        set { _name = value; }
    }
    // stamina
    public int _staminaCost = 3;
    public override int StaminaCost
    {
        get { return _staminaCost; }
        set { _staminaCost = value; }
    }
    //damage
    public int _damage = 2;
    public override int Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }
    // attack range
    public float _range = 2f;
    public override float Range
    {
        get { return _range; }
        set { _range = value; }
    }
    // local position for equip
    public Vector3 _localPosition = new Vector3(-0.652f, 0.424f, 0);
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
    ////set up attack range sprite
    //public SpriteRenderer _attackRange;
    //public override SpriteRenderer attackRange
    //{
    //    get { return _attackRange; }
    //    set { _attackRange = value; }
    //}


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
}
