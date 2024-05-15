using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IngresoDinero.clases
{
    public class IngresosDinero
    {
        public string id_ing { get; set; }
        public string id_tipo { get; set; }
        public string g_tipo { get; set; }
        public string f_fecha { get; set; }
        public string id_folio { get; set; }
        public string id_tipo_cot { get; set; }
        public string rut_cliente { get; set; }
        public string nombre_cliente { get; set; }
        public string g_nom_pry { get; set; }
        public string g_direccion_pry { get; set; }
        public string g_unidad { get; set; }
        public string v_monto { get; set; }
        public string id_forma_pago { get; set; }
        public string g_forma_pago { get; set; }
        public string f_vencimiento { get; set; }
        public string id_abonable_pie { get; set; }
        public string g_abonable_pie { get; set; }
        public string id_estado { get; set; }
        public string g_estado { get; set; }
        public string g_file { get; set; }

        public string f_fecha_cambio_estado { get; set; }
        public string g_observacion_ingreso { get; set; }
        public string id_estado_anterior { get; set; }
        public string id_ing_anterior { get; set; }
        public string b_regularizado { get; set; }
        public string g_nom { get; set; }
        public string g_ape_pat { get; set; }
        public string g_ape_mat { get; set; }
        public string v_monto_uf { get; set; }
        public string g_direccion_cli { get; set; }
        public string g_comuna_cli { get; set; }
        public string id_banco { get; set; }
        public string g_banco { get; set; }
        public string g_cta_cte { get; set; }
        public string g_serie { get; set; }
        public string g_num_casa { get; set; }            
        public string id_dom_com { get; set; }
        public string b_manual { get; set; }
        public string g_mail_per { get; set; }
        public string f_facturacion { get; set; }
        public string g_factura { get; set; }
        public string folio_factura { get; set; }
        public string id_plaza { get; set; }
        public string id_empresa { get; set; }
        public string g_empresa { get; set; }
        public string rut_facturar { get; set; }
        public string nom_facturar { get; set; }

    }
    public class listas_combobox
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public string periodo_dia { get; set; }
        public string periodo_mes { get; set; }
    }
    public class HeaderExcelFormato
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string archivo { get; set; }
    }
    public class perfilamiento
    {
        public string g_estado_promesa { get; set; }
        public string g_estado_factura { get; set; }
    }
    public class NuevoEstado
    {
        public string estado { get; set; }
        public string mensaje { get; set; }
        public string promesa { get; set; }
    }
    public class subir_archivo
    {
        public string estado { get; set; }
        public string mensaje { get; set; }
    }
    public class Datos_Inmobiliaria
    {
        public string id_inmob { get; set; }
        public string g_rut { get; set; }
        public string g_razon_social { get; set; }
        public string g_giro { get; set; }
        public string g_direccion { get; set; }
        public string id_comuna { get; set; }
        public string g_contacto { get; set; }
    }
    public class NuevoEstadoInmobiliaria
    {
        public string estado { get; set; }
        public string mensaje { get; set; }
        public string inmobiliaria { get; set; }
    }

    public class Datos_Proyecto
    {
        public string id_pry { get; set; }
        public string p_com_inmob { get; set; }
        public string b_com_inmob { get; set; }
        public string forma_pago { get; set; }
        public string condiciones_comisiones { get; set; }

        public string rut_facturacion { get; set; }
        public string razon_social_facturacion { get; set; }
        public string giro_facturacion { get; set; }
        public string direccion_facturacion { get; set; }
        public string comuna_facturacion { get; set; }
        public string contacto_facturacion { get; set; }
        public string flag_facturacion { get; set; }
    }
    public class NuevoEstadoProyecto
    {
        public string estado { get; set; }
        public string mensaje { get; set; }
        public string proyecto { get; set; }
    }
    public class NuevoValorComision
    {
        public string estado { get; set; }
        public string mensaje { get; set; }
        public string promesa { get; set; }
    }

    public class listascombobox
    {
        public int id { get; set; }
        public string descripcion { get; set; }
    }

    public class EstadoIngresoDinero
    {
        public string fechacambioestado { get; set; }
        public string g_usu { get; set; }
        public string mensaje { get; set; }
        public string g_estado_anterior { get; set; }
        public string g_estado_nuevo { get; set; }
        public string f_estado { get; set; }
        public string monto { get; set; }
       
    }

    public class ObservacionIngresoDinero
    { 
        public string id_observacion { get; set; }
        public string id_usu { get; set; }
        public string g_nom_usu { get; set; }
        public string f_observacion { get; set; }
        public string id_ingreso { get; set; }
        public string g_observacion { get; set; }
        public string g_estado_anterior { get; set; }
        public string g_estado_nuevo { get; set; }
        public string f_observacion_cambio_estado { get; set; }
        public string g_mensaje { get; set; }
    }
}