using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IngresoDinero.clases
{
    public class ListadoCargaFacturas
    {
        public string id_ing { get; set; }
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
        public string g_forma_pago { get; set; }
        public string g_serie { get; set; }
        public string f_vencimiento { get; set; }
        public string g_abonable_pie { get; set; }
        public string g_estado { get; set; }
        public string g_file { get; set; }
        public string b_regularizado { get; set; }
        public string f_facturacion { get; set; }
        public string g_factura { get; set; }
        public string folio_factura { get; set; }
        public string mensaje { get; set; }
    }
}