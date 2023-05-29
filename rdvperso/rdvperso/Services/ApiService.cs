using Newtonsoft.Json;
using rdvperso.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace rdvperso.Services
{
    public class ApiService
    {
        /////////API POST CUSTOMER
        public static async Task<bool> RegisterUser(string FirstName, string LastName, string Email, string Phone, string Address, string City, string Zip)
        {
            var register = new Register()
            {
                firstName = FirstName,
                lastName = LastName,
                email = Email,
                phone = Phone,
                address = Address,
                city = City,
                zip = Zip
            };
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", AppSettings.ApiKey);
            var json = JsonConvert.SerializeObject(register);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(AppSettings.ApiUrl + "index.php/api/v1/customers", content);
            if (!response.IsSuccessStatusCode) return false;
            var JsonResult = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Register>(JsonResult);
            Preferences.Set("idclient", result.id);
            Preferences.Set("nomclient", result.lastName);
            Preferences.Set("prenomclient", result.firstName);
            Preferences.Set("codePTclient", result.zip);
            Preferences.Set("mailclient", result.email);
            Preferences.Set("telclient", result.phone);
            Preferences.Set("addressclient", result.address);
            Preferences.Set("cityclient", result.city);
            return true;
        }
        /////////API UPDATE CUSTOMER
        public static async Task<bool> UpdateUser(Register register)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", AppSettings.ApiKey);
            var json = JsonConvert.SerializeObject(register);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync(AppSettings.ApiUrl + "index.php/api/v1/customers/" + register.id, content);
            if (!response.IsSuccessStatusCode) return false;
            var JsonResult = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Register>(JsonResult);
            Preferences.Set("idclient", result.id);
            Preferences.Set("nomclient", result.lastName);
            Preferences.Set("prenomclient", result.firstName);
            Preferences.Set("codePTclient", result.zip);
            Preferences.Set("mailclient", result.email);
            Preferences.Set("telclient", result.phone);
            Preferences.Set("addressclient", result.address);
            Preferences.Set("cityclient", result.city);
            return true;
        }
        /////////API POST RDV
        public static async Task<bool> PostAppointments(Appointments appointments)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", AppSettings.ApiKey);
            var json = JsonConvert.SerializeObject(appointments);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(AppSettings.ApiUrl + "index.php/api/v1/appointments", content);
            var JsonResult = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Appointments>(JsonResult);
            if (!response.IsSuccessStatusCode) return false;
            Preferences.Set("idrdv", result.id);
            Preferences.Set("debutrdv", result.start);
            Preferences.Set("servicerdv", result.serviceId);
            return true;
        }
        /////////API GET CUSTOMER
        public static async Task<List<Register>> GetAllCustomer()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", AppSettings.ApiKey);
            var response = await httpClient.GetAsync(AppSettings.ApiUrl + "index.php/api/v1/customers/");
            var JsonResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Register>>(JsonResult);
        }
        public static async ValueTask<bool> VerifCustomer(Register register)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", AppSettings.ApiKey);
            var response = await httpClient.GetAsync(AppSettings.ApiUrl + "index.php/api/v1/customers/" + register.id);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }
        public static async ValueTask<Register> GetCustomer(Register register)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", AppSettings.ApiKey);
            var response = await httpClient.GetAsync(AppSettings.ApiUrl + "index.php/api/v1/customers/" + register.id);
            var JsonResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Register>(JsonResult);
        }
        /////////API GET
        public static async ValueTask<Categories> GetCategoriesById(Categories categories)
        {
            var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", AppSettings.ApiKey);
        var response = await httpClient.GetAsync(AppSettings.ApiUrl + "index.php/api/v1/categories/" + categories.id);
            var JsonResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Categories>(JsonResult);
        }
        public static async Task<List<Categories>> GetCategories()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", AppSettings.ApiKey);
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + "index.php/api/v1/categories/");
            return JsonConvert.DeserializeObject<List<Categories>>(response);
        }
        public static async Task<List<ClassServices>> GetServices()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", AppSettings.ApiKey);
            var response = await httpClient.GetAsync(AppSettings.ApiUrl + "index.php/api/v1/services/");
            var JsonResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ClassServices>>(JsonResult);
        }
        public static async Task<List<Categories>> GetCategoriesByName(string name)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", AppSettings.ApiKey);
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + "index.php/api/v1/categories?q=" + name);
            return JsonConvert.DeserializeObject<List<Categories>>(response);
        }
        /////////API GET HEURES
        public static async ValueTask<List<string>> GetAvailabilities(Rdv rdv)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", AppSettings.ApiKey);
            var response = await httpClient.GetAsync(AppSettings.ApiUrl + string.Format("index.php/api/v1/availabilities?providerId={0}&serviceId={1}&date={2}", rdv.providerId, rdv.serviceId, rdv.date));
            var JsonResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<string>>(JsonResult);
        }
        /////////API GET MES RDV
        public static async Task<List<Aggregates.MyArray>> GetAppointments()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", AppSettings.ApiKey);
            var response = await httpClient.GetAsync(AppSettings.ApiUrl + String.Format("index.php/api/v1/appointments?aggregates"));
            var JsonResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Aggregates.MyArray>>(JsonResult);
        }
        /////////NEW API DELETE RDV
        public static async Task<bool> DeleteAppointments(Appointments appointements)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", AppSettings.ApiKey);
            var response = await httpClient.DeleteAsync(AppSettings.ApiUrl + "index.php/api/v1/appointments/" + appointements.id);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }
        /////////NEW API DELETE ACCOUNT
        public static async Task<bool> DeleteAccount(Register register)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", AppSettings.ApiKey);
            var response = await httpClient.DeleteAsync(AppSettings.ApiUrl + "index.php/api/v1/customers/" + register.id);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }
        /////////NEW API GET PROVIDER 
        public static async Task<List<ClassProv>> GetProviders()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", AppSettings.ApiKey);
            var response = await httpClient.GetAsync(AppSettings.ApiUrl + String.Format("index.php/api/v1/providers?fields=id,firstName,lastName,email,mobile,phone,address,city,state,zip,notes,timezone,services"));
            var JsonResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ClassProv>>(JsonResult);
        }
    }
}