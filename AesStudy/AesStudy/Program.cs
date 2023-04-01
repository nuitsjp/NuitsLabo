using System;
using System.Security.Cryptography;
using System.Text;

var plaintext = "Hello World!";
var key = Encoding.UTF8.GetBytes("eu8G!eNEh@i6PVy!sDJ8kMhCm2XGpr4o");
var iv = GenerateInitialVector();

var encrypted = Encrypt(plaintext, key, iv);
var decrypted = Decrypt(encrypted, key, iv);

Console.WriteLine("Original  : {0}", plaintext);
Console.WriteLine("Encrypted : {0}", Convert.ToBase64String(encrypted));
Console.WriteLine("Decrypted : {0}", decrypted);

static byte[] Encrypt(string plainText, byte[] key, byte[] iv)
{
    using var aesAlg = Aes.Create();
    aesAlg.KeySize = 256;
    aesAlg.Key = key;
    aesAlg.IV = iv;

    var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

    using var msEncrypt = new MemoryStream();
    using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
    using (var swEncrypt = new StreamWriter(csEncrypt))
    {
        swEncrypt.Write(plainText);
    }

    return msEncrypt.ToArray();
}

static string Decrypt(byte[] cipherText, byte[] key, byte[] iv)
{
    using var aesAlg = Aes.Create();
    aesAlg.KeySize = 256;
    aesAlg.Key = key;
    aesAlg.IV = iv;

    var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

    using var msDecrypt = new MemoryStream(cipherText);
    using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
    using var srDecrypt = new StreamReader(csDecrypt);
    return srDecrypt.ReadToEnd();
}

static byte[] GenerateInitialVector()
{
    var iv = new byte[16];
    var today = BitConverter.GetBytes(DateTime.Today.ToBinary());
    Array.Copy(today, 0, iv, 0, today.Length);
    Array.Copy(today, 0, iv, 8, today.Length);

    return iv;
}