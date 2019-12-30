using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleStorageServiceBootcamp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilitySerialization
{
    public class Utility
    {
        public static List<T> ReadLogsInBucket<T>(RegionEndpoint regionEndPoint, string bucketName, string prefix) where T : Log
        {

            List<T> results = new List<T>();
            var response = ReadObjectsFromBucket(regionEndPoint, bucketName, prefix);
            foreach (S3Object objectKey in response.Result.S3Objects)
            {
                List<T> logs = ReadObjectFromBucket<T>(regionEndPoint, bucketName,objectKey);

                if(logs!=null)
                results.AddRange(logs);
            }
            return results;
        }
        private static List<T> Deserialize<T>(string jsonData) where T: Log
        {
          
            List<T> items = new List<T>();
            using (JsonTextReader reader = new JsonTextReader(new StringReader(jsonData)))
            {
                reader.SupportMultipleContent = true;

                while (true)
                {
                    if (!reader.Read())
                    {
                        break;
                    }
                    
                    //JObject jObj = JObject.Load(reader);
                    //var props = jObj.Properties().Where(p => p.Name.StartsWith("binary_")).ToList();
                    //props.ForEach(prop => Console.WriteLine(prop.Name));

                    JsonSerializer serializer = new JsonSerializer();
                    T log = serializer.Deserialize<T>(reader);

                    var transformedLog = TransFormLog(log);
                    items.Add(transformedLog);
                }
            }
            return items;
        }
        private static List<T> ReadObjectFromBucket<T>(RegionEndpoint regionEndpoint, string bucketName,S3Object s3Object) where T :Log
        {
            if (!IsFolder(s3Object))
            {
                var logs = SimpleStorageService.ReadLogsFromBucket(regionEndpoint, bucketName, s3Object.Key);

                var deserializedLogs = Deserialize<T>(logs.Result);
                return deserializedLogs;
            }
            return null;

        }
        private static async Task<ListObjectsResponse> ReadObjectsFromBucket(RegionEndpoint regionEndpoint, string bucketName, string prefix)
        {
            using (var client = SimpleStorageService.GetClient(regionEndpoint))
            {
                ListObjectsRequest request = new ListObjectsRequest
                {
                    BucketName = bucketName,
                    Prefix = prefix
                };

                return await client.ListObjectsAsync(request);
            }
        }

        private static T TransFormLog<T>(T log) where T : Log
        {
            if(log.BinaryResponse!=null)
            {
                var binaryResponseGZipDecode = InterpretEncodedString(log.BinaryResponse);
                log.BinaryResponse = binaryResponseGZipDecode;
            }

            if(log.BinaryRequest != null)
            {
                var binaryRequestGZipDecode = InterpretEncodedString(log.BinaryRequest);
                log.BinaryRequest = binaryRequestGZipDecode;
            }
            return log;
        }

        private static string InterpretEncodedString(string encryptedData)
        {
            var base64DecodedData = Base64Decode(encryptedData);
            return GZipDecompressString(base64DecodedData);
        }
        private static byte[] Base64Decode(string encryptedData)
        {
            return Convert.FromBase64String(encryptedData);
        }
        private static string GZipDecompressString(byte[] compressedTextByte)
        {

            using (var resultMemoryStream = new MemoryStream())
            using (var inputMemoryStream = new MemoryStream(compressedTextByte))
            {
                var gzipStream = new GZipStream(inputMemoryStream, CompressionMode.Decompress);
                gzipStream.CopyTo(resultMemoryStream);

                return Encoding.UTF8.GetString(resultMemoryStream.ToArray());
            }
        }

        private static bool IsFolder(S3Object s3Object)
        {
            return s3Object.Key.EndsWith('/');
        }


    }
}
