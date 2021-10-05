using Newtonsoft.Json;
using rdvperso.Models;
using System;
using rdvperso.Services;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace rdvperso.Services
{
   public class ApiMautic
    {
        public static async Task<bool> AddContact(string FirstName, string LastName, string Email, string Phone, string Address, string City, string Zip, string Country)
        {       
            var newcontact = new NewContact()
            {
                firstname = FirstName,
                lastname = LastName,
                email = Email,
                phone = Phone,
                address1 = Address,
                city = City,
                zipcode = Zip,
                country = Country,
                
            };
            var userName = "BLABLA";
            var passwd = "BLABLA";
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(newcontact);
            var authToken = Encoding.ASCII.GetBytes($"{userName}:{passwd}");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(AppSettings.ApiMauticUrl + "contacts/new", content);           
            if (!response.IsSuccessStatusCode) return false;
            var JsonResult = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<NewContact>(JsonResult);
            Preferences.Set("idmautic", result.id);
            return true;
        }

        /////////NEW API DELETE ACCOUNT
        public static async Task<bool> DeleteId()
        {
            var mautic = Preferences.Get("idmautic", 0);
            var newcontact = new NewContact()
            {
                id = mautic,

             };         
            var userName = "BLABLA";
            var passwd = "BLABLA";
            var httpClient = new HttpClient();
            var authToken = Encoding.ASCII.GetBytes($"{userName}:{passwd}");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));
            var response = await httpClient.DeleteAsync(AppSettings.ApiMauticUrl + string.Format("contacts/{0}/delete", newcontact.id));
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

    }
}
