using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectClasses;

namespace GetAnIOTSampleApp.Shared
{

    public class WeatherForecast
    {
        public static Dictionary<string, List<Project>> Projects { get; set; }
        public static List<string> Devices { get; set; }
        public static List<Tuple<string, int>> DeviceCounts {get; set;}
        public static List<IGrouping<char, KeyValuePair<string, List<Project>>>> AlphaSort { get; set; }

        public static void Init(Dictionary<string, List<Project>> projects)
        {
            Projects = projects;
            Devices =  Projects.Keys.ToList();
            DeviceCounts = (from p in Projects
                           select new Tuple<string, int>(p.Key, p.Value.Count()))
                            .ToList();
            /*List<char> apl = Projects.GroupBy(x => x.Key[0])
                         .Select(g => g.First())
                         .ToList();*/

            //IEnumerable<IGrouping<char, List<Project>>>
            //var alpha = (IEnumerable<IGrouping<char, List<Project>>>)
            //List<IGrouping<char,KeyValuePair<string, List<Project>>>>
            AlphaSort = Projects.GroupBy(x => char.ToUpper(x.Key[0]))
                .ToList();
            //AlphaSort[0]['A']

        }

        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public string Summary { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}
