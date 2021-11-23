using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace veterNetFram.Dominio
{
    public class Clientes
    {
        public int id_cliente { get; set; }
        public string c_nombre { get; set; }
        public bool sexo { get; set; }
        public bool borrado { get; set; }

        public override string ToString()
        {
            return c_nombre;
        }



    }
}
