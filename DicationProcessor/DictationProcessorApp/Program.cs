using System;
using System.IO;
using System.Runtime.InteropServices;
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
            
            if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                AppKitHelper.DisplayAlert(message);
            }
                
        }
    }
}
