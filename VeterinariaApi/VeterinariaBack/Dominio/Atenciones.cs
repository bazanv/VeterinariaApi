using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinariaBack.Dominio
{
    public class Atenciones
    {
        public int id_atencion { get; set; }
        public DateTime fecha { get; set; }
        public string a_descrip { get; set; }
        public double importe { get; set; }
        public Mascotas mascota { get; set; }
        public Usuarios usuario { get; set; }
        public List<Detalles> detalle { get; set; }
        public bool borrado { get; set; }

        public Atenciones()
        {
            detalle = new List<Detalles>();
            mascota = new Mascotas();
            usuario = new Usuarios();
        }

        public double CalcularTotal()
        {
            double total = 0;
            foreach (Detalles item in detalle)
            {
                total += item.CalcularSubTotal();
            }
            return total;

        }


    }
}
