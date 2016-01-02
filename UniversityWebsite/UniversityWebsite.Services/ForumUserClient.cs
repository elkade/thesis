using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UniversityWebsite.Services
{
    public class ForumUserClient : IDisposable
    {
        private class CreateDto 
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public bool IsAdmin { get; set; }
        }

        private class EditDto
        {
            public string OldEmail { get; set; }
            public string NewEmail { get; set; }
            public bool IsAdmin { get; set; }
        }

        private class DeleteDto
        {
            public string Email { get; set; }
        }

        private class ResultDto
        {
            public bool IsSuccess { get; set; }
            public string Message { get; set; }
        }

        private readonly string _root;
        private readonly HttpClient _client;

        public ForumUserClient(string root)
        {
            _root = root;
            _client = new HttpClient();
        }

        public bool TryAddUser(string email, string password, bool isAdmin)
        {
            using (HttpResponseMessage response = _client.PostAsJsonAsync(_root + "/sso/register", new CreateDto { Email = email, Password = password, IsAdmin = isAdmin }).Result)
            using (HttpContent content = response.Content)
            {
                string json = content.ReadAsStringAsync().Result;
                ResultDto result = JsonConvert.DeserializeObject<ResultDto>(json);
                if (result.IsSuccess)
                    return true;
                return false;
                //throw new Exception("Adding user to forum did not succeed.");
            }
        }

        public bool TryEditUser(string oldemail, bool isAdmin, string newemail)
        {
            using (HttpResponseMessage response = _client.PostAsJsonAsync(_root + "/sso/changerole", new EditDto { OldEmail = oldemail, IsAdmin = isAdmin, NewEmail = newemail}).Result)
            using (HttpContent content = response.Content)
            {
                string json = content.ReadAsStringAsync().Result;
                ResultDto result = JsonConvert.DeserializeObject<ResultDto>(json);
                if (result.IsSuccess)
                    return true;
                return false;
                //throw new Exception("Adding user to forum did not succeed.");
            }
        }

        public bool TryDeleteUser(string email)
        {
            using (HttpResponseMessage response = _client.PostAsJsonAsync(_root + "/sso/remove", new DeleteDto { Email = email}).Result)
            using (HttpContent content = response.Content)
            {
                string json = content.ReadAsStringAsync().Result;
                ResultDto result = JsonConvert.DeserializeObject<ResultDto>(json);
                if (result.IsSuccess)
                    return true;
                return false;
                //throw new Exception("Adding user to forum did not succeed.");
            }
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
