using System;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace key_vault_console_app
{
    class Program
    {
        const string CLIENTSECRET = "6d8eb8b7-ed12-4fdb-b937-11a5f95cf479"; 
        const string CLIENTID = "f094a9c3-ce14-4e59-b39d-e2389b3108c4";
        const string BASESECRETURI = 
        "https://vault0202.vault.azure.net"; // available from the Key Vault resource page            string keyVaultName = "vault0202";
        static KeyVaultClient kvc = null;
        static void Main(string[] args)
        {
            DoVault();

            Console.ReadLine();

        }
        public static async Task<string> GetToken(string authority, string resource, string scope)
        {
        var authContext = new AuthenticationContext(authority);
        ClientCredential clientCred = new ClientCredential(CLIENTID, CLIENTSECRET);
        AuthenticationResult result = await authContext.AcquireTokenAsync(resource, clientCred);

        if (result == null)
        throw new InvalidOperationException("Failed to obtain the JWT token");

        return result.AccessToken;
        }
        private static void DoVault()
        {
        kvc = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetToken));
        SecretBundle secret = Task.Run( () => kvc.GetSecretAsync(BASESECRETURI + 
        @"/secrets/" + "test")).ConfigureAwait(false).GetAwaiter().GetResult();
        Console.WriteLine(secret.Value);
        Console.ReadLine();

        }

    }
}
