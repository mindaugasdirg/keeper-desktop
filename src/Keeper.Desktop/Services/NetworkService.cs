using Keeper.Desktop.Models;
using Keeper.Desktop.Properties;
using Keeper.Desktop.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Keeper.Desktop.Services
{
    public class NetworkService
    {
        private const string url = "https://keeperweb.herokuapp.com";
        private string token;

        public async Task<bool> Login()
        {
            if (!Settings.Default.SynchronizationOn)
                return true;

            var key = Settings.Default.ClientKey;
            var secret = Settings.Default.ClientSecret;
            var body = string.Format("{{ \"key\": \"{0}\", \"secret\": \"{1}\" }}", key, secret);

            var request = CreatePostRequest("/profile/login", body);
            var response = (HttpWebResponse)await request.GetResponseAsync();
            if (response.StatusCode != HttpStatusCode.OK)
                return false;
            token = ReadResponse(response);
            return true;
        }

        public async Task<bool> Register()
        {
            var secret = Settings.Default.ClientSecret;
            var request = CreatePostRequest("/profile", secret.AddQuotes());
            var response = (HttpWebResponse)await request.GetResponseAsync();
            if (response.StatusCode != HttpStatusCode.OK)
                return false;
            var key = ReadResponse(response);
            Settings.Default.ClientKey = key;
            Settings.Default.Save();
            return true;
        }

        public async Task<bool> Add(DataTransaction transaction)
        {
            var content = JsonSerializer.Serialize(transaction);
            var request = CreatePostRequest("/transactions", content.AddQuotes());
            var response = (HttpWebResponse)await request.GetResponseAsync();
            if (response.StatusCode != HttpStatusCode.OK)
                return false;
            var lastSynched = int.Parse(ReadResponse(response));
            Settings.Default.LastSynched = lastSynched;
            Settings.Default.Save();
            return true;
        }

        public async Task<List<DataTransaction>> GetFromLastSynched()
        {
            var lastSynched = Settings.Default.LastSynched;
            var request = CreateDefaultRequest(string.Format("/transactions/{0}", lastSynched));
            var response = (HttpWebResponse)await request.GetResponseAsync();
            if (response.StatusCode != HttpStatusCode.OK)
                return null;
            var data = ReadResponse(response);

            var messages = JsonSerializer.Deserialize<List<SynchedData>>(data, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            var transactions = new List<DataTransaction>();
            foreach(var message in messages)
            {
                lastSynched = Math.Max(lastSynched, message.Id);
                var transaction = ParseDataTransaction(message.Data);
                transactions.Add(transaction);
            }
            Settings.Default.LastSynched = lastSynched;
            Settings.Default.Save();
            return transactions;
        }

        public DataTransaction ParseDataTransaction(string json)
        {
            using (var document = JsonDocument.Parse(json))
            {
                var root = document.RootElement;
                var type = (DataTransaction.DataType)root.GetProperty("Type").GetInt32();
                var action = (DataTransaction.ActionType)root.GetProperty("Action").GetInt32();
                var data = root.GetProperty("Data").ToString();

                object obj = null;
                switch(type)
                {
                    case DataTransaction.DataType.Account:
                        obj = JsonSerializer.Deserialize<Account>(data);
                        break;
                    case DataTransaction.DataType.Activity:
                        obj = JsonSerializer.Deserialize<Activity>(data);
                        break;
                    case DataTransaction.DataType.Category:
                        obj = JsonSerializer.Deserialize<Category>(data);
                        break;
                    case DataTransaction.DataType.TimeEntry:
                        obj = JsonSerializer.Deserialize<TimeEntry>(data);
                        break;
                    case DataTransaction.DataType.Transaction:
                        obj = JsonSerializer.Deserialize<Transaction>(data);
                        break;
                }

                return new DataTransaction()
                {
                    Action = action,
                    Type = type,
                    Data = obj
                };
            }
        }

        private string ReadResponse(HttpWebResponse response)
        {
            var content = string.Empty;
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                content = reader.ReadToEnd();
            }
            return content;
        }

        private WebRequest CreatePostRequest(string path, string body)
        {
            var request = CreateDefaultRequest(path);
            request.Method = "POST";
            using (var stream = request.GetRequestStream())
            {
                stream.Write(Encoding.UTF8.GetBytes(body));
            }
            return request;
        }

        private WebRequest CreateDefaultRequest(string path)
        {
            var client = WebRequest.Create(new Uri(url + path));
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            if (!string.IsNullOrWhiteSpace(token))
                client.Headers.Add(HttpRequestHeader.Authorization, string.Format("bearer {0}", token));
            return client;
        }
    }
}
