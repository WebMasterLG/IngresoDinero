using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.IO;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Text;
using IngresoDinero.clases;
using IngresoDinero.Models;
using System.Globalization;
using IngresoDinero.Helpers;

namespace IngresoDinero.Controllers
{
    public class FacturasNuboxController : Controller
    {
        // GET: FacturasNubox
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult _FormularioCargaArchivo()
        {
            return PartialView();
        }

        //public ActionResult _ResultadoCargarArchivos()
        //{
        //    return PartialView();
        //}

        private string NombreArchivoUrl(string nombreArchivo)
        {
            // Reemplaza caracteres no compatibles para la URL de descarga
            nombreArchivo = nombreArchivo
                .Replace('á', 'a').Replace('Á', 'A')
                .Replace('ç', 'c').Replace('Ç', 'C')
                .Replace('é', 'e').Replace('É', 'E')
                .Replace('í', 'i').Replace('Í', 'I')
                .Replace('ñ', 'n').Replace('Ñ', 'N')
                .Replace('ó', 'o').Replace('Ó', 'O')
                .Replace('ú', 'u').Replace('Ú', 'U')
                .Replace('ü', 'u').Replace('Ü', 'U');

            return Regex.Replace(nombreArchivo, @"[^.&\-\(\)\[\]0-9A-Za-z]+", "_");
        }

        [HttpPost]
        public ActionResult _ResultadoCargarArchivos(HttpPostedFileBase archivo)
        {
            List<ListadoCargaFacturas> lst = new List<ListadoCargaFacturas>();

            //int IdUsuario = int.Parse(Request.Cookies["cookiePerfil"]["usuario"].ToString());
            //int IdUsuario = 10016;
            string dir = Server.MapPath("~/Documentos");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            if (archivo != null && archivo.ContentLength > 0)
            {
                lst = LeerPDF(archivo.InputStream, dir);
                return PartialView(lst);
            }
            else
            {
                return PartialView(lst);
            }
        }

        [HttpPost]
        public ActionResult _CargarArchivoFactura(int id_ing, HttpPostedFileBase archivo)
        {
            List<ListadoCargaFacturas> lst = new List<ListadoCargaFacturas>();

            //int IdUsuario = int.Parse(Request.Cookies["cookiePerfil"]["usuario"].ToString());
            //int IdUsuario = 10016;
            string dir = Server.MapPath("~/Documentos");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            if (archivo != null && archivo.ContentLength > 0)
            {
                lst = LeerPDF(archivo.InputStream, dir, id_ing);
                return Json(lst);
            }
            else
            {
                return Json(lst);
            }
        }

        [HttpPost]
        public ActionResult _EliminarArchivoFactura(int id_ing)
        {
            int IdUsuario = int.Parse(Request.Cookies["cookiePerfil"]["usuario"].ToString());
            FacturaNubox.Llama_Facturas_NBX_Cargar_Factura(IdUsuario, 1, id_ing);
            return Json(new { estado = true });
        }

        private List<ListadoCargaFacturas> LeerPDF(Stream stream, string directorio_salida, int id_ing = 0)
        {
            int IdUsuario = int.Parse(Request.Cookies["cookiePerfil"]["usuario"].ToString());
            List<ListadoCargaFacturas> lst = new List<ListadoCargaFacturas>();

            using (PdfReader reader = new PdfReader(stream))
            {
                for (int page = 1; page <= reader.NumberOfPages; page++)
                {
                    ITextExtractionStrategy its = new SimpleTextExtractionStrategy();
                    string s = PdfTextExtractor.GetTextFromPage(reader, page, its);

                    s = Encoding.UTF8.GetString(Encoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(s)));

                    string fechaEmision = TraeFecha(s, "Fecha.Emisión");
                    string fechaVencimiento = TraeFecha(s, "Fecha.Vencimiento");
                    string Folio = TraeFactura(s);
                    string neto = Monto(s, "Monto.Neto");
                    string iva = Monto(s, "IVA");
                    string exento = Monto(s, "Monto.Exento");
                    string total = Monto(s, "Total");
                    //string rut = TraeRut(s);
                    string rut = "";
                    string descripcion_factura = Descripcion(s, @"ASESORIA INMOBILIARIA\.");//TraeFactura(s);

                    var proyectos = TraeProyectos(s);
                
                    var dptos = Dptos(s);

                    string NombreArchivo = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Folio + "_" + fechaEmision + ".pdf";
                    List<ListadoCargaFacturas> carga = new List<ListadoCargaFacturas>();
                    bool saveFile = false;
                    if (id_ing > 0)
                    {
                        string proy2 = proyectos.Count > 0 ? proyectos[0] : "";
                        string dpto2 = dptos.Count > 0 ? dptos[0] : "";
                        carga = FacturaNubox.Llama_Facturas_NBX_Cargar_Factura(IdUsuario, 0, id_ing, fechaEmision, fechaVencimiento, int.Parse(Folio), NombreArchivo, proy2, dpto2, rut, neto != null ? int.Parse(neto) : 0, iva != null ? int.Parse(iva) : 0, exento != null ? int.Parse(exento) : 0, int.Parse(total), descripcion_factura);
                        if (carga.Count > 0)
                        {
                            saveFile = true;
                        }
                        lst.AddRange(carga);
                    }
                    else if (proyectos.Count > 0 && dptos.Count > 0)
                    {
                        foreach (var proy in proyectos)
                        {
                            System.Diagnostics.Debug.WriteLine(proy);
                            foreach (var dpto in dptos)
                            {
                                System.Diagnostics.Debug.WriteLine(dpto);
                                carga = FacturaNubox.Llama_Facturas_NBX_Cargar(0, fechaEmision, fechaVencimiento, int.Parse(Folio), NombreArchivo, IdUsuario, proy, dpto, rut, neto != null ? int.Parse(neto) : 0, iva != null ? int.Parse(iva) : 0, exento != null ? int.Parse(exento) : 0, int.Parse(total), descripcion_factura);
                                if (carga.Count > 0) {
                                    saveFile = true;
                                }
                                lst.AddRange(carga);
                            }
                        }
                    }

                    if (saveFile)
                    {
                        string RutaSalida = System.IO.Path.Combine(directorio_salida, NombreArchivo);
                        System.Diagnostics.Debug.WriteLine(RutaSalida);
                        ExtractPages(reader, RutaSalida, page, page);
                    }

                    if (id_ing > 0)
                    {
                        break;
                    }
                }
            }

            return lst;
        }

        private void ExtractPages(PdfReader reader, string outputPDFpath, int startpage, int endpage)
        {
            using (var fs = new FileStream(outputPDFpath, FileMode.Create))
            {
                byte[] content;
                using (var ms = new MemoryStream())
                {
                    using (Document sourceDocument = new Document(reader.GetPageSizeWithRotation(startpage)))
                    {
                        using (PdfCopy pdfCopyProvider = new PdfCopy(sourceDocument, ms))
                        {
                            sourceDocument.Open();
                            for (int i = startpage; i <= endpage; i++)
                            {
                                PdfImportedPage importedPage = pdfCopyProvider.GetImportedPage(reader, i);
                                pdfCopyProvider.AddPage(importedPage);
                            }
                        }
                    }
                    content = ms.ToArray();
                }
                content = PdfUtils.RemovePdfMark(content);
                fs.Write(content, 0, content.Length);
            }
        }

        //private string TraeRut(string texto)
        //{
        //    int x = 0;
        //    string resultado = null;
        //    string PatternRUT = @"[0-9]{1,2}\.[0-9]{3}\.[0-9]{3}\-[0-9|K]{1}";
        //    Regex BRut = new Regex(PatternRUT, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
        //    MatchCollection Esta = BRut.Matches(texto);
        //    foreach (Match v in Esta)
        //    {
        //        if (x == 1)
        //        {
        //            resultado = v.Groups[0].ToString().Replace(".", "");
        //        }
        //        x++;
        //    }
        //    return resultado;
        //}

        private string TraeFecha(string text, string campo)
        {
            string res = null;
            CultureInfo ci = new CultureInfo("es-CL");
            string pattern = campo + @".(([0-9]{1,2}).de.([a-zA-Z]{1,}).de.([0-9]{1,}))";
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
            MatchCollection matches = rgx.Matches(text);
            if (matches.Count > 0)
            {
                string dateStr = matches[0].Groups[1].ToString();
                DateTime date = DateTime.Parse(dateStr, ci);
                res = date.ToString("yyyy-MM-dd", ci);
            }
            return res;
        }

        private string TraeFactura(string texto)
        {

            System.Diagnostics.Debug.WriteLine(texto);
            string folio = null;
            string pattern = @"Nº.([0-9]{1,})"; //Se quita el punto ya que la nueva factura 
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
            MatchCollection matches = rgx.Matches(texto);
            if (matches.Count > 0)
                folio = matches[0].Groups[1].ToString();

            return folio;
        }

        private string Monto(string texto, string campo)
        {
            string resultado = null;
            string PatternRUT = campo + @".([0-9]{1,}\.{0,}[0-9]{1,}\.{0,}[0-9]{0,}\.{0,}[0-9]{1,}\.{0,})";
            Regex BRut = new Regex(PatternRUT, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
            MatchCollection Esta = BRut.Matches(texto);
            foreach (Match v in Esta)
            {
                resultado = v.Groups[1].ToString().Replace(".", "");
            }
            return resultado;
        }

        //private List<Dictionary<string, string>> Clientes_Dptos(string texto)
        //{
        //    var clientesDptos = new List<Dictionary<string, string>>();

        //    string Pattern = @"^([a-z]+.[a-z]+.[a-z]*.[a-z]*.[a-z]*.[a-z]*).DPTO.(.*)$";
        //    Regex rgx = new Regex(Pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
        //    MatchCollection matches = rgx.Matches(texto);
        //    foreach (Match m in matches)
        //    {
        //        string cliente = m.Groups[1].ToString().Trim();
        //        string strDptos = m.Groups[2].ToString().Trim();

        //        string[] departamentos = strDptos.Split(',');

        //        foreach (string departamento in departamentos)
        //        {
        //            clientesDptos.Add(new Dictionary<string, string>
        //            {
        //                { "cliente", cliente },
        //                { "departamento", departamento.Trim() },
        //            });
        //        }
        //    }
        //    return clientesDptos;
        //}

        private string Descripcion(string texto, string campo)
        {
            string descripcion = "";

            string Pattern = campo + @".\n(.*)\n\d+ [\d\.]+ [a-z]+ [\d\.]+\n";
            Regex rgx = new Regex(Pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
            MatchCollection matches = rgx.Matches(texto);

            if (matches.Count == 1)
                descripcion = matches[0].Groups[1].ToString().Trim();

            return descripcion;
        }

        private List<string> TraeProyectos(string text)
        {
            var proyectos = new List<string>();
            //string pattern = @"(?:ASESORIA.INMOBILIARIA.+Proyecto.([\w\s]+).UF)|(?:Asesoría.Financiera.+\((\w+)\).De?pto.+\n)|(?:Asesoría.Financiera.+Proyectos?.([A-Z]+)(?:.-.([A-Z]+))?(?:.-.([A-Z]+))?.-.De?ptos?)";
            string pattern = @"(?:ASESORIA.INMOBILIARIA.+Proyecto.([\w\s]+).UF)|(?:Asesoría.Financiera.+\((\w+)\).De?pto.+\n)|(?:Asesoría.Financiera.+Proyectos?.([A-Z]+)(?:.-.([A-Z]+))?(?:.-.([A-Z]+))?.-.De?ptos?)|(?:\d?\s?Asesoría Financiera\s(?:-)?\s?([\w\s\?]+)\s\((\w+\s?\(?\w+\)?)\)\sDpto\d+)";
            
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
            MatchCollection matches = rgx.Matches(text);
            if (matches.Count > 0)
            {
                for (int i = 1; i < matches[0].Groups.Count; i++)
                {
                    string group = matches[0].Groups[i].ToString();
                    if (!string.IsNullOrEmpty(group))
                    {
                        proyectos.Add(group.Trim());
                    }
                }
            }
            return proyectos.Distinct().ToList();
        }

        private List<string> Dptos(string text)
        {
            var dptos = new List<string>();

            //string pattern = @"^(?:[a-z]+.[a-z]+.[a-z]*.[a-z]*.[a-z]*.[a-z]*).De?pto.?(.*)$|^(?:^Asesoría.Financiera.+\(\w+\).De?pto(\w?\s?\d+\w?))|^(?:^Asesoría.Financiera.+Proyectos?.+.-.De?ptos?.(\w?\s?\d+\w?)(?:.\/.(\w?\s?\d+\w?))?)$";
            string pattern = @"^(?:[a-z]+.[a-z]+.[a-z]*.[a-z]*.[a-z]*.[a-z]*).De?pto(.*)$|^(?:^\d?\s?Asesoría.Financiera.+\(\w+\).De?pto(\w?\s?\d+-?\w?))|^(?:^Asesoría.Financiera.+Proyectos?.+.-.De?ptos?.(\w?\s?\d+\w?)(?:.\/.(\w?\s?\d+\w?))?)$";

            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
            MatchCollection matches = rgx.Matches(text);
            foreach (Match m in matches)
            {
                for (int i = 1; i < m.Groups.Count; i++)
                {
                    string group = m.Groups[i].ToString();
                    if (!string.IsNullOrEmpty(group))
                    {
                        string[] departamentos = group.Split(',');
                        foreach (var item in departamentos)
                        {
                            
                            dptos.Add(item.Replace('\n', ' ').Trim());
                        }
                    }
                }
            }
            return dptos.Distinct().ToList();
        }
    }
}
