using System;

namespace ConDep.Dsl.Config
{
    [Serializable]
    public class DiskInfo
    {
        public string DeviceId { get; set; }
        public long SizeInKb { get; set; }
        public long FreeSpaceInKb { get; set; }
        public string Name { get; set; }
        public string FileSystem { get; set; }
        public string VolumeName { get; set; }

        public long SizeInMb { get { return SizeInKb / 1024; } }
        public int SizeInGb { get { return Convert.ToInt32(SizeInMb) / 1024; } }
        public int SizeInTb { get { return Convert.ToInt32(SizeInGb) / 1024; } }

        public long FreeSpaceInMb { get { return FreeSpaceInKb / 1024; } }
        public int FreeSpaceInGb { get { return Convert.ToInt32(FreeSpaceInMb) / 1024; } }
        public int FreeSpaceInTb { get { return Convert.ToInt32(FreeSpaceInGb) / 1024; } }

        public long UsedInKb { get { return SizeInKb - FreeSpaceInKb; } }
        public long UsedInMb { get { return UsedInKb / 1024; } }
        public int UsedInGb { get { return Convert.ToInt32(UsedInMb) / 1024; } }
        public int UsedInTb { get { return Convert.ToInt32(UsedInGb) / 1024; } }

        public int PercentUsed { get { return SizeInKb == 0 ? 0 : Convert.ToInt32(UsedInKb * 100 / SizeInKb); } }
        public int PercentFree { get { return SizeInKb == 0 ? 0 : 100 - PercentUsed; } }
    }
}