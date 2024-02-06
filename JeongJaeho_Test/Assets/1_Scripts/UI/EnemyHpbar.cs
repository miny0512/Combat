using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpbar : MonoBehaviour
{
    private Camera mainCam;
    public float Yoffset;
    private void Awake()
    {
        mainCam = Camera.main;
        var pos = transform.position;
        pos.y += Yoffset;
        transform.position = pos;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(transform.position + mainCam.transform.rotation * Vector3.forward, mainCam.transform.rotation * Vector3.up);
    }
}
