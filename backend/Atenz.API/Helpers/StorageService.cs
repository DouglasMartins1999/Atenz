using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel;

namespace Atenz.API.Helpers
{
    public class StorageService
    {
        public string bucket;
        private readonly MinioClient s3client;

        public StorageService(IConfiguration config)
        {
            var region = config.GetValue<string>("Storage:Region");
            var endpoint = config.GetValue<string>("Storage:Endpoint");
            var accessKey = Environment.GetEnvironmentVariable("S3_ACCESS_KEY");
            var secretKey = Environment.GetEnvironmentVariable("S3_SECRET_KEY");

            Console.WriteLine(region);
            Console.WriteLine(endpoint);

            bucket = config.GetValue<string>("Storage:DefaultBucket");
            s3client = new MinioClient(endpoint, accessKey, secretKey, region).WithSSL();
        }

        public async Task<string> GetPreSignedLink(string obj, int expires = 60 * 60 * 4)
        {
            return await s3client.PresignedGetObjectAsync(bucket, obj, expires);
        }

        public async Task<ObjectStat> GetObjectInfo(string obj)
        {
            return await s3client.StatObjectAsync(bucket, obj);
        }
    }
}