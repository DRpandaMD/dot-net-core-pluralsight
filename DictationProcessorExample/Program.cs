using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace DictationProcessorExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // iterate through subfolder of '/mnt/uploads
            foreach( var subfolder in Directory.GetDirectories("../Data/uploads"))
            {
                // get metadata file
                var metadataFilePath = Path.Combine(subfolder, "metadata.json");
                Console.WriteLine($"Reading {metadataFilePath}");
                
                var metadataCollection = GetMetadata(metadataFilePath);

                // for each audio file listed in metadata::
                foreach (var metadata in metadataCollection)
                {
                    // - get aboslute file path
                    var audioFilepath = Path.Combine(subfolder, metadata.File.FileName);
                    // - verify file checksum
                    // - generate a unique identifier
                    // - compress it
                    // - create a standalone metadata file
                }
                
            }

            
        }

        static string GetCheckSum(string filePath)
        {
            var fileStream = File.Open(filePath, FileMode.Open);
            var md5 = System.Security.Cryptography.MD5.Create();
            var md5Bytes = md5.ComputeHash(fileStream);
            return BitConverter.ToString(md5Bytes);
        }

        static List<Metadata> GetMetadata(string metadataFilePath)
        {
            // extract metadata, including audio info, from metadata file
                // -- open file stream
                var metadataFileStream = File.Open(metadataFilePath, FileMode.Open);
                var settings = new DataContractJsonSerializerSettings
                {
                    DateTimeFormat = new DateTimeFormat("yyyy-MM-dd'T'HH:mm:ssz")
                };
                // -- set up serializer
                var serializer = new DataContractJsonSerializer(typeof(List<Metadata>), settings);
                // here we will call our serializer object to read our filestream and cast it to our list
                 return (List<Metadata>)serializer.ReadObject(metadataFileStream);
        }
    }
}
