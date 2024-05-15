using IngresoDinero.Models;
using IngresoDinero.clases;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace IngresoDinero.Controllers
{
    public class IngresoDineroController : Controller
    {
        
        public ActionResult Index()
        {
            if (Request.Cookies["cookiePerfil"] == null)
            {
                /*ATENCIÓN: Esta variable de entorno NO debe crearse en PRODUCCIÓN */
                string _cookiePerfil = Environment.GetEnvironmentVariable("cookiePerfil", EnvironmentVariableTarget.Machine);
                if (_cookiePerfil != null)
                {
                    HttpCookie cookiePerfil = new HttpCookie("cookiePerfil", _cookiePerfil);
                    cookiePerfil.Expires = DateTime.Now.AddDays(7);
                    Response.Cookies.Add(cookiePerfil);
                }
                else
                {
                    return View("_ErrorPerfil");
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult _ListaIngresoDinero(string n_rut_cliente, string id_estado, string id_forma_pago,string id_tipo, string fecha_desde, string fecha_hasta, string empresa, string n_rut_cliente_facturar,string ids_cotizaciones, int proc, int regularizados)
        {
            if (Request.Cookies["cookiePerfil"] == null || Request.Cookies["cookiePerfil"]["usuario"] == null) return Content("Debe autenticarse en Cotizaciones.");

            int idUsuario = int.Parse(Request.Cookies["cookiePerfil"]["usuario"].ToString());
            //int idUsuario = 10008;
            string fecha_desde_format = fecha_desde + "-01";
            DateTime fecha_hasta_date = DateTime.Parse(fecha_hasta + "-01");
            int lastDay = DateTime.DaysInMonth(fecha_hasta_date.Year, fecha_hasta_date.Month);
            string fecha_hasta_format = fecha_hasta + "-" + lastDay.ToString();
          
            return PartialView(IngresoDineroModel.Llama_Ingreso_Dinero_Carga_Lista(idUsuario, n_rut_cliente, id_estado, id_forma_pago,id_tipo, fecha_desde_format, fecha_hasta_format, empresa, n_rut_cliente_facturar,ids_cotizaciones, proc, regularizados));
        }
    
        [HttpPost]
        public ActionResult ExportarExcelInterfazIngresoDinero(FormCollection f)
        {
            if (Request.Cookies["cookiePerfil"] == null || Request.Cookies["cookiePerfil"]["usuario"] == null) return Content("Debe autenticarse en Cotizaciones.");

            int IdUsuario = int.Parse(string.IsNullOrEmpty(Request.Cookies["cookiePerfil"]["usuario"].ToString()) == true ? "10008" : Request.Cookies["cookiePerfil"]["usuario"].ToString());
            //int IdUsuario = 10008;
            int idFormato = int.Parse(string.IsNullOrEmpty(f["idFormatoInterfazCobro"]) == true ? "0" : f["idFormatoInterfazCobro"].ToString());
            string grilla = f["grilla"].ToString();


            int cliente = int.Parse(string.IsNullOrEmpty(f["ClienteInterfaz"]) == true ? "0" : f["ClienteInterfaz"].ToString());
            int estado = int.Parse(string.IsNullOrEmpty(f["EstadoInterfaz"]) == true ? "0" : f["EstadoInterfaz"].ToString());
            int forma_pago = int.Parse(string.IsNullOrEmpty(f["FormaPagoInterfaz"]) == true ? "0" : f["FormaPagoInterfaz"].ToString());
            int id_tipo_ingreso = int.Parse(string.IsNullOrEmpty(f["idTipoIngreso"]) == true ? "0" : f["idTipoIngreso"].ToString());

            GridView gv = new GridView();
            if (!string.IsNullOrEmpty(f["FechaInicialInterfaz"]) && !string.IsNullOrEmpty(f["FechaInicialInterfaz"]))
            {
                DateTime fechIni = DateTime.ParseExact(f["FechaInicialInterfaz"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime fechFin = DateTime.ParseExact(f["FechaFinalInterfaz"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                gv.DataSource = IngresoDineroModel.Llama_GeneraExcelInterfazCobro(IdUsuario, "INGRESO_DINERO", grilla, idFormato, cliente, estado, forma_pago, id_tipo_ingreso, fechIni, fechFin);
            }
            else
            {
                gv.DataSource = IngresoDineroModel.Llama_GeneraExcelInterfazCobro(IdUsuario, "INGRESO_DINERO", grilla, idFormato, cliente, estado, forma_pago, id_tipo_ingreso, null, null);
            }

            var lstInformes = IngresoDineroModel.Llama_GeneraExcelHeaderInterfaz(IdUsuario, "INGRESO_DINERO", grilla);

            var informe = lstInformes.Where(inf => inf.id == idFormato)?.First();

            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + informe.archivo);
            Response.ContentType = "application/vnd.ms-excel";

            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);

            gv.RenderControl(objHtmlTextWriter);

            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();

            return null;
        }


        [HttpGet]
        public JsonResult FormatosExcelInterfaz(string grilla, string fechaI, string fechaF)
        {
            if (Request.Cookies["cookiePerfil"] == null || Request.Cookies["cookiePerfil"]["usuario"] == null)
                return Json(new
                {
                    resp = new List<HeaderExcelFormato>()
                }, JsonRequestBehavior.AllowGet);
            int usuario = int.Parse(Request.Cookies["cookiePerfil"]["usuario"].ToString());
            //int usuario = 10008;
            //DateTime fechIni = DateTime.ParseExact(fechaI.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            //DateTime fechFin = DateTime.ParseExact(fechaF.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

            return Json(new { resp = IngresoDineroModel.Llama_GeneraExcelHeaderInterfaz(usuario, "INGRESO_DINERO", grilla/*, fechIni, fechFin*/) }, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public ActionResult Cambia_Estado_Ingreso_Dinero_2(string chk_seleccionados, int IdEstado)
        //{
        //    //int usuario = int.Parse(Request.Cookies["cookiePerfil"]["usuario"].ToString());
        //    int usuario = 10008;
        //    IngresoDineroModel.LlamaCambioEstado(usuario, chk_seleccionados, IdEstado, "");
        //    return null;
        //}


        [HttpPost]
        public ActionResult _Cambia_Estado_Ingreso_Dinero(string chk_seleccionados, int IdEstado, string fecha, string comentario, string rut_facturar)
        {
            if (Request.Cookies["cookiePerfil"] == null || Request.Cookies["cookiePerfil"]["usuario"] == null) return Content("Debe autenticarse en Cotizaciones.");

            int usuario = int.Parse(Request.Cookies["cookiePerfil"]["usuario"].ToString());
            //int usuario = 10008;
            List<EstadoIngresoDinero> final = new List<EstadoIngresoDinero>();

            //string[] fec = fecha.Split('-');
            //string fecha2 = fec[2] + "/" + fec[1] + "/" + fec[0];

            DateTime fechacambio = DateTime.ParseExact(fecha.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            List<EstadoIngresoDinero> lst1 = IngresoDineroModel.Llama_Pagos_CambiarEstado_Ingreso_Dinero(usuario, chk_seleccionados, IdEstado, fechacambio,  comentario, rut_facturar);
                foreach (var item in lst1)
                {
                    final.Add(new EstadoIngresoDinero
                    {

                        fechacambioestado = item.fechacambioestado,
                        g_usu= item.g_usu,
                        mensaje= item.mensaje,
                        g_estado_anterior = item.g_estado_anterior,
                        g_estado_nuevo = item.g_estado_nuevo,
                        f_estado = item.f_estado,
                        monto=item.monto
       
                    });
                }

            ViewBag.fecha = fecha;

            return PartialView(final);
        }



        [HttpPost]
        public ActionResult _Cambia_Estado_Ingreso_Dinero_2_Cobros(string chk_seleccionados, int IdEstado, string fecha, string comentario)
        {
            if (Request.Cookies["cookiePerfil"] == null || Request.Cookies["cookiePerfil"]["usuario"] == null) return Content("Debe autenticarse en Cotizaciones.");

            int usuario = int.Parse(Request.Cookies["cookiePerfil"]["usuario"].ToString());
            //int usuario = 10008;
            List<EstadoIngresoDinero> final = new List<EstadoIngresoDinero>();

            //string[] fec = fecha.Split('-');
            //string fecha2 = fec[2] + "/" + fec[1] + "/" + fec[0];

            DateTime fechacambio = DateTime.ParseExact(fecha.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            List<EstadoIngresoDinero> lst1 = IngresoDineroModel.Llama_Pagos_CambiarEstado_Ingreso_Dinero_2_Cobros(usuario, chk_seleccionados, IdEstado, fechacambio, comentario);
            foreach (var item in lst1)
            {
                final.Add(new EstadoIngresoDinero
                {

                    fechacambioestado = item.fechacambioestado,
                    g_usu = item.g_usu,
                    mensaje = item.mensaje,
                    g_estado_anterior = item.g_estado_anterior,
                    g_estado_nuevo = item.g_estado_nuevo,
                    f_estado = item.f_estado,
                    monto = item.monto

                });
            }

            return PartialView(final);
        }

        [HttpGet]
        public JsonResult HistorialObservacionesIngresoDinero(int id_ingreso)
        {
            if (Request.Cookies["cookiePerfil"] == null || Request.Cookies["cookiePerfil"]["usuario"] == null)
                return Json(new
                {
                    resp = new List<ObservacionIngresoDinero>()
                }, JsonRequestBehavior.AllowGet);
            return Json(new { resp = IngresoDineroModel.Llama_HistorialIngresoDinero(id_ingreso) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult _ObservacionesIngresoDinero(int id_ingreso)
        {
            if (Request.Cookies["cookiePerfil"] == null || Request.Cookies["cookiePerfil"]["usuario"] == null) return Content("Debe autenticarse en Cotizaciones.");
            ViewBag.id_ingreso = id_ingreso;
            return PartialView(IngresoDineroModel.Llama_CargaIngresoDinero(id_ingreso));
        }

        [HttpGet]
        public ActionResult _Ingreso_Regularizacion(int id_ingreso)
        {
            if (Request.Cookies["cookiePerfil"] == null || Request.Cookies["cookiePerfil"]["usuario"] == null) return Content("Debe autenticarse en Cotizaciones.");
            ViewBag.id_ingreso = id_ingreso;
            return PartialView(IngresoDineroModel.Llama_CargaIngresoDinero(id_ingreso));
        }

        [HttpGet]
        public ActionResult _Agregar_Ingreso_Dinero(int id_ingreso)
        {
            if (Request.Cookies["cookiePerfil"] == null || Request.Cookies["cookiePerfil"]["usuario"] == null) return Content("Debe autenticarse en Cotizaciones.");

            ViewBag.id_ingreso = id_ingreso;

            return PartialView(IngresoDineroModel.Llama_CargaIngresoDinero(id_ingreso));
        }

        [HttpGet]
        public JsonResult Carga_Datos_Cliente(int ing_rut_cliente)
        {
            if (Request.Cookies["cookiePerfil"] == null || Request.Cookies["cookiePerfil"]["usuario"] == null)
                return Json(new
                {
                    resp = new List<ObservacionIngresoDinero>()
                }, JsonRequestBehavior.AllowGet);

            return Json(new { resp = IngresoDineroModel.Llama_Carga_Cliente(ing_rut_cliente) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Carga_Clientes_A_Facturar(string id_ing)
        {
            if (Request.Cookies["cookiePerfil"] == null || Request.Cookies["cookiePerfil"]["usuario"] == null)
                return Json(new
                {
                    resp = new List<ObservacionIngresoDinero>()
                }, JsonRequestBehavior.AllowGet);

            return Json(MaestrosModel.ComboboxClienteGeneralAFacturar(id_ing), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult _Agregar_Observacion_IngresoDinero(int proc, int id_ingreso, string observacion, string g_nom_usu)
        {
            if (Request.Cookies["cookiePerfil"] == null || Request.Cookies["cookiePerfil"]["usuario"] == null)
                return Json(new
                {
                    Resultado = new
                    {
                        estado = "false",
                        mensaje = "Debe autenticarse en Cotizaciones.",
                    }
                });

            int IdUsuario = int.Parse(Request.Cookies["cookiePerfil"]["usuario"].ToString());
            //int IdUsuario = 10008;
            return Json(new { Resultado = IngresoDineroModel.Llama_Ingreso_Actualiza_Observacion_Graba(proc, id_ingreso, IdUsuario, observacion, g_nom_usu) });
        }


        [HttpPost]
        public JsonResult Ingreso_Regulariza(List<IngresosDinero> lstIng)
        {
            if (Request.Cookies["cookieperfil"] == null || Request.Cookies["cookieperfil"]["usuario"] == null)
                return Json(new
                {
                    estado = false,
                    mensaje = "debe autenticarse en cotizaciones.",
                });

            int IdUsuario = int.Parse(Request.Cookies["cookieperfil"]["usuario"].ToString());

            //int IdUsuario = 10008;

            int proc = 0;
            for (int i = 0; i < lstIng.Count; i++)
            {
                var ing = lstIng[i];
                if (i > 0)
                    proc = 1;

                IngresoDineroModel.Llama_Ingreso_Regulariza(IdUsuario, proc, int.Parse(ing.id_ing), ing.f_fecha, ing.rut_cliente, ing.nombre_cliente, int.Parse(ing.v_monto), int.Parse(ing.id_forma_pago), ing.f_vencimiento, int.Parse(ing.id_abonable_pie), int.Parse(ing.id_banco), ing.g_cta_cte ?? "", ing.g_serie ?? "");
            }

            return Json(new
            {
                estado = true,
            });
        }

        [HttpPost]
        public JsonResult Ingreso_Guardar_Ingreso(int proc,int id_ing,int id_tipo,int ing_rut_cliente,string g_dv,string ing_nombre_cliente,string ing_g_ape_pat,string ing_g_ape_mat,string ing_f_fecha_cambio_estado,string ing_g_direccion_cli,string ing_f_vencimiento,string ing_g_observacion_ingreso,string ing_g_direccion_cli_numero,int id_plaza,int id_empresa,int id_forma_pago, string g_cta_cte, string g_serie, int id_banco = 0, int id_comuna = 0, string ing_monto_uf = "", int ing_v_monto = 0)
        {
            if (Request.Cookies["cookiePerfil"] == null || Request.Cookies["cookiePerfil"]["usuario"] == null)
                return Json(new
                {
                    estado = false,
                    mensaje = "Debe autenticarse en Cotizaciones.",
                });

            int idUsuario = int.Parse(Request.Cookies["cookiePerfil"]["usuario"].ToString());
            //int idUsuario = 10008;
            //string fecha_desde_format = fecha_desde + "-01";
            //string fecha_hasta_format = fecha_hasta + "-30";

            DateTime? fechacambioestado = !string.IsNullOrEmpty(ing_f_fecha_cambio_estado) ? DateTime.ParseExact(ing_f_fecha_cambio_estado, "yyyy-MM-dd", CultureInfo.InvariantCulture) : (DateTime?)null;
            DateTime? fechavencimiento = !string.IsNullOrEmpty(ing_f_vencimiento) ? DateTime.ParseExact(ing_f_vencimiento, "yyyy-MM-dd", CultureInfo.InvariantCulture) : (DateTime?)null;
            float monto_uf = 0;
            if(ing_f_vencimiento == "")
            {
                fechavencimiento = fechacambioestado;
            }
            //int monto_peso = 0;
            if(ing_monto_uf!="")
            {
                 monto_uf = float.Parse(ing_monto_uf);
            }
            else
            {
                 monto_uf = 0;
            }
            
            int id = IngresoDineroModel.Llama_Ingreso_Guardar_Ingreso(proc,idUsuario, id_ing, id_tipo, ing_rut_cliente, g_dv, ing_nombre_cliente,ing_g_ape_pat,ing_g_ape_mat, monto_uf, ing_v_monto, fechacambioestado, ing_g_direccion_cli, fechavencimiento, ing_g_observacion_ingreso, ing_g_direccion_cli_numero,id_plaza,id_empresa, id_forma_pago, id_comuna, id_banco, g_cta_cte, g_serie);

            return Json(new
            {
                estado = true,
                id_ing = id
            });
        }

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
        public JsonResult Subir_Archivo_Ingreso(int id_ing, HttpPostedFileBase file)
        {
            if (Request.Cookies["cookiePerfil"] == null || Request.Cookies["cookiePerfil"]["usuario"] == null)
                return Json(new
                {
                    estado = false,
                    mensaje = "Debe autenticarse en Cotizaciones.",
                });

            int idUsuario = int.Parse(Request.Cookies["cookiePerfil"]["usuario"].ToString());
            //int idUsuario = 10008;

            // Valida que el archivo exista y que no esté vacío
            if (file != null && file.ContentLength == 0)
                return Json(new
                {
                    estado = false,
                    mensaje = "Archivo vacío"
                });

            string dirDoc = Server.MapPath("~/Documentos");

            // Crea la carpeta si no existe
            if (!Directory.Exists(dirDoc))
                Directory.CreateDirectory(dirDoc);

            // Obtiene el archivo anterior si se cambia el archivo
            string archivo_anterior = "";
            var ing = IngresoDineroModel.Llama_CargaIngresoDinero(id_ing);

            if (ing.Count > 0 && !string.IsNullOrEmpty(ing[0].g_file))
            {
                archivo_anterior = ing[0].g_file;
                archivo_anterior = Path.Combine(dirDoc, archivo_anterior);
            }

            string nombreArchivo = Path.GetFileName(file.FileName);

            nombreArchivo = NombreArchivoUrl(nombreArchivo);

            nombreArchivo = id_ing.ToString() + "_" + nombreArchivo;

            // Establece un nombre temporal para la subida
            string nombreTemporal = "__TEMP__" + nombreArchivo;
            string tempPath = Path.Combine(dirDoc, nombreTemporal);

            string nombreArchivoSinExt = Path.GetFileNameWithoutExtension(Path.Combine(dirDoc, nombreArchivo));
            string extension = Path.GetExtension(Path.Combine(dirDoc, nombreArchivo));

            nombreArchivo = nombreArchivoSinExt + extension;

            // Renombra el nombre del archivo a subir si el nombre del nuevo archivo existe en la carpeta
            int i = 0;
            while (System.IO.File.Exists(Path.Combine(dirDoc, nombreArchivo)))
            {
                i++;
                nombreArchivo = nombreArchivoSinExt + "_" + i.ToString() + extension;
            }

            string destPath = Path.Combine(dirDoc, nombreArchivo);

            // Elimina el archivo temporal si existe
            if (System.IO.File.Exists(tempPath))
                System.IO.File.Delete(tempPath);

            // Guarda el archivo temporal
            try
            {
                file.SaveAs(tempPath);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    estado = false,
                    mensaje = "Error al procesar el archivo \"" + file.FileName + "\": " + ex.ToString()
                });
            }

            string g_file = nombreArchivo;

            // Registra el archivo al catálogo
            try
            {
                IngresoDineroModel.Llama_Ingreso_Guardar_Ingreso_Archivo(idUsuario, id_ing, g_file);

                // Renombra el archivo temporal al archivo definitivo
                System.IO.File.Move(tempPath, destPath);

                // Si existe el archivo anterior registrado, se procede a eliminarlo
                if (archivo_anterior != "" && System.IO.File.Exists(archivo_anterior))
                    System.IO.File.Delete(archivo_anterior);

                return Json(new
                {
                    estado = true,
                    mensaje = "Archivo subido!"
                });
            }
            catch (Exception ex)
            {
                // Elimina el archivo temporal en caso de algún error
                System.IO.File.Delete(tempPath);

                return Json(new
                {
                    estado = false,
                    mensaje = "Error al registrar el archivo \"" + file.FileName + "\": " + ex.ToString()
                });
            }
        }
    }
}