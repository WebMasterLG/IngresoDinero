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
    public class MaestrosModel
    {
        private static readonly string GESTION_PROD20 = Environment.GetEnvironmentVariable("GESTION_PROD20", EnvironmentVariableTarget.Machine);
        private static readonly string GESTION_PROD = Environment.GetEnvironmentVariable("GESTION_PROD", EnvironmentVariableTarget.Machine);
        private static readonly string CORRETAJE = Environment.GetEnvironmentVariable("CORRETAJE", EnvironmentVariableTarget.Machine);

        public static List<listascombobox> ComboboxClienteGeneral()
        {
            List<listascombobox> lst = new List<listascombobox>();
            string Llama_PA = "EXEC INGRESO_DINERO_Carga_Combo_Clientes";
            using (SqlConnection conn = new SqlConnection(GESTION_PROD))
            {
                conn.Open();
                SqlCommand comando = new SqlCommand(Llama_PA, conn);
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

        public static List<listascombobox> ComboboxClienteGeneralAFacturar()
        {
            List<listascombobox> lst = new List<listascombobox>();
            string Llama_PA = "EXEC INGRESO_DINERO_Carga_Combo_Clientes_A_Facturar";
            using (SqlConnection conn = new SqlConnection(GESTION_PROD))
            {
                conn.Open();
                SqlCommand comando = new SqlCommand(Llama_PA, conn);
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

        public static List<listascombobox> ComboboxClienteGeneralAFacturar(string id_ing)
        {
            List<listascombobox> lst = new List<listascombobox>();
            string Llama_PA = "EXEC INGRESO_DINERO_Carga_Combo_Clientes_A_Facturar @id_ing";
            using (SqlConnection conn = new SqlConnection(GESTION_PROD))
            {
                conn.Open();
                SqlCommand comando = new SqlCommand(Llama_PA, conn);
                comando.Parameters.AddWithValue("@id_ing", id_ing);
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

        public static List<Lista> CargarLista(int idLista)
        {
            List<Lista> lstLista = new List<Lista>();
            SqlDataAdapter adp = new SqlDataAdapter("exec Carga_Combo_Listas_Generales " + idLista.ToString(), CORRETAJE);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                lstLista.Add(new Lista
                {
                    IdLista = row["id"].ToString(),
                    Descripcion = row["descripcion"].ToString()
                });

            }
            return lstLista;
        }

        public static List<Lista> CargaListaCotizaciones(int idLista)
        {
            List<Lista> lstLista = new List<Lista>();
            SqlDataAdapter adp = new SqlDataAdapter("exec Carga_Listas " + idLista.ToString(),
                GESTION_PROD);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                lstLista.Add(new Lista
                {
                    IdLista = row["c_item"].ToString(),
                    Descripcion = row["g_glosa1"].ToString(),
                    NValor1 = row["n_valor1"].ToString(),
                });

            }
            return lstLista;
        }

        public static List<Lista> CargaInmobAsesor(int rut)
        {
            List<Lista> lstLista = new List<Lista>();
            SqlDataAdapter adp = new SqlDataAdapter("exec Carga_Combo_Inmobiliaria_Asesor " + rut, CORRETAJE);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                lstLista.Add(new Lista
                {
                    IdLista = row["c_item"].ToString(),
                    Descripcion = row["g_glosa1"].ToString()
                });

            }
            return lstLista;
        }


        public static List<Lista> CargaComunas(int idRegion)
        {
            List<Lista> lstLista = new List<Lista>();
            SqlDataAdapter adp = new SqlDataAdapter("exec dbo.Carga_Comunas " + idRegion.ToString(), CORRETAJE);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                lstLista.Add(new Lista
                {
                    IdLista = row["c_item"].ToString(),
                    Descripcion = row["g_glosa1"].ToString()
                });

            }
            return lstLista;
        }

        public static List<Lista> CargaProyectos(int idInmobiliaria)
        {
            List<Lista> lstLista = new List<Lista>();
            SqlDataAdapter adp = new SqlDataAdapter("exec dbo.Carga_Combo_Proyectos " + idInmobiliaria.ToString(), CORRETAJE);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                lstLista.Add(new Lista
                {
                    IdLista = row["id_pry"].ToString(),
                    Descripcion = row["g_nom_pry"].ToString()
                });

            }
            return lstLista;
        }

        public static List<Lista> CargaProyectosUsuario(int idInmobiliaria, int rut)
        {
            List<Lista> lstLista = new List<Lista>();
            SqlDataAdapter adp = new SqlDataAdapter("exec Carga_Combo_Proyectos_asesor " + idInmobiliaria + ", " + rut, CORRETAJE);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                lstLista.Add(new Lista
                {
                    IdLista = row["id_pry"].ToString(),
                    Descripcion = row["g_nom_pry"].ToString()
                });

            }
            return lstLista;
        }

        public static List<Lista> CargaUnidades(int idProyecto)
        {
            List<Lista> lstLista = new List<Lista>();
            SqlDataAdapter adp = new SqlDataAdapter("exec dbo.Carga_Combo_Propiedad " + idProyecto.ToString(),
                CORRETAJE);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                lstLista.Add(new Lista
                {
                    IdLista = row["id_prop"].ToString(),
                    Descripcion = row["n_dep"].ToString()
                });

            }
            return lstLista;
        }

        public static List<Lista> Giros_SII(int usuario)
        {
            List<Lista> lstLista = new List<Lista>();
            SqlDataAdapter adp = new SqlDataAdapter("exec dbo.Carga_Combo_Giros " + usuario.ToString(), CORRETAJE);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                lstLista.Add(new Lista
                {
                    IdLista = row["c_giro"].ToString(),
                    Descripcion = row["glosa"].ToString()
                });

            }
            return lstLista;
        }

        public static estados ReportarErrorInterno(string url, string html, string usu = "")
        {
            estados res = new estados();
            string sql = "EXEC dbo.Reportar_Error_Interno @url, @html, @usu";
            using (SqlConnection conn = new SqlConnection(GESTION_PROD))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@url", url);
                cmd.Parameters.AddWithValue("@html", html);
                cmd.Parameters.AddWithValue("@usu", usu);
                SqlDataReader Resultado = cmd.ExecuteReader();
                if (Resultado.HasRows)
                {
                    while (Resultado.Read())
                    {
                        res = new estados
                        {
                            estado = Resultado["estado"].ToString(),
                            mensaje = Resultado["mensaje"].ToString(),
                        };
                    }
                }
                conn.Close();
            }
            return res;
        }

        public static List<TipoArchivo> TipoArchivo(string tipo)
        {
            var res = new List<TipoArchivo>();
            string sql = "EXEC CORRETAJE.dbo.Catalogo_Carga_Combo_Tipo @tipo";
            using (SqlConnection conn = new SqlConnection(GESTION_PROD))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@tipo", tipo);
                SqlDataReader Resultado = cmd.ExecuteReader();
                if (Resultado.HasRows)
                {
                    while (Resultado.Read())
                    {
                        res.Add(new TipoArchivo
                        {
                            id = Resultado["id"].ToString(),
                            glosa = Resultado["nombre"].ToString(),
                        });
                    }
                }
                conn.Close();
            }
            return res;
        }

        public static bool RegistrarArchivoDBMasivo(int usuario, int id_prop, int tipo_doc, string descripcion, string nombre_archivo, string rut, string id_contrato, string fecha, int proc, int id_catalogo)
        {
            string Llama_PA = "EXEC Catalogo_Ingresa_Registro_Masivo @id_usu, @id_prop, @c_tipo, @descrip, @fecha, '1900-01-01', @archivo, @rut, @id_contrato,@proc,@id_catalogo";
            using (SqlConnection conn = new SqlConnection(CORRETAJE))
            {
                conn.Open();
                SqlCommand comando = new SqlCommand(Llama_PA, conn);
                comando.Parameters.AddWithValue("@id_usu", usuario);
                comando.Parameters.AddWithValue("@id_prop", id_prop);
                comando.Parameters.AddWithValue("@c_tipo", tipo_doc);
                comando.Parameters.AddWithValue("@descrip", descripcion);
                comando.Parameters.AddWithValue("@fecha", Convert.ToDateTime(fecha));
                //comando.Parameters.AddWithValue("@f_ter_per", DBNull.Value);
                comando.Parameters.AddWithValue("@archivo", nombre_archivo);
                comando.Parameters.AddWithValue("@proc", proc);
                comando.Parameters.AddWithValue("@id_catalogo", id_catalogo);
                comando.Parameters.AddWithValue("@rut", rut);
                comando.Parameters.AddWithValue("@id_contrato", id_contrato);

                SqlDataReader Resultados = comando.ExecuteReader();
                conn.Close();
            }
            return true;
        }
    }
}