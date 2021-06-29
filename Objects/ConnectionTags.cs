namespace CSVTest
{
    class ConnectionTags
    {
        public string connections = "";
        public string tagName = "";
        public string description = "";
        public string tagType = "";
        public string dataClass = "";


        public string[] getStringLineFormat(ConnectionTags c)
        {
            string[] output = new string[5]{c.connections, c.tagName, c.description, c.tagType, c.dataClass};

            return output;
        }
    }
}