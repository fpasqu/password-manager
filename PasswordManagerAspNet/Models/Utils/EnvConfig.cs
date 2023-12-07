namespace PasswordManagerAspNet.Core.Models.Utils
{
    public class EnvConfig
    {
        public string Instance { get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ApiAudience { get; set; }
        public string CallbackPath { get; set; }
        public string GroupId { get; set; }
        public string DbName { get; set; }
    }
}
