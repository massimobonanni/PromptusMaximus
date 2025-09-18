using System.Security.Cryptography;
using System.Text;

namespace PromptusMaximus.Core.Security;

public class LinuxProtectedDataProvider : IProtectedDataProvider
{
    private const int KeySize = 32; // 256 bits
    private const int IvSize = 16;  // 128 bits

    public async Task<byte[]> ProtectAsync(byte[] data, string? optionalEntropy = null)
    {
        using var aes = Aes.Create();
        var key = await GenerateKeyAsync(optionalEntropy);
        aes.Key = key;
        aes.GenerateIV();

        using var encryptor = aes.CreateEncryptor();
        using var msEncrypt = new MemoryStream();
        
        // Write IV first
        await msEncrypt.WriteAsync(aes.IV, 0, aes.IV.Length);
        
        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
        {
            await csEncrypt.WriteAsync(data, 0, data.Length);
        }
        
        return msEncrypt.ToArray();
    }

    public async Task<byte[]> UnprotectAsync(byte[] encryptedData, string? optionalEntropy = null)
    {
        using var aes = Aes.Create();
        var key = await GenerateKeyAsync(optionalEntropy);
        aes.Key = key;

        using var msDecrypt = new MemoryStream(encryptedData);
        
        // Read IV
        var iv = new byte[IvSize];
        await msDecrypt.ReadAsync(iv, 0, iv.Length);
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor();
        using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
        using var msPlain = new MemoryStream();
        
        await csDecrypt.CopyToAsync(msPlain);
        return msPlain.ToArray();
    }

    private async Task<byte[]> GenerateKeyAsync(string? optionalEntropy)
    {
        // Create a deterministic key based on user identity and machine
        var keyMaterial = new StringBuilder();
        keyMaterial.Append(Environment.UserName);
        keyMaterial.Append(Environment.MachineName);
        keyMaterial.Append(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
        
        if (!string.IsNullOrEmpty(optionalEntropy))
        {
            keyMaterial.Append(optionalEntropy);
        }

        // Use PBKDF2 to derive a key
        using var pbkdf2 = new Rfc2898DeriveBytes(
            Encoding.UTF8.GetBytes(keyMaterial.ToString()),
            Encoding.UTF8.GetBytes("PromptusMaximus.Salt"), // Static salt
            100000, // Iterations
            HashAlgorithmName.SHA256);

        return pbkdf2.GetBytes(KeySize);
    }
}