using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinariaBack.Dominio
{
    public class Mascotas
    {
        public int id_mascota { get; set; }
        public string m_nombre { get; set; }
        public int edad { get; set; }
        public Tipos tipo { get; set; }
        public Clientes cliente { get; set; }
        public bool borrado { get; set; }

        public override string ToString()
        {
            return m_nombre;
        }

    }

}
