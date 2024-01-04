using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolShop : MonoBehaviour
{
    // get the prefabs
    public List<GameObject> tools;
    private int currentToolNum;
    // get position
    public Transform position;
    // record time
    private float currentTime;
    private float timeGap = 15f;
    private bool readyForAnotherTool = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // after 15s and ready for another tool
        if (Time.time >= currentTime + timeGap)
        {
            readyForAnotherTool = true;
        }
    }

    // open chest when players are close
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // get the random tool number
            currentToolNum = GetATool();
            // spawn the tool
            Instantiate(tools[currentToolNum], position);
            // record the time
            currentTime = Time.time;
            readyForAnotherTool = false;
        } 
    }

    int GetATool()
    {
        int toolNum = Random.Range(0, 10);
        if (toolNum >= 0 && toolNum < 9)
        {
            // 90% of bomb
            return 0;
        }
        else
        {
            // 10% of crown
            return 1;
        }
    }
}
