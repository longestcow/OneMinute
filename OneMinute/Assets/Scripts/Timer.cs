using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Timer : MonoBehaviour
{
    public float timeValue = 60;
    public Text timeText;
    public Text timeUpText;
    bool won=false;
    void Update()
    {
        if(won) return;
        if (timeValue > 0)
        {
            timeValue -= Time.deltaTime;
        }
        else
        {
            timeValue = 0;
            timeUpText.text = "<color=#F33D3D>Time's</color> <color=#2D7B9D>Up!</color>";
            GameObject.Find("GameManager").GetComponent<GameManager>().timeOver(false);
        }
        DisplayTime(timeValue);
    }

    void DisplayTime(float timeToDisplay)
    {
        if(timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void victory()
    {
        won=true;
        timeUpText.text="<color=#F33D3D>You</color> <color=#2D7B9D>win!</color>";//finishing time and ranking in lb
    }
}
