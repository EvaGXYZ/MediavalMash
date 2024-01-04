using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputHandler : MonoBehaviour
{
    // input system
    private PlayerInput PlayerInput;
    private PlayerController playerController;

    private void Awake()
    {
        // setup input system and get player index
        PlayerInput = GetComponent<PlayerInput>();
        // find all player controller
        var playerControllers = FindObjectsOfType<PlayerController>();

        // get player index
        var index = PlayerInput.playerIndex;

        playerController = playerControllers.FirstOrDefault(p => p.GetPlayerIndex() == index);
        

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
