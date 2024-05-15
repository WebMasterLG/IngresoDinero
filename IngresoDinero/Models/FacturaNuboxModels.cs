using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using IngresoDinero.clases;
using IngresoDinero.Helpers;

namespace IngresoDinero.Models
{
    public class FacturaNubox
    {
        private static readonly string GESTION_PROD20 = Environment.GetEnvironmentVariable("GESTION_PROD20", EnvironmentVariableTarget.Machine);
        private static readonly string GESTION_PROD = Environment.GetEnvironmentVariable("GESTION_PROD", EnvironmentVariableTarget.Machine);
        private static readonly string CORRETAJE = Environment.GetEnvironmentVariable("CORRETAJE", EnvironmentVariableTarget.Machine);

        public static List<ListadoCargaFacturas> Llama_Facturas_NBX_Cargar(int proc,string f_emision, string f_vencimiento, int folio_factura, string g_archivo_factura, int id_usuario,string proyecto, string depto, string rut,int neto,int iva,int exento,int total,string descripcion_factura)
        {
            
            List<ListadoCargaFacturas> lst = new List<ListadoCargaFacturas>();
            string Llama_PA = "INGRESO_DINERO_Facturas_NBX_Cargar";
            using (SqlConnection conn = new SqlConnection(GESTION_PROD))
            {
                
                SqlCommand cmd = new SqlCommand(Llama_PA, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@proc", proc);
                cmd.Parameters.AddWithValue("@f_emision", f_emision);
                cmd.Parameters.AddWithValue("@f_vencimiento", f_vencimiento);
                cmd.Parameters.AddWithValue("@folio_factura", folio_factura);
                cmd.Parameters.AddWithValue("@g_archivo_factura", g_archivo_factura);
                cmd.Parameters.AddWithValue("@id_usuario", id_usuario);
                cmd.Parameters.AddWithValue("@proyecto", proyecto);
                cmd.Parameters.AddWithValue("@depto", depto);
                cmd.Parameters.AddWithValue("@rut", rut);
                cmd.Parameters.AddWithValue("@neto", neto);
                cmd.Parameters.AddWithValue("@iva", iva);
                cmd.Parameters.AddWithValue("@exento", exento);
                cmd.Parameters.AddWithValue("@total", total);
                cmd.Parameters.AddWithValue("@descripcion_factura", descripcion_factura);

                conn.Open();
                using (SqlDataReader Resultados = cmd.ExecuteReader())
                {
                    if (Resultados.HasRows)
                    {
                        lst = Resultados.ToList<ListadoCargaFacturas>();
                    }
                }
            }
            return lst;
        }

        public static List<ListadoCargaFacturas> Llama_Facturas_NBX_Cargar_Factura(int id_usu, int proc, int id_ing, string f_emision = null, string f_vencimiento = null, int? folio_factura = null, string g_archivo_factura = null, string proyecto = null, string depto = null, string rut = null, int? neto = null, int? iva = null, int? exento = null, int? total = null, string descripcion_factura = null)
        {
            List<ListadoCargaFacturas> lst = new List<ListadoCargaFacturas>();
            string Llama_PA = "INGRESO_DINERO_Facturas_NBX_Cargar_Factura";
            using (SqlConnection conn = new SqlConnection(GESTION_PROD))
            {
                SqlCommand cmd = new SqlCommand(Llama_PA, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_usu", id_usu);
                cmd.Parameters.AddWithValue("@proc", proc);
                cmd.Parameters.AddWithValue("@id_ing", id_ing);
                if (f_emision != null)
                    cmd.Parameters.AddWithValue("@f_emision", f_emision);
                if (f_vencimiento != null)
                    cmd.Parameters.AddWithValue("@f_vencimiento", f_vencimiento);
                if (folio_factura != null)
                    cmd.Parameters.AddWithValue("@folio_factura", folio_factura);
                if (g_archivo_factura != null)
                    cmd.Parameters.AddWithValue("@g_archivo_factura", g_archivo_factura);
                if (proyecto != null)
                    cmd.Parameters.AddWithValue("@proyecto", proyecto);
                if (depto != null)
                    cmd.Parameters.AddWithValue("@depto", depto);
                if (rut != null)
                    cmd.Parameters.AddWithValue("@rut", rut);
                if (neto != null)
                    cmd.Parameters.AddWithValue("@neto", neto);
                if (iva != null)
                    cmd.Parameters.AddWithValue("@iva", iva);
                if (exento != null)
                    cmd.Parameters.AddWithValue("@exento", exento);
                if (total != null)
                    cmd.Parameters.AddWithValue("@total", total);
                if (descripcion_factura != null)
                    cmd.Parameters.AddWithValue("@descripcion_factura", descripcion_factura);

                conn.Open();
                using (SqlDataReader Resultados = cmd.ExecuteReader())
                {
                    if (Resultados.HasRows)
                    {
                        lst = Resultados.ToList<ListadoCargaFacturas>();
                    }
                }
                conn.Close();
            }
            return lst;
        }
    }
}