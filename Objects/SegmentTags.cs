namespace CSVTest
{
    class SegmentTags
    {
        public string segment = " ";
        public string tagName = " ";
        public string description = " ";
        public string tagType = " ";
        public string dataClass = " ";


        public string[] getStringLineFormat(SegmentTags p)
        {
            string[] output = new string[5]{p.segment, p.tagName, p.description, p.tagType, p.dataClass};

            return output;
        }
    }
}