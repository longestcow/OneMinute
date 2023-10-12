using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public int curr = 0;
    int levelCount = 13;
    public GameObject player1, player2;
    public DownMovement p1;
    public LeftMovement p2;
    public List<GameObject> levels; // lvl1Obj, lvl2Obj....
    public GameObject bestRun, mainMenu, timer, timeUp, settings, space, mainMenuThings, timerThings, settingsThings;
    public PlayerInput pInput;
    public GameObject up1,left1,right1,up2,left2,right2,_restart;
    public InputActionReference _move1,_jump1,_move2,_jump2,__restart;
    int bTime, hScore;
    public bool start = true, menu=false, settingsPage=false;
    DepthOfField dof;
    public Volume volume;
    InputActionRebindingExtensions.RebindingOperation rebindingOperation;
    void Start()
    {
        pInput=GetComponent<PlayerInput>();
        volume.profile.TryGet(out dof);
        for(int i = 1; i<=levelCount; i++){
            levels.Add(GameObject.Find("Lvl"+i));
        }
        if(!PlayerPrefs.HasKey("hScore")) PlayerPrefs.SetInt("hScore",0);
        if(!PlayerPrefs.HasKey("bTime")) PlayerPrefs.SetInt("bTime", 60);
        bTime=PlayerPrefs.GetInt("bTime"); hScore=PlayerPrefs.GetInt("hScore");
        timerThings.SetActive(false); settingsThings.SetActive(false);
        mainMenuThings.SetActive(true); space.SetActive(true);
        if(hScore>=13) bestRun.GetComponent<Text>().text="<color=#F33D3D>Best Time</color>: <color=#2D7B9D>"+((bTime==60)?"01:00":"00:"+(bTime<10?"0"+bTime:bTime))+"</color>";
        else bestRun.GetComponent<Text>().text="<color=#F33D3D>High Score</color>: <color=#2D7B9D>"+PlayerPrefs.GetInt("hScore")+"</color>";
        Camera.main.transform.position = new Vector3(-28.55f,-1,-10);
        p1=player1.GetComponent<DownMovement>();
        p2=player2.GetComponent<LeftMovement>();

    }
    // Update is called once per frame
    void Update()
    {
        if(start){
            if(Input.GetKeyDown(KeyCode.Space)){
                start=false;
                timeUp.GetComponent<Text>().text="";
                timerThings.SetActive(true); 
                mainMenuThings.SetActive(false); space.SetActive(false);
                nextLevel();
            }
            return;
        }

        if(menu){
            if(Input.GetKeyDown(KeyCode.Space)){
                curr=-1;
                menu=false;
                start=true;
                timerThings.SetActive(false); settingsThings.SetActive(false);
                mainMenuThings.SetActive(true); space.SetActive(true);

                if(!PlayerPrefs.HasKey("hScore")) PlayerPrefs.SetInt("hScore",0);
                if(!PlayerPrefs.HasKey("bTime")) PlayerPrefs.SetInt("bTime", 60);
                bTime=PlayerPrefs.GetInt("bTime"); hScore=PlayerPrefs.GetInt("hScore");

                if(hScore>=13) bestRun.GetComponent<Text>().text="<color=#F33D3D>Best Time</color>: <color=#2D7B9D>"+((bTime==60)?"01:00":"00:"+(bTime<10?"0"+bTime:bTime))+"</color>";
                else bestRun.GetComponent<Text>().text="<color=#F33D3D>High Score</color>: <color=#2D7B9D>"+PlayerPrefs.GetInt("hScore")+"</color>";
                Camera.main.transform.position = new Vector3(-28.55f,-1,-10);
                timer.GetComponent<Timer>().timeValue=60;
            }
            return;
        }

        
    }

    public void nextLevel()
    {
        curr+=1;
        if(curr==levelCount){
            timer.GetComponent<Timer>().victory();
            timer.SetActive(false);
            timeOver(true);
            Camera.main.transform.position+=new Vector3(30,0,0);
            return;
        }
        try{
            levels[curr].transform.Find("flagBlue").gameObject.SetActive(true);
        } catch(Exception){
            try{
            levels[curr].transform.Find("flagRed").gameObject.SetActive(true);
            } catch(Exception){}
        }
        for(int i = 0; i < levels[curr].transform.Find("powerUps").childCount; i++){
            levels[curr].transform.Find("powerUps").GetChild(i).gameObject.SetActive(true);
        }
        Camera.main.transform.position+=new Vector3(30,0,0);
        player1.GetComponent<Rigidbody2D>().velocity=new Vector3(0,0,0);
        player1.transform.position=levels[curr].transform.Find("sp1").position;
        player2.GetComponent<Rigidbody2D>().velocity=new Vector3(0,0,0);
        player2.transform.position=levels[curr].transform.Find("sp2").position;       
        
    }
    void restart()
    {
        player1.transform.position=levels[curr].transform.Find("sp1").position;
        player2.transform.position=levels[curr].transform.Find("sp2").position;
        for(int i = 0; i < levels[curr].transform.Find("powerUps").childCount; i++){
            levels[curr].transform.Find("powerUps").GetChild(i).gameObject.SetActive(true);
        }
 
    }
    public void timeOver(bool win){
        space.SetActive(true);
        space.GetComponent<RectTransform>().localPosition=new Vector3(0,-409,0);
        try{
            levels[curr].transform.Find("flagBlue").gameObject.SetActive(false);
        } catch(Exception){
            try{
            levels[curr].transform.Find("flagRed").gameObject.SetActive(false);
            } catch(Exception){}
        }
        if(PlayerPrefs.HasKey("hScore") && PlayerPrefs.GetInt("hScore")<curr)
            PlayerPrefs.SetInt("hScore", curr);
        if (win && (PlayerPrefs.HasKey("bTime") && PlayerPrefs.GetInt("bTime")>60f-timer.GetComponent<Timer>().timeValue) || !PlayerPrefs.HasKey("bTime"))
            PlayerPrefs.SetInt("bTime", (int)(60f-timer.GetComponent<Timer>().timeValue));
        menu=true;
    }

    public void OnMove1(InputAction.CallbackContext value){
        p1.movement=value.ReadValue<float>();        
    }

    public void OnJump1(InputAction.CallbackContext value){
        if(value.started) p1.Jump();
    }

    public void OnMove2(InputAction.CallbackContext value){
        p2.movement=value.ReadValue<float>();        
    }

    public void OnJump2(InputAction.CallbackContext value){
        if(value.started) p2.Jump();
    }

    public void OnRestart1(InputAction.CallbackContext value){
       if(value.started) restart();
    }

    public void settingsClick(){
        start=false; settingsPage=true;
        timerThings.SetActive(false);
        mainMenuThings.SetActive(false); space.SetActive(false);
        settingsThings.SetActive(true);
    }

    public void backClick(){
        curr=-1;
        timerThings.SetActive(false); settingsThings.SetActive(false);
        mainMenuThings.SetActive(true); space.SetActive(true);

        if(!PlayerPrefs.HasKey("hScore")) PlayerPrefs.SetInt("hScore",0);
        if(!PlayerPrefs.HasKey("bTime")) PlayerPrefs.SetInt("bTime", 60);
        bTime=PlayerPrefs.GetInt("bTime"); hScore=PlayerPrefs.GetInt("hScore");

        if(hScore>=13) bestRun.GetComponent<Text>().text="<color=#F33D3D>Best Time</color>: <color=#2D7B9D>"+((bTime==60)?"01:00":"00:"+(bTime<10?"0"+bTime:bTime))+"</color>";
        else bestRun.GetComponent<Text>().text="<color=#F33D3D>High Score</color>: <color=#2D7B9D>"+PlayerPrefs.GetInt("hScore")+"</color>";
        Camera.main.transform.position = new Vector3(-28.55f,-1,-10);
        timer.GetComponent<Timer>().timeValue=60;
        start=true;
        settingsPage=false;
    }

    public void clearStats(){
        PlayerPrefs.DeleteAll();
    }

    public void RebindComplete(GameObject _object, int index, InputActionReference action) {
        rebindingOperation.Dispose();
        _object.GetComponentInChildren<Text>().text=InputControlPath.ToHumanReadableString(action.action.bindings[index].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        action.action.Enable();
    }

    public void rebindUp1(){
        up1.GetComponentInChildren<Text>().text="...";
        _jump1.action.Disable();
        rebindingOperation = _jump1.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindComplete(up1, 0, _jump1))
            .Start();
    }
    public void rebindLeft1(){
        left1.GetComponentInChildren<Text>().text="...";
        _move1.action.Disable();
        rebindingOperation = _move1.action.PerformInteractiveRebinding()
            .WithTargetBinding(1)
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindComplete(left1, 1, _move1))
            .Start();
    }
    public void rebindRight1(){
        right1.GetComponentInChildren<Text>().text="...";
        _move1.action.Disable();
        rebindingOperation = _move1.action.PerformInteractiveRebinding()
            .WithTargetBinding(2)
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindComplete(right1, 2, _move1))
            .Start();
    }

    public void rebindUp2(){
        up2.GetComponentInChildren<Text>().text="...";
        _jump2.action.Disable();
        rebindingOperation = _jump2.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindComplete(up2, 0, _jump2))
            .Start();
    }
    public void rebindLeft2(){
        left2.GetComponentInChildren<Text>().text="...";
        _move2.action.Disable();
        rebindingOperation = _move2.action.PerformInteractiveRebinding()
            .WithTargetBinding(1)
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindComplete(left2, 1, _move2))
            .Start();
    }
    public void rebindRight2(){
        right2.GetComponentInChildren<Text>().text="...";
        _move2.action.Disable();
        rebindingOperation = _move2.action.PerformInteractiveRebinding()
            .WithTargetBinding(2)
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindComplete(right2, 2, _move2))
            .Start();
    }

    public void rebindRestart(){
        _restart.GetComponentInChildren<Text>().text="...";
        __restart.action.Disable();
        rebindingOperation = __restart.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindComplete(_restart, 0, __restart))
            .Start();
    }

    




}
