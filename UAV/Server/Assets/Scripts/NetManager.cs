using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class NetManager : Singleton<NetManager>
{
    private int index = 0;
    public Socket socket;
    public List<Client> allClient = new List<Client>();//存放所以登陆的客户端

    public void Init()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint ip = new IPEndPoint(IPAddress.Any, 12345);//定义IP
        socket.Bind(ip);//绑定IP;
        socket.Listen(10);//设置最大监听数
        Debug.Log("服务器开启");
        socket.BeginAccept(AsyAccept, null);
    }

    private void AsyAccept(IAsyncResult ar)
    {
        try
        {
            Socket so = socket.EndAccept(ar); //结束接收连接的客户端
            Client cli = new Client();
            IPEndPoint ip = so.RemoteEndPoint as IPEndPoint;
            cli.socket = so;
            cli.id = ip.Port;
            cli.index = index;
            cli.playerName = "玩家" + ++index;
            allClient.Add(cli); //将新玩家加入集合
            Debug.Log(cli.playerName + "连接服务器");
            socket.BeginAccept(AsyAccept, null); //继续等待新客户端连接
            cli.socket.BeginReceive(cli.buffer, 0, cli.buffer.Length, SocketFlags.None, AsyReceive, cli); //开始接收客户端数据

            OnSendTo(MessageID.S_Test,UTF8Encoding.UTF8.GetBytes("Test"),cli);
        }
        catch(Exception ex)
        {
            Debug.Log(ex);
        }
    }

    private void AsyReceive(IAsyncResult ar)
    {
        Client cli = ar.AsyncState as Client;
        try
        {
            int len = cli.socket.EndReceive(ar);//结束接收客户端数据
            Debug.Log("收到的数据长度为："+len);
            if (len>0)
            {
                byte[] alldata = new byte[len];//接收数据的数组
                Buffer.BlockCopy(cli.buffer,0,alldata,0,len);
                cli.my.Write(alldata,0,alldata.Length);//写入流
                while (cli.my.Length >=2)
                {
                    cli.my.Position = 0;//光标位置置零
                    int blen = cli.my.ReadUshort();
                    int alen = blen + 2;
                    if (cli.my.Length >= alen)
                    {
                        byte[] bdda = new byte[blen];
                        cli.my.Read(bdda, 0, bdda.Length);
                        int id = BitConverter.ToInt32(bdda, 0);
                        byte[] bd = new byte[bdda.Length - 4];
                        Buffer.BlockCopy(bdda,4,bd,0,bd.Length);
                        MessageManager.Instance.OnSend(id,bd,cli);
                        int sylen = (int) cli.my.Length - alen;
                        if (sylen>0)
                        {
                            byte[] syda = new byte[sylen];
                            cli.my.Read(syda, 0, sylen);
                            cli.my.Position = 0;
                            cli.my.SetLength(0);
                            cli.my.Write(syda,0,sylen);
                        }
                        else
                        {
                            cli.my.Position = 0;
                            cli.my.SetLength(0);
                            break;;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                cli.socket.BeginReceive(cli.buffer, 0, cli.buffer.Length, SocketFlags.None, AsyReceive, cli);
            }
            else
            {
                cli.socket.Shutdown(SocketShutdown.Both);
                cli.socket.Close();
                Debug.Log(cli.playerName+"断开连接");
                allClient.Remove(cli);
                
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void OnSendTo(int id, byte[] bd, Client client)
    {
        byte[] send = MakeData(id, bd);
        client.socket.BeginSend(send, 0, send.Length, SocketFlags.None, AsySend, client);
    }

    private byte[] MakeData(int id, byte[] bytes)
    {
        using (MyMemoryStream mm = new MyMemoryStream())
        {
            byte[] msg = new byte[bytes.Length + 4];
            byte[] msgid = BitConverter.GetBytes(id);
            Buffer.BlockCopy(msgid,0,msg,0,4);
            Buffer.BlockCopy(bytes,0,msg,4,bytes.Length);
            mm.WriteUShort((ushort)msg.Length);
            mm.Write(msg,0,msg.Length);
            return mm.ToArray();
        }
    }

    void AsySend(IAsyncResult ar)
    {
        Client cli = ar.AsyncState as Client;
        try
        {
            int len = cli.socket.EndSend(ar);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
