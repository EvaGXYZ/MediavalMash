using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tool : MonoBehaviour
{
    //public string _name;
    public abstract string Name { get; set; }
    //public Vector3 _localPosition;
    public abstract Vector3 LocalPosition { get; set; }
    // public bool _isThrowing;
    public abstract bool IsThrowing { get; set; }
    // public bool _isPicked;
    public abstract bool IsPicked { get; set; }
    //get parent player
    public PlayerController parentPlayer;
    // public Vector2 _direcion;
    public abstract Vector2 Direction { get; set; }

    public  Vector2 MoveDirection { get; set; }

    public float MoveSpeed;

    public float MoveDuration;

    public float MoveStartTime;
    //public void GetParentPlayer()
    //{
    //    // get parent player when weapon is with player
    //    if (IsPicked)
    //    {
    //        parentPlayer = this.GetComponentInParent<PlayerController>();
    //    }
    //}

    public void InitializeTool(PlayerController pc)
    {
        parentPlayer = pc;
    }
    // make damage
    public PlayerController target;


    public void StartThrowing(Vector2 moveDirection )
    {
        MoveDirection = moveDirection;
        MoveStartTime = Time.time;
        IsThrowing = true;
    }

    public void Move()
    {
        transform.position += new Vector3(MoveDirection.x, MoveDirection.y, 0) * MoveSpeed  * Time.deltaTime;
    }

    public void MoveEnd()
    {
        IsPicked = false;
        IsThrowing = false;
    }

    private void OnDestroy()
    {
        if(parentPlayer != null)
        {
            parentPlayer.ResetToolStat();
        }
    }
}
