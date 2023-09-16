using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public string gravity = "down";
    public bool wasd = true;
    public int id;
    public int moveSpeed = 10;
    Vector2 gravityDirection;
    Rigidbody2D rb;
    KeyCode[] controls={KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow};
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        if(wasd){
            controls[0]=KeyCode.W; controls[1]=KeyCode.A; controls[2]=KeyCode.S; controls[3]=KeyCode.D;
        }
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z+((gravity=="down")?0:(gravity=="left")?-90:(gravity=="right")?90:180));
        gravityDirection=((gravity=="down")?new Vector2(0,-9.81f):(gravity=="up")?new Vector2(0,9.81f):(gravity=="left")?new Vector2(-9.81f,0):new Vector2(9.81f,0));
    }

    void FixedUpdate()
    {
        rb.AddForce(gravityDirection);
    }

    void Update()
    {
        if(!(Input.GetKeyUp(controls[3]) && Input.GetKeyUp(controls[1]))) //movement
            rb.velocity = ((Input.GetKeyUp(controls[1]) || Input.GetKeyUp(controls[3]))?0:Input.GetAxis("Horizontal"+id) * moveSpeed) * transform.right;
    }
}
