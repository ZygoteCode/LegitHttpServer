namespace LegitHttpServer
{
    using System.Collections.Generic;
    using System.Net.Sockets;
    using System.Text;
    using System.Net;

    public class HttpRequest
    {
        private string URI, body;
        private HttpMethod method;
        private HttpVersion version;
        private List<HttpHeader> headers;
        private TcpClient tcpClient;
        private NetworkStream networkStream;

        public HttpRequest(TcpClient tcpClient, NetworkStream networkStream, string URI = "/", HttpMethod method = HttpMethod.GET, HttpVersion version = HttpVersion.HTTP_11, List<HttpHeader> headers = null, string body = "")
        {
            this.tcpClient = tcpClient;
            this.networkStream = networkStream;

            if (headers == null)
            {
                headers = new List<HttpHeader>();
            }

            this.URI = URI;
            this.method = method;
            this.version = version;
            this.headers = headers;
        }

        public string GetURI()
        {
            return this.URI;
        }

        public HttpMethod GetMethod()
        {
            return this.method;
        }

        public HttpVersion GetVersion()
        {
            return this.version;
        }

        public List<HttpHeader> GetHeaders()
        {
            return this.headers;
        }

        public string GetBody()
        {
            return this.body;
        }

        public string GetVersionStr()
        {
            switch (this.version)
            {
                case HttpVersion.HTTP_10:
                    return "HTTP/1.0";
                case HttpVersion.HTTP_11:
                    return "HTTP/1.1";
                default:
                    return null;
            }
        }

        public string GetMethodStr()
        {
            return this.method.ToString();
        }

        public Dictionary<string, string> GetHeadersAsDictionary()
        {
            Dictionary<string, string> theHeaders = new Dictionary<string, string>();

            foreach (HttpHeader header in this.headers)
            {
                theHeaders.Add(header.GetName(), header.GetValue());
            }

            return theHeaders;
        }

        public void SetURI(string URI)
        {
            this.URI = URI;
        }

        public void SetMethod(HttpMethod method)
        {
            this.method = method;
        }

        public void SetVersion(HttpVersion version)
        {
            this.version = version;
        }

        public void SetHeaders(List<HttpHeader> headers)
        {
            this.headers = headers;
        }

        public void SetBody(string body)
        {
            this.body = body;
        }

        public void SetBody(byte[] body)
        {
            this.body = Encoding.UTF8.GetString(body);
        }

        public TcpClient GetTcpClient()
        {
            return this.tcpClient;
        }

        public NetworkStream GetNetworkStream()
        {
            return this.networkStream;
        }

        public void WriteResponse(HttpResponse response)
        {
            byte[] sendBytes = HttpParser.ParseResponse(response);
            networkStream.Write(sendBytes, 0, sendBytes.Length);
            networkStream.Close();
            tcpClient.Close();
        }

        public IPEndPoint GetEndPoint()
        {
            return ((IPEndPoint)this.tcpClient.Client.RemoteEndPoint);
        }

        public string GetIpAddress()
        {
            return GetEndPoint().Address.ToString();
        }

        public string GetHeader(string name)
        {
            foreach (HttpHeader header in headers)
            {
                if (header.GetName().Equals(name))
                {
                    return header.GetValue();
                }
            }

            return null;
        }

        public bool HasBody()
        {
            return this.body != null && this.body != "";
        }
    }
}