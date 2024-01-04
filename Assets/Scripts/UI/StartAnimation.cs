using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimation : MonoBehaviour
{
    // movement
    private Vector2 movement;
    public float moveSpeed = 0.005f;

    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        movement = new Vector2 (this.transform.position.x + moveSpeed, this.transform.position.y);
        this.transform.position = movement;
    }
}
