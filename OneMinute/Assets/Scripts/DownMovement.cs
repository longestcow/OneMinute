using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DownMovement : MonoBehaviour
{
    public float moveSpeed = 10, jumpVelocity=10, fallOff = 3, fallMultiplier = 4, dJump=0, dash=0;
    Rigidbody2D rb;
    ConstantForce2D cf;
    public CircleCollider2D feet;
    public GameManager gm;
    bool grounded=true;
    public KeyCode[] controls={KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow};
    public float movement, currentValue=0.0f,  smoothTime = 0.12f, smoothingV = 0.1f;
    LeftMovement player2;
    void Awake()
    {
        gm=GameObject.Find("GameManager").GetComponent<GameManager>();
        rb=GetComponent<Rigidbody2D>();
        cf=GetComponent<ConstantForce2D>();
        cf.force=new Vector2(0,-9.81f);
        feet=GetComponentInChildren<CircleCollider2D>();
        player2 = GameObject.Find("Player2").GetComponent<LeftMovement>();
    }



    void Update()
    {
        if(gm.start || gm.settingsPage) return;
        if(!grounded && (feet.IsTouchingLayers(LayerMask.GetMask("wall"))||feet.IsTouchingLayers(LayerMask.GetMask("player")))){
            grounded=true;
            AudioManager.instance.Play("Drop1");
        }
        else if(grounded && !(feet.IsTouchingLayers(LayerMask.GetMask("wall"))||feet.IsTouchingLayers(LayerMask.GetMask("player"))))
            grounded=false;

        currentValue = Mathf.SmoothDamp(currentValue, movement, ref smoothingV, smoothTime);
        rb.velocity = new Vector2((currentValue * moveSpeed * transform.right).x, rb.velocity.y);
        
 
        if(rb.velocity.y < fallOff)
             rb.velocity += (transform.up * cf.force * fallMultiplier  * Time.deltaTime);
        
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
        if(other.gameObject.name=="flagBlue"){
            other.gameObject.SetActive(false);
            gm.nextLevel();
            AudioManager.instance.Play("FlagCapture");
       }
    }

    public void Jump()
    {
        if(gm.start || gm.settingsPage) return;
        if(grounded){
            rb.velocity=(transform.up*jumpVelocity);
            AudioManager.instance.Play("P1Jump");
        }

        else if(dJump>0){
            rb.velocity=(transform.up*(jumpVelocity*1.25f));
            dJump-=1;
            AudioManager.instance.Play("DoubleJump");
        }
    }


}
