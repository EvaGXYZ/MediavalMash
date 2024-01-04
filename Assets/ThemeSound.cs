using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeSound : MonoBehaviour
{

    private void Awake()
    {
        // keep the same game manager through the game play
        DontDestroyOnLoad(this.gameObject);
    }

        // Start is called before the first frame update
        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level == 6)
        {
            //game manager is destroyable
            //can carry the game manager for next scene
            Destroy(this.gameObject);
        }
    }
}
