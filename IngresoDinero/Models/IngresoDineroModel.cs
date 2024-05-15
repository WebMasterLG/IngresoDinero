using IngresoDinero.clases;
using IngresoDinero.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace IngresoDinero.Models
{
    public class IngresoDineroModel
    {
        private static readonly string GESTION_PROD20 = Environment.GetEnvironmentVariable("GESTION_PROD20", EnvironmentVariableTarget.Machine);
        private static readonly string GESTION_PROD = Environment.GetEnvironmentVariable("GESTION_PROD", EnvironmentVariableTarget.Machine);
        private static readonly string CORRETAJE = Environment.GetEnvironmentVariable("CORRETAJE", EnvironmentVariableTarget.Machine);

        public static List<IngresosDinero> Llama_Ingreso_Dinero_Carga_Lista(int id_usu, string n_rut_cliente, string id_estado, string id_forma_pago,string id_tipo, string fecha_desde, string fecha_hasta, string empresa, string n_rut_cliente_facturar,string ids_cotizaciones, int proc, int regularizados)
        {
            List<IngresosDinero> lst = new List<IngresosDinero>();
            string Llama_PA = "INGRESO_DINERO_Carga_Lista_V2";
            using (SqlConnection conn = new SqlConnection(GESTION_PROD))
            {
                conn.Open();
                SqlCommand comando = new SqlCommand(Llama_PA, conn);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@id_usu", id_usu);
                comando.Parameters.AddWithValue("@n_rut_cliente", n_rut_cliente);
                comando.Parameters.AddWithValue("@id_estado", id_estado);
                comando.Parameters.AddWithValue("@id_forma_pago", id_forma_pago);
                comando.Parameters.AddWithValue("@fecha_desde", fecha_desde);
                comando.Parameters.AddWithValue("@fecha_hasta", fecha_hasta);
                comando.Parameters.AddWithValue("@id_tipo", id_tipo);
                comando.Parameters.AddWithValue("@empresa", empresa);
                comando.Parameters.AddWithValue("@n_rut_cliente_facturar", n_rut_cliente_facturar);
                comando.Parameters.AddWithValue("@ids_cotizaciones", ids_cotizaciones);
                comando.Parameters.AddWithValue("@proc", proc);
                comando.Parameters.AddWithValue("@regularizados", regularizados);
                SqlDataReader Resultados = comando.ExecuteReader();
                if (Resultados.HasRows)
                {
                    lst = Resultados.ToList<IngresosDinero>();
                }
                conn.Close();
            }
            return lst;
        }

        public static DataSet Llama_GeneraExcelInterfazCobro(int idUsuario, string sistema, string grilla, int idFormato, int cliente, int estado, int forma_pago, int id_tipo_ingreso, DateTime? fechaIni, DateTime? fechaFin)
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
                    //cmd.Parameters.AddWithValue("@formato", idFormato);                
                    if (fechaIni != null) cmd.Parameters.AddWithValue("@Fecha_Desde", fechaIni);
                    if (fechaFin != null) cmd.Parameters.AddWithValue("@Fecha_Hasta", fechaFin);
                    cmd.Parameters.AddWithValue("@n_rut_cliente", cliente);
                    cmd.Parameters.AddWithValue("@id_estado", estado);
                    cmd.Parameters.AddWithValue("@id_forma_pago", forma_pago);
                    cmd.Parameters.AddWithValue("@id_tipo_ingreso", id_tipo_ingreso);
                    conn.Open();
                    SqlDataAdapter adp = new SqlDataAdapter();
                    adp.SelectCommand = cmd;
                    adp.Fill(ds);
                }
            }
            return ds;
        }

        public static List<clases.HeaderExcelFormato> Llama_GeneraExcelHeaderInterfaz(int idUsuario, string sistema, string grilla/*, DateTime f_cobrosdesde, DateTime f_cobroshasta*/)
        {
            List<clases.HeaderExcelFormato> lst = new List<clases.HeaderExcelFormato>();
            string Llama_PA = "EXEC EXCEL_Genera @id_usu,@tipo,@sistema,@grilla";
            using (SqlConnection conn = new SqlConnection(GESTION_PROD))
            {
                conn.Open();
                SqlCommand comando = new SqlCommand(Llama_PA, conn);
                comando.Parameters.AddWithValue("@id_usu", idUsuario);
                comando.Parameters.AddWithValue("@tipo", 0);
                comando.Parameters.AddWithValue("@sistema", sistema);
                comando.Parameters.AddWithValue("@grilla", grilla);
                //comando.Parameters.AddWithValue("@formato", 0);
                //comando.Parameters.AddWithValue("@Fecha_Desde", f_cobrosdesde);
                //comando.Parameters.AddWithValue("@Fecha_Hasta", f_cobroshasta);
                SqlDataReader Resultados = comando.ExecuteReader();
                if (Resultados.HasRows)
                {
                    while (Resultados.Read())
                    {
                        lst.Add(new clases.HeaderExcelFormato
                        {
                            id = (int)Resultados["ID"],
                            nombre = Resultados["Nombre"].ToString(),
                            archivo = Resultados["Archivo"].ToString(),
                        });
                    }
                }
                conn.Close();
            }
            return lst;
        }

        //public static bool LlamaCambioEstado(int usuario,string idsIngreso, int idEstado,string comentario )
        //{
        //    string Llama_PA = "EXEC IngresoDinero_CambiarEstado @id_usu, @IdsIngresoDinero, @IdEstadoIngreso, @Observacion";
        //    using (SqlConnection conn = new SqlConnection(GESTION_PROD))
        //    {
        //        conn.Open();
        //        SqlCommand comando = new SqlCommand(Llama_PA, conn);
        //        comando.Parameters.AddWithValue("@id_usu", usuario);
        //        comando.Parameters.AddWithValue("@IdsIngresoDinero", idsIngreso);
        //        comando.Parameters.AddWithValue("@IdEstadoIngreso", idEstado);
        //        comando.Parameters.AddWithValue("@Observacion", comentario);
        //        SqlDataReader Resultados = comando.ExecuteReader();
        //        conn.Close();
        //    }
        //    return true;
        //}

        public static List<EstadoIngresoDinero> Llama_Pagos_CambiarEstado_Ingreso_Dinero(int usuario, string idsIngreso, int idEstado,DateTime fEstado, string comentario, string rut_facturar)
        {
            List<EstadoIngresoDinero> lst = new List<EstadoIngresoDinero>();
            string Llama_PA = "INGRESO_DINERO_Cambiar_Estado_Multiple_V2";
            using (SqlConnection conn = new SqlConnection(GESTION_PROD))
            {
                conn.Open();
                SqlCommand comando = new SqlCommand(Llama_PA, conn);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@id_usu", usuario);
                comando.Parameters.AddWithValue("@IdsIngresoDinero", idsIngreso);
                comando.Parameters.AddWithValue("@IdEstadoIngreso", idEstado);
                comando.Parameters.AddWithValue("@fEstado", fEstado);
                comando.Parameters.AddWithValue("@Observacion", comentario);
                comando.Parameters.AddWithValue("@RutFacturar", rut_facturar);

                SqlDataReader Resultados = comando.ExecuteReader();
                if (Resultados.HasRows)
                {
                    while (Resultados.Read())
                    {
                        lst.Add(new EstadoIngresoDinero
                        {

                            fechacambioestado = Resultados["fecha"].ToString(),
                            g_usu = Resultados["g_usu"].ToString(),
                            mensaje = Resultados["mensaje"].ToString(),
                            g_estado_anterior = Resultados["g_estado_anterior"].ToString(),
                            g_estado_nuevo = Resultados["g_estado_nuevo"].ToString(),
                            f_estado = Resultados["fecha_estado"].ToString(),
                            monto = Resultados["monto"].ToString(),
                           
                        });
                    }
                }
                conn.Close();
            }
            return lst;
        }

        public static List<EstadoIngresoDinero> Llama_Pagos_CambiarEstado_Ingreso_Dinero_2_Cobros(int usuario, string idsIngreso, int idEstado, DateTime fEstado, string comentario)
        {
            List<EstadoIngresoDinero> lst = new List<EstadoIngresoDinero>();
            string Llama_PA = "INGRESO_DINERO_Cambiar_Estado_Multiple_2_Cobros";
            using (SqlConnection conn = new SqlConnection(GESTION_PROD))
            {
                conn.Open();
                SqlCommand comando = new SqlCommand(Llama_PA, conn);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@id_usu", usuario);
                comando.Parameters.AddWithValue("@IdsIngresoDinero", idsIngreso);
                comando.Parameters.AddWithValue("@IdEstadoIngreso", idEstado);
                comando.Parameters.AddWithValue("@fEstado", fEstado);
                comando.Parameters.AddWithValue("@Observacion", comentario);

                SqlDataReader Resultados = comando.ExecuteReader();
                if (Resultados.HasRows)
                {
                    while (Resultados.Read())
                    {
                        lst.Add(new EstadoIngresoDinero
                        {

                            fechacambioestado = Resultados["fecha"].ToString(),
                            g_usu = Resultados["g_usu"].ToString(),
                            mensaje = Resultados["mensaje"].ToString(),
                            g_estado_anterior = Resultados["g_estado_anterior"].ToString(),
                            g_estado_nuevo = Resultados["g_estado_nuevo"].ToString(),
                            f_estado = Resultados["fecha_estado"].ToString(),
                            monto = Resultados["monto"].ToString(),

                        });
                    }
                }
                conn.Close();
            }
            return lst;
        }


        public static List<ObservacionIngresoDinero> Llama_HistorialIngresoDinero(int id_ingreso)
        {
            var resp = new List<ObservacionIngresoDinero>();

            string Llama_PA = "INGRESO_DINERO_Lista_Observaciones";
            using (SqlConnection conn = new SqlConnection(GESTION_PROD))
            {
                conn.Open();
                SqlCommand comando = new SqlCommand(Llama_PA, conn);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@IdIngreso", id_ingreso);
                comando.Parameters.AddWithValue("@proceso", 1);

                SqlDataReader Resultados = comando.ExecuteReader();
                while (Resultados.Read())
                {
                    resp.Add(new ObservacionIngresoDinero
                    {
                        id_usu = Resultados["id_usu"].ToString(),
                        g_nom_usu = Resultados["g_nom_usu"].ToString(),
                        f_observacion = Resultados["f_observacion"].ToString(),
                        id_ingreso = Resultados["id_ingreso"].ToString(),
                        g_observacion = Resultados["g_observacion"].ToString(),
                        g_estado_anterior = Resultados["g_estado_anterior"].ToString(),
                        g_estado_nuevo = Resultados["g_estado_nuevo"].ToString(),
                        f_observacion_cambio_estado = Resultados["f_observacion_cambio_estado"].ToString(),
                        g_mensaje = Resultados["g_mensaje"].ToString()

                    });
                }
                conn.Close();
            }

            return resp;
        }

        public static List<IngresosDinero> Llama_Carga_Cliente(int ing_rut_cliente)
        {
            var resp = new List<IngresosDinero>();

            string Llama_PA = "Carga_Cliente";
            using (SqlConnection conn = new SqlConnection(GESTION_PROD))
            {
                conn.Open();
                SqlCommand comando = new SqlCommand(Llama_PA, conn);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@n_rut", ing_rut_cliente);
                //comando.Parameters.AddWithValue("@proceso", 1);

                SqlDataReader Resultados = comando.ExecuteReader();
                while (Resultados.Read())
                {
                    resp.Add(new IngresosDinero
                    {
                        g_nom = Resultados["g_nom"].ToString(),
                        g_ape_pat = Resultados["g_ape_pat"].ToString(),
                        g_ape_mat = Resultados["g_ape_mat"].ToString(),
                        g_direccion_cli = Resultados["g_dom_par"].ToString(),
                        g_num_casa = Resultados["g_num_casa"].ToString(),
                        id_dom_com = Resultados["id_dom_com"].ToString(),
                        g_mail_per = Resultados["g_mail_per"].ToString()
                    });
                }
                conn.Close();
            }

            return resp;
        }

        public static List<IngresosDinero> Llama_CargaIngresoDinero(int id_ingreso)
        {
            var resp = new List<IngresosDinero>();

            string Llama_PA = "INGRESO_DINERO_Carga_Ingreso";
            using (SqlConnection conn = new SqlConnection(GESTION_PROD))
            {
                conn.Open();
                SqlCommand comando = new SqlCommand(Llama_PA, conn);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@id_ingreso", id_ingreso);
                //comando.Parameters.AddWithValue("@proceso", 2);

                SqlDataReader Resultados = comando.ExecuteReader();
                while (Resultados.Read())
                {
                    resp.Add(new IngresosDinero
                    {
                        //id_usu = Resultados["id_usu"].ToString(),
                        //g_nom_usu = Resultados["g_nom_usu"].ToString(),
                        //f_observacion = Resultados["f_observacion"].ToString(),
                        //id_ingreso = Resultados["id_ingreso"].ToString(),
                        //g_observacion = Resultados["g_observacion"].ToString(),
                        //g_estado_anterior = Resultados["g_estado_anterior"].ToString(),
                        //g_estado_nuevo = Resultados["g_estado_nuevo"].ToString(),
                        //f_observacion_cambio_estado = Resultados["f_observacion_cambio_estado"].ToString(),
                        //g_mensaje = Resultados["g_mensaje"].ToString()
                        id_ing= Resultados["id_ing"].ToString(),      
                        id_tipo = Resultados["id_tipo"].ToString(),     
                        g_tipo = Resultados["g_tipo"].ToString(),      
                        f_fecha = Resultados["f_fecha"].ToString(),      
                        id_folio = Resultados["id_folio"].ToString(),
                        rut_cliente = Resultados["rut_cliente"].ToString(),
                        nombre_cliente = Resultados["nombre_cliente"].ToString(),      
                        //g_ape_pat = Resultados["g_ape_pat"].ToString(),     
                        //g_ape_mat = Resultados["g_ape_mat"].ToString(),      
                        //id_pry = Resultados["id_pry"].ToString(),      
                        g_nom_pry = Resultados["g_nom_pry"].ToString(),      
                        g_direccion_pry = Resultados["g_direccion_pry"].ToString(),
                        g_unidad = Resultados["g_unidad"].ToString(),
                        v_monto = Resultados["v_monto"].ToString(),
                        id_forma_pago = Resultados["id_forma_pago"].ToString(),    
                        g_forma_pago = Resultados["g_forma_pago"].ToString(),     
                        f_vencimiento= Resultados["f_vencimiento"].ToString(), 
                        //id_abonable_pie = Resultados["id_abonable_pie"].ToString(),    
                        g_abonable_pie = Resultados["g_abonable_pie"].ToString(),     
                        id_estado = Resultados["id_estado"].ToString(),     
                        g_estado = Resultados["g_estado"].ToString(),      
                        g_file = Resultados["g_file"].ToString(),    
                        f_fecha_cambio_estado = Resultados["f_fecha_cambio_estado"].ToString(),
                        g_observacion_ingreso = Resultados["g_observacion"].ToString(), 
                        id_estado_anterior= Resultados["id_estado_anterior"].ToString(),                       
                        g_nom = Resultados["g_nom"].ToString(),
                        g_ape_pat = Resultados["g_ape_pat"].ToString(),
                        g_ape_mat = Resultados["g_ape_mat"].ToString(),
                        v_monto_uf = Resultados["v_monto_uf"].ToString(),
                        g_direccion_cli = Resultados["g_direccion_cli"].ToString(),
                        g_comuna_cli = Resultados["g_comuna_cli"].ToString(),
                        id_banco = Resultados["id_banco"].ToString(),
                        g_banco = Resultados["g_banco"].ToString(),
                        g_cta_cte = Resultados["g_cta_cte"].ToString(),
                        g_serie = Resultados["g_serie"].ToString(),
                        g_num_casa = Resultados["g_num_casa"].ToString(),
                        id_dom_com = Resultados["id_dom_com"].ToString(),
                        g_mail_per = Resultados["g_mail_per"].ToString(),
                        id_plaza = Resultados["id_plaza"].ToString(),
                        id_empresa = Resultados["id_empresa"].ToString(),
                    });
                }
                conn.Close();
            }

            return resp;
        }
        public static List<ObservacionIngresoDinero> Llama_Ingreso_Actualiza_Observacion_Carga(int proc, int id_ingreso, int id_usuario, string observacion, string g_nom_usu)
        {
            List<ObservacionIngresoDinero> lst = new List<ObservacionIngresoDinero>();
            string Llama_PA = "INGRESO_DINERO_Actualiza_Observaciones";
            using (SqlConnection conn = new SqlConnection(GESTION_PROD))
            {
                conn.Open();
                SqlCommand comando = new SqlCommand(Llama_PA, conn);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@proc", proc);
                comando.Parameters.AddWithValue("@id_ingreso", id_ingreso);
                comando.Parameters.AddWithValue("@id_usu", id_usuario);
                comando.Parameters.AddWithValue("@observacion", observacion);
                comando.Parameters.AddWithValue("@g_nom_usu", g_nom_usu);
                SqlDataReader Resultados = comando.ExecuteReader();
                if (Resultados.HasRows)
                {
                    while (Resultados.Read())
                    {
                        lst.Add(new ObservacionIngresoDinero
                        {
                            id_observacion = Resultados["id_observacion"].ToString(),
                            id_usu = Resultados["id_usu"].ToString(),
                            g_nom_usu = Resultados["g_nom_usu"].ToString(),
                            f_observacion = Resultados["f_observacion"].ToString(),
                            id_ingreso = Resultados["id_ingreso"].ToString(),
                            g_observacion = Resultados["g_observacion"].ToString()
                        });
                    }
                }
                conn.Close();
            }
            return lst;
        }
        public static string Llama_Ingreso_Actualiza_Observacion_Graba(int proc, int id_ingreso, int id_usu, string observacion, string g_nom_usu)
        {
            //List<retorno_obs> lst = new List<retorno_obs>();
            string Llama_PA = "INGRESO_DINERO_Actualiza_Observaciones";
            using (SqlConnection conn = new SqlConnection(GESTION_PROD))
            {
                conn.Open();
                SqlCommand comando = new SqlCommand(Llama_PA, conn);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@proc", proc);
                comando.Parameters.AddWithValue("@id_ingreso", id_ingreso);
                comando.Parameters.AddWithValue("@id_usu", id_usu);
                comando.Parameters.AddWithValue("@observacion", observacion);
                comando.Parameters.AddWithValue("@g_nom_usu", g_nom_usu);

                SqlDataReader Resultados = comando.ExecuteReader();

                conn.Close();
            }
            //return lst;
            return null;
        }


        public static int Llama_Ingreso_Guardar_Ingreso(int proc,int idUsuario, int id_ing, int id_tipo, int ing_rut_cliente,string g_dv, string ing_nombre_cliente, string ing_g_ape_pat, string ing_g_ape_mat, float ing_monto_uf, int ing_v_monto, DateTime? ing_f_fecha_cambio_estado, string ing_g_direccion_cli, DateTime? ing_f_vencimiento, string ing_g_observacion_ingreso,string ing_g_direccion_cli_numero,int id_plaza,int id_empresa,int id_forma_pago,int id_comuna, int id_banco, string g_cta_cte, string g_serie)
        {
            int id = 0;
            string Llama_PA = "INGRESO_DINERO_Guarda_Ingreso_V2";
            using (SqlConnection conn = new SqlConnection(GESTION_PROD))
            {
                conn.Open();
                SqlCommand comando = new SqlCommand(Llama_PA, conn);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@proc", proc);
                comando.Parameters.AddWithValue("@id_usu", idUsuario);
                comando.Parameters.AddWithValue("@id_ingreso", id_ing);
                comando.Parameters.AddWithValue("@id_tipo", id_tipo);
                comando.Parameters.AddWithValue("@n_rut", ing_rut_cliente);
                comando.Parameters.AddWithValue("@g_dv", g_dv);      
                comando.Parameters.AddWithValue("@g_nom", ing_nombre_cliente);
                comando.Parameters.AddWithValue("@g_ape_pat", ing_g_ape_pat);
                comando.Parameters.AddWithValue("@g_ape_mat", ing_g_ape_mat);
                comando.Parameters.AddWithValue("@id_banco", id_banco);
                comando.Parameters.AddWithValue("@g_cta_cte", (object)g_cta_cte ?? DBNull.Value);
                comando.Parameters.AddWithValue("@g_serie", (object)g_serie ?? DBNull.Value);
                comando.Parameters.AddWithValue("@v_monto_uf", ing_monto_uf);
                comando.Parameters.AddWithValue("@v_monto", ing_v_monto);
                comando.Parameters.AddWithValue("@f_fecha_cambio_estado", (object)ing_f_fecha_cambio_estado ?? DBNull.Value);
                comando.Parameters.AddWithValue("@g_direccion_cli", (object)ing_g_direccion_cli ?? DBNull.Value);
                comando.Parameters.AddWithValue("@f_vencimiento", (object)ing_f_vencimiento ?? DBNull.Value);
                comando.Parameters.AddWithValue("@g_observacion", ing_g_observacion_ingreso);
                comando.Parameters.AddWithValue("@ing_g_direccion_cli_numero", (object)ing_g_direccion_cli_numero ?? DBNull.Value);
                comando.Parameters.AddWithValue("@id_forma_pago", id_forma_pago);
                comando.Parameters.AddWithValue("@id_dom_com", id_comuna);
                comando.Parameters.AddWithValue("@id_plaza", id_plaza);
                comando.Parameters.AddWithValue("@id_empresa", id_empresa);
                SqlDataReader Resultados = comando.ExecuteReader();

                if (Resultados.HasRows)
                {
                    if (Resultados.Read())
                    {
                        id = int.Parse(Resultados["id_ing"].ToString());
                    }
                }

                conn.Close();
            }

            return id;
        }

        public static int Llama_Ingreso_Guardar_Ingreso_Archivo(int idUsuario, int id_ing, string g_file)
        {
            int id = 0;
            string Llama_PA = "INGRESO_DINERO_Guarda_Ingreso_V2";
            using (SqlConnection conn = new SqlConnection(GESTION_PROD))
            {
                conn.Open();
                SqlCommand comando = new SqlCommand(Llama_PA, conn);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@proc", 3);
                comando.Parameters.AddWithValue("@id_usu", idUsuario);
                comando.Parameters.AddWithValue("@id_ingreso", id_ing);
                comando.Parameters.AddWithValue("@g_file", g_file);
                SqlDataReader Resultados = comando.ExecuteReader();

                if (Resultados.HasRows)
                {
                    if (Resultados.Read())
                    {
                        id = int.Parse(Resultados["id_ing"].ToString());
                    }
                }

                conn.Close();
            }

            return id;
        }

        public static bool Llama_Ingreso_Regulariza(int IdUsuario, int proc, int id_ingreso, string f_fecha, string rut_cliente, string nombre_cliente, int v_monto, int forma_pago, string f_vencimiento, int id_abonable_pie, int id_banco, string g_cta_cte, string g_serie)
        {
            string SP = "INGRESO_DINERO_Regulariza";
            using (SqlConnection conn = new SqlConnection(GESTION_PROD))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(SP, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_usu", IdUsuario);
                cmd.Parameters.AddWithValue("@proc", proc);
                cmd.Parameters.AddWithValue("@id_ingreso", id_ingreso);
                cmd.Parameters.AddWithValue("@f_fecha", f_fecha);
                cmd.Parameters.AddWithValue("@rut_cliente", rut_cliente);
                cmd.Parameters.AddWithValue("@nombre_cliente", nombre_cliente);
                cmd.Parameters.AddWithValue("@v_monto", v_monto);
                cmd.Parameters.AddWithValue("@forma_pago", forma_pago);
                cmd.Parameters.AddWithValue("@f_vencimiento", f_vencimiento);
                cmd.Parameters.AddWithValue("@id_abonable_pie", id_abonable_pie);
                cmd.Parameters.AddWithValue("@id_banco", id_banco);
                cmd.Parameters.AddWithValue("@g_cta_cte", g_cta_cte);
                cmd.Parameters.AddWithValue("@g_serie", g_serie);
                SqlDataReader Resultados = cmd.ExecuteReader();
                conn.Close();
            }
            return true;
        }
    }
}