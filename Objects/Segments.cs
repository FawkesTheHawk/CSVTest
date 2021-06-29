namespace CSVTest
{
    class Segments
    {
        public string segment = " ";
        public string description = " ";

        public string[] getStringLineFormat(Segments s)
        {
            string[] output = new string[2]{s.segment, s.description};

            return output;
        }
    }
}