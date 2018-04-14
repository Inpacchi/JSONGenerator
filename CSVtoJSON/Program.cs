using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace CSVtoJSON
{
    internal class Program
    {
        private static CardAttributes addSupportType(string[] values, string type)
        {
            var name = values[3];
            var attributes = values[4].Split('/');
            var description = values[5];

            var resistance = int.Parse(attributes[0]);
            var morale = int.Parse(attributes[1]);
            
            return new CardAttributes(name, type, resistance, morale, description);
        }
        
        private static CardAttributes addTroopType(string[] values, string type)
        {
            var name = values[3];
            var attributes = values[4].Split('/');
            var description = values[5];

            var attack = int.Parse(attributes[0]);
            var resistance = int.Parse(attributes[1]);
            var morale = int.Parse(attributes[2]);
            
            return new CardAttributes(name, type, resistance, morale, description);
        }
        
        private static CardAttributes addCommanderType(string[] values, string type, string troopName)
        {
            var name = values[3];
            var description = values[5];
            var initiative = int.Parse(values[4]);
            var link = troopName;
            
            return new CardAttributes(name, type, initiative, description, link);
        }
        
        private static CardAttributes addOfficerType(string[] values, string type)
        {
            var name = values[3];
            var description = values[5];
            
            return new CardAttributes(name, type, description);
        }

        private static void serializeJson(List<CardAttributes> cardList)
        {
            var setting = new JsonSerializerSettings();
            setting.DefaultValueHandling = DefaultValueHandling.Ignore;
            
            var jsonOutput = JsonConvert.SerializeObject(cardList, Formatting.Indented, setting);
            
            File.WriteAllText(saveToFile(), jsonOutput);
        }

        private static StreamReader readFromFile()
        {
            using (var fileDialog = new OpenFileDialog())
            {
                fileDialog.Title = "Select a .tsv file";
            
                fileDialog.ShowDialog();

                var fileName = fileDialog.FileName;
            
                return new StreamReader(fileName);
            }
            
        }

        private static string saveToFile()
        {
            using (var fileDialog = new SaveFileDialog())
            {
                fileDialog.Filter = "JSON (*.json)|*.json";
                fileDialog.Title = "Choose a location to save the .json file to";
            
                MessageBox.Show("Choose a location to save the .json file to.");
                fileDialog.ShowDialog();

                return fileDialog.FileName;
            }
        }

        [STAThread]
        public static void Main(string[] args)
        {
            // Prompt the user for a .tsv file input.
            MessageBox.Show("Welcome to my JSON generator, made specifically for Throne!\n\n" +
                            "Select a .tsv file. ");
            var tsvReader = readFromFile();
            
            // This is where we will store all of our formatted cards in JSON format.
            List<CardAttributes> cardList = new List<CardAttributes>();
            
            while (!tsvReader.EndOfStream)
            {
                var line = tsvReader.ReadLine();
                
                /* If the line begins with XX/XX where X is some number between 0 and 9, then we know it is a date, and
                   lines that begin with a date are the lines where there is a card. */
                if (Regex.IsMatch(line, @"(\d{2}/){2}"))
                {
                    /* This is the reason we use a .tsv file (tab-delimitted) as we can easily discern where the tabs
                     are in the file by using the .Split() method. We store each value of the line into this lineValues
                     array, which does consist of some values we don't need. That's why we set whatever values we need 
                     in our specific if statements. */
                    var lineValues = line.Split('\t');
                    var type = lineValues[2];
                    
                    // Now we check what kind of card object we are creating.
                    if (type.Equals("Support") || type.Equals("Heroic Support"))
                    {
                        cardList.Add(addSupportType(lineValues, type));
                    } 
                    else if (type.Equals("Officer") || type.Equals("Heroic Officer"))
                    {
                        cardList.Add(addOfficerType(lineValues, type));
                    } 
                    else if (type.Equals("Troop"))
                    {
                        cardList.Add(addTroopType(lineValues, type));
                    } 
                    else if (type.Equals("Commander"))
                    {
                        var troop = tsvReader.ReadLine().Split('\t');
                        
                        cardList.Add(addCommanderType(lineValues, type, troop[3]));
                        cardList.Add(addTroopType(troop, "Troop"));
                    }
                }
            }
            serializeJson(cardList);
            Console.WriteLine("Generation finished.");
        }
    }
}