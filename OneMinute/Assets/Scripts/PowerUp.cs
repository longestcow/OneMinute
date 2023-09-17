using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    Vector3 sPos, tPos;
    float amp=0.1f, freq=1f;
    void Start()
    {
        sPos = transform.position;
    }
    void Update()
    {
        tPos=sPos;
        tPos.y+= Mathf.Sin (Time.fixedTime * Mathf.PI * freq) * amp;
        transform.position=tPos;
    }
}
