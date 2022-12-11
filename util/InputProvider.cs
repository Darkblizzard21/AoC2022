using System.Configuration;

namespace AoC2022.util
{
    public enum InputType
    {
        Sample, 
        Input
    }

    public class InputProvider
    {
        string path;

        public InputProvider(string day)
        {
            path = ConfigurationManager.AppSettings.Get("InputFolder") + "\\" + day + "\\";
        }

        public string Get(InputType type, int index = 0)
        {
            string basis = type switch
            {
                InputType.Input => "input",
                InputType.Sample => "sample",
                _ => throw new ArgumentException()
            };
            if (0 < index) basis += index;
            string file = basis + ".txt";
            return File.ReadAllText(path + file);
        }
    }
}
