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
        if(y)
            tPos.y+= Mathf.Sin (Time.fixedTime * Mathf.PI * freq) * amp;
        else
            tPos.x+= Mathf.Sin (Time.fixedTime * Mathf.PI * freq) * amp;

        transform.position=tPos;
    }
}
