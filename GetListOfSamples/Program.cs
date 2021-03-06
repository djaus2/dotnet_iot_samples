﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GetListOfSamples
{
    class Program
    {
        public class Project
        {
            public string Name { get; set; }
            public string Path { get; set; }
            public string DeviceName { get; set; }
            public string ProjectFileName { get; set; }
            public string ProjectCSFileName { get; set; }
        }

        public class ProjectCount
        {
            public string DeviceName { get; set; }
            public int Count { get; set; } = 0;
        }

        static void Main(string[] args)
        {
            //var projects = GetDict(@"C:\Users\DavidJones\source\dotnet\djaus2-2-iot\iot\src\Devices");
            var projects =GetDict( @"C:\Users\DavidJones\source\dotnet-djaus-iot\iot");
            var devices = projects.Keys;
            var devices2 = from p in projects select new Tuple<string,int>( p.Key, p.Value.Count() );
        }

        static Dictionary<string, List<Project>> GetDict(string path)
        {            
            
            var projects = GetProjects(path);
            var devices = projects.GroupBy(x => x.DeviceName)
                                .Select(g => g.First().DeviceName)
                                .ToList();

            //var devices2 = projects.GroupBy(x => x.DeviceName);

            Dictionary<string, List<Project>> myDictionary = projects
                .GroupBy(o => o.DeviceName)
                .ToDictionary(g => g.Key, g => g.ToList());
            return myDictionary;
        }

        static List<Project> GetProjects (string path)
        { 
            List<Project> Projects = new List<Project>();
            Console.WriteLine("Hello World!");
            string[] dirs = Directory.GetDirectories(
                path);

            foreach (string dir in dirs)
            {
                string[] folders = dir.Split("\\");
                string DeviceName = folders.Last();
                Console.WriteLine(DeviceName);
                string[] samplesDirPaths = Directory.GetDirectories(dir, "samples");
                foreach (var sampleDirPath in samplesDirPaths)
                {
                    string[] projFilePaths = Directory.GetFiles(sampleDirPath, "*.csproj");
                    foreach (string projFilePath in projFilePaths)
                    {
                        var projFileName = Path.GetFileName(projFilePath);
                        string ProjectName = Path.GetFileNameWithoutExtension(projFilePath);
                        var filenameCS = ProjectName + ".cs";
                  
                        string[] csFilePaths = Directory.GetFiles(sampleDirPath, filenameCS);
                        if (csFilePaths.Length != 0)
                        {
                            foreach (string csFilePath in csFilePaths)
                            {
                                var csFileName = Path.GetFileName(csFilePath);
                                Console.WriteLine(ProjectName);
                                Console.WriteLine(sampleDirPath);
                                Console.WriteLine(projFileName);
                                Console.WriteLine(csFileName);

                                Projects.Add(
                                new Project
                                {
                                    Name = ProjectName,
                                    DeviceName = DeviceName,
                                    Path = sampleDirPath,
                                    ProjectCSFileName = projFileName,
                                    ProjectFileName = csFileName //Should be Program.cs
                                });
                            }
                        }
                        else
                        {
                            string[] csFilePaths2 = Directory.GetFiles(sampleDirPath, "Program.cs");
                            if (csFilePaths2.Length != 0)
                            {
                                foreach (string csFilePath2 in csFilePaths2)
                                {
                                    var csFileName2 = Path.GetFileName(csFilePath2);
                                    Console.WriteLine(ProjectName);
                                    Console.WriteLine(sampleDirPath);
                                    Console.WriteLine(projFileName);
                                    Console.WriteLine(csFileName2);

                                    Projects.Add(
                                    new Project {
                                        Name = ProjectName,
                                        DeviceName = DeviceName,
                                        Path = sampleDirPath,
                                        ProjectCSFileName = projFileName,
                                        ProjectFileName = csFileName2 //Should be Program.cs
                                    });
                                }
                            }
                        }

                    }
                }      
            }
            return Projects;
        }
    }
}
