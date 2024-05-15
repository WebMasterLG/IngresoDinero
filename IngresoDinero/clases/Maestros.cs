using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IngresoDinero.clases
{
    public class Lista
    {
        public string IdLista { get; set; }
        public string Descripcion { get; set; }
        public string NValor1 { get; set; }
    }

    public class estados
    {
        public string estado { get; set; }
        public string mensaje { get; set; }
        public int id { get; set; }
    }

    public class TipoArchivo
    {
        public string id { get; set; }
        public string glosa { get; set; }
    }
}