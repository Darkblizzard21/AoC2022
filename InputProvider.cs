using System.Configuration;

namespace AoC2022
{
    public enum Part
    {
        First, Second
    }

    public class InputProvider
    {
        string path;

        public InputProvider(string day)
        {
            path = ConfigurationManager.AppSettings.Get("InputFolder") + "\\" + day + "\\";
        }

        public string GetInput(Part part = Part.First) => part switch
        {
            Part.First => File.ReadAllText(path + "input.txt"),
            Part.Second => File.ReadAllText(path + "input2.txt"),
            _ => throw new ArgumentException()
        };

        public string GetSample(Part part = Part.First) => part switch
        {
            Part.First => File.ReadAllText(path + "sample.txt"),
            Part.Second => File.ReadAllText(path + "sample2.txt"),
            _ => throw new ArgumentException()
        };
    }
}
