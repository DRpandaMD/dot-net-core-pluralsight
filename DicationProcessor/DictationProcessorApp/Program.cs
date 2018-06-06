using System;
using System.IO;
using DictationProcessorLib;

namespace DictationProcessorApp
{
    class Program
    {
        static void Main(string[] args)
        {
            int i = 0;

            foreach( var subfolder in Directory.GetDirectories("../Data/uploads"))
            {
                var uploadProcessor = new UploadProcessor(subfolder);
                uploadProcessor.Process();
                i++;
            }

            var message = $"{i} uploads were processed";
            AppKitHelper.DisplayAlert(message);
                
        }
    }
}
