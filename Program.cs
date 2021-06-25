using System;
using System.IO;
using System.Text;


namespace CSVTest
{
    class Program
    {
        static StreamReader sr = new StreamReader("C:\\Users\\abazajian\\source\\repos\\CSVTest\\UpstreamModel_Val_22JUN2021.txt");
        static void Main()
        {
            string line;
            string header;
            while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains("Process Modeling:"))
                    {
                        string[] split1 = line.Split(":");
                        header = FormatString(split1[1]);
                        if (header.Equals("Units"))
                        {
                            ProcessModelingUnits();
                        }

                        // if header.equals Unit Tags make new method for this
                        if (header.Equals("Unit Tags"))
                        {
                            ProcessModelingUnitTags();
                        }
                    }
                    
                }
            
        }

        static void ProcessModelingUnits()
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                string[] introLine = new string[5]{"Unit", "Description", "Tag Name", "Tag Type", "Data Class"};
                var fir = introLine[0];
                var sec = introLine[1];
                var thi = introLine[2];
                var fou = introLine[3];
                var fif = introLine[4];

                var newLine = string.Format("{0},{1},{2},{3},{4}", fir, sec, thi, fou, fif);

                sb.AppendLine(newLine);
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains("Unit:"))
                    {
                        sb.AppendLine(LookForUnitAndDescription(line));
                    }
                    else if (line.Contains("Process Modeling: Unit Tags"))
                    {
                        return;
                    }
                }

                System.IO.File.WriteAllText("C:\\Users\\abazajian\\source\\repos\\CSVTest\\asd.csv", sb.ToString());
                Console.WriteLine("Im Here");
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
        }

        static void ProcessModelingUnitTags()
        {

        }

        static string LookForUnitAndDescription(string line)
        {
            string[] split1 = line.Split("Unit:");
            string first = FormatString(split1[1]);
            string second = " ";
            bool nextlinebool = false;

            string nextLine;
            while ((nextLine = sr.ReadLine()) != string.Empty)
            {
                if (nextLine.Contains("Description:"))
                {
                    string[] split2 = nextLine.Split("Description:");
                    second = FormatString(split2[1]);

                    nextlinebool = true;
                }
                else if (nextlinebool)
                {
                    second = FormatString(second) + FormatString(nextLine);
                    ;
                }
                else
                {
                    Console.WriteLine("You shouldn't end up here. something is weird with this description");
                    nextlinebool = false;
                } 
                
            }

            var output = string.Format("{0},{1}", first, second);
            return output;
        }

        static string FormatString(string lineUnformatted)
        {
            string formattedLine;
            try
            {
                lineUnformatted = lineUnformatted.Replace("," , "");
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

    }

    
}
