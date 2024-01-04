using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionIndicator : MonoBehaviour
{
    public PlayerController parentPlayer;
    public Vector2 direction;

    private Vector2 defaultDirection = Vector2.left;
    // Start is called before the first frame update
    void Start()
    {
        parentPlayer = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        direction = parentPlayer.movement;
        if(direction != Vector2.zero)
        {
            //var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var angle = Vector2.Angle(defaultDirection, direction);
            if(direction.y > 0)
            {
                angle = 360 - angle;
            }
            //Debug.Log("<color=yellow>player movement:" + direction + "--angle is:" + angle + "</color>");
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
