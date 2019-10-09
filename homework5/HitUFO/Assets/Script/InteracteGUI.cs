using Interfaces;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InteracteGUI : MonoBehaviour
{
    UserAction user_act;
    private bool flag = false;
    private float slip;
    private float Now;
    int round = 1;
    private GUIStyle Style = new GUIStyle ();

	void Start ()
    {
        //  style init
        Style.fontSize = 16;
        Style.fontStyle = FontStyle.BoldAndItalic;
        Style.normal.textColor = Color.blue;
        Style.alignment = TextAnchor.MiddleCenter;
        //  user_act init
        user_act = SSDirector.getInstance().currentScenceController as UserAction;
        //  Time.time
        slip = Time.time;
    }

    private void OnGUI()
    {
        if (!flag)
            slip = Time.time;
        GUI.Label(new Rect(680, 30, 100, 70), "Score: " + user_act.GetScore().ToString(), Style);
        GUI.Label(new Rect(680, 45, 100, 70), "Time: " + ((int)(Time.time - slip)).ToString(), Style);
        GUI.Label(new Rect(680, 60, 100, 70), "Round: " + round, Style);
        // GUI.Label(new Rect(680, 75, 100, 70), "Miss: " + user_act.GetMiss(), Style);
        if (!flag)
        {
            if (GUI.Button(new Rect(380, 200, 140, 70), "Start"))
            {
                slip = Time.time;
                flag = true;
                user_act.Restart();
            }
        }
        if (flag)
        {
            round = user_act.GetRound();
            if (Input.GetButtonDown("Fire1"))
            {
                Vector3 pos = Input.mousePosition;
                user_act.Hit(pos);
            }
            //  限制 round
            if (round > 4)
            {
                round = 4;
                if (user_act.GameFinish())
                {
                    flag = false;
                }
            }
        }
    }
}