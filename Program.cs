using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using CSVTest;

namespace CSVTest
{
    class Program
    {
        static StreamReader sr = new StreamReader("C:\\Users\\abazajian\\source\\repos\\CSVTest\\DownstreamModel_Val_22JUN2021.txt");
        static List<string[]> unitAndDescription = new List<string[]>();
        static List<string[]> unitAndTagAndTypeAndData = new List<string[]>();
        static List<string> overallList = new List<string>();
        static List<string> entireDocument = new List<string>();
        static List<int> processModelIndexes = new List<int>();

        static List<Connections> allConnections = new List<Connections>();
        static List<ConnectionTags> allConnectionTags = new List<ConnectionTags>();
        static List<EquipmentStatus> allEquipmentStatus = new List<EquipmentStatus>();
        static List<Processes> allProcesses = new List<Processes>();
        static List<ProcessTags> allProcessTags = new List<ProcessTags>();
        static List<Segments> allSegements = new List<Segments>();
        static List<SegmentTags> allSegementTags = new List<SegmentTags>();
        static List<Transfers> allTransfers = new List<Transfers>();
        static List<TransferTags> allTransferTags = new List<TransferTags>();
        static List<Parameters> allProcessPhases = new List<Parameters>();
        static List<Parameters> allTransferPhases = new List<Parameters>();

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
                        ProcessModelingProcessTags(indexOfProcessModel, i);
                        ExportFunctionProcessTags();
                        break;
                    case "Process Phases":
                        ProcessModelingPhases(indexOfProcessModel, i, "Process");
                        ExportPhases("Process");
                        break;
                    case "Segments":
                        ProcessModelingSegments(indexOfProcessModel, i);
                        ExportFunctionSegments();
                        break;
                    case "Segment Tags":
                        ProcessModelingSegmentTags(indexOfProcessModel, i);
                        ExportFunctionSegmentTags();
                        break;
                    case "Transfers":
                        ProcessModelingTransfers(indexOfProcessModel, i);
                        ExportFunctionTransfers();
                        break;
                    case "Transfer Tags":
                        ProcessModelingTransferTags(indexOfProcessModel, i);
                        ExportFunctionTransferTags();
                        break;
                    case "Transfer Phases":
                        ProcessModelingPhases(indexOfProcessModel, i, "Transfer");
                        ExportPhases("Transfer");
                        break;
                    default:
                        Console.WriteLine("Default case");
                        break;
                }

                i++;
            }

        }

    #region Phases

        static void ProcessModelingPhases(int indexOfEntireDocument, int indexOfProcessModel, string phaseType)
        {
            int endingIndex = entireDocument.Count-1;
            List<int> parameterIndex = new List<int>(); 
            if (indexOfProcessModel == processModelIndexes.Count-1)
            {
                endingIndex = entireDocument.Count;
            }
            else 
            {
                endingIndex = processModelIndexes[indexOfProcessModel+1];
            }

            for(;indexOfEntireDocument<endingIndex;indexOfEntireDocument++)
            {
                string line = entireDocument[indexOfEntireDocument];
                if (line.Contains("Parameter:"))
                {
                    parameterIndex.Add(indexOfEntireDocument);
                }
            }

            foreach (int index in parameterIndex)
            {
                int upperIndex = processModelIndexes[indexOfProcessModel];
                Parameters p = new Parameters();
                p.torp = GrabStringValueFromLine(entireDocument[GetIndexFromAboveForParamter(index, phaseType+":", upperIndex)]);
                p.phase = GrabStringValueFromLine(entireDocument[GetIndexFromAboveForParamter(index, "Phase:", upperIndex)]);
                p.phaseDescription = GetDescriptionForAnyinPhase(GetIndexFromAboveForParamter(index, "Description:", upperIndex));
                p.parameter = GrabStringValueFromLine(entireDocument[index]);
                p.parameterDescription = GetDescriptionForAnyinPhase(index+1);
                p.type = GrabStringValueFromLine(entireDocument[GetIndexFromBelowForParamter(index, "Type:", endingIndex)]);
                //getting target & target value
                int indexOfMultiChoice = GetIndexFromBelowForParamter(index, "Target:", endingIndex);
                if (indexOfMultiChoice != 1)
                {
                    // Console.WriteLine(string.Format("the target line for {0}, is {1}", p.parameter, entireDocument[indexOfMultiChoice]));
                    p.target = GrabStringValueFromLine(entireDocument[indexOfMultiChoice]);
                    string checkLine = entireDocument[indexOfMultiChoice+1];
                    if (checkLine.Contains(":"))
                    {
                        p.targetValue = GrabStringValueFromLine(checkLine);
                    }
                }
                indexOfMultiChoice = GetIndexFromBelowForParamter(index, "Actual:", endingIndex);
                if (indexOfMultiChoice != 1)
                {
                    p.actual = GrabStringValueFromLine(entireDocument[indexOfMultiChoice]);
                }
                indexOfMultiChoice = GetIndexFromBelowForParamter(index, "High Dev.:", endingIndex);
                if (indexOfMultiChoice != 1)
                {
                    p.highDev = GrabStringValueFromLine(entireDocument[indexOfMultiChoice]);
                    string checkLine = entireDocument[indexOfMultiChoice+1];
                    if (checkLine.Contains(":"))
                    {
                        p.highDevValue = GrabStringValueFromLine(checkLine);
                    }
                }
                indexOfMultiChoice = GetIndexFromBelowForParamter(index, "Low Dev.:", endingIndex);
                if (indexOfMultiChoice != 1)
                {
                    p.lowDev = GrabStringValueFromLine(entireDocument[indexOfMultiChoice]);
                    string checkLine = entireDocument[indexOfMultiChoice+1];
                    if (checkLine.Contains(":"))
                    {
                        p.lowDevValue = GrabStringValueFromLine(checkLine);
                    }
                }
                indexOfMultiChoice = GetIndexFromBelowForParamter(index, "High Limit:", endingIndex);
                if (indexOfMultiChoice != 1)
                {
                    p.highLimit = GrabStringValueFromLine(entireDocument[indexOfMultiChoice]);
                    string checkLine = entireDocument[indexOfMultiChoice+1];
                    if (checkLine.Contains(":"))
                    {
                        p.highLimitValue = GrabStringValueFromLine(checkLine);
                    }
                }
                indexOfMultiChoice = GetIndexFromBelowForParamter(index, "Low Limit:", endingIndex);
                if (indexOfMultiChoice != 1)
                {
                    p.lowLimit = GrabStringValueFromLine(entireDocument[indexOfMultiChoice]);
                    string checkLine = entireDocument[indexOfMultiChoice+1];
                    if (checkLine.Contains(":"))
                    {
                        p.lowLimitValue = GrabStringValueFromLine(checkLine);
                    }
                }
                indexOfMultiChoice = GetIndexFromBelowForParamter(index, "Preact:", endingIndex);
                if (indexOfMultiChoice != 1)
                {
                    p.preact = GrabStringValueFromLine(entireDocument[indexOfMultiChoice]);
                    string checkLine = entireDocument[indexOfMultiChoice+1];
                    if (checkLine.Contains(":"))
                    {
                        p.preactValue = GrabStringValueFromLine(checkLine);
                    }
                }
                indexOfMultiChoice = GetIndexFromBelowForParamter(index, "Lot Code:", endingIndex);
                if (indexOfMultiChoice != 1)
                {
                    p.lotCode = GrabStringValueFromLine(entireDocument[indexOfMultiChoice]);
                }
                indexOfMultiChoice = GetIndexFromBelowForParamter(index, "Mat. Id:", endingIndex);
                if (indexOfMultiChoice != 1)
                {
                    p.matId = GrabStringValueFromLine(entireDocument[indexOfMultiChoice]);
                }

                if (phaseType.Equals("Process"))
                {
                    allProcessPhases.Add(p);
                }
                else if (phaseType.Equals("Transfer"))
                {
                    allTransferPhases.Add(p);
                }

            }
        }

        static string GetDescriptionForAnyinPhase(int startIndex)
        {
            string output = "";
            int endIndex = FindEndIndex(":", startIndex+1);
            for(;startIndex<endIndex;startIndex++)
            {
                string line = entireDocument[startIndex];
                if (line.Contains("Description:"))
                {
                    output = GrabStringValueFromLine(line);
                }
                else 
                {
                    output = output + " " + line;
                }
            }
            return output;
        }

        static int GetIndexFromAboveForParamter(int index, string searchString, int upperIndex)
        {
            bool isItemFound = false;
            int output = 1;
            while(index>upperIndex && !isItemFound)
            {
                string line = entireDocument[index];
                if (line.Contains(searchString))
                {
                    output = index;
                    isItemFound = true;
                }
                index--;
            }
            return (output);
        }

        static int GetIndexFromBelowForParamter(int index, string searchString, int lowerIndex)
        {
            bool isItemFound = false;
            int output = 1;
            index++;
            while(index<lowerIndex && !isItemFound && !entireDocument[index].Contains("Parameter:"))
            {
                string line = entireDocument[index];
                if (line.Contains(searchString))
                {
                    output = index;
                    isItemFound = true;
                }
                index++;
            }
            return (output);
        }

    #endregion

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
        List<Processes> holderProcessList = new List<Processes>();
        foreach(int i in indexOfProcesses)
        {
            Processes p = new Processes();
            p.process = GrabStringValueFromLine(entireDocument[i]);
            p.description = GetDescriptionFromProcesses(i, processModelIndexes[indexOfProcessModels+1]);
            // Console.WriteLine("i = " + i);
            p = GetUnitAttyValFromProcesses(i, processModelIndexes[indexOfProcessModels+1], p);
            holderProcessList.Add(p);
        }
        // Console.WriteLine("length of indexofProcesses: " + indexOfProcesses.Count);

        foreach (Processes proc in holderProcessList)
        {
            foreach(string unitLine in proc.unitLine)
            {
                Processes p = new Processes();
                p.process = proc.process;
                p.description = proc.description;
                // Console.WriteLine(unitLine);
                string[] newline = unitLine.Split(' ');
                if (newline.Length>1)
                {
                    int j = 0;
                    for(int i=0;i<newline.Length;i++)
                    {
                        if (!string.IsNullOrEmpty(newline[i]))
                        {
                            newline[j] = newline[i];
                            j++;
                        }
                    }
                    p.units = newline[0];
                    p.attributes = newline[1];
                    p.value = newline[2];
                }
                else 
                {
                    p.units = newline[0];
                    p.attributes = " ";
                    p.value = " ";
                }

                allProcesses.Add(p);               
            }
        }

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

    static Processes GetUnitAttyValFromProcesses(int startIndex, int endIndex, Processes p)
    {
        startIndex = FindEndIndex("-----", startIndex)+1;
        int i = 0;
        while (startIndex<endIndex && !entireDocument[startIndex].Contains("Process"))
        {
            p.numberOfUnits = i;
            p.unitLine.Add(entireDocument[startIndex]);
            startIndex++;
        }

        return p;
    }
    
    static void ProcessModelingProcessTags(int indexOfEntireDocument, int indexOfProcessModels)
    {
        string line = "";
        List<int> allProcessTagIndexes = new List<int>();
        List<int> allTagNames = new List<int>();
        List<int> allDataClasses = new List<int>();
        List<int> allTagTypes = new List<int>();
        List<int> allDescriptions = new List<int>();
        for(;indexOfEntireDocument<processModelIndexes[indexOfProcessModels+1]; indexOfEntireDocument++)
        {
            line = entireDocument[indexOfEntireDocument];
            if (line.Contains("Process:"))
            {
                allProcessTagIndexes.Add(indexOfEntireDocument);
            }
            else if (line.Contains("Tag Name:"))
            {
                allTagNames.Add(indexOfEntireDocument);
            }
            else if (line.Contains("Description:"))
            {
                allDescriptions.Add(indexOfEntireDocument);
            }
            else if (line.Contains("Tag Type:"))
            {
                allTagTypes.Add(indexOfEntireDocument);
            }
            else if (line.Contains("Data Class:"))
            {
                allDataClasses.Add(indexOfEntireDocument);
            }
        }
        allProcessTagIndexes.Add(processModelIndexes[indexOfProcessModels+1]);
        for(int i=0;i<allTagNames.Count;i++)
        {
            ProcessTags t = new ProcessTags();
            int smallIndex = allProcessTagIndexes.FindLastIndex(x => x<allTagNames[i]);
            t.process = GrabStringValueFromLine(entireDocument[allProcessTagIndexes[smallIndex]]);
            t.dataClass = GrabStringValueFromLine(entireDocument[allDataClasses[i]]);
            t.description = GetDescriptionOfProcessTags(allDescriptions[i]);
            t.tagName = GrabStringValueFromLine(entireDocument[allTagNames[i]]);
            t.tagType = GrabStringValueFromLine(entireDocument[allTagTypes[i]]);

            allProcessTags.Add(t);
        }

    }

    static string GetDescriptionOfProcessTags(int indexOfEntireDocument)
    {
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

    #region segments
        static void ProcessModelingSegments(int indexOfEntireDocument, int indexOfProcessModels)
        {
            List<int> allSegIndexes = new List<int>();
            for(;indexOfEntireDocument<processModelIndexes[indexOfProcessModels+1]; indexOfEntireDocument++)
            {
                string line = entireDocument[indexOfEntireDocument];
                if (line.Contains("Segment:"))
                {
                    allSegIndexes.Add(indexOfEntireDocument);
                }
            }

            foreach(int i in allSegIndexes)
            {
                Segments s = new Segments();
                s.segment = GrabStringValueFromLine(entireDocument[i]);
                int endIndex = FindEndIndex("Process Modeling:", "Segment:", i+1);
                s.description = GetDescriptionFromSegment(i+1, endIndex);

                allSegements.Add(s);
            }
        }

        static string GetDescriptionFromSegment(int startIndex, int endIndex)
        {
            string output = " ";
            while(startIndex<endIndex)
            {
                string line = entireDocument[startIndex];
                if(line.Contains("Description:"))
                {
                    output = GrabStringValueFromLine(line);
                }
                else
                {
                    output = output + " " + line;
                }
                startIndex++;
            }

            return output;
        }
    
    static void ProcessModelingSegmentTags(int indexOfEntireDocument, int indexOfProcessModels)
    {
        string line = "";
        List<int> allProcessTagIndexes = new List<int>();
        List<int> allTagNames = new List<int>();
        List<int> allDataClasses = new List<int>();
        List<int> allTagTypes = new List<int>();
        List<int> allDescriptions = new List<int>();
        for(;indexOfEntireDocument<processModelIndexes[indexOfProcessModels+1]; indexOfEntireDocument++)
        {
            line = entireDocument[indexOfEntireDocument];
            if (line.Contains("Segment:"))
            {
                allProcessTagIndexes.Add(indexOfEntireDocument);
            }
            else if (line.Contains("Tag Name:"))
            {
                allTagNames.Add(indexOfEntireDocument);
            }
            else if (line.Contains("Description:"))
            {
                allDescriptions.Add(indexOfEntireDocument);
            }
            else if (line.Contains("Tag Type:"))
            {
                allTagTypes.Add(indexOfEntireDocument);
            }
            else if (line.Contains("Data Class:"))
            {
                allDataClasses.Add(indexOfEntireDocument);
            }
        }
        allProcessTagIndexes.Add(processModelIndexes[indexOfProcessModels+1]);
        for(int i=0;i<allTagNames.Count;i++)
        {
            SegmentTags t = new SegmentTags();
            int smallIndex = allProcessTagIndexes.FindLastIndex(x => x<allTagNames[i]);
            t.segment = GrabStringValueFromLine(entireDocument[allProcessTagIndexes[smallIndex]]);
            t.dataClass = GrabStringValueFromLine(entireDocument[allDataClasses[i]]);
            t.description = GetDescriptionOfSegmentTags(allDescriptions[i]);
            t.tagName = GrabStringValueFromLine(entireDocument[allTagNames[i]]);
            t.tagType = GrabStringValueFromLine(entireDocument[allTagTypes[i]]);

            allSegementTags.Add(t);
        }

    }

    static string GetDescriptionOfSegmentTags(int indexOfEntireDocument)
    {
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

    #region Transfers

    static void ProcessModelingTransfers(int indexOfEntireDocument, int indexOfProcessModels)
    {
        List<int> indexOfTransfers = new List<int>();
        List<int> indexOfConnections = new List<int>();

        for(;indexOfEntireDocument<processModelIndexes[indexOfProcessModels+1]; indexOfEntireDocument++)
        {
            string line = entireDocument[indexOfEntireDocument];
            if (line.Contains("Transfer:"))
            {
                indexOfTransfers.Add(indexOfEntireDocument);
            }
        }
        List<Transfers> holderTransferList = new List<Transfers>();
        foreach(int i in indexOfTransfers)
        {
            Transfers p = new Transfers();
            p.transfer = GrabStringValueFromLine(entireDocument[i]);
            p.description = GetDescriptionFromTransfers(i+3, processModelIndexes[indexOfProcessModels+1]);
            p.sourceProcessClass = GrabStringValueFromLine(entireDocument[i+1]);
            p.destProcessClass = GrabStringValueFromLine(entireDocument[i+2]);
            p = GetUnitAttyValFromTransfers(i, processModelIndexes[indexOfProcessModels+1], p);
            holderTransferList.Add(p);
        }
        // Console.WriteLine("length of indexofProcesses: " + indexOfTransfers.Count);

        foreach (Transfers proc in holderTransferList)
        {
            foreach(string transferLine in proc.connectionLine)
            {
                Transfers p = new Transfers();
                p.transfer = proc.transfer;
                p.description = proc.description;
                p.sourceProcessClass = proc.sourceProcessClass;
                p.destProcessClass = proc.destProcessClass;
                string[] newline = transferLine.Split(' ');
                if (newline.Length>1)
                {
                    int j = 0;
                    for(int i=0;i<newline.Length;i++)
                    {
                        if (!string.IsNullOrEmpty(newline[i]))
                        {
                            newline[j] = newline[i];
                            j++;
                        }
                    }
                    p.connection = newline[0];
                    p.sourceUnit = newline[1];
                    p.destinationUnit = newline[2];
                }
                else 
                {
                    p.connection = newline[0];
                    p.sourceUnit = " ";
                    p.destinationUnit = " ";
                }
                allTransfers.Add(p);               
            }
        }

    }

    static string GetDescriptionFromTransfers(int startIndex, int endIndex)
    {
        string output = "";
        while(startIndex<endIndex && !entireDocument[startIndex].Contains("Connection"))
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

    static Transfers GetUnitAttyValFromTransfers(int startIndex, int endIndex, Transfers p)
    {
        startIndex = FindEndIndex("-----", startIndex)+1;
        int i = 0;
        while (startIndex<endIndex && !entireDocument[startIndex].Contains("Transfer"))
        {
            p.numberOfConnections = i;
            p.connectionLine.Add(entireDocument[startIndex]);
            startIndex++;
        }

        return p;
    }
    
    static void ProcessModelingTransferTags(int indexOfEntireDocument, int indexOfProcessModels)
    {
        string line = "";
        List<int> allTTagIndexes = new List<int>();
        List<int> allTagNames = new List<int>();
        List<int> allDataClasses = new List<int>();
        List<int> allTagTypes = new List<int>();
        List<int> allDescriptions = new List<int>();
        for(;indexOfEntireDocument<processModelIndexes[indexOfProcessModels+1]; indexOfEntireDocument++)
        {
            line = entireDocument[indexOfEntireDocument];
            if (line.Contains("Transfer:"))
            {
                allTTagIndexes.Add(indexOfEntireDocument);
            }
            else if (line.Contains("Tag Name:"))
            {
                allTagNames.Add(indexOfEntireDocument);
            }
            else if (line.Contains("Description:"))
            {
                allDescriptions.Add(indexOfEntireDocument);
            }
            else if (line.Contains("Tag Type:"))
            {
                allTagTypes.Add(indexOfEntireDocument);
            }
            else if (line.Contains("Data Class:"))
            {
                allDataClasses.Add(indexOfEntireDocument);
            }
        }
        allTTagIndexes.Add(processModelIndexes[indexOfProcessModels+1]);
        for(int i=0;i<allTagNames.Count;i++)
        {
            TransferTags t = new TransferTags();
            int smallIndex = allTTagIndexes.FindLastIndex(x => x<allTagNames[i]);
            t.transfer = GrabStringValueFromLine(entireDocument[allTTagIndexes[smallIndex]]);
            t.dataClass = GrabStringValueFromLine(entireDocument[allDataClasses[i]]);
            t.description = GetDescriptionOfTransferTags(allDescriptions[i]);
            t.tagName = GrabStringValueFromLine(entireDocument[allTagNames[i]]);
            t.tagType = GrabStringValueFromLine(entireDocument[allTagTypes[i]]);

            allTransferTags.Add(t);
        }

    }

    static string GetDescriptionOfTransferTags(int indexOfEntireDocument)
    {
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
            string output = "";
            try {
                return FormatString(line.Split(":")[1].Trim());
            }
            catch
            {
                return line;
            }
            

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

        static void ExportFunctionProcessTags()
        {
            Console.WriteLine("Got to ProcessTags export function");
            StringBuilder sb = new StringBuilder();

            string[] header = new string[5]{"Process", "Tag Name", "Description", "Tag Type", "Data Class"};
            var headerline = string.Format("{0},{1},{2},{3},{4}", header);
            sb.AppendLine(headerline);

            foreach (ProcessTags tag in allProcessTags)
            {
                string[] obj = tag.getStringLineFormat(tag);
                var output = string.Format("{0},{1},{2},{3},{4}", obj);

                sb.AppendLine(output);
            }

            System.IO.File.WriteAllText("C:\\Users\\abazajian\\source\\repos\\CSVTest\\CSVs\\ProcessTags.csv", sb.ToString());
        }

        static void ExportFunctionSegments()
        {
            Console.WriteLine("Got to Segments export function");
            StringBuilder sb = new StringBuilder();

            string[] header = new string[2]{"Segment", "Description"};
            var headerline = string.Format("{0},{1}", header);
            sb.AppendLine(headerline);

            foreach (Segments s in allSegements)
            {
                string[] obj = s.getStringLineFormat(s);
                var output = string.Format("{0},{1}", obj);

                sb.AppendLine(output);
            }

            System.IO.File.WriteAllText("C:\\Users\\abazajian\\source\\repos\\CSVTest\\CSVs\\Segments.csv", sb.ToString());
        }

        static void ExportFunctionSegmentTags()
        {
            Console.WriteLine("Got to SegmentTags export function");
            StringBuilder sb = new StringBuilder();

            string[] header = new string[5]{"Segment", "Tag Name", "Description", "Tag Type", "Data Class"};
            var headerline = string.Format("{0},{1},{2},{3},{4}", header);
            sb.AppendLine(headerline);

            foreach (SegmentTags tag in allSegementTags)
            {
                string[] obj = tag.getStringLineFormat(tag);
                var output = string.Format("{0},{1},{2},{3},{4}", obj);

                sb.AppendLine(output);
            }

            System.IO.File.WriteAllText("C:\\Users\\abazajian\\source\\repos\\CSVTest\\CSVs\\SegmentTags.csv", sb.ToString());
        }
    
        static void ExportFunctionTransfers()
        {
            Console.WriteLine("Got to Transfers export function");
            StringBuilder sb = new StringBuilder();

            string[] header = new string[7]{"Transfer", "Source Process Class", "Dest. Process Class", "Description", "Connection", "Source Unit", "Destination Unit"};
            var headerline = string.Format("{0},{1},{2},{3},{4},{5},{6}", header);
            sb.AppendLine(headerline);

            foreach(Transfers p in allTransfers)
            {
                string[] obj = p.getStringLineFormat(p);
                var output = string.Format("{0},{1},{2},{3},{4},{5},{6}", obj);

                sb.AppendLine(output);
            }

            System.IO.File.WriteAllText("C:\\Users\\abazajian\\source\\repos\\CSVTest\\CSVs\\Transfers.csv", sb.ToString());
        }
    
        static void ExportFunctionTransferTags()
        {
            Console.WriteLine("Got to TransferTags export function");
            StringBuilder sb = new StringBuilder();

            string[] header = new string[5]{"Transfer", "Tag Name", "Description", "Tag Type", "Data Class"};
            var headerline = string.Format("{0},{1},{2},{3},{4}", header);
            sb.AppendLine(headerline);

            foreach (TransferTags tag in allTransferTags)
            {
                string[] obj = tag.getStringLineFormat(tag);
                var output = string.Format("{0},{1},{2},{3},{4}", obj);

                sb.AppendLine(output);
            }

            System.IO.File.WriteAllText("C:\\Users\\abazajian\\source\\repos\\CSVTest\\CSVs\\TransferTags.csv", sb.ToString());
        }

        static void ExportPhases(string phaseType)
        {
            Console.WriteLine(string.Format("Got to Phase export function for phaseType: {0}", phaseType));
            StringBuilder sb = new StringBuilder();
            Parameters header = new Parameters();
            header.torp = phaseType;
            header.phase = "Phase";
            header.phaseDescription = "Phase Description";
            header.parameter = "Parameter";
            header.parameterDescription = "Parameter Description";
            header.type = "Type";
            header.target = "Target";
            header.targetValue = "Target Value";
            header.actual = "Actual";
            header.highDev = "High Dev.";
            header.highDevValue = "High Dev. Value";
            header.lowDev = "Low Dev.";
            header.lowDevValue = "Low Dev. Value";
            header.highLimit = "High Limit";
            header.highLimitValue = "High Limit Value";
            header.lowLimit = "Low Limit";
            header.lowLimitValue = "Low Limit Value";
            header.preact = "Preact";
            header.preactValue = "Preact Value";
            header.lotCode = "Lot Code";
            header.matId = "Mat. Id";
            
            // string[] headerObj = header.getStringLineFormat(header);
            // // foreach(string h in headerObj){
            // //     Console.WriteLine(h);
            // // }
            // var headerLine = string.Format("{0}, {1}, {2}, {3}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}", headerObj);

            // sb.Append(headerLine);

            if (phaseType.Equals("Process"))
            {
                Console.WriteLine(allProcessPhases[0].torp.ToString());
                allProcessPhases.Insert(0, header);
                foreach(Parameters param in allProcessPhases)
                {
                    string[] obj = param.getStringLineFormat(param);
                    var output = string.Format("{0}, {1}, {2}, {3}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}", obj);

                    sb.AppendLine(output);
                }

                System.IO.File.WriteAllText("C:\\Users\\abazajian\\source\\repos\\CSVTest\\CSVs\\ProcessPhases.csv", sb.ToString());
            }
            else if (phaseType.Equals("Transfer"))
            {
                allTransferPhases.Insert(0, header);
                foreach(Parameters param in allTransferPhases)
                {
                    string[] obj = param.getStringLineFormat(param);
                    var output = string.Format("{0}, {1}, {2}, {3}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}", obj);

                    sb.AppendLine(output);
                }

                System.IO.File.WriteAllText("C:\\Users\\abazajian\\source\\repos\\CSVTest\\CSVs\\TransferPhases.csv", sb.ToString());
            }
        }
    #endregion
    



    //Class Program closing bracket
    }
}
