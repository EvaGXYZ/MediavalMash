using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public PolygonCollider2D spikeCollider;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        // find animator and collider
        spikeCollider =GetComponent<PolygonCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // enabel the collider
    public void EnableCollider()
    {
        spikeCollider.enabled = true;
    }

    // disable the collider
    public void DisableCollider()
    {
        spikeCollider.enabled = false;
    }

}
