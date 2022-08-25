namespace LegitHttpServer
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Runtime;

    public class HttpServer
    {
        private TcpListener tcpListener;
        private ushort port;
        private bool noDelay;

        public HttpServer(ushort port = 80, bool start = true)
        {
            this.port = port;
            this.tcpListener = new TcpListener(IPAddress.Any, this.port);
        }

        public ushort GetPort()
        {
            return this.port;
        }

        public TcpListener GetTcpListener()
        {
            return this.tcpListener;
        }

        public void Start()
        {
            this.tcpListener.Start();
        }

        public void Stop()
        {
            this.tcpListener.Stop();
        }

        public bool IsNoDelay()
        {
            return this.noDelay;
        }

        public void SetNoDelay(bool noDelay)
        {
            this.noDelay = noDelay;
        }

        public HttpRequest HandleRequest()
        {
            TcpClient client = this.tcpListener.AcceptTcpClient();
            client.NoDelay = this.noDelay;
            NetworkStream stream = client.GetStream();
            byte[] content = Utils.NetworkStreamToBytes(stream);
            HttpRequest request = HttpParser.ParseRequest(content, client, stream);
            return request;
        }
    }
}