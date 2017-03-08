using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MobileFlowers.Classes;
using Newtonsoft.Json;

namespace MobileFlowers.Service
{
    public  class ApiService
    {
        //Método GET generico:
        public async Task<Response> Get<T>(string urlBase, string servicePrefix, string controller)
         {
             try
             {
               

                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                 var url = $"{servicePrefix}{controller}";
                 var response = await client.GetAsync(url);
                
                 if (!response.IsSuccessStatusCode)
                 {
                     return new Response()
                     {
                         IsSuccess =  false,
                         Message = response.StatusCode.ToString(),
                     };
                 }

                 var result = await response.Content.ReadAsStringAsync();
                 var list = JsonConvert.DeserializeObject<List<T>>(result);

                 return new Response()
                 {
                     IsSuccess = true,
                     Message = "OK.!",
                     Result = list,
                 };
             }
             catch(Exception ex)
             {

                 return new Response()
                 {
                     IsSuccess = false,
                     Message = ex.Message,
                 };
             }
         }

        //Método POST generico:
        public async Task<Response> Post<T>(string urlBase, string servicePrefix, string controller, T model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var newRecord = JsonConvert.DeserializeObject<T>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Record added OK",
                    Result = newRecord,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }

        }


        //Método Update Generico
        public async Task<Response> Put<T>(string urlBase, string servicePrefix, string controller, T model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = $"{servicePrefix}{controller}/{model.GetHashCode()}";
                var response = await client.PutAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var newRecord = JsonConvert.DeserializeObject<T>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Record Update OK",
                    Result = newRecord,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }

        }

        //Método Delete Generico:
        public async Task<Response> Delete<T>(string urlBase, string servicePrefix, string controller, T model)
        {
            try
            {

                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = $"{servicePrefix}{controller}/{model.GetHashCode()}";
                var response = await client.DeleteAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response()
                    {

                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),

                    };
                }


                return new Response()
                {

                    IsSuccess = true,
                    Message = "Record Delete OK.!",

                };
            }
            catch (Exception ex)
            {

                return new Response()
                {
                    IsSuccess = false,
                    Message = ex.Message,

                };
            }
        }
    }
}
