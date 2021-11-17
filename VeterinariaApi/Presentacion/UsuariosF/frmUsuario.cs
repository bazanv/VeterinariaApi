using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using veterNetFram.Dominio;
using veterNetFram.Servicios;

namespace veterNetFram.Presentacion.UsuarioF
{
    public partial class frmUsuario : Form
    {
        private IService servicio;
    

        public frmUsuario()
        {
            InitializeComponent();
            InitializeDataGridView();
            servicio = new UsuarioService();

        }

        private void frmUsuarios_Load(object sender, EventArgs e)
        {
            
            this.CenterToParent();
        }

       

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            frmABMUsuario formulario = new frmABMUsuario();
            formulario.ShowDialog();
                        
            dgvUsers.DataSource = servicio.ObtenerTodos();

            //Forzamos el evento Click para actualizar la grilla.
           //btnConsultar_Click(sender, e);
        }

        private void chkTodos_CheckedChanged(object sender, EventArgs e)
        {
            {
                if (chkTodos.Checked)
                {
                    txtNombre.Enabled = false;
                   
                }
                else
                {
                    txtNombre.Enabled = true;
                 
                }
            }
        }

        private void btnSalir_Click(System.Object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void btnConsultar_Click(System.Object sender, System.EventArgs e)
        {
            var filters = new Dictionary<string, object>();
            IList<Usuarios> res = new List<Usuarios>();

            if (!chkTodos.Checked)
            {
               

                //Validar si el textBox "Nombre" esta vacio
                if (txtNombre.Text != string.Empty)
                {
                    //Si el txtBox tiene un texto no vacio entonces recuperamos el valor del texto
                    filters.Add("usuario", txtNombre.Text);
                }

                if (filters.Count > 0)
                {
                    res = servicio.ConsultarConFiltro(filters);

                    if (res.Count == 0)
                        MessageBox.Show("No se encontraron coincidencias", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    else
                        dgvUsers.DataSource = res;
                }                                  
                else
                    MessageBox.Show("Debe ingresar al menos un criterio", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }

            else
                dgvUsers.DataSource = servicio.ObtenerTodos();
            
        }

        private void btnEditar_Click(System.Object sender, System.EventArgs e)
        {

            frmABMUsuario formulario = new frmABMUsuario();
            //Asi obtenemos el item seleccionado de la grilla
            var usuario = (Usuarios) dgvUsers.CurrentRow.DataBoundItem;
            formulario.InicializarFormulario(frmABMUsuario.FormMode.modificar, usuario);
            formulario.ShowDialog();

            dgvUsers.DataSource = servicio.ObtenerTodos();
            //Forzamos el evento Click para actalizar el DataGridView
            //btnConsultar_Click(sender, e);

        }

        private void btnQuitar_Click(System.Object sender, System.EventArgs e)
        {
            frmABMUsuario formulario = new frmABMUsuario();
            //Asi obtenemos el item seleccionado de la grilla
            var usuario = (Usuarios)dgvUsers.CurrentRow.DataBoundItem;

            formulario.InicializarFormulario(frmABMUsuario.FormMode.eliminar, usuario);
            formulario.ShowDialog();

            dgvUsers.DataSource = servicio.ObtenerTodos();
            //Forzamos el evento Click para actalizar el DataGridView
            //btnConsultar_Click(sender, e);


        }

        private void InitializeDataGridView()
        {
            // Cree un DataGridView no vinculado declarando un recuento de columnas.
            dgvUsers.ColumnCount = 3;
            dgvUsers.ColumnHeadersVisible = true;

            // Configuramos la AutoGenerateColumns en false para que no se autogeneren las columnas
            dgvUsers.AutoGenerateColumns = false;

            // Cambia el estilo de la cabecera de la grilla.
            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();

            columnHeaderStyle.BackColor = Color.Beige;
            columnHeaderStyle.Font = new Font("Verdana", 8, FontStyle.Bold);
            dgvUsers.ColumnHeadersDefaultCellStyle = columnHeaderStyle;

            // Definimos el nombre de la columnas y el DataPropertyName que se asocia a DataSource
            dgvUsers.Columns[0].Name = "Usuario";
            dgvUsers.Columns[0].DataPropertyName = "Usuario";
            // Definimos el ancho de la columna.

            dgvUsers.Columns[1].Name = "Email";
            dgvUsers.Columns[1].DataPropertyName = "Email";

            dgvUsers.Columns[2].Name = "Borrado";
            dgvUsers.Columns[2].DataPropertyName = "borrado";


            // Cambia el tamaño de la altura de los encabezados de columna.
            dgvUsers.AutoResizeColumnHeadersHeight();

            // Cambia el tamaño de todas las alturas de fila para ajustar el contenido de todas las celdas que no sean de encabezado.
            dgvUsers.AutoResizeRows(
                DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders);
        }

        private void dgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnEditar.Enabled = true;
            btnQuitar.Enabled = true;
        }
    }
}
