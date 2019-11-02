using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  处理玩家和巡逻兵的视野碰撞 -> 追踪
public class VisionCollide : MonoBehaviour
{
    public int sign = 0;
    FirstController first_controller;

    private void Start()
    {
        first_controller = SSDirector.getInstance().currentScenceController as FirstController;
    }

    void OnTriggerEnter(Collider collider)
    {
        // Debug.Log("sign: " + sign);
        if (collider.gameObject.tag == "Player")
        {
            // Debug.Log("!!! sign: " + sign);
            first_controller.SetPlayerArea(sign);
            EventManager.Instance.RangerAddScore();
        }
    }
}
