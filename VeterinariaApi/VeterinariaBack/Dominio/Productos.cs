using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinariaBack.Dominio
{
    public class Productos
    {
        public int id_producto { get; set; }
       
        public string p_nombre { get; set; }
        public double precio { get; set; }
  
        public bool borrado { get; set; }

        public override string ToString()
        {
            return p_nombre;
        }

    }
}
