using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace veterNetFram.Dominio
{
    public class Detalles
    {
        public int id_detalle { get; set; }

        public int id_atencion { get; set; }

        public Productos producto { get; set; }
        public int cantidad { get; set; }
        public double p_facturado { get; set; }

        public bool borrado { get; set; }

        public double CalcularSubtotal()
        {
            return p_facturado * cantidad;
        }

        public double CalcularSubTotal()
        {
            return p_facturado * cantidad;
        }

        public Detalles()
        {
            producto = new Productos();
        }

    }
}
