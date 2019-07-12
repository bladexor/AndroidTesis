using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Farma.Web.Helpers
{
    public class ProvitaredHelper
    {
        //PARA SUGERENCIAS DE NOMBRES DE MEDICINAS DESDE EL API PROVITARED
        public async Task<List<Models.MedicineViewModel>> BuscarMedicinaPor(string letra)
        {
            var urlBase = "http://provitared.omaryesidmarino.com/";

            try
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };
                //var url = $"{servicePrefix}{controller}";
                var response = await client.GetAsync("api.php?TYPE=GET_SUGGESTS&INITIALS=" + letra);
                var result = await response.Content.ReadAsStringAsync();

                var list = JsonConvert.DeserializeObject<List<Farma.Web.Models.MedicineViewModel>>(result);

                return list;

            }
            catch (Exception ex)
            {
                return null;
            }

            //await this.context.SaveChangesAsync();
        }

    }
}
