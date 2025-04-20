namespace LegitHttpServer
{
    public class HttpHeader
    {
        private string name, value;

        public HttpHeader(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        public string GetName()
        {
            return this.name;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public string GetValue()
        {
            return this.value;
        }

        public void SetValue(string value)
        {
            this.value = value;
        }
    }
}