using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace veterNetFram.Dominio
{
    public class Tipos
    {
        public int  id_tipo { get; set; }
        public string t_descrip{ get; set; }
        public bool borrado { get; set; }

        public override string ToString()
        {
            return t_descrip;
        }

    }
}
