using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftMovement : MonoBehaviour
{
    public bool wasd = true;
    public float moveSpeed = 10, jumpVelocity=10, fallOff = 3, fallMultiplier = 4, dJump=0, dash=0;
    Rigidbody2D rb;
    ConstantForce2D cf;
    public CircleCollider2D feet;
    public KeyCode[] controls={KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow};
    void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
        cf=GetComponent<ConstantForce2D>();
        if(wasd){
            controls[0]=KeyCode.W; controls[1]=KeyCode.A; controls[2]=KeyCode.S; controls[3]=KeyCode.D;
        }
        cf.force=new Vector2(-9.81f,0);
        feet=GetComponent<CircleCollider2D>();
    }



    void Update()
    {
        if(!(Input.GetKeyUp(controls[3]) && Input.GetKeyUp(controls[1]))){ //movement
            rb.velocity = new Vector2(rb.velocity.x,(((Input.GetKeyUp(controls[1]) || Input.GetKeyUp(controls[3]))?0:Input.GetAxis("Horizontal"+(wasd?2:1)) * moveSpeed) * transform.right).x*-1);
        }
        if(Input.GetKeyDown(controls[0]) && (feet.IsTouchingLayers(LayerMask.GetMask("wall"))||feet.IsTouchingLayers(LayerMask.GetMask("player")))){ //jump
            rb.velocity=new Vector2((transform.right*jumpVelocity).x, rb.velocity.y);
            AudioManager.instance.Play("P2Jump");
            //play jump sfx
        }
        else if(Input.GetKeyDown(controls[0]) && dJump>0){
            rb.velocity=new Vector2((transform.right*(jumpVelocity*1.25f)).x, rb.velocity.y);
            dJump-=1;
            AudioManager.instance.Play("DoubleJump");
            //play double jump sfx
        }
        if(rb.velocity.x < fallOff)
             rb.velocity += (transform.right * cf.force * fallMultiplier  * Time.deltaTime);
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer==7){
            AudioManager.instance.Play("P2Powerup");
            //play powerup sfx
            if(other.gameObject.name.StartsWith("dJump"))
                dJump+=1;
            else if(other.gameObject.name.StartsWith("dash"))
                dash+=1;
            
            other.gameObject.SetActive(false);
        }
    }
}
