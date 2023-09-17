using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public int curr = 0;
    public GameObject player1, player2;
    public List<GameObject> levels; // lvl1Obj, lvl2Obj....
    public GameObject bestTime, highScore, mainMenu, timer, timeUp;
    
    bool start = true;

    void Start()
    {
        for(int i = 1; i<=1; i++){
            levels.Add(GameObject.Find("Lvl"+i));
        }
        timer.SetActive(false); timeUp.SetActive(false);
        bestTime.SetActive(true); highScore.SetActive(true); mainMenu.SetActive(true);
        bestTime.GetComponent<Text>().text="<color=#F33D3D>Best Time</color>: <color=#2D7B9D>"+(PlayerPrefs.HasKey("bTime")?PlayerPrefs.GetInt("bTime"):"01:00")+"</color>";
        highScore.GetComponent<Text>().text="<color=#F33D3D>High Score</color>: <color=#2D7B9D>"+(PlayerPrefs.HasKey("hScore")?PlayerPrefs.GetInt("hScore"):"0")+"</color>";


    }
    // Update is called once per frame
    void Update()
    {
        if(start){
            if(Input.GetKeyDown(KeyCode.Space)){
                start=false;
                timer.SetActive(true); timeUp.SetActive(true);
                bestTime.SetActive(false); highScore.SetActive(false); mainMenu.SetActive(false);
                nextLevel();
            }
            return;
        }
        
        if(Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.RightShift))
            restart();
        
    }

    void nextLevel()
    {
        curr+=1;
        Camera.main.transform.position+=new Vector3(30,0,0);
        player1.transform.position=levels[curr].transform.Find("sp1").position;
        player2.transform.position=levels[curr].transform.Find("sp2").position;    
        
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
