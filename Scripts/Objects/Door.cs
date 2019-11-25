using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public enum DoorType
{
    key,
    enemy,
    button
}

//ol(OL
public class Door : Interactable
{
    [Header("Door variables")]
    public DoorType thisDoorType;
    public bool open= false;
    public Inventory playerInventory;
    public SpriteRenderer doorSprite;
    public BoxCollider2D physicsCollider;

    //d0ne if the trigger is part 0f the sprite
    /*private void Start()
    {
        doorSprite = GetComponent<SpriteRenderer>();
    }*/


    void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("Sword1"))
        {
            if (playerInRange && thisDoorType == DoorType.key)
            {
                if (playerInventory.numberOfKeys > 0)
                {
                    playerInventory.numberOfKeys--;
                    Open();
                }
            }
        }
    }

    public void Open()
    {
        doorSprite.enabled = false;
        open = true;
        physicsCollider.enabled = false;
    }

    public void Close()
    {
        doorSprite.enabled = true;
        open = false;
        physicsCollider.enabled = enabled;
    }


}
