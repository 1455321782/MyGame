using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Client
{
    public Socket socket;
    public int index;
    public int id;
    public string playerName;
    public byte[] buffer = new byte[1024];
    public MyMemoryStream my = new MyMemoryStream();
}
