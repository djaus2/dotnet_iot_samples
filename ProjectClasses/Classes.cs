using System;
using System.IO;

namespace ProjectClasses
{
    public class Project
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string DeviceName { get; set; }
        public string ProjectFileName { get; set; }
        public string ProjectFileName_WithoutText
        {
            get
            {
                if (ProjectFileName.Substring(ProjectFileName.Length - ".txt".Length).ToLower() == ".txt")
                    return ProjectFileName.Substring(0, ProjectFileName.Length - ".txt".Length);
                else
                    return ProjectFileName;
            }
        }
        public string ProjectCSFileName { get; set; }
        public string ProjectCSFileName_WithoutText
        { 
            get {
                if (ProjectCSFileName.Substring(ProjectCSFileName.Length - ".txt".Length).ToLower() == ".txt")
                    return ProjectCSFileName.Substring(0, ProjectCSFileName.Length - ".txt".Length);
                else
                    return ProjectCSFileName;
            } 
        }
        public string ProjectPNGFileName { get; set; }
        public int NoCSFiles { get; set; } = 1;
    }

    public class ProjectCount
    {
        public string DeviceName { get; set; }
        public int Count { get; set; } = 0;
    }
}
