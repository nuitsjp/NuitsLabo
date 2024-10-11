using System;
using System.Text;
using System.Security.Cryptography;

// 暗号化関数
static string Encrypt(string data)
{
    byte[] dataBytes = Encoding.UTF8.GetBytes(data);
    byte[] encryptedBytes = ProtectedData.Protect(dataBytes, null, DataProtectionScope.CurrentUser);
    return Convert.ToBase64String(encryptedBytes);
}

// 復号化関数
static string Decrypt(string encryptedData)
{
    byte[] encryptedBytes = Convert.FromBase64String(encryptedData);
    byte[] decryptedBytes = ProtectedData.Unprotect(encryptedBytes, null, DataProtectionScope.CurrentUser);
    return Encoding.UTF8.GetString(decryptedBytes);
}

// メイン処理
string originalData = "This is a secret message.";
Console.WriteLine($"Original data: {originalData}");

string encryptedData = Encrypt(originalData);
Console.WriteLine($"Encrypted data: {encryptedData}");

string decryptedData = Decrypt(encryptedData);
Console.WriteLine($"Decrypted data: {decryptedData}");