using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO.Compression;

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
                    var md5Checksum = GetCheckSum(audioFilepath);
                    if (md5Checksum.Replace("-", "").ToLower() != metadata.File.Md5Checksum)
                    {
                        throw new Exception("checksum not validated! File corrupted");
                    }
                    // - generate a unique identifier
                    var uniqueID = Guid.NewGuid();
                    metadata.File.FileName = uniqueID + ".WAV";
                    var newpath = Path.Combine("../Data/ready_for_transcription", uniqueID + ".WAV");
                    // - compress it
                    CreateCompressedFile(audioFilepath, newpath);
                    // - create a standalone metadata file
                    SaveSingleMetadata(metadata, newpath + ".json");
                }
            }
        }

        static void CreateCompressedFile(string inputFilePath, string outputFilePath)
        {   
            // add '.gz' to reflect the compressed file
            outputFilePath += ".gz"; 
            // console output to track status
            Console.WriteLine($"Creating {outputFilePath}");

            // create our IO file streams
            var inputFileStream = File.Open(inputFilePath, FileMode.Open);
            var outputFileStream = File.Create(outputFilePath);
            var gzipStream = new GZipStream(outputFileStream, CompressionLevel.Optimal);
            inputFileStream.CopyTo(gzipStream);

        }

        static string GetCheckSum(string filePath)
        {
            var fileStream = File.Open(filePath, FileMode.Open);
            var md5 = System.Security.Cryptography.MD5.Create();
            var md5Bytes = md5.ComputeHash(fileStream);
            fileStream.Close();
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

        static void SaveSingleMetadata(Metadata metadata, string metadataFilePath)
        {
            // extract metadata, including audio info, from metadata file
                // -- open file stream
                Console.WriteLine($"Writing new metadata {metadataFilePath}");
                var metadataFileStream = File.Open(metadataFilePath, FileMode.Create);
                var settings = new DataContractJsonSerializerSettings
                {
                    DateTimeFormat = new DateTimeFormat("yyyy-MM-dd'T'HH:mm:ssz")
                };
                // -- set up serializer
                var serializer = new DataContractJsonSerializer(typeof(Metadata), settings);
                // here we will call our serializer object to read our filestream and cast it to our list
                serializer.WriteObject(metadataFileStream, metadata);
        }
    }
}
