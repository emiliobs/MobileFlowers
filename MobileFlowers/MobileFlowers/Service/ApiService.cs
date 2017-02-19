using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MobileFlowers.Service
{
     public  class ApiService
    {
         public async Task<List<T>> Get<T>(string urlBase, string servicePrefix, string controller)
         {
             try
             {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                 var url = $"{servicePrefix}{controller}";
                 var response = await client.GetAsync(url);

                 if (!response.IsSuccessStatusCode)
                 {
                     return null;
                 }

                 var result = await response.Content.ReadAsStringAsync();
                 var list = JsonConvert.DeserializeObject<List<T>>(result);

                 return list;
             }
             catch
             {

                 return null;
             }
         }

    }
}
