using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;

namespace JiChatApi.Service
{
    public static class RongYunService
    {
        private static string Host = "api.rong-api.com";
        private static ConfigurationBuilder _configBuilder;
        private static string AppKey;
        private static string AppSecret;
        private static string Content_Type = "application/x-www-form-urlencoded";
        static RongYunService()
        {
            _configBuilder = new ConfigurationBuilder();
            _configBuilder.AddJsonFile("appsettings.json");
            var configuration = _configBuilder.Build();
            AppKey = configuration["AppKey"]!;
            AppSecret = configuration["AppSecret"]!;
        }

        static public async Task<string> getUserToken(long userId)
        {
            using (HttpClient client = new HttpClient())
            {
                var nonce = Random.Shared.Next().ToString();
                var epoch = new DateTime(1970, 1, 1, 0, 0, 0);
                var timeStamp = (DateTime.Now - epoch).TotalMilliseconds.ToString();
                var encoding = Encoding.UTF8;
                var signatureBytes = SHA1.HashData(encoding.GetBytes(AppSecret + nonce + timeStamp));
                var builder = new StringBuilder();
                foreach (var b in signatureBytes)
                {
                    builder.AppendFormat("{0:x2}", b);
                }
                var signature = builder.ToString();
                client.DefaultRequestHeaders.Add("Host", Host);
                client.DefaultRequestHeaders.Add("App-Key", AppKey);
                client.DefaultRequestHeaders.Add("Nonce", nonce);
                client.DefaultRequestHeaders.Add("Timestamp", timeStamp);
                client.DefaultRequestHeaders.Add("Signature", signature);

                var parameters = new Dictionary<string, string> { { "userId", userId.ToString() } };
                var content = new FormUrlEncodedContent(parameters);
                var response = await client.PostAsync("https://api.rong-api.com/user/getToken.json", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);
                if (dict != null && dict["token"] != null)
                    return dict["token"]!;
                else
                    return "";
            }
        }
        static public async Task<string> sendAddGroupRequest(long fromUserId, long toUserId)
        {
            using (HttpClient client = new HttpClient())
            {
                var nonce = Random.Shared.Next().ToString();
                var epoch = new DateTime(1970, 1, 1, 0, 0, 0);
                var timeStamp = (DateTime.Now - epoch).TotalMilliseconds.ToString();
                var encoding = Encoding.UTF8;
                var signatureBytes = SHA1.HashData(encoding.GetBytes(AppSecret + nonce + timeStamp));
                var builder = new StringBuilder();
                foreach (var b in signatureBytes)
                {
                    builder.AppendFormat("{0:x2}", b);
                }
                var signature = builder.ToString();
                client.DefaultRequestHeaders.Add("Host", Host);
                client.DefaultRequestHeaders.Add("App-Key", AppKey);
                client.DefaultRequestHeaders.Add("Nonce", nonce);
                client.DefaultRequestHeaders.Add("Timestamp", timeStamp);
                client.DefaultRequestHeaders.Add("Signature", signature);
                var parameters = new Dictionary<string, string> { 
                    { 
                        "fromUserId", fromUserId.ToString() 
                    } ,
                    {
                        "toUserId",toUserId.ToString()
                    },
                    {
                        "objectName","RC:CmdNtf"
                    }
                };
                var content = new FormUrlEncodedContent(parameters);
                var response = await client.PostAsync("https://api.rong-api.com/message/system/publish.json", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);
                if (dict != null && dict["token"] != null)
                    return dict["token"]!;
                else
                    return "";
            }
        }
    }
}
