using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel;

namespace Atenz.API.Helpers
{
    public class StorageService
    {
        private List<Storage> storages;
        private Regex pattern = new Regex("\\w{0,}\\.\\w{0,}@s3:\\/\\/.{0,}");

        public StorageService(IConfiguration config)
        {
            String json = File.ReadAllText("storageSettings.json");
            storages = System.Text.Json.JsonSerializer.Deserialize<List<Storage>>(json);

            foreach (var item in storages)
            {
                item.client = new MinioClient(item.host, item.accessKey, item.secretKey, item.region);
                if(item.withSSL){
                    item.client.WithSSL();
                }
            }
        }

        public async Task<string> GetPreSignedLink(string link, int expires = 60 * 60 * 2)
        {
            var obj = this.IsS3Link(link);
            if(obj == null) return link;

            return await obj.client.PresignedGetObjectAsync(obj.bucket, obj.file, expires);
        }

        public async Task<ObjectStat> GetObjectInfo(string link)
        {
            var obj = this.IsS3Link(link);
            if(obj == null) return null;
            
            return await obj.client.StatObjectAsync(obj.bucket, obj.file);
        }

        private StorageObject IsS3Link(string link)
        {
            if(!this.pattern.IsMatch(link)) return null;   
            // Links must follow this structure: [bucket].[storageID]@s3://[object path]

            var bucket = link.Split(".")[0];
            var hostName = link.Split(".")[1].Split("@")[0];
            var file = link.Split("@s3://")[1];
            var storage = this.storages.Find(s => s.id == hostName);

            return new StorageObject(){
                client = storage.client,
                bucket = bucket,
                file = file
            };
        }
    }

    public class Storage {
        public string id { get; set; }
        public string host { get; set; }
        public string region { get; set; }
        public bool withSSL { get; set; }
        public string accessKey { get; set; }
        public string secretKey { get; set; }
        public MinioClient client { get; set; }
        public string bucket { get; set; }
    }

    public class StorageObject {
        public MinioClient client { get; set; }
        public string bucket { get; set; }
        public string file { get; set; }
    }
}