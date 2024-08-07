using System;
using System.Collections.Generic;
using System.IO;

namespace Amido.Stacks.Testing.Settings
{
    public class DotEnv
    {
        public static void Load()
        {
            Load(".env");
        }

        public static void Load(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath));

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"No .env file found in path {filePath}");
                return;
            }
            var vars = Read(filePath);

            //TODOs: 
            //Handle Referenced EnvVars: MAIL_FROM_ADDRESS =${MAIL_USERNAME} or MAIL_FROM_ADDRESS=$(MAIL_USERNAME)
            foreach (var EnvVar in vars)
            {
                Environment.SetEnvironmentVariable(EnvVar.Key, EnvVar.Value);
            }
        }

        private static IDictionary<string, string> Read(string filePath)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            int row = 0;

            var lines = File.ReadAllLines(filePath);

            string str = string.Empty;
            foreach (var text in lines)
            {
                row++;

                string text2 = text.Trim();
                if (!string.IsNullOrWhiteSpace(text2) && text2[0] != ';' && text2[0] != '#' && text2[0] != '/')
                {
                    if (text2[0] == '[' && text2[text2.Length - 1] == ']')
                    {
                        str = text2.Substring(1, text2.Length - 2) + ":";
                    }
                    else
                    {
                        int num = text2.IndexOf('=');
                        if (num < 0)
                        {
                            throw new Exception($"Invalid format on row {row}. Found: {text}");
                        }
                        string text3 = str + text2.Substring(0, num).Trim();
                        string text4 = text2.Substring(num + 1).Trim();
                        if (text4.Length > 1 && text4[0] == '"' && text4[text4.Length - 1] == '"')
                        {
                            text4 = text4.Substring(1, text4.Length - 2);
                        }
                        if (dictionary.ContainsKey(text3))
                        {
                            throw new Exception($"Duplicate key at row {row}: {text3}"); //probably we should just replace the values
                        }
                        dictionary[text3] = text4;
                    }
                }
            }
            return dictionary;
        }
    }
}
