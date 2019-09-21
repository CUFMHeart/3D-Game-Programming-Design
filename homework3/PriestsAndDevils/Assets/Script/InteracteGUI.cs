using Interfaces;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteracteGUI : MonoBehaviour
{
    UserAction user_act;
    private GUIStyle Style = new GUIStyle ();

    public int SetState {
        get 
        {
            return GameState; 
        }
        set
        {
            GameState = value;
        } 
    }
    static int GameState = 0;

	void Start ()
    {
        //  style init
        Style.fontSize = 32;
        Style.fontStyle = FontStyle.BoldAndItalic;
        Style.normal.textColor = Color.blue;
        Style.alignment = TextAnchor.MiddleCenter;
        //  user_act init
        user_act = SSDirector.getInstance().currentScenceController as UserAction;
    }

    private void OnGUI()
    {
        if (GameState == 1)
        {
            GUI.Label(new Rect(380, 100, 100, 50), "Win", Style);
        }
        else if (GameState == 2)
        {
            GUI.Label(new Rect(380, 100, 100, 50), "Gameover", Style);
        }
        if (GUI.Button(new Rect(380, 400, 140, 70), "Restart"))
        {
            GameState = 0;
            user_act.Restart();
        }
    }
}

public class ClickGUI : MonoBehaviour{
    UserAction user_act;
    GameObjects item;

    public void setController(GameObjects Object)
    {
        item = Object;
    }

    void Start()
    {
        user_act = SSDirector.getInstance().currentScenceController as UserAction;
    }

    void OnMouseDown()
    {
        if (gameObject.name == "boat")
        {
            user_act.ClickBoat();
        }
        else
        {
            user_act.ClickObject(item);
        }
    }
}