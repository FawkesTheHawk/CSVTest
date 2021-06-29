using System.Collections.Generic;

namespace CSVTest
{
    class Parameters
    {
        public string torp = "";
        public string phase = "";
        public string phaseDescription = "";
        public string parameter = "";
        public string parameterDescription = ""; 
        public string type = "";
        public string target = "";
        public string targetValue = "";
        public string actual = "";
        public string highDev = "";
        public string highDevValue = "";
        public string lowDev = "";
        public string lowDevValue = "";
        public string highLimit = "";
        public string highLimitValue = "";
        public string lowLimit = "";
        public string lowLimitValue = "";
        public string preact = "";
        public string preactValue = "";
        public string lotCode = "";
        public string matId = "";

        public string[] getStringLineFormat(Parameters obj)
        {
            string[] output;

            output = new string[21]{obj.torp, obj.phase, obj.phaseDescription, obj.parameter, obj.parameterDescription, obj.type, obj.target, obj.targetValue, obj.actual,
            obj.highDev, obj.highDevValue, obj.lowDev, obj.lowDevValue, obj.highLimit, obj.highLimitValue, obj.lowLimit, obj.lowLimitValue, obj.preact,
            obj.preactValue, obj.lotCode, obj.matId};
            
            return output;
        }
    }
}
