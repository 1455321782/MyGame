using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class ClientManager : Singleton<ClientManager>
{
    public Socket socket;
    public byte[] buffer = new byte[2048];//缓存区
    public Queue<byte[]> msgqueue = new Queue<byte[]>();//处理消息的队列
    public MyMemoryStream my = new MyMemoryStream();

    public void Init()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.BeginConnect("127.0.0.1", 12345, AsyConnect, null);//连接服务器
    }

    void AsyConnect(IAsyncResult ar)
    {
        try
        {
            socket.EndConnect(ar);//结束连接
            Debug.Log("连接服务器成功");
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, AsyReceive, null);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    void AsyReceive(IAsyncResult ar)
    {
        try
        {
            int len = socket.EndReceive(ar);//结束接收服务器数据
            if (len>0)
            {
                byte[] alldata = new byte[len];//接收数据的数组
                Buffer.BlockCopy(buffer,0,alldata,0,len);
                my.Write(alldata,0,alldata.Length);//写入流
                while (my.Length >= 2)
                {
                    my.Position = 0;
                    int blen = my.ReadUshort();
                    int alen = blen + 2;
                    if (my.Length >= alen)
                    {
                        byte[] bdda = new byte[blen];
                        my.Read(bdda, 0, bdda.Length);
                        msgqueue.Enqueue(bdda);
                        int sylen = (int) my.Length - alen;
                        if (sylen>0)
                        {
                            byte[] syda = new byte[sylen];
                            my.Read(syda, 0, sylen);
                            my.Position = 0;
                            my.SetLength(0);
                            my.Write(syda,0,sylen);
                        }
                        else
                        {
                            my.Position = 0;
                            my.SetLength(0);
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, AsyReceive, null);
            }
            else
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void OnSendTo(int id,byte[] bd)
    {
        byte[] send = MakeData(id, bd);
        socket.BeginSend(send, 0, send.Length, SocketFlags.None, AsySend, null);
    }

    byte[] MakeData(int id, byte[] bd)
    {
        using (MyMemoryStream mm = new MyMemoryStream())
        {
            byte[] msg = new byte[bd.Length + 4];
            byte[] msgid = BitConverter.GetBytes(id);
            Buffer.BlockCopy(msgid,0,msg,0,4);
            Buffer.BlockCopy(bd,0,msg,4,bd.Length);
            mm.WriteUShort((ushort)msg.Length);
            mm.Write(msg,0,msg.Length);
            return mm.ToArray();
        }
    }

    void AsySend(IAsyncResult ar)
    {
        try
        {
            int len = socket.EndSend(ar);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void UpDate()
    {
        while (msgqueue.Count>0)
        {
            byte[] bdda = msgqueue.Dequeue();
            int id = BitConverter.ToInt32(bdda, 0);
            byte[] bd = new byte[bdda.Length - 4];
            Buffer.BlockCopy(bdda,4,bd,0,bd.Length);
            MessageManager.Instance.OnSend(id,bd);
        }
    }
}
