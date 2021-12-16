using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;
using MyPlane;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    AirPlane airPlane = new AirPlane();
    private List<Vector3> lists = new List<Vector3>();
    private Vector3[] points;
    private Vector3 v3;
    private int n = 0;
    private bool falg = true;
    void Awake()
    {
        points = new Vector3[4]
        {
            new Vector3(47, 65, 79),
            new Vector3(84, 65, 79),
            new Vector3(87, 65, 46),
            new Vector3(47, 65, 52),
        };
    }
    void Start()
    {

       // InvokeRepeating("SendPlanePos", 0, 0.5f);
        //圆的点
        for (int i = 0; i < 361; i++)
        {
            Vector3 point = Quaternion.Euler(new Vector3(0, i, 0)) * transform.forward * 30;
            lists.Add(point);
        }
    }

    private void Update()
    {
        if (falg)
        {//自动
            if (Vector3.Distance(points[n],v3)>=0.5f)
            {
                transform.Translate(Vector3.forward * Time.deltaTime * 10);
                v3 = transform.position;
            }
            else
            {
                n++;
                if (n == 4) n = 0;
                transform.LookAt(points[n]);
            }
        }
        else{//人工
            Flight();}
        //切换模式
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            falg = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject box = Instantiate(Resources.Load<GameObject>("Box"));
            box.transform.position = transform.position;
            foreach (var client in NetManager.Instance.allClient)
            {
                NetManager.Instance.OnSendTo(MessageID.S_AirDrop, new byte[1],client);
            }
            
        }

        SendPlanePos();
    }

    void SendPlanePos()
    {
        //将服务器飞机位置发送给所有客户端
        foreach (var client in NetManager.Instance.allClient)
        {
            PlaneTransform(client);
        }
    }
    public void PlaneTransform(Client client)
    {
        airPlane.Px = transform.position.x;
        airPlane.Py = transform.position.y;
        airPlane.Pz = transform.position.z;
        airPlane.Rx = transform.eulerAngles.x;
        airPlane.Ry = transform.eulerAngles.y;
        airPlane.Rz = transform.eulerAngles.z;
        NetManager.Instance.OnSendTo(MessageID.S_PlaneMove, airPlane.ToByteArray(), client);
    }

    void Flight()
    {
        //前进后退
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * 10);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(-Vector3.forward * Time.deltaTime * 10);
        }
        
        //左右转向
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * Time.deltaTime * 50);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(-Vector3.up * Time.deltaTime * 50);
        }

        //上升下降
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(0, 0.1f, 0);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(0, -0.1f, 0);
        }

        //左右侧身
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.eulerAngles += new Vector3(0, 0, 1) * Time.deltaTime * 20;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.eulerAngles += new Vector3(0, 0, -1) * Time.deltaTime * 20;
        }
    }
}
