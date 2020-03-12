using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BILWeb.Print
{
    public class SocketHelper
    {
        private Socket sk;
        private IPEndPoint iep;

        public SocketHelper(string ip,int port)
        {
            iep = new IPEndPoint(IPAddress.Parse(ip), port);
            sk = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect()
        {
            if (!sk.Connected)
                sk.Connect(iep);
            sk.SendTimeout = 1000;
        }


        private bool TestPrintConnection(ref string ErrMsg)
        {
            try
            {
                if (!sk.Connected)
                    sk.Connect(iep);

                ////查詢條碼打印機狀態，如果返回出錯狀態"1"
                //sk.Send(Encoding.Default.GetBytes("~HQES"));
                //byte[] data = new byte[1024];
                //int recv = sk.Receive(data);//用于表示客户端发送的信息长度
                //if (recv < 70 || Convert.ToChar(data[70]) != '0')
                //{
                //    Close();
                //    ErrMsg = "打印异常：打印机状态异常";
                //    return false;
                //}
                return true;
            }
            catch(Exception ex)
            {
                Close();
                ErrMsg = "打印异常："+ex.ToString();
                return false;
            }
        }

        public void Close()
        {
            if (sk.Connected)
                sk.Close();
        }

        public bool Send(List<string> strs, ref string ErrMsg)
        { 
            try
            {
                if (!TestPrintConnection(ref ErrMsg))
                    return false;
                foreach (string bars in strs)
                {
                    byte[] message;
                    message = Encoding.GetEncoding("BIG5").GetBytes(bars);
                    sk.Send(message);
                }
                Close();
                return true;
            }

            catch (Exception exx)
            {
                Close();
                ErrMsg = "打印异常："+exx.ToString();
                return false;
            }
        }
      
    }
}
