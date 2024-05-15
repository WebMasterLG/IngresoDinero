using IngresoDinero.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IngresoDinero.Helpers
{
    public class ErrorHandler
    {
        /// <summary>
        /// Envia el detalle de la excepción a los operadores.
        /// Este método se implementó en Global.asax.cs para recibir excepciones del aplicativo.
        /// </summary>
        public static void EnviarError(HttpRequest req, Exception e)
        {
            Dictionary<string, string> error = new Dictionary<string, string>
                {
                    {"Tipo", e.GetType().ToString()},
                    {"Mensaje", e.Message},
                    {"StackTrace", e.StackTrace}
                };

            foreach (DictionaryEntry data in e.Data)
                error.Add(data.Key.ToString(), data.Value.ToString());

            string html = "";

            foreach (KeyValuePair<string, string> item in error)
            {
                html += "<p><strong>" + item.Key + "</strong><br><pre>" + item.Value + "</pre></p>";
            }

            string url = req.Url.AbsoluteUri;

            string usu = "";

            if (req.Cookies["coneccionIF"] != null)
                usu = req.Cookies["coneccionIF"]["nombre_usuario"] + " (" + req.Cookies["coneccionIF"]["mail"] + ")";

            MaestrosModel.ReportarErrorInterno(url, html, usu);
        }
    }
}