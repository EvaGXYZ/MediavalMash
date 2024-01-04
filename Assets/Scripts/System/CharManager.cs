using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CharManager : MonoBehaviour
{
    // link the sprite to the player
    public PlayerController currentPlayer;
    private SpriteRenderer sr;
    private int playerIndex;
    public string savingSprite;
    // add the images
    public List<Image> images = new List<Image>();
    // set up the selected sprite number
    private int selectedSprite = 0;
    private Sprite spriteSelected;

    // check player is ready
    public bool playerIsReady = false;
    // UI input
    private bool isBack = false;
    private bool isNext = false;
    private bool isConfirmed = false;
    private bool isCancelled = false;

    private void Awake()
    {
        // get current player's sprite renderer
        sr = currentPlayer.PlayerSprite;
        //only display the intial image
        images[0].gameObject.SetActive(true);
        images[1].gameObject.SetActive(false);
        images[2].gameObject.SetActive(false);
        images[3].gameObject.SetActive(false);

        playerIndex = currentPlayer.GetPlayerIndex();
    }
    private void Start()
    {
        // load previous option
        //if (!PlayerPrefs.HasKey("selectedSprite"))
        //{
        //    selectedSprite = 0;
        //}
        //else {
        //    LoadSprite();
        //}
    }

    private void Update()
    {
        // when press the button, call the function
        // next and back only work when player is not ready
        if (isBack && !playerIsReady)
        {
            BackOption();
            // reset the bool
            isBack = false;
        }
        if (isNext && !playerIsReady)
        {
            NextOption();
            // reset the bool
            isNext = false;
        }
        // when press the button, call the function
        if (isConfirmed && !playerIsReady)
        {
            SpriteConfirmed();
        }
        // cancel the choice
        if (isCancelled && playerIsReady)
        {
            SpriteCancelled();
        }
    }
   
    // set up UI input
    public void OnBack(InputAction.CallbackContext context)
    {
        context.action.performed += ctx => isBack = true;
    }
    public void OnNext(InputAction.CallbackContext context)
    {
        context.action.performed += ctx => isNext = true;

    }
    public void OnConfirm(InputAction.CallbackContext context)
    {
        isConfirmed = context.action.triggered;
    }
    public void OnCancel(InputAction.CallbackContext context)
    {
        isCancelled = context.action.triggered;
    }


    // load next sprite
    public void NextOption()
    {
        //print("button clicked!");
        //update the selected sprite
        selectedSprite = selectedSprite + 1;
        //print(selectedSprite);

        // reload from start
        if (selectedSprite >= images.Count)
        {
            selectedSprite = 0;
            // load the initial one and disable the rest
            images[1].gameObject.SetActive(false);
            images[2].gameObject.SetActive(false);
            images[3].gameObject.SetActive(false);
            images[0].gameObject.SetActive(true);
        }
        else
        {
            // load next one
            // disable the current image
            images[selectedSprite - 1].gameObject.SetActive(false);
            // enable the next one
            images[selectedSprite].gameObject.SetActive(true);
            //update the selected sprite
        }
        
    }

    // load previous sprite
    public void BackOption()
    {
        //update the selected sprite
        selectedSprite = selectedSprite - 1;

        if (selectedSprite < 0)
        {
            selectedSprite = images.Count - 1;
            // load the last one and disable the rest
            images[3].gameObject.SetActive(true);
            images[1].gameObject.SetActive(false);
            images[2].gameObject.SetActive(false);
            images[0].gameObject.SetActive(false);
        }
        else {
            // load previous one
            // disable the current image
            images[selectedSprite+1].gameObject.SetActive(false);
            // enable the previous one
            images[selectedSprite].gameObject.SetActive(true);
        }
    }

    // Save the sprite index
    public void SaveSprite()
    {
        //playerIndex = currentPlayer.GetPlayerIndex();
        savingSprite = "Player" +playerIndex + " SelectedSprite";
        PlayerPrefs.SetInt(savingSprite, selectedSprite);
    }

    // load the sprite index
    public int LoadSprite()
    {
       return PlayerPrefs.GetInt(savingSprite);
    }

    // feedback when player choose the sprite
    public void SpriteConfirmed()
    {
        // change the color
        images[selectedSprite].color = ConfirmColor(playerIndex);
        // update the bool
        playerIsReady = true;
        // update the sprite of player
        spriteSelected = UpdateSprite(selectedSprite);
        sr.sprite = spriteSelected;
        SaveSprite();
    }
    public Sprite UpdateSprite(int selectedSprite)
    {
        return images[selectedSprite].sprite;
    }
    // cancel the choice
    public void SpriteCancelled()
    {
        // change the color back
        images[selectedSprite].color = Color.white;
        // update the bool
        playerIsReady = false;
    }
    // start the game
    public void PlayGame()
    {
        // save the sprite as a prefab
        //PrefabUtility.SaveAsPrefabAsset(playerSkin, "Assets/Prefabs/Skins/Sprite.prefab");
        // load the game scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // change confirm color based on player index
    Color ConfirmColor(int playerIndex)
    {
        if (playerIndex == 0)
        {
            return Color.green;
        }
        else if (playerIndex == 1)
        {
            return Color.red;
        }
        else if (playerIndex == 2)
        {
            return Color.blue;
        }
        else {
            return Color.yellow;
        }
    }
}
