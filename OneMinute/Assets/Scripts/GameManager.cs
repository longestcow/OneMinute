using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int curr = 0;
    public GameObject player1, player2;
    public List<GameObject> levels; // lvl1Obj, lvl2Obj....

    void Start()
    {
        for(int i = 1; i<=1; i++){
            levels.Add(GameObject.Find("Lvl"+i));
        }
    }
    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.RightShift))
            restart();
        
    }

    void nextLevel()
    {
        //transition currLevel%2=redOrBlueTransition
        //if level is last, playerPref set victory and time, show win screen
        
    }
    void restart()
    {
        for(int i = 0; i < levels[curr].transform.Find("powerUps").childCount; i++){
            levels[curr].transform.Find("powerUps").GetChild(i).gameObject.SetActive(true);
        }
 
        player1.transform.position=levels[curr].transform.Find("sp1").position;
        player2.transform.position=levels[curr].transform.Find("sp2").position;

    }
    void timeOver(){
        //playerPref set highscore, show loss screen
    }
}
