using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LoopAr.DataLayer.DeSerialization
{
     public static class CsvDeSerializer
    {
        private const char CsvSeparator = ',';

        private const char CsvReplacement = '.';

        //TODO: maybe this to bool-method to get the feedback storing failed
        public static void WriteCSVFile(List<string[]> csvInput, string storePath, string fileName, string fileAppendix)
        {
            List<string> combinedCsvLines = new List<string>();
            foreach (string[] lineGroup in csvInput)
            {
                string[] replacedStrings = new string[lineGroup.Length];
                for (int i = 0; i < lineGroup.Length; i++)
                {
                    if (lineGroup[i] != null)
                        replacedStrings[i] = lineGroup[i].Replace(CsvSeparator, CsvReplacement);
                    else
                        replacedStrings[i] = lineGroup[i];
                }

                combinedCsvLines.Add(String.Join(CsvSeparator.ToString(), replacedStrings));
            }

            try
            {
                if (!Directory.Exists(storePath))
                {
                    Directory.CreateDirectory(storePath);
                }

                using (FileStream fileStream =
                    new FileStream(storePath + fileName + fileAppendix,
                        FileMode.Create))
                {
                    using (StreamWriter csvWriter =
                        new StreamWriter(fileStream))
                    {
                        foreach (string csvLine in combinedCsvLines)
                        {
                            csvWriter.WriteLine(csvLine);
                        }

                        csvWriter.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }


        public static void ReadCSVFile(string filePath, ref List<string[]> csvReadedFile)
        {
            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                {
                    using (StreamReader csvReader = new StreamReader(fileStream))
                    {
                        while (!csvReader.EndOfStream)
                        {
                            string readLine = csvReader.ReadLine();
                            string[] strings = readLine.Split(CsvSeparator);
                            csvReadedFile.Add(strings);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }
    }
}