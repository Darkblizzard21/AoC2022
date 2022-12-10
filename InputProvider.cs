using System.Configuration;

namespace AoC2022
{
    public enum Type
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

        public string Get(Type type, int index = 0)
        {
            string basis = type switch
            {
                Type.Input => "input",
                Type.Sample => "sample",
                _ => throw new ArgumentException()
            };
            if (0 < index) basis += index;
            string file = basis + ".txt";
            return File.ReadAllText(path + file);
        }
    }
}
