using System;
using System.Net.Http;
using Newtonsoft.Json;

namespace UniversityWebsite.Services
{
    /// <summary>
    /// Klient HTTP odpowiedzialny za integrację systemu głównego z forum.
    /// </summary>
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

        /// <summary>
        /// Tworzy nową instancję klienta.
        /// </summary>
        /// <param name="root">Bazowa część adresu URL forum.</param>
        public ForumUserClient(string root)
        {
            _root = root;
            _client = new HttpClient();
        }

        /// <summary>
        /// Dodaje nowego użytkownika do systemu forum.
        /// </summary>
        /// <param name="email">Email użytkownika</param>
        /// <param name="password">Hasło użytkownika</param>
        /// <param name="isAdmin">Wartość logiczna mówiąca, czy nowy użytkownik ma uprawnienia administratora.</param>
        /// <returns>Wartość logiczna powodzenia operacji dodawania użytkownika.</returns>
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

        /// <summary>
        /// Edytuje dane użytkownika forum.
        /// </summary>
        /// <param name="oldemail">Dotychczasowy email użytkownika.</param>
        /// <param name="isAdmin">Wartość logiczna mówiąca, czy użytkownik po edycji ma mieć uprawnienia administratora.</param>
        /// <param name="newemail">Nowy email użytkownika</param>
        /// <returns>Wartość logiczna powodzenia operacji edycji użytkownika.</returns>
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

        /// <summary>
        /// Usuwa użytkownika forum
        /// </summary>
        /// <param name="email">Email użytkownika</param>
        /// <returns>Wartość logiczna powodzenia operacji usunięcia użytkownika.</returns>
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

        /// <summary>
        /// Implementacja metody "Dispose" interfejsu IDisposable
        /// </summary>
        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
