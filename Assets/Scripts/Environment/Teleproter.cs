using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleproter : MonoBehaviour
{
    [SerializeField] private Transform destination;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        destination.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // move to the new teleporter
    public Transform GetDestination()
    {
        return destination;
    }
}
