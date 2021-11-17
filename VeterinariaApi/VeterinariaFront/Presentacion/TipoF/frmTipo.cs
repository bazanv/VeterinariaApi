
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
using VeterinariaBack.Dominio;
using VeterinariaBack.Servicios;
using VeterinariaFront.HttpSingleton;
using Newtonsoft.Json;

namespace VeterinariaFront.Presentacion.TipoF
{
    public partial class frmTipo : Form
    {
        private TipoService servicio;
    

        public frmTipo()
        {
            InitializeComponent();
            InitializeDataGridView();
            servicio = new TipoService();

        }

        private void frmUsuarios_Load(object sender, EventArgs e)
        {
            
            this.CenterToParent();
        }

       

        private async void btnNuevo_Click(object sender, EventArgs e)
        {
            frmABMTipo formulario = new frmABMTipo();
            formulario.ShowDialog();

            string url = "https://localhost:44365/api/Tipo/ConsultarTodos";
            var resultado = await SingletonHttp.GetInstancia().GetAsync(url);
            var lst = JsonConvert.DeserializeObject<List<Tipos>>(resultado);
            dgvUsers.DataSource = lst;

            //dgvUsers.DataSource = servicio.ObtenerTodos();

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

        private async void btnConsultar_Click(System.Object sender, System.EventArgs e)
        {
            var filters = new Dictionary<string, object>();
            IList<Tipos> res = new List<Tipos>();

            if (!chkTodos.Checked)
            {
               

                //Validar si el textBox "Nombre" esta vacio
                if (txtNombre.Text != string.Empty)
                {
                    //Si el txtBox tiene un texto no vacio entonces recuperamos el valor del texto
                    filters.Add("descripcion", txtNombre.Text);
                }

                if (filters.Count > 0)
                {
                    //string filJson = JsonConvert.SerializeObject(filters);
                    //string url = "https://localhost:44365/api/Tipo/ConsultarFiltro";
                    //var resultado = await SingletonHttp.GetInstancia().PostAsync(url, filJson);
                    //var lst = JsonConvert.DeserializeObject<List<Tipos>>(resultado);

                    res = servicio.ConsultarConFiltro(filters);

                    if (res.Count == 0)
                    {
                        MessageBox.Show("No se encontraron coincidencias", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                               
                    }
                    else
                        dgvUsers.DataSource = res;
                }
                else
                    MessageBox.Show("Debe ingresar al menos un criterio", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }

            else
            {
                // dgvUsers.DataSource = servicio.ObtenerTodos();

                string url = "https://localhost:44365/api/Tipo/ConsultarTodos";
            var resultado = await SingletonHttp.GetInstancia().GetAsync(url);
            var lst = JsonConvert.DeserializeObject<List<Tipos>>(resultado);

            dgvUsers.DataSource = lst;

            }


        }

        private async void btnEditar_Click(System.Object sender, System.EventArgs e)
        {

            frmABMTipo formulario = new frmABMTipo();
            //Asi obtenemos el item seleccionado de la grilla
            var obj = (Tipos) dgvUsers.CurrentRow.DataBoundItem;
            formulario.InicializarFormulario(frmABMTipo.FormMode.modificar, obj);
            formulario.ShowDialog();


            string url = "https://localhost:44365/api/Tipo/ConsultarTodos";
            var resultado = await SingletonHttp.GetInstancia().GetAsync(url);
            var lst = JsonConvert.DeserializeObject<List<Tipos>>(resultado);
            dgvUsers.DataSource = lst;

            //dgvUsers.DataSource = servicio.ObtenerTodos();
            //Forzamos el evento Click para actalizar el DataGridView
            //btnConsultar_Click(sender, e);

        }

        private async void btnQuitar_Click(System.Object sender, System.EventArgs e)
        {
            frmABMTipo formulario = new frmABMTipo();
            //Asi obtenemos el item seleccionado de la grilla
            var obj = (Tipos)dgvUsers.CurrentRow.DataBoundItem;

            formulario.InicializarFormulario(frmABMTipo.FormMode.eliminar, obj);
            formulario.ShowDialog();


            string url = "https://localhost:44365/api/Tipo/ConsultarTodos";
            var resultado = await SingletonHttp.GetInstancia().GetAsync(url);
            var lst = JsonConvert.DeserializeObject<List<Tipos>>(resultado);
            dgvUsers.DataSource = lst;

            //dgvUsers.DataSource = servicio.ObtenerTodos();
            //Forzamos el evento Click para actalizar el DataGridView
            //btnConsultar_Click(sender, e);


        }

        private void InitializeDataGridView()
        {
            // Cree un DataGridView no vinculado declarando un recuento de columnas.
            dgvUsers.ColumnCount = 2;
            dgvUsers.ColumnHeadersVisible = true;

            // Configuramos la AutoGenerateColumns en false para que no se autogeneren las columnas
            dgvUsers.AutoGenerateColumns = false;

            // Cambia el estilo de la cabecera de la grilla.
            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();

            columnHeaderStyle.BackColor = Color.Beige;
            columnHeaderStyle.Font = new Font("Verdana", 8, FontStyle.Bold);
            dgvUsers.ColumnHeadersDefaultCellStyle = columnHeaderStyle;

            // Definimos el nombre de la columnas y el DataPropertyName que se asocia a DataSource
            dgvUsers.Columns[0].Name = "Tipo de Mascota";
            dgvUsers.Columns[0].DataPropertyName = "t_descrip";
            // Definimos el ancho de la columna.

            dgvUsers.Columns[1].Name = "Borrado";
            dgvUsers.Columns[1].DataPropertyName = "borrado";


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
