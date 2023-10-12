using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    Vector3 sPos, tPos;
    public float amp=0.1f, freq=1f;
    public bool y = true;
    void Start()
    {
        sPos = transform.position;
    }
    void Update()
    {
        tPos=sPos;
        if(y){
            tPos.y+= Mathf.Sin (Time.fixedTime * Mathf.PI * freq) * amp;
            tPos.x=transform.position.x;
        }
        else{
            tPos.x+= Mathf.Sin (Time.fixedTime * Mathf.PI * freq) * amp;
            tPos.y=transform.position.y;
        }
        transform.position=tPos;
    }
}
