using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LSC.AzureConcepts.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KeyVaultController : ControllerBase
    {
        private readonly SecretClient _secretClient;
        private const string SecretName = "DatabaseConnectionString";
        private const string KeyVaultUrl = "https://lsc-azurekv-demo.vault.azure.net/";

        public KeyVaultController()
        {
            // Using Managed Identity to authenticate with Key Vault
            _secretClient = new SecretClient(new Uri(KeyVaultUrl), new DefaultAzureCredential());
        }

        [HttpGet("get-secret")]
        public async Task<IActionResult> GetSecret()
        {
            try
            {
                var secret = await _secretClient.GetSecretAsync(SecretName);
                return Ok(new { SecretName, SecretValue = secret.Value.Value });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error accessing Key Vault", Error = ex.Message });
            }
        }
    }
}
