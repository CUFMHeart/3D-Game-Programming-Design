using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mouse : MonoBehaviour
{
    Transform trans;
    Quaternion quaternion;
    public Vector2 lerp = Vector2.zero;
    public Vector2 v2 = new Vector2(4f, 2f);

    void Start()
    {
        trans = transform;
        quaternion = trans.localRotation;
    }

    void Update()
    {
        Vector3 pos = Input.mousePosition;
        float mywidth = Screen.width * 0.5f;
        float myheight = Screen.height * 0.5f;
        float x = Mathf.Clamp((pos.x - mywidth) / mywidth, -0.5f, 0.5f);
        float y = Mathf.Clamp((pos.y - myheight) / myheight, -0.5f, 0.5f);
        lerp = Vector2.Lerp(lerp, new Vector2(x, y), Time.deltaTime * 2f);
        trans.localRotation = quaternion * Quaternion.Euler(-lerp.y * v2.y, -lerp.x * v2.x, 0f);
    }
}
