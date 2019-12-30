using System;
using UtilitySerialization;
using SimpleStorageServiceBootcamp.Models;
using Amazon;
using Amazon.S3.Model;
using System.IO;
using System.Threading.Tasks;

namespace SimpleStorageServiceBootcamp
{
    public class Program
    {
       
        static void Main(string[] args)
        {
            string s3ServiceUrl = "http://localhost:4572";
            string bucketName = "travel-stage-log-analysis";
            string prefix = "CloudEngineering/logrestore-sample";
            

            var logs = Utility.ReadLogsInBucket<Log>(RegionEndpoint.USEast1, bucketName,prefix);
            logs.ForEach(item =>
            {
                PrintReponse(item);
            });
            
            Console.ReadKey(true);
        }

        public static void PrintReponse(Log item)
        {
            Console.WriteLine("Id : {0}", item.Id);
            Console.WriteLine("Log Time : {0}", item.LogTime);
            Console.WriteLine("Type: {0}", item.Type);
            Console.WriteLine("AppName: {0}", item.AppName);
            Console.WriteLine("Binary Request: {0}", item.BinaryRequest);
            Console.WriteLine("Binary Response: {0}", item.BinaryResponse);
            Console.WriteLine("###############################");
        }
    }
}
