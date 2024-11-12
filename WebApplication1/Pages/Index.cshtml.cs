using Azure.Security.KeyVault.Secrets;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Certificates;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

public class IndexModel : PageModel
{
    private readonly SecretClient _secretClient;
    private readonly KeyClient _keyClient;
    private readonly CertificateClient _certificateClient;

    public string SecretValue { get; private set; }
    public string KeyName { get; private set; }
    public string KeyType { get; private set; }
    public int? KeySize { get; private set; }
    public string CertificateName { get; private set; }
    public string CertificateValue { get; private set; }
    public string CertificateIssuer { get; private set; }
    public string CertificateSubject { get; private set; }

    public IndexModel(SecretClient secretClient, KeyClient keyClient, CertificateClient certificateClient)
    {
        _secretClient = secretClient;
        _keyClient = keyClient;
        _certificateClient = certificateClient;
    }

    public async Task OnGetAsync()
    {
        // Retrieve a secret
        var secretName = "connection-string";
        KeyVaultSecret secret = await _secretClient.GetSecretAsync(secretName);
        SecretValue = secret.Value;

        // Retrieve a key
        var keyName = "encryption-key";
        KeyVaultKey key = await _keyClient.GetKeyAsync(keyName);
        KeyName = key.Name;
        KeyType = key.KeyType.ToString();
        KeySize = key.Key.N?.Length * 8; // Key size in bits (for RSA keys)

        // Retrieve a certificate
        var certificateName = "ssl-certificate";
        var certificate = await _certificateClient.GetCertificateAsync(certificateName);
        CertificateName = certificate.Value.Name;

        // Retrieve issuer and subject from the certificate policy
        CertificateIssuer = certificate.Value.Policy.IssuerName; // Correct way to get the issuer name
        CertificateSubject = certificate.Value.Policy.Subject;   // Correct way to get the subject name

        // Retrieve the certificate's raw value from Key Vault as a secret
        var certificateSecret = await _secretClient.GetSecretAsync(certificateName);
        CertificateValue = certificateSecret.Value.ToString(); // This is the base64-encoded certificate data

    }
}
