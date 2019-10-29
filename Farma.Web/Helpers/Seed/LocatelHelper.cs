using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Farma.Web.Models.Helpers.Locatel;
using Newtonsoft.Json;

namespace Farma.Web.Helpers
{
    public class LocatelHelper
    {
        string urlBase = "https://api.locatel.com.ve/Rest/PublicService.svc/";
        
        public async Task<Models.Helpers.Locatel.ResponseFindProduct> FindProduct(string letras)
        {
            
            try
            {
                var request = new Request
                {
                    ApiKey = "D9909F32-D003-4D7F-A82D-F8843E2FD046",
                    Count=10,
                    Search = letras,
                    StartIndex = 0,
                    StoreId = null
                };
                
                var requestString = JsonConvert.SerializeObject(request);
                var content = new StringContent(requestString, Encoding.UTF8, "application/json");
                
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                client.DefaultRequestHeaders.Host = "api.locatel.com.ve";
                client.DefaultRequestHeaders.Add("Origin","https://www.locatel.com.ve");
               // client.DefaultRequestHeaders.From = "https://www.locatel.com.ve";
                
                //var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync("FindProducts",content);
                var result = await response.Content.ReadAsStringAsync();

                var list = JsonConvert.DeserializeObject<Farma.Web.Models.Helpers.Locatel.ResponseFindProduct>(result);

                list.Products=list.Products.FindAll(z => z.GlobalAvailability == 3);
                return list;

            }
            catch (Exception ex)
            {Console.WriteLine("Error Locatel");
                return null;
                
            }

            //await this.context.SaveChangesAsync();
        }
        
        
        public async Task<Models.Helpers.Locatel.ResponseAvalavility> GetAvailability(string productId)
        {
            
            try
            {
                var request = new Request
                {
                    ApiKey = "D9909F32-D003-4D7F-A82D-F8843E2FD046",
                    ProductId = productId,
                    StoreId = null
                };
                
                var requestString = JsonConvert.SerializeObject(request);
                var content = new StringContent(requestString, Encoding.UTF8, "application/json");
                
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                client.DefaultRequestHeaders.Host = "api.locatel.com.ve";
                client.DefaultRequestHeaders.Add("Origin","https://www.locatel.com.ve");
                // client.DefaultRequestHeaders.From = "https://www.locatel.com.ve";
                
                //var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync("GetAvailability",content);
                var result = await response.Content.ReadAsStringAsync();

                var list = JsonConvert.DeserializeObject<Farma.Web.Models.Helpers.Locatel.ResponseAvalavility>(result);

                list.Availabilities=list.Availabilities.FindAll(z => z.Availability == 3);
                return list;

            }
            catch (Exception ex)
            {Console.WriteLine("Error Locatel");
                return null;
                
            }

            //await this.context.SaveChangesAsync();
        }
    }
}