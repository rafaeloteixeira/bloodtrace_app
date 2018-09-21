using BloodTrace.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BloodTrace.Services
{
    public class ApiServices
    {
        public async Task<bool> RegisterUser(string email, string password, string confirmPassword)
        {
            bool retorno = false;
            try
            {
                var registerModel = new RegisterModel()
                {
                    Email = email,
                    Password = password,
                    ConfirmPassword = confirmPassword
                };

                var httpClient = new HttpClient(new HttpClientHandler
                {
                    UseProxy = false
                });
                var json = JsonConvert.SerializeObject(registerModel);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("http://mybloodapp.azurewebsites.net/api/Account/Register", content);
                retorno = response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }

            return retorno;
        }

        public async Task<bool> LoginUser(string email, string password)
        {
            bool retorno = false;
            try
            {
                var keyvalues = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("username", email),
                    new KeyValuePair<string, string>("password", password),
                    new KeyValuePair<string, string>("grant_type", "password"),
                };

                var request = new HttpRequestMessage(HttpMethod.Post, "http://mybloodapp.azurewebsites.net/Token");
                request.Content = new FormUrlEncodedContent(keyvalues);
                var httpClient = new HttpClient(new HttpClientHandler
                {
                    UseProxy = false
                });
                var response = await httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();
                JObject jObject = JsonConvert.DeserializeObject<dynamic>(content);
                var accessToken = jObject.Value<string>("access_token");
                Settings.AccessToken = accessToken;
                Settings.UserName = email;
                Settings.Password = password;
                retorno = response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }

            return retorno;
        }

        public async Task<List<BloodUser>> FindBlood(string state, string city, string bloodType)
        {
            var httpClient = new HttpClient(new HttpClientHandler
            {
                UseProxy = false
            });
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Settings.AccessToken);
            var bloodApiUrl = "http://mybloodapp.azurewebsites.net/api/BloodUsers";
            var json = await httpClient.GetStringAsync($"{bloodApiUrl}?bloodGroup={bloodType}&city={city}&state={state}");
            return JsonConvert.DeserializeObject<List<BloodUser>>(json);
        }

        public async Task<List<BloodUser>> LatestBloodUser()
        {
            var httpClient = new HttpClient(new HttpClientHandler
            {
                UseProxy = false
            });
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Settings.AccessToken);
            var bloodApiUrl = "http://mybloodapp.azurewebsites.net/api/BloodUsers";
            var json = await httpClient.GetStringAsync(bloodApiUrl);
            return JsonConvert.DeserializeObject<List<BloodUser>>(json);
        }

        public async Task<bool> RegisterDonor(BloodUser bloodUser)
        {
            bool retorno = false;
            try
            {
                var json = JsonConvert.SerializeObject(bloodUser);
                var httpClient = new HttpClient(new HttpClientHandler
                {
                    UseProxy = false
                });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Settings.AccessToken);
                var bloodApiUrl = "http://mybloodapp.azurewebsites.net/api/BloodUsers";
                var response = await httpClient.PostAsync(bloodApiUrl, content);
                retorno = response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }

            return retorno;
        }

        public async Task<List<Estado>> ListarUFs()
        {
            var httpClient = new HttpClient(new HttpClientHandler
            {
                UseProxy = false
            });
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Settings.AccessToken);
            var ibgeUrl = "https://servicodados.ibge.gov.br/api/v1/localidades/estados";
            var json = await httpClient.GetStringAsync(ibgeUrl);
            return JsonConvert.DeserializeObject<List<Estado>>(json);
        }
        public async Task<List<Cidade>> ListarCidades(int UF)
        {
            var httpClient = new HttpClient(new HttpClientHandler
            {
                UseProxy = false
            });
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Settings.AccessToken);
            var ibgeUrl = "https://servicodados.ibge.gov.br/api/v1/localidades/estados/" + UF + "/municipios";
            var json = await httpClient.GetStringAsync(ibgeUrl);
            return JsonConvert.DeserializeObject<List<Cidade>>(json);
        }
    }
}
