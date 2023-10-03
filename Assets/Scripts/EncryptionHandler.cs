//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;
//using System.Text;
//using System.Security.Cryptography;

//public class EncryptionHandler : MonoBehaviour
//{
//    /// <summary>
//    /// Generate JWT Token after successful login.
//    /// </summary>
//    /// <param name="accountId"></param>
//    /// <returns>jwt token.</returns>

//    string key = "gQC/X,J@<t>gR'+39S!,:Z{/gh(UH&Hw"; //set any string of 32 chars                 
//    string iv = "UtqG8aqdBe#Vr'=7"; //set any string of 16 chars

//    public string AESEncryption(string inputData)   //AES -  Eecryption
//    {
//        AesCryptoServiceProvider AEScryptoProvider = new AesCryptoServiceProvider();
//        AEScryptoProvider.BlockSize = 128;
//        AEScryptoProvider.KeySize = 256;
//        AEScryptoProvider.Key = ASCIIEncoding.ASCII.GetBytes(key);
//        AEScryptoProvider.IV = ASCIIEncoding.ASCII.GetBytes(iv);
//        AEScryptoProvider.Mode = CipherMode.CBC;
//        AEScryptoProvider.Padding = PaddingMode.PKCS7;

//        byte[] txtByteData = ASCIIEncoding.ASCII.GetBytes(inputData);
//        ICryptoTransform trnsfrm = AEScryptoProvider.CreateEncryptor(AEScryptoProvider.Key, AEScryptoProvider.IV);

//        byte[] result = trnsfrm.TransformFinalBlock(txtByteData, 0, txtByteData.Length);
//        return Convert.ToBase64String(result);
//    }

//    public string AESDecryption(string inputData)   //AES -  Decryption
//    {
//        AesCryptoServiceProvider AEScryptoProvider = new AesCryptoServiceProvider();
//        AEScryptoProvider.BlockSize = 128;
//        AEScryptoProvider.KeySize = 256;
//        AEScryptoProvider.Key = ASCIIEncoding.ASCII.GetBytes(key);
//        AEScryptoProvider.IV = ASCIIEncoding.ASCII.GetBytes(iv);
//        AEScryptoProvider.Mode = CipherMode.CBC;
//        AEScryptoProvider.Padding = PaddingMode.PKCS7;

//        byte[] txtByteData = Convert.FromBase64String(inputData);
//        ICryptoTransform trnsfrm = AEScryptoProvider.CreateDecryptor();

//        byte[] result = trnsfrm.TransformFinalBlock(txtByteData, 0, txtByteData.Length);
//        return ASCIIEncoding.ASCII.GetString(result);
//    }

//    //string secretKey = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";
//    string secretKey = "]6b/:{5n&J!KkHDm";

//    public string JwtEncryption(long playerID, string sessionID)      //For API, return token as string
//    {       
//        var payload = new Dictionary<string, object>();
//        long unixTime = new DateTimeOffset(DateTime.UtcNow.AddMinutes(-5)).ToUnixTimeSeconds();
//        long unixTime2 = new DateTimeOffset(DateTime.UtcNow.AddHours(1)).ToUnixTimeSeconds();

//        payload.Add("id", playerID.ToString().Trim() );
//        payload.Add("sessionid", sessionID.Trim() );
//        payload.Add("nbf", unixTime);
//        payload.Add("exp", unixTime2);
//        payload.Add("iat", unixTime);
//        payload.Add("iss", "SonicIssuer");
//        payload.Add("aud", "SonicAudience");

//        string token = JWT.JsonWebToken.Encode(payload, secretKey, JWT.JwtHashAlgorithm.HS256);     //Encode as JWT

//        return token.Trim();
//        /*
//        var payload = new Dictionary<string, object>
//{
//    { "claim1", 0 },
//    { "claim2", "claim2-value" }
//};
//        const string secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";

//        IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // symmetric
//        IJsonSerializer serializer = new JsonNetSerializer();
//        IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
//        IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

//        var token = encoder.Encode(payload, secret);*/
//    }

//    public Dictionary<string, object> JwtDecryption(string token)    //For API, return payload of JWT as dictionary
//    {
//        Dictionary<string, object> payload = new Dictionary<string, object>();

//        try
//        {
//            string jsonPayload = JWT.JsonWebToken.Decode(token, secretKey);     //Decode JWT
//            //print(jsonPayload);
//            payload = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonPayload);
//        }
//        catch (JWT.SignatureVerificationException)
//        {
//            print("Invalid token!");
//        }

//        return payload;
//    }

//    /*
//public string GenerateJwtToken(string id, string sessionId)
//{
//    var tokenHandler = new JwtSecurityTokenHandler();
//    var key = Encoding.ASCII.GetBytes(Startup.StaticConfig["Jwt:Key"]);
//    var tokenDescriptor = new SecurityTokenDescriptor
//    {
//        Subject = new ClaimsIdentity(new[] {
//                new Claim("id", id),
//                new Claim("sessionid", sessionId)
//            }),
//        Expires = DateTime.UtcNow.AddHours(1),
//        Issuer = Startup.StaticConfig["Jwt:Issuer"],
//        Audience = Startup.StaticConfig["Jwt:Audience"],
//        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
//    };
//    var token = tokenHandler.CreateToken(tokenDescriptor);
//    return tokenHandler.WriteToken(token);
//}
//*/
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Security.Cryptography;
using System.Security.Claims;

public class EncryptionHandler : MonoBehaviour
{
    /// <summary>
    /// Generate JWT Token after successful login.
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns>jwt token.</returns>

    string key = "n86Iwf9t$xZ9mhNGo54A*FaaaAfUOIZX"; //set any string of 32 chars                 
    string iv = "B9of31c3b&7*EI0#"; //set any string of 16 chars

    public string AESEncryption(string inputData)   //AES -  Eecryption
    {
        AesCryptoServiceProvider AEScryptoProvider = new AesCryptoServiceProvider();
        AEScryptoProvider.BlockSize = 128;
        AEScryptoProvider.KeySize = 256;
        AEScryptoProvider.Key = ASCIIEncoding.ASCII.GetBytes(key);
        AEScryptoProvider.IV = ASCIIEncoding.ASCII.GetBytes(iv);
        AEScryptoProvider.Mode = CipherMode.CBC;
        AEScryptoProvider.Padding = PaddingMode.PKCS7;

        byte[] txtByteData = ASCIIEncoding.ASCII.GetBytes(inputData);
        ICryptoTransform trnsfrm = AEScryptoProvider.CreateEncryptor(AEScryptoProvider.Key, AEScryptoProvider.IV);

        byte[] result = trnsfrm.TransformFinalBlock(txtByteData, 0, txtByteData.Length);
        return Convert.ToBase64String(result);
    }

    public string AESDecryption(string inputData)   //AES -  Decryption
    {
        AesCryptoServiceProvider AEScryptoProvider = new AesCryptoServiceProvider();
        AEScryptoProvider.BlockSize = 128;
        AEScryptoProvider.KeySize = 256;
        AEScryptoProvider.Key = ASCIIEncoding.ASCII.GetBytes(key);
        AEScryptoProvider.IV = ASCIIEncoding.ASCII.GetBytes(iv);
        AEScryptoProvider.Mode = CipherMode.CBC;
        AEScryptoProvider.Padding = PaddingMode.PKCS7;

        byte[] txtByteData = Convert.FromBase64String(inputData);
        ICryptoTransform trnsfrm = AEScryptoProvider.CreateDecryptor();

        byte[] result = trnsfrm.TransformFinalBlock(txtByteData, 0, txtByteData.Length);
        return ASCIIEncoding.ASCII.GetString(result);
    }

    string secretKey = "zvvQK057n5#AzuBo";
    //string secretKey = "]6b/:{5n&J!KkHDm";

    public string JwtEncryption(string userName, string nftAddress)      //For API, return token as string
    {
        var payload = new Dictionary<string, object>();

        long unixTime = new DateTimeOffset(DateTime.UtcNow.AddMinutes(-5)).ToUnixTimeSeconds();
        long unixTime2 = new DateTimeOffset(DateTime.UtcNow.AddHours(1)).ToUnixTimeSeconds();

        payload.Add("username", userName.Trim());
        payload.Add("nftaddress", nftAddress.Trim());
        payload.Add("jti", Guid.NewGuid().ToString());
        payload.Add("nbf", unixTime);
        payload.Add("exp", unixTime2);
        payload.Add("iat", unixTime);
        payload.Add("iss", "MetahorseIssuer");
        payload.Add("aud", "MetahorseAudience");

        string token = JWT.JsonWebToken.Encode(payload, secretKey, JWT.JwtHashAlgorithm.HS256);     //Encode as JWT

        return token.Trim();
    }
    /*
    var payload = new Dictionary<string, object>
//{
//    { "claim1", 0 },
//    { "claim2", "claim2-value" }
//};
//        const string secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";

//        IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // symmetric
//        IJsonSerializer serializer = new JsonNetSerializer();
//        IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
//        IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

//        var token = encoder.Encode(payload, secret);*/
    //    }

    //    public Dictionary<string, object> JwtDecryption(string token)    //For API, return payload of JWT as dictionary
    //    {
    //        Dictionary<string, object> payload = new Dictionary<string, object>();

    //        try
    //        {
    //            string jsonPayload = JWT.JsonWebToken.Decode(token, secretKey);     //Decode JWT
    //            //print(jsonPayload);
    //            payload = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonPayload);
    //        }
    //        catch (JWT.SignatureVerificationException)
    //        {
    //            print("Invalid token!");
    //        }

    //        return payload;
    //    }


    //    public string GenerateJwtToken(string id, string sessionId)
    //{
    //    var tokenHandler = new JwtSecurityTokenHandler();
    //    var key = Encoding.ASCII.GetBytes(Startup.StaticConfig["Jwt:Key"]);
    //    var tokenDescriptor = new SecurityTokenDescriptor
    //    {
    //        Subject = new ClaimsIdentity(new[] {
    //            new Claim("id", id),
    //            new Claim("sessionid", sessionId)
    //        }),
    //        Expires = DateTime.UtcNow.AddHours(1),
    //        Issuer = Startup.StaticConfig["Jwt:Issuer"],
    //        Audience = Startup.StaticConfig["Jwt:Audience"],
    //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    //    };
    //    var token = tokenHandler.CreateToken(tokenDescriptor);
    //    return tokenHandler.WriteToken(token);
    //}

}
