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
    public List<Client> allClient = new List<Client>();//������Ե�½�Ŀͻ���

    public void Init()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint ip = new IPEndPoint(IPAddress.Any, 12345);//����IP
        socket.Bind(ip);//��IP;
        socket.Listen(10);//������������
        Debug.Log("����������");
        socket.BeginAccept(AsyAccept, null);
    }

    private void AsyAccept(IAsyncResult ar)
    {
        try
        {
            Socket so = socket.EndAccept(ar); //�����������ӵĿͻ���
            Client cli = new Client();
            IPEndPoint ip = so.RemoteEndPoint as IPEndPoint;
            cli.socket = so;
            cli.id = ip.Port;
            cli.index = index;
            cli.playerName = "���" + ++index;
            allClient.Add(cli); //������Ҽ��뼯��
            Debug.Log(cli.playerName + "���ӷ�����");
            socket.BeginAccept(AsyAccept, null); //�����ȴ��¿ͻ�������
            cli.socket.BeginReceive(cli.buffer, 0, cli.buffer.Length, SocketFlags.None, AsyReceive, cli); //��ʼ���տͻ�������

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
            int len = cli.socket.EndReceive(ar);//�������տͻ�������
            Debug.Log("�յ������ݳ���Ϊ��"+len);
            if (len>0)
            {
                byte[] alldata = new byte[len];//�������ݵ�����
                Buffer.BlockCopy(cli.buffer,0,alldata,0,len);
                cli.my.Write(alldata,0,alldata.Length);//д����
                while (cli.my.Length >=2)
                {
                    cli.my.Position = 0;//���λ������
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
                Debug.Log(cli.playerName+"�Ͽ�����");
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
