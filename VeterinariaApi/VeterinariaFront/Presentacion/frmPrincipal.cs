
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VeterinariaBack.Dominio;
using VeterinariaFront.Presentacion.UsuarioF;
using VeterinariaFront.Presentacion.ClienteF;
using VeterinariaFront.Presentacion.TipoF;
using VeterinariaFront.Presentacion.MascotaF;
using VeterinariaFront.Presentacion.AtencionF;

namespace VeterinariaFront.Presentacion
{
    public partial class frmPrincipal : Form
    {
        public Usuarios uLog{ get; set; }
        public frmPrincipal()
        {
            InitializeComponent();
            
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            
            
            lblUsuarioLogueado.Text = "Bienvenido Veterinario:   "+uLog.ToString().ToUpper();
        }

        private void frmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult rpta;
            rpta = MessageBox.Show("Seguro que desea salir?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rpta == DialogResult.No)
                e.Cancel = true;
        }

      
        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            frmUsuario frmU = new frmUsuario();
            frmU.ShowDialog();
            
        }

        private void animalesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTipo frmT = new frmTipo();
            frmT.ShowDialog();
        }

        private void mascotaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMascota frmM = new frmMascota();
            frmM.ShowDialog();

        }

        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCliente frmC = new frmCliente();
            frmC.ShowDialog();
        }

        private void atencionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAtencion frmA = new frmAtencion();
            frmA.uLog = uLog;
            frmA.ShowDialog();
        }
    }
}
