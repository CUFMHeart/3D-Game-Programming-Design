using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  处理玩家和巡逻兵的物理碰撞 -> 游戏结束
public class PlayerCollide : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Debug.Log("EventManager.Instance.RangerGameover()");
            EventManager.Instance.RangerGameover();
        }
    }
}
