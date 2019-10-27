using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Farma.Web.Models.Helpers;
using Farma.Web.Models.Helpers.Farmatodo;
using Newtonsoft.Json;

namespace Farma.Web.Helpers
{
    public class FarmatodoHelper
    {
        //PARA OBTENER PRODUCTOS DESDE EL API FARMATODO
        public async Task<Models.Helpers.Farmatodo.RootObject> BuscarProducto(string letras)
        {
            var urlBase = "https://vcojeyd2po-dsn.algolia.net/1/indexes/prod-vzla/";

            try
            {
                var request = new Request
                {
                    getRankingInfo = "false",
                    hitsPerPage = "5",
                    page = "0",
                    query = letras,
                    filters = "idStoreGroup:427 AND colorNumber > 0"
                };
                
                var requestString = JsonConvert.SerializeObject(request);
                var content = new StringContent(requestString, Encoding.UTF8, "application/json");
                
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };
                
                
                //var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync("query?x-algolia-agent=Algolia%20for%20vanilla%20JavaScript%203.22.1&x-algolia-application-id=VCOJEYD2PO&x-algolia-api-key=e6f5ccbcdea95ff5ccb6fda5e92eb25c",content);
                var result = await response.Content.ReadAsStringAsync();

                var list = JsonConvert.DeserializeObject<Farma.Web.Models.Helpers.Farmatodo.RootObject>(result);
Console.WriteLine("FARMATODO " + list.hits.Count);
                return list;

            }
            catch (Exception ex)
            {Console.WriteLine("Error Farmatodo");
                return null;
                
            }

            //await this.context.SaveChangesAsync();
        }
    }
}