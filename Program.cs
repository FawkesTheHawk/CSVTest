using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using CSVTest;

namespace CSVTest
{
    class Program
    {
        static StreamReader sr = new StreamReader("C:\\Users\\abazajian\\source\\repos\\CSVTest\\UpstreamModel_Val_22JUN2021.txt");
        static List<string[]> unitAndDescription = new List<string[]>();
        static List<string[]> unitAndTagAndTypeAndData = new List<string[]>();
        static List<string> overallList = new List<string>();
        static List<string> entireDocument = new List<string>();
        static List<int> processModelIndexes = new List<int>();

        static List<Connections> allConnections = new List<Connections>();
        static List<ConnectionTags> allConnectionTags = new List<ConnectionTags>();
        static List<EquipmentStatus> allEquipmentStatus = new List<EquipmentStatus>();
        static List<Processes> allProcesses = new List<Processes>();

        static void Main()
        {
            string readLine;
            while ((readLine = sr.ReadLine()) != null)
            {
                if(readLine.Trim().Equals(String.Empty))
                {
                    continue;
                }
                entireDocument.Add(FormatString(readLine));
            }

            foreach (string line in entireDocument)
            {
                if (line.Contains("Process Modeling:"))
                {
                    processModelIndexes.Add(entireDocument.IndexOf(line));
                }
            }
            int i = 0;
            foreach (int indexOfProcessModel in processModelIndexes)
            {
                string header = GrabStringValueFromLine(entireDocument[indexOfProcessModel]);
                switch(header)
                {
                    case "Units":
                        ProcessModelingUnits(indexOfProcessModel, i);
                        break;
                    case "Unit Tags":
                        ProcessModelingUnitTags(indexOfProcessModel, i);
                        ExportFuctionUnitTags();
                        break;
                    case "Connections":
                        ProcessModelingConnections(indexOfProcessModel, i);
                        ExportFunctionConnections();
                        break;
                    case "Connection Tags":
                        ProcessModelingConnectionTags(indexOfProcessModel, i);
                        ExportFunctionConnectionTags();
                        break;
                    case "Equipment Status":
                        ProcessModelingEquipmentStatus(indexOfProcessModel, i);
                        ExportFunctionEquipmentStatus();
                        break;
                    case "Processes":
                        ProcessModelingProcesses(indexOfProcessModel, i);
                        ExportFunctionProcesses();
                        break;
                    case "Process Tags":
                        // ProcessModelingUnits();
                        break;
                    case "Process Phases":
                        // ProcessModelingUnits();
                        break;
                    case "Segments":
                        // ProcessModelingUnits();
                        break;
                    case "Segment Tags":
                        // ProcessModelingUnits();
                        break;
                    case "Transfers":
                        // ProcessModelingUnits();
                        break;
                    case "Transfer Tags":
                        // ProcessModelingUnits();
                        break;
                    case "Transfer Phases":
                        // ProcessModelingUnits();
                        break;
                    default:
                        Console.WriteLine("Default case");
                        break;
                }

                i++;
            }

        }

    #region Unit and Unit Tags
        #region  ProcessModeling: Units
            static void ProcessModelingUnits(int indexOfEntireDocument, int indexOfProcessModels)
            {
                int startIndex = indexOfEntireDocument;
                int currentIndex = startIndex;
                int endIndex = processModelIndexes[indexOfProcessModels+1];
                try
                {
                    List<int> unitsIndexes = new List<int>();
                    for (;currentIndex < endIndex; currentIndex++)
                    {
                        string line = entireDocument[currentIndex];
                        if (line.Contains("Unit:"))
                        {
                            unitsIndexes.Add(currentIndex);
                        }
                    }
                    
                    for (int i=0; i<unitsIndexes.Count-1; i++)
                    {
                        unitAndDescription.Add(LookForUnitAndDescription(unitsIndexes[i]));
                    }

                    // foreach (string[] line in unitAndDescription){
                    //     Console.WriteLine(line[0] + "," + line[1]);
                    // }
                }
                catch (Exception e){Console.WriteLine("Exception: " + e.Message); }
                finally{Console.WriteLine("Executing finally block.");}
            }

            static string[] LookForUnitAndDescription(int indexOfEntireDocument)
            {
                string first = GrabStringValueFromLine(entireDocument[indexOfEntireDocument]);
                string second = " ";

                string line;
                indexOfEntireDocument++;
                while (!(entireDocument[indexOfEntireDocument].Contains("Unit:")))
                {
                    line = entireDocument[indexOfEntireDocument];
                    if (line.Contains("Description:"))
                    {
                        second = GrabStringValueFromLine(line);
                    }
                    else {
                        second = second + " " +entireDocument[indexOfEntireDocument].Trim();
                    }
                    indexOfEntireDocument++;
                    
                }
                string[] output = new string[2]{first, second};
                return output;
            }

        #endregion

        #region Unit Tags
        static void ProcessModelingUnitTags(int indexOfEntireDocument, int indexOfProcessModels)
        {
            int startIndex = indexOfEntireDocument;
            int currentIndex = startIndex;
            int endIndex = processModelIndexes[indexOfProcessModels+1];
            try
            {
                // a list of locations where "Unit:" is found within the header "Process Modeling: Unit Tags"
                List<int> unitsIndexes = new List<int>();
                for (;currentIndex < endIndex; currentIndex++)
                {
                    string line = entireDocument[currentIndex];
                    if (line.Contains("Unit:"))
                    {
                        unitsIndexes.Add(currentIndex);
                    }
                }
                
                // Console.WriteLine("units length in Units Tags: " + unitsIndexes.Count);
                for (int i=0; i<unitsIndexes.Count-1; i++)
                {
                    string unitName = GrabStringValueFromLine(entireDocument[unitsIndexes[i]]);
                    // Console.WriteLine("unit names: " + unitName);
                    LookForTagTypeData(unitsIndexes[i], unitName);
                }
            }
            catch (Exception e){Console.WriteLine("Exception: " + e.Message); }
            finally{Console.WriteLine("Executing finally block.");}
            
        }
        #endregion
        static string GetDescriptionFromUnit(string unitName)
        {
            string description = "no description found";
            foreach (string[] UandD in unitAndDescription)
            {
                if (UandD[0].Equals(unitName))
                {
                    description = UandD[1];
                }
            }
            return description;
        }

        static void LookForTagTypeData(int indexOfEntireDocument, string unitName)
        {
            indexOfEntireDocument++;
            List<string> nameIndexes = new List<string>();
            List<string> typeIndexes = new List<string>();
            List<string> classIndexes = new List<string>();
            while (!(entireDocument[indexOfEntireDocument].Contains("Unit:")))
            {
                string line = entireDocument[indexOfEntireDocument];
                // Console.WriteLine("line: " + line);
                string newLine = line.Split(":")[0].Trim();
                if (newLine.Equals("Tag Name"))
                {
                    nameIndexes.Add(GrabStringValueFromLine(line));
                }
                else if (newLine.Equals("Tag Type"))
                {
                    typeIndexes.Add(GrabStringValueFromLine(line));
                }
                else if (newLine.Equals("Data Class"))
                {
                    classIndexes.Add(GrabStringValueFromLine(line));
                }                
                indexOfEntireDocument++;
            }
            for(int i=0; i<nameIndexes.Count-1;i++)
            {
                string[] output = new string[4]{unitName, nameIndexes[i], typeIndexes[i], classIndexes[i]};
                unitAndTagAndTypeAndData.Add(output);
            }
        }
    #endregion

    #region Process Phases





    #endregion

    #region Connections
    static void ProcessModelingConnections(int indexOfEntireDocument, int indexOfProcessModels)
    {
        Console.WriteLine("Got to ProcessModeling Connections Function");
        int segmentIndex = indexOfEntireDocument;
        string tempConnection = "";
        indexOfEntireDocument++;
        while(indexOfEntireDocument<processModelIndexes[indexOfProcessModels+1])
        {
            
            string line = entireDocument[indexOfEntireDocument];
            if (line.Contains("Connection:"))
            {
                Connections connections = new Connections();
                tempConnection = GrabStringValueFromLine(line);
                connections.connection = tempConnection;
                // Console.WriteLine("Starting new connection: " + tempConnection);
                connections.sourceUnit = GrabStringValueFromLine(entireDocument[indexOfEntireDocument+1]);
                connections.destUnit = GrabStringValueFromLine(entireDocument[indexOfEntireDocument+2]);
                connections.preact = GrabStringValueFromLine(entireDocument[indexOfEntireDocument+3]);
                connections.description = GetDescriptionOfConnection(indexOfEntireDocument+4, tempConnection);
                connections.segments = "No Segment";

                allConnections.Add(connections);
            }
            if (line.Contains("Segments"))
            {
                // Console.WriteLine("current Line looking for segment: " + line);
                segmentIndex = indexOfEntireDocument+2;

                while(!entireDocument[segmentIndex].Contains("Connection:") && segmentIndex<processModelIndexes[indexOfProcessModels+1])
                {

                    Connections newConnectWithSeg = new Connections();
                    Connections holder = allConnections.Find(x => x.connection.Equals(tempConnection));
                    newConnectWithSeg.connection = holder.connection;
                    newConnectWithSeg.description = holder.description;
                    newConnectWithSeg.destUnit = holder.destUnit;
                    newConnectWithSeg.preact = holder.preact;
                    newConnectWithSeg.sourceUnit = holder.sourceUnit;
                    newConnectWithSeg.segments = FormatString(entireDocument[segmentIndex]);
                    // Console.WriteLine(string.Format("tried to write this segment: {0} for connection {1}",FormatString(entireDocument[segmentIndex]),tempConnection ));
                    allConnections.Add(newConnectWithSeg);
                    segmentIndex++;
                }
            }

            indexOfEntireDocument++;
        }
    }

    static string GetDescriptionOfConnection(int indexOfEntireDocument, string currentConnection)
    {
        // Console.WriteLine("Getting multiline description");
        string line = entireDocument[indexOfEntireDocument];

        if (!line.Contains("Description:"))
        {
            return null;
        }

        string lineValue = GrabStringValueFromLine(line);
        int endIndex = FindEndIndex("Segments", "Connection:", "Process Modeling:",indexOfEntireDocument);

        for (; indexOfEntireDocument<endIndex; indexOfEntireDocument++)
        {
            lineValue = lineValue + " " + FormatString(entireDocument[indexOfEntireDocument]);
        }
        return lineValue;
    }

    static void ProcessModelingConnectionTags(int indexOfEntireDocument, int indexOfProcessModels)
    {
        string line = "";
        List<int> allConnectionTagIndexes = new List<int>();
        List<int> allConnectionTagNames = new List<int>();
        List<int> allConnectionTagDataClasses = new List<int>();
        List<int> allConnectionTagTypes = new List<int>();
        List<int> allConnectionTagDescriptions = new List<int>();
        for(;indexOfEntireDocument<processModelIndexes[indexOfProcessModels+1]; indexOfEntireDocument++)
        {
            line = entireDocument[indexOfEntireDocument];
            if (line.Contains("Connection:"))
            {
                allConnectionTagIndexes.Add(indexOfEntireDocument);
            }
            else if (line.Contains("Tag Name:"))
            {
                allConnectionTagNames.Add(indexOfEntireDocument);
            }
            else if (line.Contains("Description:"))
            {
                allConnectionTagDescriptions.Add(indexOfEntireDocument);
            }
            else if (line.Contains("Tag Type:"))
            {
                allConnectionTagTypes.Add(indexOfEntireDocument);
            }
            else if (line.Contains("Data Class:"))
            {
                allConnectionTagDataClasses.Add(indexOfEntireDocument);
            }
        }
        allConnectionTagIndexes.Add(processModelIndexes[indexOfProcessModels+1]);
        // Console.WriteLine(string.Format("Found these things and they are of length {1}, {2}, {3}",))
        for(int i=0;i<allConnectionTagNames.Count;i++)
        {
            ConnectionTags nCT = new ConnectionTags();
            int smallIndex = allConnectionTagIndexes.FindLastIndex(x => x<allConnectionTagNames[i]);
            // Console.WriteLine("small index: " + smallIndex);
            // Console.WriteLine("sConnectionTagIndex[0] " + allConnectionTagIndexes[0] + "   " +  allConnectionTagIndexes[1]);
            // Console.WriteLine("allConnectionTagNames[i]: "+ allConnectionTagNames[i]);
            nCT.connections = GrabStringValueFromLine(entireDocument[allConnectionTagIndexes[smallIndex]]);
            nCT.dataClass = GrabStringValueFromLine(entireDocument[allConnectionTagDataClasses[i]]);
            nCT.description = GetDescriptionOfConnectionTags(allConnectionTagDescriptions[i]);
            nCT.tagName = GrabStringValueFromLine(entireDocument[allConnectionTagNames[i]]);
            nCT.tagType = GrabStringValueFromLine(entireDocument[allConnectionTagTypes[i]]);

            allConnectionTags.Add(nCT);
        }

    }

    static string GetDescriptionOfConnectionTags(int indexOfEntireDocument)
    {
        // Console.WriteLine("Getting multiline description");
        string line = entireDocument[indexOfEntireDocument];

        if (!line.Contains("Description:"))
        {
            return null;
        }

        string lineValue = GrabStringValueFromLine(line);
        int endIndex = FindEndIndex("Tag Type:",indexOfEntireDocument);

        for (; indexOfEntireDocument<endIndex; indexOfEntireDocument++)
        {
            lineValue = lineValue + " " + FormatString(entireDocument[indexOfEntireDocument]);
        }

        if (lineValue.Trim().Equals("Description:")){
            lineValue = "";
        }
        return lineValue;
    }

    #endregion

    #region Equipment Status
        static void ProcessModelingEquipmentStatus(int indexOfEntireDocument, int indexOfProcessModels)
        {
            List<int> indexOfEquipmentStatuses = new List<int>();
            List<int> indexOfEquipAvailability = new List<int>();
            List<int> indexOfEquipUsage = new List<int>();
            for (int i=indexOfEntireDocument;i<processModelIndexes[indexOfProcessModels+1];i++)
            {
                string line = entireDocument[i];
                if (line.Contains("Equipment Status:"))
                {
                    indexOfEquipmentStatuses.Add(i);
                }
                else if (line.Contains("Availability:"))
                {
                    indexOfEquipAvailability.Add(i);
                }
                else if (line.Contains("Description:"))
                {
                    indexOfEquipUsage.Add(i);
                }
            }

            for(int i=0;i<indexOfEquipmentStatuses.Count;i++)
            {
                EquipmentStatus newES = new EquipmentStatus();
                newES.equipmentStatus = GrabStringValueFromLine(entireDocument[indexOfEquipmentStatuses[i]]);
                newES.availability = GrabStringValueFromLine(entireDocument[indexOfEquipAvailability[i]]);
                newES.usage = FormatString(GetUsageFromEquipmentStatus(indexOfEquipUsage[i], processModelIndexes[indexOfProcessModels+1]));

                allEquipmentStatus.Add(newES);
            }
        }

        static string GetUsageFromEquipmentStatus(int startIndex, int endIndex)
        {
            string output = "";
            while(!entireDocument[startIndex].Contains("Equipment Status:") && startIndex<endIndex)
            {
                string line = entireDocument[startIndex];
                if (line.Contains("Usage:"))
                {
                    output = line.Split("Usage:")[1];
                }
                else
                {
                    output = output + " " + FormatString(line);
                }
                startIndex++;
            }

            return output;
        }
    #endregion

    #region Processes

    static void ProcessModelingProcesses(int indexOfEntireDocument, int indexOfProcessModels)
    {
        List<int> indexOfProcesses = new List<int>();
        List<int> indexOfDescription = new List<int>();
        List<int> indexOfUnits = new List<int>();

        for(;indexOfEntireDocument<processModelIndexes[indexOfProcessModels+1]; indexOfEntireDocument++)
        {
            string line = entireDocument[indexOfEntireDocument];
            if (line.Contains("Process:"))
            {
                indexOfProcesses.Add(indexOfEntireDocument);
            }
        }

        foreach(int i in indexOfProcesses)
        {
            Processes p = new Processes();
            p.process = GrabStringValueFromLine(entireDocument[i]);
            p.description = GetDescriptionFromProcesses(i, processModelIndexes[indexOfProcessModels+1]);
            Console.WriteLine("i = " + i);
            GetUnitAttyValFromProcesses(i, processModelIndexes[indexOfProcessModels+1], p);
        }
        Console.WriteLine("length og indexofProcesses: " + indexOfProcesses.Count);

    }

    static string GetDescriptionFromProcesses(int startIndex, int endIndex)
    {
        string output = "";
        while(startIndex<endIndex && !entireDocument[startIndex].Contains("Units"))
        {
            string line = entireDocument[startIndex];
            if (line.Contains("Description"))
            {
                output = GrabStringValueFromLine(line);
            }
            else {
                output = output + " " + line;
            }
            startIndex++;
        }
        return output;
    }

    static void GetUnitAttyValFromProcesses(int startIndex, int endIndex, Processes p)
    {
        startIndex= startIndex +2;
        while (startIndex<endIndex && !entireDocument[startIndex].Contains("Process"))
        {
            Console.WriteLine("start index = " + startIndex);
            string line = entireDocument[startIndex];
            if (line.Contains("Units") || line.Contains("Attributes") || line.Contains("--------"))
            {
                Console.WriteLine("continues");
                startIndex++;
                continue;
            }
            else
            {
                Processes newP = new Processes();
                newP.process = p.process;
                newP.description = p.description;

                while(newP.units.Equals(string.Empty))
                {
                    line = entireDocument[startIndex];
                    string[] brokline = line.Split(" ");
                    if (brokline.Length == 0)
                    {
                        startIndex++;
                    }
                    if (brokline.Length >= 1)
                    {
                        newP.units = brokline[0];
                        continue;
                    }
                    if (brokline.Length >= 2)
                    {
                        newP.attributes = brokline[1];
                        continue;
                    }
                    if (brokline.Length >= 3)
                    {
                        newP.value = brokline[2];
                        continue;
                    }
                allProcesses.Add(newP);
                }
            }
            startIndex++;
        }
    }
    

    #endregion

    #region Common Functions

        static string FormatString(string lineUnformatted)
        {
            string formattedLine;
            try
            {
                lineUnformatted = lineUnformatted.Replace("," , "");
                if (lineUnformatted.Contains("Date"))
                {
                    lineUnformatted = lineUnformatted.Replace("Date" , "");
                }
            }
            catch
            {
                Console.WriteLine("Line" + lineUnformatted + "Does not contain commas");
            }
            finally
            {
                formattedLine = lineUnformatted.Trim();
            }
            return (formattedLine);
        }

        static int FindEndIndex(string endPhrase1, int startIndex)
        {
            while(!entireDocument[startIndex].Contains(endPhrase1))
            {
                startIndex++;
            }

            return startIndex;
        }

        static int FindEndIndex(string endPhrase1, string endPhrase2, int startIndex)
        {
            while(!entireDocument[startIndex].Contains(endPhrase1) & !entireDocument[startIndex].Contains(endPhrase2))
            {
                startIndex++;
            }

            return startIndex;
        }

        static int FindEndIndex(string endPhrase1, string endPhrase2, string endPhrase3, int startIndex)
        {
            while(!entireDocument[startIndex].Contains(endPhrase1) & !entireDocument[startIndex].Contains(endPhrase2) & !entireDocument[startIndex].Contains(endPhrase3) )
            {
                startIndex++;
            }

            return startIndex;
        }
        static string GrabStringValueFromLine(string line)
        {
            return FormatString(line.Split(":")[1]);
        }

    #endregion


    #region final export functions
        static void ExportFuctionUnitTags()
        {
            Console.WriteLine("Got to UnitTags Export Function");
            StringBuilder sb = new StringBuilder();
            string[] introLine = new string[5]{"Unit", "Description", "Tag Name", "Tag Type", "Data Class"};
            var fir = introLine[0];
            var sec = introLine[1];
            var thi = introLine[2];
            var fou = introLine[3];
            var fif = introLine[4];

            var line = (string.Format("{0},{1},{2},{3},{4}", fir, sec, thi, fou, fif));
            sb.AppendLine(line);

            // Console.WriteLine("count of unit and whatever: " + unitAndTagAndTypeAndData.Count);

            for (int j = 0; j<unitAndTagAndTypeAndData.Count-1; j++)
            {
                
                string[] rightline = unitAndTagAndTypeAndData[j];
                var newLine = string.Format("{0},{1},{2},{3},{4}", rightline[0], GetDescriptionFromUnit(rightline[0]), rightline[1], rightline[2], rightline[3]);
                sb.AppendLine(newLine);

            }
            System.IO.File.WriteAllText("C:\\Users\\abazajian\\source\\repos\\CSVTest\\CSVs\\UnitTags.csv", sb.ToString());
        }

        static void ExportFunctionConnections()
        {
            Console.WriteLine("Got to Connections Export Function");
            StringBuilder sb = new StringBuilder();

            string[] header = new string[6]{"Connections", "Source Unit", "Dest. Unit", "Preact", "description", "Segments"};
            var headerline = string.Format("{0},{1},{2},{3},{4},{5}",header[0], header[1], header[2], header[3], header[4], header[5]);
            sb.AppendLine(headerline);


            foreach(Connections c in allConnections)
            {
                string[] obj = c.getStringLineFormat(c);
                var output = string.Format("{0},{1},{2},{3},{4},{5}", obj[0], obj[1], obj[2], obj[3], obj[4], obj[5]);

                sb.AppendLine(output);
            }

            System.IO.File.WriteAllText("C:\\Users\\abazajian\\source\\repos\\CSVTest\\CSVs\\Connections.csv", sb.ToString());
        }

        static void ExportFunctionConnectionTags()
        {
            Console.WriteLine("Got to ConnectionTags export function");
            StringBuilder sb = new StringBuilder();

            string[] header = new string[5]{"Connections", "Tag Name", "Description", "Tag Type", "Data Class"};
            var headerline = string.Format("{0},{1},{2},{3},{4}", header);
            sb.AppendLine(headerline);

            // Console.WriteLine("length of Connection Tags: " + allConnectionTags.Count);

            foreach (ConnectionTags tag in allConnectionTags)
            {
                string[] obj = tag.getStringLineFormat(tag);
                var output = string.Format("{0},{1},{2},{3},{4}", obj);

                sb.AppendLine(output);
            }

            System.IO.File.WriteAllText("C:\\Users\\abazajian\\source\\repos\\CSVTest\\CSVs\\ConnectionTags.csv", sb.ToString());
        }

        static void ExportFunctionEquipmentStatus()
        {
            Console.WriteLine("Got to Equipment Status export function");
            StringBuilder sb = new StringBuilder();

            string[] header = new string[3]{"Equipment Status", "Availability", "Usage"};
            var headerline = string.Format("{0},{1},{2}", header);
            sb.AppendLine(headerline);

            foreach (EquipmentStatus es in allEquipmentStatus)
            {
                string[] obj = es.getStringLineFormat(es);
                var output = string.Format("{0},{1},{2}", obj);

                sb.AppendLine(output);
            }

            System.IO.File.WriteAllText("C:\\Users\\abazajian\\source\\repos\\CSVTest\\CSVs\\EquipmentStatus.csv", sb.ToString());
        }

        static void ExportFunctionProcesses()
        {
            Console.WriteLine("Got to Processes export function");
            StringBuilder sb = new StringBuilder();

            string[] header = new string[5]{"Process", "Description", "Units", "Attributes", "Value"};
            var headerline = string.Format("{0},{1},{2},{3},{4}", header);
            sb.AppendLine(headerline);

            foreach(Processes p in allProcesses)
            {
                string[] obj = p.getStringLineFormat(p);
                var output = string.Format("{0},{1},{2},{3},{4}", obj);

                sb.AppendLine(output);
            }

            System.IO.File.WriteAllText("C:\\Users\\abazajian\\source\\repos\\CSVTest\\CSVs\\Processes.csv", sb.ToString());
        }


    #endregion
    



    //Class Program closing bracket
    }
}
