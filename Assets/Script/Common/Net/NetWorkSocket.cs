using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;

public class NetWorkSocket : SingletonMono<NetWorkSocket> {
    
    //缓冲区
    private byte[] buffer=new byte[10240];

    #region 发送数据所需变量
    //发送消息队列
    private Queue<byte[]> m_SendQueue=new Queue<byte[]>();
    //检查发送消息队列委托
    private Action m_CheckSendQueue;
    private const int m_CompressLen = 200;
    #endregion

    #region 接收数据所需变量
    //接收数据包的字节数组缓冲区
    private byte[] m_ReceiveBuffer = new byte[10240];
    //接收数据包的数据流
    private MMO_MemoryStream m_ReceiveMs = new MMO_MemoryStream();
    //接受数据包的队列
    private Queue<byte[]> m_ReceiveQueue=new Queue<byte[]>();

    private int m_ReceiveCount = 0;
    #endregion

    private Socket m_Client;

    protected override void OnStart()
    {
        base.OnStart();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        while (true)
        {
            if (m_ReceiveCount <= 5)
            {
                m_ReceiveCount++;
                lock (m_ReceiveQueue)
                {
                    if (m_ReceiveQueue.Count > 0)
                    {
                        byte[] buffer = m_ReceiveQueue.Dequeue();
                        bool isCompress;
                        ushort crc;
                        
                        byte[] crcBuffer = new byte[buffer.Length - 3];
                        using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
                        {
                            isCompress=ms.ReadBool();
                            crc = ms.ReadUShort();
                            ms.Read(crcBuffer, 0, crcBuffer.Length);
                        }

                        if (crc == Crc16.CalculateCrc16(crcBuffer))
                        {
                            crcBuffer = SecurityUtil.Xor(crcBuffer);
                            if (isCompress)
                            {
                                crcBuffer = ZlibHelper.DeCompressBytes(crcBuffer);
                            }
                            ushort protoCode;
                            byte[] protoContent = new byte[buffer.Length - 2];
                            using (MMO_MemoryStream ms = new MMO_MemoryStream(crcBuffer))
                            {
                                protoCode = ms.ReadUShort();
                                ms.Read(protoContent, 0, protoContent.Length);
                            }
                            EnventDispather.Instance.Dispach(protoCode, protoContent);
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                m_ReceiveCount = 0;
                break;
            }
        }
    }

    void OnDestroy()
    {
        if (m_Client != null && m_Client.Connected)
        {
            m_Client.Shutdown(SocketShutdown.Both);
            m_Client.Close();
        }
    }

    #region Connect 连接到服务器
    /// <summary>
    /// 连接到服务器
    /// </summary>
    /// <param name="ip"></param>
    /// <param name="port"></param>
    public void Connect(string ip, int port)
    {
        if (m_Client != null && m_Client.Connected) return;
        m_Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            m_Client.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
            m_CheckSendQueue = OnCheckSendQueueCallback;
            ReceiveMsg();
            Debug.Log("连接成功");
        }
        catch (Exception e)
        {
            Debug.Log("连接失败" + e.Message);
            throw;
        }
    }

    #endregion

    #region OnCheckSendQueueCallback 检查发送消息队列委托回调
    /// <summary>
    /// 检查发送消息队列委托回调
    /// </summary>
    private void OnCheckSendQueueCallback()
    {
        lock (m_SendQueue)
        {
            if (m_SendQueue.Count > 0)
            {
                Send(m_SendQueue.Dequeue());
            }
        }
    }
    #endregion

    #region SendMsg 发送消息（把消息加入队列）
    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="buffer"></param>
    public void SendMsg(byte[] buffer)
    {
        //得到封装好的数据包
        buffer = MakeDataPackage(buffer);
        lock (m_SendQueue)
        {
            m_SendQueue.Enqueue(buffer);
            m_CheckSendQueue.BeginInvoke(null,null);
        }
    }

    #endregion

    #region MakeDataPackage 封装数据包
    /// <summary>
    /// 封装数据包
    /// </summary>
    /// <param name="buffer"></param>
    private byte[] MakeDataPackage(byte[] buffer)
    {
        //1.压缩
        bool isCompress = buffer.Length > m_CompressLen;
        if (isCompress)
        {
            buffer = ZlibHelper.CompressBytes(buffer);
        }

        //2.异或
        buffer = SecurityUtil.Xor(buffer);
        
        //3.计算CRC
        ushort crc = Crc16.CalculateCrc16(buffer);

        byte[] retData;
        using (MMO_MemoryStream ms=new MMO_MemoryStream())
        {
            ms.WriteUShort((ushort)(buffer.Length+3));
            ms.WriteBool(isCompress);
            ms.WriteUShort(crc);
            ms.Write(buffer,0,buffer.Length);
            retData = ms.ToArray();
        }
        return retData;
    }

    #endregion

    #region Send 真正发送数据包到服务器
    /// <summary>
    /// 真正发送数据包到服务器
    /// </summary>
    /// <param name="data"></param>
    private void Send(byte[] data)
    {
        m_Client.BeginSend(data, 0, data.Length, SocketFlags.None, SendCallback, m_Client);
    }

    #endregion

    #region SendCallback 真正发送数据包回调
    /// <summary>
    /// 真正发送数据包回调
    /// </summary>
    /// <param name="ar"></param>
    private void SendCallback(IAsyncResult ar)
    {
        m_Client.EndSend(ar);
        OnCheckSendQueueCallback();
    }

    #endregion


    //================================================

    #region ReceiveMsg 接受数据
    /// <summary>
    /// 接受数据
    /// </summary>
    private void ReceiveMsg()
    {
        //异步接受数据
        m_Client.BeginReceive(m_ReceiveBuffer, 0, m_ReceiveBuffer.Length, SocketFlags.None, ReceiveCallback, m_Client);
    }
    #endregion

    #region ReceiveCallback 接收数据回调
    /// <summary>
    /// 接收数据回调
    /// </summary>
    /// <param name="ar"></param>
    private void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            int len = m_Client.EndReceive(ar);
            if (len > 0)//接受到了数据
            {
                //把接收到的数据写入到缓冲数据流的尾部
                m_ReceiveMs.Position = m_ReceiveMs.Length;
                //把制定长度的字节写入数据流
                m_ReceiveMs.Write(m_ReceiveBuffer, 0, len);
                //为什么判断条件是2？因为写数据的时候包头用的是ushort
                if (m_ReceiveMs.Length > 2)//至少有一个不完整的包
                {
                    while (true)
                    {
                        m_ReceiveMs.Position = 0;
                        int currMsgLen = m_ReceiveMs.ReadUShort();
                        int currFullMsgLen = currMsgLen + 2;
                        //数据流的长度>=整个包的长度
                        if (m_ReceiveMs.Length >= currFullMsgLen)//至少有一个完整包
                        {
                            byte[] buffer = new byte[currMsgLen];
                            //把数据流的指针放到包体的位置
                            m_ReceiveMs.Position = 2;
                            m_ReceiveMs.Read(buffer, 0, currMsgLen);
                            lock (m_ReceiveQueue)
                            {
                                m_ReceiveQueue.Enqueue(buffer);
                            }
                            

                            //==============处理剩余字节===============
                            int remainByteLen = (int)m_ReceiveMs.Length - currFullMsgLen;//剩余字节长度
                            if (remainByteLen > 0)//如果有剩余字节
                            {
                                byte[] remainBuffer = new byte[remainByteLen];
                                m_ReceiveMs.Position = currFullMsgLen;
                                m_ReceiveMs.Read(remainBuffer, 0, remainByteLen);
                                //清空数据流
                                m_ReceiveMs.Position = 0;
                                m_ReceiveMs.SetLength(0);
                                //把剩余数据重新写入数据流
                                m_ReceiveMs.Write(remainBuffer, 0, remainByteLen);
                                remainBuffer = null;
                            }
                            else//没有剩余字节
                            {
                                //清空数据流
                                m_ReceiveMs.Position = 0;
                                m_ReceiveMs.SetLength(0);
                                break;
                            }
                        }
                        else//还没有完整的数据包
                        {
                            break;
                        }
                    }
                }
                //进行下一次接收数据
                ReceiveMsg();
            }
            else//没接收到数据
            {
                Debug.Log(string.Format("客户端{0}断开连接", m_Client.RemoteEndPoint.ToString()));
            }
        }
        catch
        {
            Debug.Log(string.Format("客户端{0}断开连接", m_Client.RemoteEndPoint.ToString()));
        }

    }
    #endregion

}
