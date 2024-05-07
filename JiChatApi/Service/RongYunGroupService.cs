using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;

namespace JiChatApi.Service
{
    public static class RongYunGroupService
    {
        private static string Host = "api.rong-api.com";
        private static ConfigurationBuilder _configBuilder;
        private static string AppKey;
        private static string AppSecret;
        private static string Content_Type = "application/x-www-form-urlencoded";
        static RongYunGroupService()
        {
            _configBuilder = new ConfigurationBuilder();
            _configBuilder.AddJsonFile("appsettings.json");
            var configuration = _configBuilder.Build();
            AppKey = configuration["AppKey"]!;
            AppSecret = configuration["AppSecret"]!;
        }
        static public async Task<string?> createGroup(long ownerId,long groupId,string groupName)
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
                    { "userId", ownerId.ToString() },
                    {"groupId",groupId.ToString() },
                    {"groupName",groupName }
                };
                var content = new FormUrlEncodedContent(parameters);
                var response = await client.PostAsync("https://api.rong-api.com/group/create.json", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);
                if (dict != null)
                {
                    return dict["code"];
                }
                else
                {
                    return null;
                }
            }
        }
        static public async Task<string> dismissGroup(long ownerId, long groupId)
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
                    { "userId", ownerId.ToString() },
                    { "groupId",groupId.ToString() },
                };
                var content = new FormUrlEncodedContent(parameters);
                var response = await client.PostAsync("https://api.rong-api.com/group/dismiss.json", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);
                if (dict != null)
                {
                    return dict["code"];
                }
                else
                {
                    return "";
                }
            }
        }
        static public async Task<string> joinGroup(long userId, long groupId)
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
                    { "userId", userId.ToString() },
                    { "groupId",groupId.ToString() },
                };
                var content = new FormUrlEncodedContent(parameters);
                var response = await client.PostAsync("https://api.rong-api.com/group/join.json", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);
                if (dict != null)
                {
                    return dict["code"];
                }
                else
                {
                    return "";
                }
            }
        }
        static public async Task<string> quitGroup(long ownerId, long groupId)
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
                    { "userId", ownerId.ToString() },
                    { "groupId",groupId.ToString() },
                };
                var content = new FormUrlEncodedContent(parameters);
                var response = await client.PostAsync("https://api.rong-api.com/group/quit.json", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);
                if (dict != null)
                {
                    return dict["code"];
                }
                else
                {
                    return "";
                }
            }
        }
    }
}
