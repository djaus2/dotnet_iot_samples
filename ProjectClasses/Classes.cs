using System;

namespace ProjectClasses
{
    public class Project
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string DeviceName { get; set; }
        public string ProjectFileName { get; set; }
        public string ProjectCSFileName { get; set; }
        public string ProjectPNGFileName { get; set; }
        public int NoCSFiles { get; set; } = 1;
    }

    public class ProjectCount
    {
        public string DeviceName { get; set; }
        public int Count { get; set; } = 0;
    }
}
