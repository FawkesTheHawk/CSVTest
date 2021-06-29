using System.IO;
using System.Text;
using System.Collections.Generic;

namespace CSVTest
{
    class Transfers
    {
        public string transfer = "";
        public string sourceProcessClass = "";
        public string destProcessClass = "";
        public string description = "";
        public string connection = "";
        public string sourceUnit = "";
        public string destinationUnit = "";
        public int numberOfConnections = 0;
        public List<string> connectionLine = new List<string>();

        public string[] getStringLineFormat(Transfers p)
        {
            string[] output = new string[7]{p.transfer, p.sourceProcessClass, p.destProcessClass, p.description, p.connection, p.sourceUnit, p.destinationUnit};

            return output;
        }
    }
}