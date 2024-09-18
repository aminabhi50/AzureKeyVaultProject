using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Register services for Razor Pages
builder.Services.AddRazorPages();

// Access the Key Vault URL from appsettings.json
var keyVaultUrl = builder.Configuration["AzureKeyVault:VaultUrl"];

// Configure Azure Key Vault client
var client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());
builder.Services.AddSingleton(client);

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
