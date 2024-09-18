using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

public class IndexModel : PageModel
{
    private readonly SecretClient _secretClient;

    public string SecretValue { get; private set; }

    public IndexModel(SecretClient secretClient)
    {
        _secretClient = secretClient;
    }

    public async Task OnGetAsync()
    {
        var secretName = "connection-string";
        KeyVaultSecret secret = await _secretClient.GetSecretAsync(secretName);
        SecretValue = secret.Value;
    }
}
