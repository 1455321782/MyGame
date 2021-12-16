using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 相机作为子物体，挂在相机上
/// </summary>
[DisallowMultipleComponent]
public class MouseLook : MonoBehaviour
{
    [Header("鼠标灵敏度")]
    public float mouseSpeed = 500f;
    [Header("角色")]
    public Transform playerBody;
    float xRotation = 0f;
    // Start is called before the first frame update
    void Start()
    {
        //将鼠标隐藏
        Cursor.lockState = CursorLockMode.Confined;
    }
    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime;
        //这里的Mouse X和Mouse Y是鼠标所控制的X，Y，这里在前面新定义了一个鼠标移动的速度mouseSpeed，用来控制鼠标移动速度，Time.deltaTime是为了解决帧率问题
        xRotation -= mouseY;//不能为+=，会让鼠标控制的摄像机方向颠倒
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);//将摄像机上下可调节范围控制在-90到90度之间
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);//绕着y轴旋转
    }
}
