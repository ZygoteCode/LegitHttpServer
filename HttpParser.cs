namespace LegitHttpServer
{
    using System.Collections.Generic;
    using System;
    using Microsoft.VisualBasic;
    using System.Text;
    using System.Net.Sockets;
    using System.Linq;

    public class HttpParser
    {
        public static HttpRequest ParseRequest(byte[] content, TcpClient tcpClient, NetworkStream networkStream)
        {
            string data = Encoding.UTF8.GetString(content);
            HttpRequest request = new HttpRequest(tcpClient, networkStream);
            List<string> parts = new List<string>();
            List<HttpHeader> headers = new List<HttpHeader>();

            foreach (string part in Utils.SplitToLines(data))
            {
                parts.Add(part);
            }

            for (int i = 0; i < parts.Count - 1; i++)
            {
                if (i == 0)
                {
                    string firstLine = parts[0];
                    string[] splitted = firstLine.Split(' ');
                    request.SetMethod((HttpMethod)Enum.Parse(typeof(HttpMethod), splitted[0]));
                    request.SetURI(splitted[1]);

                    switch (splitted[2])
                    {
                        case "HTTP/1.0":
                            request.SetVersion(HttpVersion.HTTP_10);
                            break;
                        case "HTTP/1.1":
                            request.SetVersion(HttpVersion.HTTP_11);
                            break;
                    }
                }
                else
                {
                    if (parts[i] == "")
                    {
                        break;
                    }
                    else
                    {
                        string[] splitted = Strings.Split(parts[i], ": ");
                        headers.Add(new HttpHeader(splitted[0], splitted[1]));
                    }
                }
            }

            request.SetHeaders(headers);

            try
            {
                int lastIndex = 0;
                bool meet = false;

                for (int i = 0; i < content.Length; i++)
                {
                    if (meet)
                    {
                        if (content[i] != '\r')
                        {
                            meet = false;
                        }
                        else
                        {
                            lastIndex = i + 2;
                            break;
                        }
                    }
                    else
                    {
                        if (content[i] == '\n')
                        {
                            meet = true;
                            lastIndex = i;
                        }
                    }
                }

                request.SetBody(content.Skip(lastIndex).ToArray());
            }
            catch
            {

            }

            return request;
        }

        public static byte[] ParseResponse(HttpResponse response)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(response.GetVersionStr() + " " + response.GetStatusCode() + " " + response.GetStatusDescription());

            foreach (HttpHeader header in response.GetHeaders())
            {
                builder.AppendLine(header.GetName() + ": " + header.GetValue());
            }

            builder.AppendLine("");

            byte[] first = Encoding.UTF8.GetBytes(builder.ToString());
            byte[] second = response.GetBody();

            return Utils.Combine(first, second);
        }
    }
}