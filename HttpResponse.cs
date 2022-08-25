namespace LegitHttpServer
{
    using System.Collections.Generic;
    using System.Text;

    public class HttpResponse
    {
        private HttpVersion version;
        private int statusCode;
        private string statusDescription;
        private List<HttpHeader> headers;
        private byte[] body;

        public HttpResponse(HttpVersion version = HttpVersion.HTTP_11, int statusCode = 200, string statusDescription = "OK", List<HttpHeader> headers = null, byte[] body = null)
        {
            if (headers != null)
            {
                this.headers = headers;
            }
            else
            {
                this.headers = new List<HttpHeader>();
            }

            this.version = version;
            this.statusCode = statusCode;
            this.statusDescription = statusDescription;
            this.body = body;
        }

        public HttpVersion GetVersion()
        {
            return this.version;
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

        public void SetVersion(HttpVersion version)
        {
            this.version = version;
        }

        public int GetStatusCode()
        {
            return this.statusCode;
        }

        public void SetStatusCode(int statusCode)
        {
            this.statusCode = statusCode;
        }

        public string GetStatusDescription()
        {
            return this.statusDescription;
        }

        public void SetStatusDescription(string statusDescription)
        {
            this.statusDescription = statusDescription;
        }

        public byte[] GetBody()
        {
            return this.body;
        }

        public void SetBody(string body)
        {
            this.body = Encoding.UTF8.GetBytes(body);
        }

        public void SetBody(byte[] body)
        {
            this.body = body;
        }

        public List<HttpHeader> GetHeaders()
        {
            return this.headers;
        }

        public void SetHeaders(List<HttpHeader> headers)
        {
            this.headers = headers;
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

        public void AddHeader(string name, string value)
        {
            this.headers.Add(new HttpHeader(name, value));
        }

        public void AddHeader(HttpHeader header)
        {
            this.headers.Add(header);
        }

        public int GetBodyLength()
        {
            return this.body.Length;
        }
    }
}