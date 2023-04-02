using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


using var rsa = new RSACryptoServiceProvider();
var privateKeyXml = rsa.ToXmlString(true);
var publicKeyXml = rsa.ToXmlString(false);

var issuer = "issuer";
var audience = "audience";

var tokenString = CreateTokenString(privateKeyXml, issuer, audience);
Console.WriteLine(tokenString);

var token = ParseToken(tokenString, publicKeyXml, issuer, audience);
Console.WriteLine(token);



static string CreateTokenString(string privateKeyXml, string issuer, string audience)
{
    // 署名資格を作成する
    var rsa = new RSACryptoServiceProvider();
    rsa.FromXmlString(privateKeyXml);
    var key = new RsaSecurityKey(rsa);
    var credentials = new SigningCredentials(key, "RS256");

    // クレームを作成する。
    var claims = new[]
    {
        new Claim(ClaimTypes.Name, "John Doe"),
        new Claim(ClaimTypes.Email, "john.doe@example.com")
    };
    // トークンの属性オブジェクトを作成する。
    var descriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(claims),
        Issuer = issuer,
        Audience = audience,
        Expires = DateTime.UtcNow.AddDays(1),
        SigningCredentials = credentials,
    };

    // トークンを生成する。
    var handler = new JwtSecurityTokenHandler();
    var token = handler.CreateJwtSecurityToken(descriptor);

    // トークンを文字列かする。
    return handler.WriteToken(token);
}

static SecurityToken ParseToken(string tokenString, string publicKeyXml, string issuer, string audience)
{
    // 署名検証用の鍵を作成する。
    var rsa = new RSACryptoServiceProvider();
    rsa.FromXmlString(publicKeyXml);
    var key = new RsaSecurityKey(rsa);


    // トークン検証用パラメータ
    var validationParams = new TokenValidationParameters
    {
        ValidIssuer = issuer,
        ValidAudience = audience,
        ValidateLifetime = true,
        IssuerSigningKey = key,
    };

    // トークンを検証し、検証済みトークンを取得する。
    // トークン操作用のクラス
    var handler = new JwtSecurityTokenHandler();
    handler.ValidateToken(tokenString, validationParams, out var token);

    return token;
}