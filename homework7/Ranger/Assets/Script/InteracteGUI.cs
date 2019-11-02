using Interfaces;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InteracteGUI : MonoBehaviour
{
    UserAction user_act;
    ISceneController scene_ctrl;
    private GUIStyle Style = new GUIStyle ();
    private bool flag = false;
    private float slip;

	void Start ()
    {
        //  style init
        Style.fontSize = 16;
        Style.fontStyle = FontStyle.BoldAndItalic;
        Style.normal.textColor = Color.blue;
        Style.alignment = TextAnchor.MiddleCenter;
        //  user_act scene_ctrl init
        user_act = SSDirector.getInstance().currentScenceController as UserAction;
        scene_ctrl = SSDirector.getInstance().currentScenceController as ISceneController;
        //  Time.time
        slip = Time.time;
    }

    //  获取方向键信息
    private void Update()
    {
        float move_x = Input.GetAxis("Horizontal");
        float move_z = Input.GetAxis("Vertical");
        user_act.PlayerMove(move_x, move_z);
    }

    private void OnGUI()
    {
        if (!flag)
        {
            slip = Time.time;
        }
        GUI.Label(new Rect(680, 30, 100, 70), "Score: " + user_act.GetScore().ToString(), Style);
        GUI.Label(new Rect(680, 45, 100, 70), "Time: " + ((int)(Time.time - slip)).ToString(), Style);
        if (!flag)
        {
            if (GUI.Button(new Rect(380, 200, 140, 70), "Start"))
            {
                flag = true;
                scene_ctrl.LoadResources();
                slip = Time.time;
                user_act.Restart();
            }
        }
        else
        {
            if (!user_act.GetGameState())
            {
                flag = false;
            }
        }
    }
}