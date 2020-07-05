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
        public string folder;
        private readonly MinioClient s3client;

        public StorageService(IConfiguration config)
        {
            var region = config.GetValue<string>("Storage:Region");
            var endpoint = config.GetValue<string>("Storage:Endpoint");
            var accessKey = Environment.GetEnvironmentVariable("S3_ACCESS_KEY");
            var secretKey = Environment.GetEnvironmentVariable("S3_SECRET_KEY");

            bucket = config.GetValue<string>("Storage:DefaultBucket");
            folder = config.GetValue<string>("Storage:DefaultFolder");
            s3client = new MinioClient(endpoint, accessKey, secretKey, region).WithSSL();
        }

        public async Task<string> GetPreSignedLink(string link, int expires = 60 * 60 * 2)
        {
            var obj = this.IsS3Link(link);
            if(obj == null) return null;

            return await s3client.PresignedGetObjectAsync(bucket, folder + "/" + obj, expires);
        }

        public async Task<ObjectStat> GetObjectInfo(string link)
        {
            var obj = this.IsS3Link(link);
            if(obj == null) return null;
            
            return await s3client.StatObjectAsync(bucket, folder + "/" + obj);
        }

        private string IsS3Link(string link)
        {
            var prefix = "s3://";
            var isS3 = link.Contains(prefix);
            return isS3 ? link.Replace(prefix, "") : null;
        }
    }
}