using System.Configuration;
using System.Collections.Specialized;

namespace AoC2022
{
    public class InputProvider
    {
        string path;

        public InputProvider(string day)
        {
            path = ConfigurationManager.AppSettings.Get("InputFolder") + "\\" + day + "\\";
        }

        public string GetInput()
        {
            return File.ReadAllText(path + "input.txt");
        }
    }
}
