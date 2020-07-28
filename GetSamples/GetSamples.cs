using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using ProjectClasses;

namespace GetSamples
{
    public static class GetSamplesProjects
    {
        public static string GenerateTextPath = "c:\\temp\\dotnet";
        //const string DefaultPath = @"C:\Users\DavidJones\source\dotnet\djaus2-2-iot\iot\src\Devices";
        public static  string DefaultPath =  @"C:\Users\DavidJones\source\dotnet-djaus-iot\iot";
            //public static void GetSamplesList(string[] args)
            //{
            //    string path = DefaultPath;
            //    if (args.Length != 0)
            //        path = args[0];

            //    var projects = GetDict(path);
            //    var devices = projects.Keys;
            //    var devices2 = from p in projects select new Tuple<string, int>(p.Key, p.Value.Count());
            //}

            public static Dictionary<string, List<Project>> GetDict(string path, string generateTextPath)
            {

            GenerateTextPath = generateTextPath;
            if (path == "")
                path = DefaultPath;
            path += @"\src\Devices";
                var projects = GetProjects(path);
            List<Project> mediaProjects = GetProjects(Path.Combine(path, "media"));
            projects.AddRange(mediaProjects);
            System.Diagnostics.Debug.WriteLine("=======");
            System.Diagnostics.Debug.WriteLine(projects.Count());
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
            string UpperPath = System.IO.Directory.GetParent(path).FullName;
            //Remove drive from the path
            UpperPath = UpperPath.Substring(2);
            List<Project> Projects = new List<Project>();
            System.Diagnostics.Debug.WriteLine("Hello dotnet/iot-ers");
            string[] dirs = Directory.GetDirectories(
                path);

#if COPYREPOSITORY
            
            var dpath =  GenerateTextPath;
            if (Directory.Exists(dpath))
            {
                Directory.Delete(dpath,true);
            }
            Directory.CreateDirectory(GenerateTextPath);
#endif
            
            string profile = "";
            foreach (string dir in dirs)
            {
                string s1 = $"<None Update=\"{dir}\\ReadMe.md\">";
                string s2 = $"<CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>";
                string s3 = "</None>";
                string s4 = $"<None Update=\"{dir}\\samples\\*.*\" >";
                string s5 = $"<CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>";
                string s6 = "</None>";
                profile += s1 + "\r\n";
                profile += s2 + "\r\n";
                profile += s3 + "\r\n";
                profile += s4 + "\r\n";
                profile += s5 + "\r\n";
                profile += s6 + "\r\n";
#if COPYREPOSITORY
                var ddir = dir.Replace(UpperPath, GenerateTextPath);
                Directory.CreateDirectory(ddir);
                var readMeFilePath1 = Path.Combine(dir, "ReadMe.md");
                if (File.Exists(readMeFilePath1))
                {
                    string textReadMe = System.IO.File.ReadAllText(readMeFilePath1);
                    var dreadMeFilePath1 = Path.Combine(ddir, "ReadMe.md");
                    File.WriteAllText(dreadMeFilePath1, textReadMe);
                }
#endif
                string[] folders = dir.Split("\\");
                string DeviceName = folders.Last();
                System.Diagnostics.Debug.WriteLine(DeviceName);
                string[] samplesDirPaths = Directory.GetDirectories(dir, "samples");
                foreach (var sampleDirPath in samplesDirPaths)
                {
#if COPYREPOSITORY
                    var dsampleDirPath = sampleDirPath.Replace(UpperPath, GenerateTextPath);
                    Directory.CreateDirectory(dsampleDirPath);
                    bool firstProjectinDir = true;
#endif
                    string[] projFilePaths = Directory.GetFiles(sampleDirPath, "*.csproj");
                    if (projFilePaths.Length==0)
                        projFilePaths = Directory.GetFiles(sampleDirPath, "*.csproj.txt");
                     
                    foreach (string projFilePath in projFilePaths)
                    {

                        // Get Project Name
                        var projFileName = Path.GetFileName(projFilePath);
                        string ProjectName = Path.GetFileNameWithoutExtension(projFilePath);
                        if (ProjectName.Contains(".csproj"))
                            ProjectName = Path.GetFileNameWithoutExtension(ProjectName);

                        // Get number of source files per project
                        //In same cases there are more than one projects in samples, eg Bmxx80
                        string[] csFiles = Directory.GetFiles(sampleDirPath, "*.cs");
                        int NoCSFiles = csFiles.Length / projFilePaths.Length; //A bit hueristic here!
                        if (NoCSFiles==0)
                        {
                            string[] csFiles2 = Directory.GetFiles(sampleDirPath, "*.cs.txt");
                            NoCSFiles = csFiles2.Length / projFilePaths.Length;
                        };

#if COPYREPOSITORY
                        // Copy all of sample folder to target, but only one for each Device
                        // Multiple projects in samples for some devices
                        if (firstProjectinDir)
                        {
                            firstProjectinDir = false;
                            string[] csFilePathsSamples = Directory.GetFiles(sampleDirPath, "*.*");
                            foreach (string src in csFilePathsSamples)
                            {
                                string dest = src.Replace(UpperPath, GenerateTextPath);
                                string extn = Path.GetExtension(dest);
                                if ((extn == ".cs") || (extn == ".csproj"))
                                {
                                    dest += ".txt";
                                }
                                File.Copy(src, dest);
                            }
                        }
#endif
                        // Find the source file (Can be .cs or .cs.txt)
                        var filenameCS = ProjectName + ".cs";

                        string[] csFilePaths = Directory.GetFiles(sampleDirPath, filenameCS);
                        if (csFilePaths.Length == 0) 
                        { 
                            csFilePaths = Directory.GetFiles(sampleDirPath, filenameCS + ".txt");
                            if (csFilePaths.Length == 0)
                            {
                                //This next allows for: Some project names have s appended to make the source file names.
                                var filename2CS = ProjectName + "s.cs";
                                csFilePaths = Directory.GetFiles(sampleDirPath, filename2CS);
                                if (csFilePaths.Length == 0)
                                {
                                    csFilePaths = Directory.GetFiles(sampleDirPath, filename2CS + ".txt");
                                    if (csFilePaths.Length == 0)
                                    {
                                        //This next allows for: Some project names have s removed to make the source file names.
                                        var filename4CS = ProjectName.Substring(0, ProjectName.Length - 1) + ".cs";
                                        csFilePaths = Directory.GetFiles(sampleDirPath, filename4CS);
                                        if (csFilePaths.Length == 0)
                                        {
                                            csFilePaths = Directory.GetFiles(sampleDirPath, filename4CS + ".txt");
                                            if (csFilePaths.Length == 0)
                                            {
                                                // Might be program.cs
                                                csFilePaths = Directory.GetFiles(sampleDirPath, "Program.cs");
                                                if (csFilePaths.Length == 0)
                                                {
                                                    csFilePaths = Directory.GetFiles(sampleDirPath, "Program.cs" + ".txt");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (csFilePaths.Length != 0)
                        {
                            string pngFileName = "";
                            string[] images = Directory.GetFiles(sampleDirPath, "*.png");
                            if (images.Length > 0)
                            {
                                //Assuming there is only one .png file in the directory
                                var imgFile = images.FirstOrDefault();
                                pngFileName = Path.GetFileNameWithoutExtension(images.FirstOrDefault()) + ".png";
                            }
                            else
                                pngFileName = "";


                            string csFilePath = csFilePaths.FirstOrDefault();
                            if (!string.IsNullOrEmpty(csFilePath))
                            {
                                var csFileName = Path.GetFileName(csFilePath);

                                System.Diagnostics.Debug.WriteLine(ProjectName);
                                Projects.Add(
                                new Project
                                {
                                    Name = ProjectName,
                                    DeviceName = DeviceName,
                                    Path = sampleDirPath,
                                    ProjectCSFileName = csFileName,
                                    ProjectFileName = projFileName,
                                    ProjectPNGFileName = pngFileName,
                                    NoCSFiles = NoCSFiles
                                });
                            }

                        }
                    }
                }
            }
            // The following generates content to include samples in server project when deployed as content.
            System.Diagnostics.Debug.WriteLine("Use this is the server project file as an <itemgroup></itemgroup>");
            System.Diagnostics.Debug.WriteLine("Path will need modification");
            System.Diagnostics.Debug.WriteLine("<itemgroup>");
            System.Diagnostics.Debug.WriteLine(profile);
            System.Diagnostics.Debug.WriteLine("</itemgroup>");
            return Projects;
        }
    }
}

