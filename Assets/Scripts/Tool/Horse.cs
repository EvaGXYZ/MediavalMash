using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse : Tool
{
    // name/type
    public string _name = "Horse";
    public override string Name
    {
        get { return _name; }
        set { _name = value; }
    }
    // local position for equip
    public Vector3 _localPosition = new Vector3(-0.057f, -0.222f, 0);
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
