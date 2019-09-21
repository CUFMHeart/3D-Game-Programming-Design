
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolution : MonoBehaviour
{
    //  设定每个行星的公转法平面为（0，ry，rz）
    //  ry，rz，公转速度v 随机取定
    public Transform center;
    public float v;
    float ry, rz;
    // Use this for initialization  
    void Start()
    {
        //  设置轨道性质
        this.transform.gameObject.GetComponent<TrailRenderer>();
        TrailRenderer tr = this.transform.gameObject.GetComponent<TrailRenderer>();
        tr.time = 7;
        tr.startWidth = 0.01f;
        tr.endWidth = 0.01f;
        tr.material = new Material(Shader.Find("Sprites/Default"));
        tr.startColor = Color.blue;
        tr.endColor = Color.green;
        //  设置公转速度和公转法平面
        v = Random.Range(60, 100);
        ry = Random.Range(15, 45);
        rz = Random.Range(15, 45);
    }
    // Update is called once per frame  
    void Update()
    {
        this.transform.RotateAround(center.position, new Vector3(0, ry, rz), v * Time.deltaTime);
    }
}
