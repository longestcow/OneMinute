using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownMovement : MonoBehaviour
{
    public bool wasd = true;
    public float moveSpeed = 10, jumpVelocity=10, fallOff = 3, fallMultiplier = 4, dJump=0, dash=0;
    Rigidbody2D rb;
    ConstantForce2D cf;
    public CircleCollider2D feet;
    public GameManager gm;
    bool grounded=false;
    public KeyCode[] controls={KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow};
    void Awake()
    {
        gm=GameObject.Find("GameManager").GetComponent<GameManager>();
        rb=GetComponent<Rigidbody2D>();
        cf=GetComponent<ConstantForce2D>();
        if(wasd){
            controls[0]=KeyCode.W; controls[1]=KeyCode.A; controls[2]=KeyCode.S; controls[3]=KeyCode.D;
        }
        cf.force=new Vector2(0,-9.81f);
        feet=GetComponent<CircleCollider2D>();

    }



    void Update()
    {
        if(!grounded && (feet.IsTouchingLayers(LayerMask.GetMask("wall"))||feet.IsTouchingLayers(LayerMask.GetMask("player")))){
            grounded=true;
            AudioManager.instance.Play("Drop1");
        }
        else if(grounded && !(feet.IsTouchingLayers(LayerMask.GetMask("wall"))||feet.IsTouchingLayers(LayerMask.GetMask("player"))))
            grounded=false;

        if(!(Input.GetKeyUp(controls[3]) && Input.GetKeyUp(controls[1]))){ //movement
            rb.velocity = new Vector2((((Input.GetKeyUp(controls[1]) || Input.GetKeyUp(controls[3]))?0:Input.GetAxis("Horizontal"+(wasd?2:1)) * moveSpeed) * transform.right).x, rb.velocity.y);
        }
        if(Input.GetKeyDown(controls[0]) && grounded){ //jump
            rb.velocity=(transform.up*jumpVelocity);
            AudioManager.instance.Play("P1Jump");
                //play jump sfx
        }
        else if(Input.GetKeyDown(controls[0]) && dJump>0){
            rb.velocity=(transform.up*(jumpVelocity*1.25f));
            dJump-=1;
            AudioManager.instance.Play("DoubleJump");
            //play double jump sfx
        }
 
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

        else if(other.gameObject.name.Equals("flagBlue")){
            gm.nextLevel();
            AudioManager.instance.Play("FlagCapture");

       }
    }

}
