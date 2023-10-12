using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class LeftMovement : MonoBehaviour
{
    public float moveSpeed = 10, jumpVelocity=10, fallOff = 3, fallMultiplier = 4, dJump=0, dash=0;
    Rigidbody2D rb;
    ConstantForce2D cf;
    public CircleCollider2D feet;
    public GameManager gm;
    bool grounded = true;
    public KeyCode[] controls={KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D};
    public float movement, currentValue=0.0f,  smoothTime = 0.12f, smoothingV = 0.1f;
    void Awake()
    {
        gm=GameObject.Find("GameManager").GetComponent<GameManager>();
        rb=GetComponent<Rigidbody2D>();
        cf=GetComponent<ConstantForce2D>();
        cf.force=new Vector2(-9.81f,0);
        feet=GetComponentInChildren<CircleCollider2D>();
    }



    void Update()
    {
        if(gm.start || gm.settingsPage) return;
        if(!grounded && (feet.IsTouchingLayers(LayerMask.GetMask("wall"))||feet.IsTouchingLayers(LayerMask.GetMask("player")))){
            grounded=true;
            AudioManager.instance.Play("Drop2");
        }
        else if(grounded && !(feet.IsTouchingLayers(LayerMask.GetMask("wall"))||feet.IsTouchingLayers(LayerMask.GetMask("player"))))
            grounded=false;
        
        currentValue = Mathf.SmoothDamp(currentValue, movement, ref smoothingV, smoothTime);
        rb.velocity = new Vector2(rb.velocity.x,(currentValue * moveSpeed * transform.right).x*-1);

        

        if(rb.velocity.x < fallOff)
             rb.velocity += (transform.right * cf.force * fallMultiplier  * Time.deltaTime);
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer==7){
            AudioManager.instance.Play("P1Powerup");
            //play powerup sfx
            if(other.gameObject.name.StartsWith("dJump"))
                dJump+=1;
            else if(other.gameObject.name.StartsWith("dash"))
                dash+=1;
            other.gameObject.SetActive(false);

        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.name=="flagRed"){
            other.gameObject.SetActive(false);
            gm.nextLevel();
            AudioManager.instance.Play("FlagCapture");
       }
    }

    public void Jump()
    {
        if(gm.start || gm.settingsPage) return;
        if(grounded){ 
            rb.velocity=new Vector2((transform.right*jumpVelocity).x, rb.velocity.y);
            AudioManager.instance.Play("P2Jump");
        }
        else if(dJump>0){
            rb.velocity=new Vector2((transform.right*(jumpVelocity*1.25f)).x, rb.velocity.y);
            dJump-=1;
            AudioManager.instance.Play("DoubleJump");
        }
    }


    

}
