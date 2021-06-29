using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace CSVTest
{
    class Connections
    {
        public string connection = "";
        public string sourceUnit = "";
        public string destUnit = "";
        public string preact = "";
        public string description = "";
        public string segments = "";

        public string[] getStringLineFormat(Connections c)
        {
            string[] output = new string[6]{c.connection, c.sourceUnit, c.destUnit, c.preact, c.description, c.segments};

            return output;
        }
    }
}


