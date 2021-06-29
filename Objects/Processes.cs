using System.IO;
using System.Text;
using System.Collections.Generic;

namespace CSVTest
{
    class Processes
    {
        public string process = "";
        public string description = "";
        public string units = "";
        public string attributes = "";
        public string value = "";
        public int numberOfUnits = 0;
        public List<string> unitLine = new List<string>();

        public string[] getStringLineFormat(Processes p)
        {
            string[] output = new string[5]{p.process, p.description, p.units, p.attributes, p.value};

            return output;
        }
    }
}