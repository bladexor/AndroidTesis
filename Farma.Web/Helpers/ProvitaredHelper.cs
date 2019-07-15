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

        //================================================================================
        // Obtiene y guarda los nombres de medicinas desde provitared y los almacena
        // en la base de datos
        //===================================================================================

        public void RellenarMedicinas()
        {
          /*  ProvitaredHelper pvr = new ProvitaredHelper();

            List<Models.MedicineViewModel> list = await pvr.BuscarMedicinaPor("a");
            foreach (var m in list) { AddMedicine(m.Name); }

            list = await pvr.BuscarMedicinaPor("b");
            foreach (var m in list) { AddMedicine(m.Name); }

            list = await pvr.BuscarMedicinaPor("c");
            foreach (var m in list) { AddMedicine(m.Name); }

            list = await pvr.BuscarMedicinaPor("d");
            foreach (var m in list) { AddMedicine(m.Name); }

            list = await pvr.BuscarMedicinaPor("e");
            foreach (var m in list) { AddMedicine(m.Name); }

            list = await pvr.BuscarMedicinaPor("f");
            foreach (var m in list) { AddMedicine(m.Name); }

            list = await pvr.BuscarMedicinaPor("g");
            foreach (var m in list) { AddMedicine(m.Name); }

            list = await pvr.BuscarMedicinaPor("h");
            foreach (var m in list) { AddMedicine(m.Name); }

            list = await pvr.BuscarMedicinaPor("i");
            foreach (var m in list) { AddMedicine(m.Name); }

            list = await pvr.BuscarMedicinaPor("j");
            foreach (var m in list) { AddMedicine(m.Name); }

            list = await pvr.BuscarMedicinaPor("k");
            foreach (var m in list) { AddMedicine(m.Name); }

            list = await pvr.BuscarMedicinaPor("l");
            foreach (var m in list) { AddMedicine(m.Name); }

            list = await pvr.BuscarMedicinaPor("m");
            foreach (var m in list) { AddMedicine(m.Name); }

            list = await pvr.BuscarMedicinaPor("n");
            foreach (var m in list) { AddMedicine(m.Name); }

            list = await pvr.BuscarMedicinaPor("o");
            foreach (var m in list) { AddMedicine(m.Name); }

            list = await pvr.BuscarMedicinaPor("p");
            foreach (var m in list) { AddMedicine(m.Name); }

            list = await pvr.BuscarMedicinaPor("q");
            foreach (var m in list) { AddMedicine(m.Name); }

            list = await pvr.BuscarMedicinaPor("r");
            foreach (var m in list) { AddMedicine(m.Name); }

            list = await pvr.BuscarMedicinaPor("s");
            foreach (var m in list) { AddMedicine(m.Name); }

            list = await pvr.BuscarMedicinaPor("t");
            foreach (var m in list) { AddMedicine(m.Name); }

            list = await pvr.BuscarMedicinaPor("u");
            foreach (var m in list) { AddMedicine(m.Name); }

            list = await pvr.BuscarMedicinaPor("v");
            foreach (var m in list) { AddMedicine(m.Name); }

            list = await pvr.BuscarMedicinaPor("w");
            foreach (var m in list) { AddMedicine(m.Name); }

            list = await pvr.BuscarMedicinaPor("x");
            foreach (var m in list) { AddMedicine(m.Name); }

            list = await pvr.BuscarMedicinaPor("y");
            foreach (var m in list) { AddMedicine(m.Name); }

            list = await pvr.BuscarMedicinaPor("z");
            foreach (var m in list) { AddMedicine(m.Name); }
            */
        }
    }
}
