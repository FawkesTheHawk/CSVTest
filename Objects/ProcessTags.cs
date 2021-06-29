namespace CSVTest
{
    class ProcessTags
    {
        public string process = " ";
        public string tagName = " ";
        public string description = " ";
        public string tagType = " ";
        public string dataClass = " ";


        public string[] getStringLineFormat(ProcessTags p)
        {
            string[] output = new string[5]{p.process, p.tagName, p.description, p.tagType, p.dataClass};

            return output;
        }
    }
}