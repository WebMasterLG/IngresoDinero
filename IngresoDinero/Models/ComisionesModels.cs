using IngresoDinero.clases;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace IngresoDinero.Models
{
    public class ComisionesModels
    {
        private static readonly string GESTION_PROD20 = Environment.GetEnvironmentVariable("GESTION_PROD20", EnvironmentVariableTarget.Machine);
        private static readonly string GESTION_PROD = Environment.GetEnvironmentVariable("GESTION_PROD", EnvironmentVariableTarget.Machine);
        private static readonly string CORRETAJE = Environment.GetEnvironmentVariable("CORRETAJE", EnvironmentVariableTarget.Machine);

        public static List<listascombobox> ListasCombobox(int id, int? id_seleccion)
        {
            List<listascombobox> lst = new List<listascombobox>();
            string Llama_PA = "EXEC Carga_Combo_Listas_Generales @id";
            using (SqlConnection conn = new SqlConnection(CORRETAJE))
            {
                conn.Open();
                SqlCommand comando = new SqlCommand(Llama_PA, conn);
                comando.Parameters.AddWithValue("@id", id);
                SqlDataReader Resultados = comando.ExecuteReader();
                if (Resultados.HasRows)
                {
                    while (Resultados.Read())
                    {
                        lst.Add(new listascombobox
                        {
                            id = int.Parse(Resultados["id"].ToString()),
                            descripcion = Resultados["descripcion"].ToString()
                        });
                    }
                }
                conn.Close();
            }
            return lst;
        }

        public static List<listascombobox> ListasCombobox2(int id, int? id_seleccion)
        {
            List<listascombobox> lst = new List<listascombobox>();
            string Llama_PA = "EXEC Carga_Combo_Listas_Generales @id";
            using (SqlConnection conn = new SqlConnection(GESTION_PROD))
            {
                conn.Open();
                SqlCommand comando = new SqlCommand(Llama_PA, conn);
                comando.Parameters.AddWithValue("@id", id);
                SqlDataReader Resultados = comando.ExecuteReader();
                if (Resultados.HasRows)
                {
                    while (Resultados.Read())
                    {
                        lst.Add(new listascombobox
                        {
                            id = int.Parse(Resultados["id"].ToString()),
                            descripcion = Resultados["descripcion"].ToString()
                        });
                    }
                }
                conn.Close();
            }
            return lst;
        }

        public static List<listascombobox> ComboboxProyectosGeneral(int usuario)
        {
            List<listascombobox> lst = new List<listascombobox>();
            string Llama_PA = "EXEC Carga_Combo_Proyectos @usuario";
            using (SqlConnection conn = new SqlConnection(CORRETAJE))
            {
                conn.Open();
                SqlCommand comando = new SqlCommand(Llama_PA, conn);
                comando.Parameters.AddWithValue("@usuario", usuario);
                SqlDataReader Resultados = comando.ExecuteReader();
                if (Resultados.HasRows)
                {
                    while (Resultados.Read())
                    {
                        lst.Add(new listascombobox
                        {
                            id = int.Parse(Resultados["id_pry"].ToString()),
                            descripcion = Resultados["g_pry"].ToString()
                        });
                    }
                }
                conn.Close();
            }
            return lst;
        }
        public static DataSet Llama_GeneraExcel(int idUsuario, string sistema, string grilla, string id_promesa, string id_proyecto, string id_estado_factura,
           string id_estado_promesa, DateTime? Fecha_Desde, DateTime? Fecha_Hasta, int idFormato)
        {
            DataSet ds = new DataSet();
            List<HeaderExcelFormato> lst = new List<HeaderExcelFormato>();
            //string Llama_PA = "EXEC  @id_usu, @tipo , @sistema, @grilla, @id_proyecto, @id_tipologia, @id_disponibilidad, @id_ejecutivo, @id_estado, @periodo, @n_ven_con, @sac_inversionista, @FechaPagoIni, @FechaPagoFin";
            using (SqlConnection conn = new SqlConnection(GESTION_PROD))
            {
                using (SqlCommand cmd = new SqlCommand("EXCEL_Genera", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_usu", idUsuario);
                    cmd.Parameters.AddWithValue("@tipo", idFormato);
                    cmd.Parameters.AddWithValue("@sistema", sistema);
                    cmd.Parameters.AddWithValue("@grilla", grilla);
                    cmd.Parameters.AddWithValue("@id_promesa", id_promesa);
                    cmd.Parameters.AddWithValue("@id_proyecto", id_proyecto);
                    cmd.Parameters.AddWithValue("@id_estado_factura", id_estado_factura);
                    cmd.Parameters.AddWithValue("@id_estado_promesa", id_estado_promesa);
                    cmd.Parameters.AddWithValue("@Fecha_Desde", Fecha_Desde);
                    cmd.Parameters.AddWithValue("@Fecha_Hasta", Fecha_Hasta);

                    conn.Open();
                    SqlDataAdapter adp = new SqlDataAdapter();
                    adp.SelectCommand = cmd;
                    adp.Fill(ds);
                }
            }
            return ds;
        }
        public static List<HeaderExcelFormato> Llama_GeneraExcelHeader(int idUsuario, string sistema, string grilla, int id_promesa, int id_proyecto, int id_estado_factura,
           int id_estado_promesa, string Fecha_Desde, string Fecha_Hasta)
        {
            List<HeaderExcelFormato> lst = new List<HeaderExcelFormato>();
            string Llama_PA = "EXEC EXCEL_Genera @id_usu, 0 , @sistema, @grilla, @id_promesa, @id_proyecto, @id_estado_factura, @id_estado_promesa, @Fecha_Desde, @Fecha_Hasta";
            using (SqlConnection conn = new SqlConnection(GESTION_PROD))
            {
                conn.Open();
                SqlCommand comando = new SqlCommand(Llama_PA, conn);
                comando.Parameters.AddWithValue("@id_usu", idUsuario);
                comando.Parameters.AddWithValue("@sistema", sistema);
                comando.Parameters.AddWithValue("@grilla", grilla);
                comando.Parameters.AddWithValue("@id_promesa", id_promesa);
                comando.Parameters.AddWithValue("@id_proyecto", id_proyecto);
                comando.Parameters.AddWithValue("@id_estado_factura", id_estado_factura);
                comando.Parameters.AddWithValue("@id_estado_promesa", id_estado_promesa);
                comando.Parameters.AddWithValue("@Fecha_Desde", Fecha_Desde);
                comando.Parameters.AddWithValue("@Fecha_Hasta", Fecha_Hasta);
                SqlDataReader Resultados = comando.ExecuteReader();
                if (Resultados.HasRows)
                {
                    while (Resultados.Read())
                    {
                        lst.Add(new HeaderExcelFormato
                        {
                            id = (int)Resultados["ID"],
                            nombre = Resultados["Nombre"].ToString()
                        });
                    }
                }
                conn.Close();
            }
            return lst;
        }

        public static List<perfilamiento> Llama_Perfilamiento_Filtros_Default_Front(int id_usuario, string modulo, string vista)
        {
            var resp = new List<perfilamiento>();

            string Llama_PA = "EXEC Perfilamiento_Filtros_Default_Front @id_usu,@modulo,@vista";
            //string Llama_PA = "EXEC Perfilamiento_Filtros_Default @id_usu,@modulo,@vista,'0','0',0";
            using (SqlConnection conn = new SqlConnection(GESTION_PROD))
            {
                conn.Open();
                SqlCommand comando = new SqlCommand(Llama_PA, conn);
                comando.Parameters.AddWithValue("@id_usu", id_usuario);
                comando.Parameters.AddWithValue("@modulo", modulo);
                comando.Parameters.AddWithValue("@vista", vista);

                SqlDataReader Resultados = comando.ExecuteReader();
                while (Resultados.Read())
                {
                    resp.Add(new perfilamiento
                    {
                        g_estado_promesa = Resultados["g_estado_promesa"].ToString(),
                        g_estado_factura = Resultados["g_estado_factura"].ToString()
                    });
                }
                conn.Close();
            }

            return resp;
        }
        public static string Llama_Comision_Inmobiliaria_Actualiza_Estado(int id_usu, int id_promesa, int id_estado_factura, DateTime fecha)
        {
            string retorno = "";
            string Llama_PA = "exec Comision_Inmobiliaria_Actualiza_Estado @id_usu, @id_promesa, @id_estado_factura, @fecha";
            using (SqlConnection conn = new SqlConnection(GESTION_PROD))
            {
                conn.Open();
                SqlCommand comando = new SqlCommand(Llama_PA, conn);
                comando.Parameters.AddWithValue("@id_usu", id_usu);
                comando.Parameters.AddWithValue("@id_promesa", id_promesa);
                comando.Parameters.AddWithValue("@id_estado_factura", id_estado_factura);
                comando.Parameters.AddWithValue("@fecha", fecha);
                SqlDataReader Resultados = comando.ExecuteReader();
                if (Resultados.HasRows)
                {
                    while (Resultados.Read())
                    {
                        retorno = Resultados["estado"].ToString() + "|" + Resultados["mensaje"].ToString() + "|" + Resultados["promesa"].ToString();
                    }
                }
                conn.Close();
            }
            return retorno;
        }
        public static List<subir_archivo> Llama_Comision_Inmobiliaria_Archivo_Adjuntar(int id_promesa, string g_file, int usuario)
        {

            List<subir_archivo> lst = new List<subir_archivo>();
            string Llama_PA = "Comision_Inmobiliaria_Archivo_Adjuntar";
            using (SqlConnection conn = new SqlConnection(GESTION_PROD))
            {
                conn.Open();
                SqlCommand comando = new SqlCommand(Llama_PA, conn);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@id_promesa", id_promesa);
                comando.Parameters.AddWithValue("@g_file", g_file);
                comando.Parameters.AddWithValue("@usuario", usuario);
                SqlDataReader Resultados = comando.ExecuteReader();
                if (Resultados.HasRows)
                {
                    while (Resultados.Read())
                    {
                        lst.Add(new subir_archivo
                        {
                            estado = Resultados["estado"].ToString(),
                            mensaje = Resultados["mensaje"].ToString()
                        });
                    }
                }
                conn.Close();
            }
            return lst;
        }
        public static List<Datos_Inmobiliaria> Llama_Comision_Inmobiliaria_Carga_Inmobiliaria(int id_usu, int id_inmob)
        {
            List<Datos_Inmobiliaria> lst = new List<Datos_Inmobiliaria>();
            string Llama_PA = "Comision_Inmobiliaria_Carga_Inmobiliaria";
            using (SqlConnection conn = new SqlConnection(GESTION_PROD))
            {
                conn.Open();
                SqlCommand comando = new SqlCommand(Llama_PA, conn);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@id_usu", id_usu);
                comando.Parameters.AddWithValue("@id_inmob", id_inmob);
                SqlDataReader Resultados = comando.ExecuteReader();
                if (Resultados.HasRows)
                {
                    while (Resultados.Read())
                    {
                        lst.Add(new Datos_Inmobiliaria
                        {
                            id_inmob = Resultados["id_inmob"].ToString(),
                            g_rut = Resultados["g_rut"].ToString(),
                            g_razon_social = Resultados["g_razon_social"].ToString(),
                            g_giro = Resultados["g_giro"].ToString(),
                            g_direccion = Resultados["g_direccion"].ToString(),
                            id_comuna = Resultados["id_comuna"].ToString(),
                            g_contacto = Resultados["g_contacto"].ToString()

                        });
                    }
                }
                conn.Close();
            }
            return lst;
        }
        public static NuevoEstadoInmobiliaria Comision_Inmobiliaria_Actualiza_Inmobiliaria(int id_usu, Datos_Inmobiliaria inmob)
        {
            var res = new NuevoEstadoInmobiliaria();
            string Llama_PA = "Comision_Inmobiliaria_Actualiza_Inmobiliaria";
            using (SqlConnection conn = new SqlConnection(GESTION_PROD))
            {
                conn.Open();
                SqlCommand comando = new SqlCommand(Llama_PA, conn);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@id_usu", id_usu);
                comando.Parameters.AddWithValue("@id_inmob", inmob.id_inmob);
                comando.Parameters.AddWithValue("@g_rut", inmob.g_rut ?? "");
                comando.Parameters.AddWithValue("@g_razon_social", inmob.g_razon_social);
                comando.Parameters.AddWithValue("@g_giro", inmob.g_giro ?? "");
                comando.Parameters.AddWithValue("@g_direccion", inmob.g_direccion ?? "");
                comando.Parameters.AddWithValue("@id_comuna", inmob.id_comuna ?? "0");
                comando.Parameters.AddWithValue("@g_contacto", inmob.g_contacto ?? "");
                SqlDataReader Resultados = comando.ExecuteReader();
                if (Resultados.HasRows)
                {
                    while (Resultados.Read())
                    {
                        res = new NuevoEstadoInmobiliaria
                        {
                            estado = Resultados["estado"].ToString(),
                            mensaje = Resultados["mensaje"].ToString(),
                            inmobiliaria = Resultados["inmobiliaria"].ToString(),
                        };
                    }
                }
                conn.Close();
            }
            return res;
        }

        public static List<Datos_Proyecto> Llama_Comision_Inmobiliaria_Carga_Proyecto(int id_usu, int id_pry)
        {
            List<Datos_Proyecto> lst = new List<Datos_Proyecto>();
            string Llama_PA = "Comision_Inmobiliaria_Carga_Proyecto";
            using (SqlConnection conn = new SqlConnection(GESTION_PROD))
            {
                conn.Open();
                SqlCommand comando = new SqlCommand(Llama_PA, conn);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@id_usu", id_usu);
                comando.Parameters.AddWithValue("@id_pry", id_pry);
                SqlDataReader Resultados = comando.ExecuteReader();
                if (Resultados.HasRows)
                {
                    while (Resultados.Read())
                    {
                        lst.Add(new Datos_Proyecto
                        {
                            id_pry = Resultados["id_pry"].ToString(),
                            p_com_inmob = Resultados["p_com_inmob"].ToString(),
                            b_com_inmob = Resultados["b_com_inmob"].ToString(),
                            forma_pago = Resultados["forma_pago"].ToString(),
                            condiciones_comisiones = Resultados["condiciones_comision"].ToString(),
                            rut_facturacion = Resultados["rut_facturacion"].ToString(),
                            razon_social_facturacion = Resultados["razon_social_facturacion"].ToString(),
                            giro_facturacion = Resultados["giro_facturacion"].ToString(),
                            direccion_facturacion = Resultados["direccion_facturacion"].ToString(),
                            comuna_facturacion = Resultados["comuna_facturacion"].ToString(),
                            contacto_facturacion = Resultados["contacto_facturacion"].ToString(),
                            flag_facturacion = Resultados["flag_facturacion"].ToString()

                        });
                    }
                }
                conn.Close();
            }
            return lst;
        }
        public static NuevoEstadoProyecto Comision_Inmobiliaria_Actualiza_Proyecto(int id_usu, Datos_Proyecto pry)
        {
            var res = new NuevoEstadoProyecto();
            string Llama_PA = "Comision_Inmobiliaria_Actualiza_Proyecto";
            using (SqlConnection conn = new SqlConnection(GESTION_PROD))
            {
                conn.Open();
                SqlCommand comando = new SqlCommand(Llama_PA, conn);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@id_usu", id_usu);
                comando.Parameters.AddWithValue("@id_pry", pry.id_pry);
                comando.Parameters.AddWithValue("@p_com_inmob", pry.p_com_inmob);
                comando.Parameters.AddWithValue("@b_com_inmob", pry.b_com_inmob);
                comando.Parameters.AddWithValue("@forma_pago", pry.forma_pago);
                comando.Parameters.AddWithValue("@condiciones_comision", pry.condiciones_comisiones);

                comando.Parameters.AddWithValue("@rut_facturacion", pry.rut_facturacion);
                comando.Parameters.AddWithValue("@razon_social_facturacion", pry.razon_social_facturacion);
                comando.Parameters.AddWithValue("@giro_facturacion", pry.giro_facturacion);
                comando.Parameters.AddWithValue("@direccion_facturacion", pry.direccion_facturacion);
                comando.Parameters.AddWithValue("@comuna_facturacion", pry.comuna_facturacion);
                comando.Parameters.AddWithValue("@contacto_facturacion", pry.contacto_facturacion);
                comando.Parameters.AddWithValue("@flag_facturacion", pry.flag_facturacion);
                SqlDataReader Resultados = comando.ExecuteReader();
                if (Resultados.HasRows)
                {
                    while (Resultados.Read())
                    {
                        res = new NuevoEstadoProyecto
                        {
                            estado = Resultados["estado"].ToString(),
                            mensaje = Resultados["mensaje"].ToString(),
                            proyecto = Resultados["proyecto"].ToString(),
                        };
                    }
                }
                conn.Close();
            }
            return res;
        }

        public static NuevoValorComision Comision_Inmobiliaria_Actualiza_Comision(int id_usu, int id_prom, string comision_uf)
        {
            var res = new NuevoValorComision();
            string Llama_PA = "Comision_Inmobiliaria_Actualiza_Comision";
            using (SqlConnection conn = new SqlConnection(GESTION_PROD))
            {
                conn.Open();
                SqlCommand comando = new SqlCommand(Llama_PA, conn);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@id_usu", id_usu);
                comando.Parameters.AddWithValue("@id_prom", id_prom);
                comando.Parameters.AddWithValue("@comision_uf", comision_uf);
                SqlDataReader Resultados = comando.ExecuteReader();
                if (Resultados.HasRows)
                {
                    while (Resultados.Read())
                    {
                        res = new NuevoValorComision
                        {
                            estado = Resultados["estado"].ToString(),
                            mensaje = Resultados["mensaje"].ToString(),
                            promesa = Resultados["promesa"].ToString(),
                        };
                    }
                }
                conn.Close();
            }
            return res;
        }
    }
}