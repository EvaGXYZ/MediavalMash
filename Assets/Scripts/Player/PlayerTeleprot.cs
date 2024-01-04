using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleprot : MonoBehaviour
{
    private GameObject currentTeleporter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Teleporter"))
        {
            // record current teleport players are on
            currentTeleporter = collision.gameObject;
        }
    }

    //
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Teleporter"))
        {
            if (collision.gameObject == currentTeleporter)
            {
                // move to the new destination when exit the teleporter
                transform.position = currentTeleporter.GetComponent<Teleproter>().GetDestination().position;
                //isTeleporting = true;

                // clear the current teleport
                currentTeleporter = null;
            }
        }
    }
}
