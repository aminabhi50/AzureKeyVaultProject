using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Certificates;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Register services for Razor Pages
builder.Services.AddRazorPages();

// Access the Key Vault URL from appsettings.json
var keyVaultUrl = builder.Configuration["AzureKeyVault:VaultUrl"];

// Configure Azure Key Vault clients for secrets, keys, and certificates
var credential = new DefaultAzureCredential();
var secretClient = new SecretClient(new Uri(keyVaultUrl), credential);
var keyClient = new KeyClient(new Uri(keyVaultUrl), credential);
var certificateClient = new CertificateClient(new Uri(keyVaultUrl), credential);

// Register each client as a singleton service
builder.Services.AddSingleton(secretClient);
builder.Services.AddSingleton(keyClient);
builder.Services.AddSingleton(certificateClient);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.Run();
