using System;
using System.IO;
using System.Text;
using System.Collections.Generic;


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
            foreach (int processModelIndex in processModelIndexes)
            {
                string header = GrabStringValueFromLine(entireDocument[processModelIndex]);
                switch(header)
                {
                    case "Units":
                        ProcessModelingUnits(processModelIndex, i);
                        break;
                    case "Unit Tags":
                        ProcessModelingUnitTags(processModelIndex, i);
                        break;
                    case "Connections":
                        // ProcessModelingUnits();
                        break;
                    case "Connection Tags":
                        // ProcessModelingUnits();
                        break;
                    case "Enumerations":
                        // ProcessModelingUnits();
                        break;
                    case "Processes":
                        // ProcessModelingUnits();
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

            SetupFunction();

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
        static string GetDescription(string unitName)
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
    #endregion

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

        // static int GetUnitAndDecriptionIndex(string unitName)
        // {
        //     int indexOfUnit;
        //     foreach (string[] line in unitAndDescription)
        //     {
        //         if (line[0].Equals(unitName))
        //         {
        //             indexOfUnit = unitAndDescription.IndexOf(line);
        //             return indexOfUnit;
        //         }
        //     }
        //     return 0;

        // }

        static string GrabStringValueFromLine(string line)
        {
            return FormatString(line.Split(":")[1]);
        }

    #endregion


    #region final export
        static void SetupFunction()
        {
            StringBuilder sb = new StringBuilder();
            string[] introLine = new string[5]{"Unit", "Description", "Tag Name", "Tag Type", "Data Class"};
            var fir = introLine[0];
            var sec = introLine[1];
            var thi = introLine[2];
            var fou = introLine[3];
            var fif = introLine[4];

            var line = (string.Format("{0},{1},{2},{3},{4}", fir, sec, thi, fou, fif));
            sb.AppendLine(line);

            Console.WriteLine("count of unit and whatever: " + unitAndTagAndTypeAndData.Count);

            for (int j = 0; j<unitAndTagAndTypeAndData.Count-1; j++)
            {
                
                string[] rightline = unitAndTagAndTypeAndData[j];
                var newLine = string.Format("{0},{1},{2},{3},{4}", rightline[0], GetDescription(rightline[0]), rightline[1], rightline[2], rightline[3]);
                sb.AppendLine(newLine);

            }
            System.IO.File.WriteAllText("C:\\Users\\abazajian\\source\\repos\\CSVTest\\asd.csv", sb.ToString());
        }
    #endregion
    }

    
}
