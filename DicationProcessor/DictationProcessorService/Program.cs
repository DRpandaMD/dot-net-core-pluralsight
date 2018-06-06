using System;
using System.IO;
using DictationProcessorLib;

namespace DictationProcessorService
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileSystemWatcher = new FileSystemWatcher("../Data/uploads", "metadata.json");
            fileSystemWatcher.IncludeSubdirectories = true;
            while(true)
            {
                var result = fileSystemWatcher.WaitForChanged(WatcherChangeTypes.Created);
                Console.WriteLine($"New Metadata file {result.Name}");
                var fullMetadataFilePath = Path.Combine("../Data/uploads", result.Name);
                var subfolder = Path.GetDirectoryName(fullMetadataFilePath);
                var processor = new UploadProcessor(subfolder);
                processor.Process();
            }
            
        }
    }
}
