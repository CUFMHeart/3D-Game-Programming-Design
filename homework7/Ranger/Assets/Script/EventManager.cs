using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EventManager
{
    //  init
    public static EventManager Instance = new EventManager();
    private EventManager() { }
    //  委托：计分
    public delegate void ScoreEvent();
    public static event ScoreEvent score_temp;
    //  委托：Game Over
    public delegate void GameoverEvent();
    public static event GameoverEvent gameover_temp;
    //  脱离追踪 -> 计分
    public void RangerAddScore()
    {
        if (score_temp != null)
        {
            score_temp();
        }
    }
    //  物理碰撞 -> Game Over
    public void RangerGameover()
    {
        if (gameover_temp != null)
        {
            gameover_temp();
        }
    }
}