using System.Security.Cryptography;
using System.Text;

var plaintext = @"DESKTOP-AUA6P9K\atsus";

var encrypted = Encrypt(plaintext);
var decrypted = Decrypt(encrypted);

Console.WriteLine("Original  : {0}", plaintext);
Console.WriteLine("Encrypted : {0}", encrypted);
Console.WriteLine("Decrypted : {0}", decrypted);


static string Encrypt(string plainText)
{
    using var aes = GenerateAes();
    var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

    using var msEncrypt = new MemoryStream();
    using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
    using (var swEncrypt = new StreamWriter(csEncrypt))
    {
        swEncrypt.Write(plainText);
    }

    return Convert.ToBase64String(msEncrypt.ToArray());
}

static string Decrypt(string cipherText)
{
    using var aes = GenerateAes();
    var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);


    using var msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText));
    using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
    using var srDecrypt = new StreamReader(csDecrypt);
    return srDecrypt.ReadToEnd();
}

static Aes GenerateAes()
{
    var iv = new byte[16];
    var today = BitConverter.GetBytes(DateTime.Today.ToBinary());
    Array.Copy(today, 0, iv, 0, today.Length);
    Array.Copy(today, 0, iv, 8, today.Length);

    var aes = Aes.Create();
    aes.KeySize = 256;
    aes.Key = Encoding.UTF8.GetBytes("eu8G!eNEh@i6PVy!sDJ8kMhCm2XGpr4o");
    aes.IV = iv;
    return aes;

}
