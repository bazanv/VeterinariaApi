using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace veterNetFram.Dominio
{
    public class Usuarios
    {

        public int id_usuario { get; set; }
        public string usuario { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public bool borrado { get; set; }

        public override string ToString()
        {
            return usuario;
        }

    }
}
