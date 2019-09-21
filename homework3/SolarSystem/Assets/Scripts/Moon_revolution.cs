using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon_revolution : MonoBehaviour
{
    //  设定月球公转法平面为（0，ry，rz）
    //  ry，rz，公转速度v 随机取定
    public Transform center;
    public float v;
    float ry, rz;
    // Use this for initialization  
    void Start()
    {
        v = Random.Range(100, 120);
        ry = Random.Range(15, 25);
        rz = Random.Range(15, 25);
    }
    // Update is called once per frame  
    void Update()
    {
        this.transform.RotateAround(center.position, new Vector3(0, ry, rz), v * Time.deltaTime);
    }
}
