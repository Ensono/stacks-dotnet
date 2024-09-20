namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.UnitTests
{
    public static class DotEnv
    {
        public static void Load()
        {
            Load(".env");
        }

        private static void Load(string filePath)
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
            foreach (var envVar in vars)
            {
                Environment.SetEnvironmentVariable(envVar.Key, envVar.Value);
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
                string trimmedText = text.Trim();

                if (!string.IsNullOrWhiteSpace(trimmedText) && trimmedText[0] != ';' && trimmedText[0] != '#' && trimmedText[0] != '/')
                {
                    str = ExtractValueFromRow(trimmedText, str, row, text, dictionary);
                }
            }
            
            return dictionary;
        }

        private static string ExtractValueFromRow(string trimmedText, string str, int row, string text, Dictionary<string, string> dictionary)
        {
            if (trimmedText[0] == '[' && trimmedText[^1] == ']')
            {
                str = trimmedText.Substring(1, trimmedText.Length - 2) + ":";
            }
            else
            {
                int equalsIndex = trimmedText.IndexOf('=');
                if (equalsIndex < 0)
                {
                    throw new Exception($"Invalid format on row {row}. Found: {text}");
                }

                string key = str + trimmedText[..equalsIndex].Trim();
                string value = trimmedText[(equalsIndex + 1)..].Trim();

                if (value.Length > 1 && value[0] == '"' && value[^1] == '"')
                {
                    value = value.Substring(1, value.Length - 2);
                }

                if (!dictionary.TryAdd(key, value))
                {
                    throw new Exception($"Duplicate key at row {row}: {key}");
                }
            }

            return str;
        }
    }
}
