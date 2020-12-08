using System.IO;

namespace GenerateSqlScripts
{
    public static class FileWriter
    {
        public static void SaveFileInCurrentDirectory(string fileName, string content)
        {
            using var file = new StreamWriter(Directory.GetCurrentDirectory() + $"\\{fileName}");
            
            file.WriteLine(content);
        }
    }
}