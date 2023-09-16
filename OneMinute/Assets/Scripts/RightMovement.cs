using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightMovement : MonoBehaviour
{
    public bool wasd = true;
    public int id;
    public float moveSpeed = 10, jumpVelocity=10, fallOff = 3, fallMultiplier = 4;
    Rigidbody2D rb;
    ConstantForce2D cf;
    public KeyCode[] controls={KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow};
    void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
        cf=GetComponent<ConstantForce2D>();
        if(wasd){
            controls[0]=KeyCode.W; controls[1]=KeyCode.A; controls[2]=KeyCode.S; controls[3]=KeyCode.D;
        }
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z+90);
        cf.force=new Vector2(9.81f,0);
    }



    void Update()
    {
        if(!(Input.GetKeyUp(controls[3]) && Input.GetKeyUp(controls[1]))){ //movement
            rb.velocity = new Vector2(rb.velocity.x,(((Input.GetKeyUp(controls[1]) || Input.GetKeyUp(controls[3]))?0:Input.GetAxis("Horizontal"+id) * moveSpeed) * -transform.right).x);
        }
        if(Input.GetKeyDown(controls[0])){ //jump
            rb.velocity=(-transform.right*jumpVelocity);
        }
        if(rb.velocity.y < fallOff){
             rb.velocity += (-transform.right * cf.force * fallMultiplier  * Time.deltaTime);
        }
    }
}
