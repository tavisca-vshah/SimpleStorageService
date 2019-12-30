using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System.IO;
using System.Threading.Tasks;

namespace UtilitySerialization
{
    public class SimpleStorageService
    {
      
        public static AmazonS3Client GetClient(RegionEndpoint region)
        {
            return new AmazonS3Client(region);
        }
        public static async Task<string> ReadLogsFromBucket(RegionEndpoint regionEndpoint, string bucketName, string ObjectKey)
        {
            using (var s3Client = GetClient(regionEndpoint))
            {
                GetObjectRequest request = new GetObjectRequest()
                {
                    BucketName = bucketName,
                    Key = ObjectKey  
                };

                using (GetObjectResponse response = await s3Client.GetObjectAsync(request))
                using (StreamReader reader = new StreamReader(response.ResponseStream))

                return reader.ReadToEnd();
            }
        }
    }
}
