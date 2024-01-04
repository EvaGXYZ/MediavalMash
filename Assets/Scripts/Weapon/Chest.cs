using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Chest : MonoBehaviour
{
    public Animator animator;
    private bool chestIsOpen = false;
    public List<GameObject> weapons;
    private int currentWeaponNum = 0;
    public Transform position;

    // Start is called before the first frame update
    private void Awake()
    {
        
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // open chest when players are close
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!chestIsOpen)
            {
                // trigger animation
                animator.SetTrigger("Open");
                // get the random tool number
                currentWeaponNum = Random.Range(0, weapons.Count);
                // spawn the tool
                Instantiate(weapons[currentWeaponNum], position);
                // update the bool
                chestIsOpen = true;
                // destory the chest so playes can equip
                //Destroy(this.gameObject, 1f);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //if (collision.gameObject.GetComponent<LayerMask>().ToString() == "Weapon"
        //    || collision.gameObject.GetComponent<LayerMask>().ToString() == "Tool")
        {
           // if (childObject == null)
            {
           //     Destroy(this);
            }
        }
    }
}
