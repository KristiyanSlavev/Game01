using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public enum PlayerState
{
    walk,
    attack,
    interact,
    stagger,
    idle
}
public class PlayerMovement : MonoBehaviour
{
    public PlayerState currentState;
    [SerializeField]
    public float moveSpeed = 5f;
    
    public float rotateAngle;
    private Vector3 movement;

    public Rigidbody2D myRigidbody;
    public Animator animator;

    //TODO break off the health system
    public FloatValue currentHealth;
    public Signal playerHealthSignal;
    public VectorValue startingPosition;

    // TODO breal off player inventory
    public Inventory playerInventory;
    public SpriteRenderer receivedItemSprite;

    // TODO player hit part of the health system
    public Signal playerHit;

    // TODO player magic part of magic system
    public Signal reduceMagic;

    // TODO break off iframe
    [Header("IFrame Stuff")]
    public Color flashColor;
    public Color regularColor;
    public float flashDuration;
    public int numberOfFlashes;
    public Collider2D triggerCollider;
    public SpriteRenderer mySprite;
    
    //TODO break off
    [Header("Projectile Stuff")]
    public GameObject projectile;
    public Item bow;


    //Vector2 stores both x and y axis
    

    void Start()
    {
        
        currentState = PlayerState.walk;

        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        animator.SetFloat("Horizontal", 0);
        animator.SetFloat("Vertical", -1);
        transform.position = startingPosition.initialValue;
    }

    // Update is called once per frame
    void Update()
    {

        if (currentState == PlayerState.interact)
        {
            return;
        }
        movement = Vector3.zero;
        //Input
        movement.x = Mathf.RoundToInt(CrossPlatformInputManager.GetAxisRaw("Horizontal"));
        movement.y = Mathf.RoundToInt(CrossPlatformInputManager.GetAxisRaw("Vertical"));

        //transform.position = new Vector2(movement.x * moveSpeed * Time.deltaTime + transform.position.x, movement.y * moveSpeed * Time.deltaTime + transform.position.y);

        if (CrossPlatformInputManager.GetButtonDown("Sword1") /*|| CrossPlatformInputManager.GetButtonDown("Arrow1")*/ 
            && currentState != PlayerState.attack && currentState != PlayerState.stagger)
        {
            
            StartCoroutine(AttackCo());
        }
        else if(CrossPlatformInputManager.GetButtonDown("Arrow1")
            && currentState != PlayerState.attack && currentState != PlayerState.stagger)
        {
            if (playerInventory.CheckForItem(bow))
            {
                StartCoroutine(SecondAttackCo());
            }
        }
        else if (currentState == PlayerState.walk || currentState == PlayerState.idle)
        {
            UpdateAnimationAndMove();
        }
        
        //UpdateAnimationAndMove();
        
        //Rotate();
    }

    //void Fire()
    //{
        /*if (CrossPlatformInputManager.GetButtonDown ("Arrow1"))
        {
            //Debug.Log("1");
            StartCoroutine(BowCo()); 
            var firedBullet = Instantiate(arrow, gun.position, gun.rotation);
            firedBullet.AddForce(gun.up * bulletSpeed);
        }*/

       
   // }

    void UpdateAnimationAndMove()
    {
        
        if (movement != Vector3.zero)
        {
            //Debug.Log("1");
            MoveCharacter();
            movement.x = Mathf.Round(movement.x);
            movement.y = Mathf.Round(movement.y);
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
           
        }
    }

    private IEnumerator AttackCo()
    {
        animator.SetBool("Attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        animator.SetBool("Attacking", false);
        yield return new WaitForSeconds(.3f);
        if (currentState != PlayerState.interact)
        {
            currentState = PlayerState.walk;
        }
        currentState = PlayerState.walk;
    }

    public void RaiseItem()
    {
        if (playerInventory.currentItem != null){
            if (currentState != PlayerState.interact)
            {
                animator.SetBool("receive item", true); 
                currentState = PlayerState.interact;
                receivedItemSprite.sprite = playerInventory.currentItem.itemSprite;
            }
            else
            {
                animator.SetBool("receive item", false);
                currentState = PlayerState.idle;
                receivedItemSprite.sprite = null;
                playerInventory.currentItem = null;
            }
        }
        
    }

    private IEnumerator SecondAttackCo()
    {
        //animator.SetBool("Bow", true);
        currentState = PlayerState.attack;
        yield return null;
        MakeArrow();
        //animator.SetBool("Bow", false);
        yield return new WaitForSeconds(.3f);
        currentState = PlayerState.walk;
    }

    // TODO part of the ability
    private void MakeArrow()
    {
        if(playerInventory.currentMagic > 0)
        {
            Vector2 temp = new Vector2(animator.GetFloat("Horizontal"), animator.GetFloat("Vertical"));
            Arrow arrow = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Arrow>();
            arrow.Setup(temp, ChooseArrowDirection());
            playerInventory.ReduceMagic(arrow.magicCost);
            reduceMagic.Raise();
        }
        
    }

    // TODO part of the ability
    Vector3 ChooseArrowDirection()
    {
        float temp = Mathf.Atan2(animator.GetFloat("Vertical"), animator.GetFloat("Horizontal"))*Mathf.Rad2Deg;
        return new Vector3(0, 0, temp);
    }

    void MoveCharacter()
    {
        //Movement
        myRigidbody.MovePosition(transform.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    // TODO move to its own script
    public void Knock(float knockTime, float damage)
    {
        currentHealth.RuntimeValue -= damage;
        playerHealthSignal.Raise();
        if (currentHealth.RuntimeValue> 0)
        {
            
            StartCoroutine(KnockCo(knockTime));
        }
        else
        {
            this.gameObject.SetActive(false);
        }
        
    }

    // TODO
    private IEnumerator KnockCo(float knockTime)
    {
        playerHit.Raise();
        if (myRigidbody != null)
        {
            StartCoroutine(FlashCo());
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            currentState = PlayerState.idle;
            myRigidbody.velocity = Vector2.zero;
        }
    }

    // TODO move to own script
    private IEnumerator FlashCo()
    {
        int temp = 0;
        triggerCollider.enabled = false;
        while (temp < numberOfFlashes)
        {
            mySprite.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            mySprite.color = regularColor;
            yield return new WaitForSeconds(flashDuration);
            temp++;
        }

        triggerCollider.enabled = true;
    }
}
