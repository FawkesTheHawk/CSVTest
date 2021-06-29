namespace CSVTest
{
    class TransferTags
    {
        public string transfer = " ";
        public string tagName = " ";
        public string description = " ";
        public string tagType = " ";
        public string dataClass = " ";


        public string[] getStringLineFormat(TransferTags p)
        {
            string[] output = new string[5]{p.transfer, p.tagName, p.description, p.tagType, p.dataClass};

            return output;
        }
    }
}