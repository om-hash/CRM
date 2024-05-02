using System;

namespace Pal.Core.Domains.Attachments
{
    public class AzureStorageConfig
    {
        public string ConnectionString { get; set; }
        public string ContainerName { get; set; }
        public int TokenExpirationMinutes { get; set; }
    }

    public class AzureStorageSASResult
    {
        public string AccountName { get; set; }
        public string AccountUrl { get; set; }
        public Uri ContainerUri { get; set; }
        public string ContainerName { get; set; }
        public Uri SASUri { get; set; }
        public string SASToken { get; set; }
        public string SASPermission { get; set; }
        public DateTimeOffset SASExpire { get; set; }
    }
}
