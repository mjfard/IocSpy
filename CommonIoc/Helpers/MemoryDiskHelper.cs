using System.IO;

namespace Library.CommonIoc.Helpers
{
    public static class MemoryDiskHelper
    {
        public static void CreateTextFileForce(string path, string contents)
        {
            CreateDirIfNotExists(path);
            File.Delete(path);
            File.WriteAllText(path, contents);
        }

        public static void CreateDirIfNotExists(string dir)
        {
            if (Directory.Exists(dir))
                return;

            var dirInfo = new FileInfo(dir).Directory;
            if (!dirInfo.Exists)
                dirInfo.Create();
        }

        public static void EmptyDirRecursive(string dir)
        {
            if(!Directory.Exists(dir)) return;

            var files = Directory.GetFiles(dir);
            var subDirs = Directory.GetDirectories(dir);

            foreach (var file in files)
            {
                File.Delete(file);
            }

            foreach (var subDir in subDirs)
            {
                Directory.Delete(subDir, true);
            }
        }
    }
}
