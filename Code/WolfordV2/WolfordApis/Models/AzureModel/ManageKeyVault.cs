using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WolfordApis.Models.AzureModel
{
    class ManageKeyVault
    {
        private readonly string KeyVaultUrl = ConfigurationManager.AppSettings["KeyVaultUri"];
        //private readonly string KeyVaultUrl = "https://wolfordkeyvaultwesteudev.vault.azure.net/";
        private readonly string EmployConnSecretName = ConfigurationManager.AppSettings["EmployeeSecretName"];
        //private readonly string EmployConnSecretName = "WolfordEmployeeConnectionString";
        public string GetEmployeeConnectionString()
        {
            SecretClientOptions options = new SecretClientOptions()
            {
                Retry =
        {
            Delay= TimeSpan.FromSeconds(2),
            MaxDelay = TimeSpan.FromSeconds(16),
            MaxRetries = 5,
            Mode = RetryMode.Exponential
         }
            };
            var client = new SecretClient(new Uri(this.KeyVaultUrl), new DefaultAzureCredential(), options);
            KeyVaultSecret secret = client.GetSecret(this.EmployConnSecretName);
            return secret.Value;
        }
    }
}