using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProjectClasses;

namespace GetSamples
{
    public static class GetSamplesProjects
    {
        //const string DefaultPath = @"C:\Users\DavidJones\source\dotnet\djaus2-2-iot\iot\src\Devices";
        public static  string DefaultPath =  @"C:\Users\DavidJones\source\dotnet-djaus-iot\iot";
            public static void GetSamplesList(string[] args)
            {
                string path = DefaultPath;
                if (args.Length != 0)
                    path = args[0];

                var projects = GetDict(path);
                var devices = projects.Keys;
                var devices2 = from p in projects select new Tuple<string, int>(p.Key, p.Value.Count());
            }

            public static Dictionary<string, List<Project>> GetDict(string path)
            {
            if (path == "")
                path = DefaultPath;
            path += @"\src\Devices";
                var projects = GetProjects(path);
                var devices = projects.GroupBy(x => x.DeviceName)
                                    .Select(g => g.First().DeviceName)
                                    .ToList();

                Dictionary<string, List<Project>> myDictionary = projects
                    .GroupBy(o => o.DeviceName)
                    .ToDictionary(g => g.Key, g => g.ToList());
                return myDictionary;
            }

            static List<Project> GetProjects(string path)
            {
                List<Project> Projects = new List<Project>();
            System.Diagnostics.Debug.WriteLine("Hello dotnet/iot-ers");
            string[] dirs = Directory.GetDirectories(
                path);

            foreach (string dir in dirs)
            {
                string[] folders = dir.Split("\\");
                string DeviceName = folders.Last();
                System.Diagnostics.Debug.WriteLine(DeviceName);
                string[] samplesDirPaths = Directory.GetDirectories(dir, "samples");
                foreach (var sampleDirPath in samplesDirPaths)
                {
                    string[] projFilePaths = Directory.GetFiles(sampleDirPath, "*.csproj");
                    foreach (string projFilePath in projFilePaths)
                    {
                        string[] csFiles = Directory.GetFiles(sampleDirPath, "*.cs");

                        int NoCSFiles = csFiles.Length / projFilePaths.Length; //A bit hueristic here!

                        var projFileName = Path.GetFileName(projFilePath);
                        string ProjectName = Path.GetFileNameWithoutExtension(projFilePath);
                        var filenameCS = ProjectName + ".cs";
                        string[] csFilePaths = Directory.GetFiles(sampleDirPath, filenameCS);
                        //This next allows for: Some project names have s appended to make the source file names.
                        var filename2CS = ProjectName + "s.cs";
                        string[] csFilePaths2 = Directory.GetFiles(sampleDirPath, filename2CS);
                        //This next allows for: Some project names have s removed to make the source file names.
                        var filename4CS = ProjectName.Substring(0, ProjectName.Length - 1) + ".cs";
                        string[] csFilePaths4 = Directory.GetFiles(sampleDirPath, filename4CS);

                        string pngFileName = "";
                        string[] images = Directory.GetFiles(sampleDirPath, "*.png");
                        if (images.Length > 0)
                        {
                            //Assuming there is only one .png file in the directory
                            pngFileName = Path.GetFileNameWithoutExtension(images.FirstOrDefault()) + ".png";
                        }
                        else
                            pngFileName = "";

                        if (csFilePaths.Length != 0)
                        {
                            foreach (string csFilePath in csFilePaths)
                            {
                                var csFileName = Path.GetFileName(csFilePath);
                                System.Diagnostics.Debug.WriteLine(ProjectName);
                                //System.Diagnostics.Debug.WriteLine(sampleDirPath);
                                //System.Diagnostics.Debug.WriteLine(projFileName);
                                //System.Diagnostics.Debug.WriteLine(csFileName);
                                //System.Diagnostics.Debug.WriteLine(pngFileName);


                                Projects.Add(
                                new Project
                                {
                                    Name = ProjectName,
                                    DeviceName = DeviceName,
                                    Path = sampleDirPath,
                                    ProjectCSFileName = csFileName,
                                    ProjectFileName = projFileName, //Should be Program.cs
                                    ProjectPNGFileName = pngFileName,
                                    NoCSFiles = NoCSFiles
                                }) ;
                            }
                        }
                        else if (csFilePaths2.Length != 0)
                        {
                            //This allows for: Some project names have s appended to make the source file names.
                            foreach (string csFilePath2 in csFilePaths2)
                            {
                                var csFileName2 = Path.GetFileName(csFilePath2);
                                System.Diagnostics.Debug.WriteLine(ProjectName);
                                //System.Diagnostics.Debug.WriteLine(sampleDirPath);
                                //System.Diagnostics.Debug.WriteLine(projFileName);
                                //System.Diagnostics.Debug.WriteLine(csFileName2);
                                //System.Diagnostics.Debug.WriteLine(pngFileName);
                                Projects.Add(
                                new Project
                                {
                                    Name = ProjectName,
                                    DeviceName = DeviceName,
                                    Path = sampleDirPath,
                                    ProjectCSFileName = csFileName2,
                                    ProjectFileName = projFileName, //Should be Program.cs
                                    ProjectPNGFileName = pngFileName,
                                    NoCSFiles = NoCSFiles
                                });
                            }
                        }
                        else if (csFilePaths4.Length != 0)
                        {
                            //This allows for: Some project names have s appended to make the source file names.
                            foreach (string csFilePath4 in csFilePaths4)
                            {
                                var csFileName4 = Path.GetFileName(csFilePath4);
                                System.Diagnostics.Debug.WriteLine(ProjectName);
                                //System.Diagnostics.Debug.WriteLine(sampleDirPath);
                                //System.Diagnostics.Debug.WriteLine(projFileName);
                                //System.Diagnostics.Debug.WriteLine(csFileName2);
                                //System.Diagnostics.Debug.WriteLine(pngFileName);
                                Projects.Add(
                                new Project
                                {
                                    Name = ProjectName,
                                    DeviceName = DeviceName,
                                    Path = sampleDirPath,
                                    ProjectCSFileName = csFileName4,
                                    ProjectFileName = projFileName, //Should be Program.cs
                                    ProjectPNGFileName = pngFileName,
                                    NoCSFiles = NoCSFiles
                                });
                            }
                        }
                        else
                        {
                            string[] csFilePaths3 = Directory.GetFiles(sampleDirPath, "Program.cs");
                            if (csFilePaths3.Length != 0)
                            {
                                foreach (string csFilePath3 in csFilePaths3)
                                {
                                    var csFileName3 = Path.GetFileName(csFilePath3);
                                    System.Diagnostics.Debug.WriteLine(ProjectName);
                                    //System.Diagnostics.Debug.WriteLine(sampleDirPath);
                                    //System.Diagnostics.Debug.WriteLine(projFileName);
                                    //System.Diagnostics.Debug.WriteLine(csFileName3);
                                    //System.Diagnostics.Debug.WriteLine(pngFileName);

                                    Projects.Add(
                                    new Project
                                    {
                                        Name = ProjectName,
                                        DeviceName = DeviceName,
                                        Path = sampleDirPath,
                                        ProjectCSFileName = csFileName3,
                                        ProjectFileName = projFileName, //Should be Program.cs
                                        ProjectPNGFileName = pngFileName,
                                        NoCSFiles = NoCSFiles
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

