using System.Collections;
using System.Collections.Generic;

public static class BuildSettings
{
    // build modes which are intended for development
    public static readonly List<BuildMode> DevelopmentBuildModes = new List<BuildMode>() { BuildMode.FreeDevelopment, BuildMode.PremiumDevelopment };

    // premium build modes
    public static readonly List<BuildMode> PremiumBuildModes = new List<BuildMode>() { BuildMode.PremiumDevelopment, BuildMode.Premium };

    // the current product version number
    public static ProductVersion CurrentProductVersion = new ProductVersion(1, 0, 0, 819);

    // the current build mode
    public static BuildMode CurrentBuildMode = BuildMode.Free;  // REMARK: You have to recompile the project for the changes to take effect
    
    // available build modes
    public enum BuildMode
    {
        FreeDevelopment, PremiumDevelopment, Free, Premium
    }

    // product version class
    public struct ProductVersion
    {
        // substantial changes
        public int MajorRelease;

        // function extensions or improvements 
        public int MinorRelease;

        // bugfixes 
        public int PatchLevel;

        // version control system commit number  (for git: git rev-list --all --count)
        public int BuildNumber;

        // ctor
        public ProductVersion(int majorRelease, int minorRelease, int patchLevel, int buildNumber)
        {
            MajorRelease = majorRelease;
            MinorRelease = minorRelease;
            PatchLevel = patchLevel;
            BuildNumber = buildNumber;
        }

        public override string ToString()
        {
            return string.Format("{0}.{1}.{2}.{3}", MajorRelease, MinorRelease, PatchLevel, BuildNumber);
        }

        // compare with another product version
        public int CompareTo(ProductVersion other)
        {
            if (MajorRelease != other.MajorRelease)
            {
                return MajorRelease.CompareTo(other.MajorRelease);
            }
            else if (MinorRelease != other.MinorRelease)
            {
                return MinorRelease.CompareTo(other.MinorRelease);
            }
            else if (PatchLevel != other.PatchLevel)
            {
                return PatchLevel.CompareTo(other.PatchLevel);
            }
            else if (BuildNumber != other.BuildNumber)
            {
                return BuildNumber.CompareTo(other.BuildNumber);
            }
            else
            {
                return 0;
            }
        }       
    }
}
