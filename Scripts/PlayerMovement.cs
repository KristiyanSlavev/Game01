using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public enum PlayerState
{
    walk,
    attack,
    interact
}
public class PlayerMovement : MonoBehaviour
{
    public PlayerState currentState;
    [SerializeField]
    public float moveSpeed = 5f;
    public float bulletSpeed = 6f;
    public float rotateAngle;

    public Rigidbody2D rb;
    public Animator animator;

    [SerializeField]
    Transform gun;
    [SerializeField]
    Rigidbody2D arrow;

    //Vector2 stores both x and y axis
    private Vector3 movement;

    void Start()
    {
        
        currentState = PlayerState.walk;
        rotateAngle = 0f;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        animator.SetFloat("Horizontal", 0);
        animator.SetFloat("Vertical", -1);
        //animator.speed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        movement = Vector3.zero;
        //Input
        movement.x = Mathf.RoundToInt(CrossPlatformInputManager.GetAxisRaw("Horizontal"));
        movement.y = Mathf.RoundToInt(CrossPlatformInputManager.GetAxisRaw("Vertical"));

        //transform.position = new Vector2(movement.x * moveSpeed * Time.deltaTime + transform.position.x, movement.y * moveSpeed * Time.deltaTime + transform.position.y);

        if (CrossPlatformInputManager.GetButtonDown("Sword1") || CrossPlatformInputManager.GetButtonDown("Arrow1") && currentState != PlayerState.attack)
        {
            Fire();
        }
        else if (currentState == PlayerState.walk)
        {
            UpdateAnimationAndMove();
        }
        
        //UpdateAnimationAndMove();
        
        Rotate();
    }

    void Fire()
    {
        if (CrossPlatformInputManager.GetButtonDown ("Arrow1"))
        {
            //Debug.Log("1");
            StartCoroutine(BowCo()); 
            var firedBullet = Instantiate(arrow, gun.position, gun.rotation);
            firedBullet.AddForce(gun.up * bulletSpeed);
        }

        if (CrossPlatformInputManager.GetButtonDown("Sword1") && currentState != PlayerState.attack)
        {
            
            StartCoroutine(AttackCo());
        }
    }

    void UpdateAnimationAndMove()
    {
        
        if (movement != Vector3.zero)
        {
            //Debug.Log("1");
            MoveCharacter();
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
        currentState = PlayerState.walk;
    }

    private IEnumerator BowCo()
    {
        animator.SetBool("Bow", true);
        currentState = PlayerState.attack;
        yield return null;
        animator.SetBool("Bow", false);
        yield return new WaitForSeconds(.3f);
        currentState = PlayerState.walk;
    }

    void Rotate()
    {
        if (movement.x == 0 && movement.y == 1)
        {
            rotateAngle = 0;
            //Debug.Log(rotateAngle);
            //animator.speed = 1;
            //animator.SetInteger("Direction", 1);
        }

        if (movement.x == 1 && movement.y == 1)
        {
            rotateAngle = -45f;
            //Debug.Log(rotateAngle);
            //animator.speed = 1;
            //animator.SetInteger("Direction", 2);
        }

        if (movement.x == 1 && movement.y == 0)
        {
            rotateAngle = -90f;
            //Debug.Log(rotateAngle);
            //animator.speed = 1;
            // animator.SetInteger("Horizontal", 3);
        }

        if (movement.x == 1 && movement.y == -1)
        {
            rotateAngle = -135f;
            //Debug.Log(rotateAngle);
            //animator.speed = 1;
            //animator.SetInteger("Direction", 4);
        }

        if (movement.x == 0 && movement.y == -1)
        {
            rotateAngle = -180f;
            //Debug.Log(rotateAngle);
            //animator.speed = 1;
            //animator.SetInteger("Direction", 5);
        }

        if (movement.x == -1 && movement.y == -1)
        {
            rotateAngle = -225f;
            //Debug.Log(rotateAngle);
            //animator.speed = 1;
            //animator.SetInteger("Direction", 6);
        }

        if (movement.x == -1 && movement.y == 0)
        {
            rotateAngle = -270f;
            //Debug.Log(rotateAngle);
            //animator.speed = 1;
            //animator.SetInteger("Direction", 7);
        }

        if (movement.x == -1 && movement.y == 1)
        {
            rotateAngle = -315;
            //Debug.Log(rotateAngle);
            //animator.speed = 1;
            // animator.SetInteger("Direction", 8);
        }

        gun.rotation = Quaternion.Euler(0f, 0f, rotateAngle);
         
    }

    void MoveCharacter()
    {
        //Movement
        rb.MovePosition(transform.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
