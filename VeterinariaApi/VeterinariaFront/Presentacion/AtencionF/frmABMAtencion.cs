
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VeterinariaBack.AccesoDatos;
using VeterinariaBack.Dominio;
using VeterinariaBack.Servicios;
using VeterinariaFront.HttpSingleton;
using Newtonsoft.Json;

namespace VeterinariaFront.Presentacion.AtencionF
{
    public partial class frmABMAtencion : Form
    {

        private FormMode formMode = FormMode.nuevo;
        private Atenciones obj;
 
        public Usuarios uLog { get; set; }


        private readonly AtencionService oService;
        private readonly MascotaService oSrvMas;
        private readonly ProductoService oPrdUsr;
        private readonly ClienteService oSerCli;

        private Atenciones oSelected;

        public frmABMAtencion()
        {
            InitializeComponent();
            InitializeDataGridView();

            oService = new AtencionService();
            oSrvMas = new MascotaService();
            oPrdUsr = new ProductoService();
            obj = new Atenciones();
            oSerCli = new ClienteService();



        }

        public enum FormMode
        {
            nuevo,          // Alta
            eliminar = 99,  // Baja
            modificar      //Modificación
            
        }

       

        private async void frmABMUsuario_Load(System.Object sender, System.EventArgs e)
        {
            this.CenterToParent();

            //LlenarCombo(cboCliente, oSerCli.ObtenerTodos(), "c_nombre", "id_cliente");
            //LlenarCombo(cboMascota, oSrvMas.ObtenerTodos(), "m_nombre", "id_mascota");
            //LlenarCombo(cboProducto, oPrdUsr.ObtenerTodos(), "p_nombre", "id_producto");

            string url = "https://localhost:44365/api/Cliente/ConsultarTodos";
            var resultado = await SingletonHttp.GetInstancia().GetAsync(url);
            var Clie = JsonConvert.DeserializeObject<List<Clientes>>(resultado);

            string url2 = "https://localhost:44365/api/Mascota/ConsultarTodos";
            var resultado2 = await SingletonHttp.GetInstancia().GetAsync(url2);
            var masc = JsonConvert.DeserializeObject<List<Mascotas>>(resultado2);

            string url3 = "https://localhost:44365/api/Producto/ConsultarTodos";
            var resultado3 = await SingletonHttp.GetInstancia().GetAsync(url3);
            var prod = JsonConvert.DeserializeObject<List<Productos>>(resultado3);

            LlenarCombo(cboCliente, Clie, "c_nombre", "id_cliente");
            LlenarCombo(cboMascota, masc, "m_nombre", "id_mascota");
            LlenarCombo(cboProducto, prod, "p_nombre", "id_producto");
            dtpFecha.Text = DateTime.Today.ToString();


            switch (formMode)
            {
                case FormMode.nuevo:
                    {
                        this.Text = "Nueva Atencion";
                        break;
                    }
                case FormMode.modificar:
                    {
                        this.Text = "Actualizar Atencion";
                        // Recuperar usuario seleccionado en la grilla 
                        MostrarDatos();
                        txtObservaciones.Enabled = true;   
                        dtpFecha.Enabled = true;

                        break;
                    }

                case FormMode.eliminar:
                    {
                        MostrarDatos();
                        this.Text = "Eliminar Atencion";
                        txtObservaciones.Enabled = false;
                        txtImporte.Enabled = false;
                        cboProducto.Enabled = false;
                        cboMascota.Enabled = false;
                        txtCantidad.Enabled = false;
                        btnAgregar.Enabled = false;
                        cboCliente.Enabled = false;


                        break;
                    }
            }
        }

        public void InicializarFormulario(FormMode op, Atenciones objSelected)
        {
            formMode = op;
            oSelected = objSelected;
        }

        private void MostrarDatos()
        {
            if (oSelected != null)
            {
                int idAten = oSelected.id_atencion;

                oSelected = (Atenciones)oService.Obtener(idAten);

                txtObservaciones.Text = oSelected.a_descrip;
                txtImporte.Text = oSelected.importe.ToString();
                cboMascota.Text = oSelected.mascota.m_nombre;
                cboCliente.Text = oSelected.mascota.cliente.c_nombre;
                dtpFecha.Text = oSelected.fecha.ToString();

                foreach(Detalles d in oSelected.detalle)
                {
                    dgvDetalle.Rows.Add(new object[] { d.cantidad, d.producto.p_nombre, d.p_facturado });
                }

                                              
            }
        }

        private async void btnAceptar_Click(System.Object sender, System.EventArgs e)
        {
            switch (formMode)
            {
                case FormMode.nuevo:
                    {
                        if (ValidarCampos())
                        {   
                            
                                
                            obj.a_descrip = txtObservaciones.Text;
                                                            
                            obj.mascota.id_mascota = (int)cboMascota.SelectedValue;

                            obj.usuario = uLog;

                            obj.fecha = dtpFecha.Value;



                            if (MessageBox.Show("Seguro que desea registrar nueva Atencion?", "Aviso", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                            {
                                string url = "https://localhost:44365/api/Atencion/CrearTrans";
                                string JSON = JsonConvert.SerializeObject(obj);
                                var resultado = await SingletonHttp.GetInstancia().PostAsync(url, JSON);
                                var res = JsonConvert.DeserializeObject<bool>(resultado);

                                // if (oService.CrearTrans(obj))
                                if (res)
                                {
                                    MessageBox.Show("Atencion registrada!", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    this.Close();
                                }
                            }                           
                            
                        }
                        break;
                    }

                case FormMode.modificar:
                    {
                        if (ValidarCampos())
                        {
                            oSelected.a_descrip = txtObservaciones.Text;
                            oSelected.importe = Convert.ToInt32(txtImporte.Text);

                            oSelected.mascota = new Mascotas();
                            oSelected.mascota.id_mascota = (int)cboMascota.SelectedValue;



                            if (MessageBox.Show("Seguro que desea actualizar Atencion?", "Aviso", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) 
                            { 
                                if (oService.Actualizar(oSelected)) 
                                {
                                    MessageBox.Show("Atencion actualizada!", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    this.Dispose();
                                }
                                else
                                    MessageBox.Show("Error al actualizar Atencion!", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }


                        break;
                    }

                case FormMode.eliminar:
                    {
                        if (MessageBox.Show("Seguro que desea deshabilitar Atencion seleccionada?", "Aviso", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                        {


                            if (oService.Eliminar(oSelected))
                            {
                                MessageBox.Show("Atencion Desabilitada", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Close();
                            }
                            else
                                MessageBox.Show("Error al desabilitar Atencion!", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        break;
                    }
            }
        }

        private bool ValidarCampos()
        {
            // campos obligatorios
            
            if (cboMascota.SelectedValue == null)
            {
                cboMascota.BackColor = Color.Red;

                cboMascota.Focus();
                return false;
            }
            cboMascota.BackColor = Color.White;

            if(dgvDetalle.Rows.Count == 0)
            {
                
                MessageBox.Show("Debe seleccionar un Producto...", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboProducto.Focus();
                return false;
            }

            //if (cboProducto.SelectedValue == null)
            //{
            //    cboProducto.BackColor = Color.Red;

            //    cboProducto.Focus();
            //    return false;
            //}
            //cboProducto.BackColor = Color.White;

            if (txtObservaciones.Text == string.Empty)
            {
                txtObservaciones.BackColor = Color.Red;
                txtObservaciones.Focus();
                return false;
            }
            else
                txtObservaciones.BackColor = Color.White;

            if (txtImporte.Text == string.Empty)
            {
                txtImporte.BackColor = Color.Red;
                txtImporte.Focus();
                return false;
            }
            else
                txtImporte.BackColor = Color.White;


            return true;
        }

        //private bool ExisteUsuario()
        //{
        //    return oService.Obtener(txtObservaciones.Text) != null;
            
        //}

        
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LlenarCombo(ComboBox cbo, Object source, string display, String value)
        {
            cbo.DataSource = source;
            cbo.DisplayMember = display;
            cbo.ValueMember = value;
            cbo.SelectedIndex = -1;
        }

        private void dgvDetalle_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //btnQuitar.Enabled = true;

            obj.detalle.RemoveAt(dgvDetalle.CurrentRow.Index);
            dgvDetalle.Rows.Remove(dgvDetalle.CurrentRow);


            obj.importe= obj.CalcularTotal();

            txtImporte.Text = obj.importe.ToString();


        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
           
            if (cboProducto.Text.Equals(string.Empty))
            {
                MessageBox.Show("Debe seleccionar un Producto...", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (string.IsNullOrEmpty(txtCantidad.Text) || !int.TryParse(txtCantidad.Text, out _) || Convert.ToInt32(txtCantidad.Text)<=0)
            {
                MessageBox.Show("Debe ingresar una cantidad válida...", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            

            if (dgvDetalle.Rows.Count>0)
            {
                foreach (DataGridViewRow row in dgvDetalle.Rows)
                {
                    if (row.Cells[1].Value.ToString().Equals(cboProducto.Text))
                    {
                        MessageBox.Show("Este producto ya está presupuestado...", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }

            }

            Detalles d = new Detalles();
            d.producto = new Productos();
           var item = (Productos)cboProducto.SelectedItem;

            d.producto.id_producto = item.id_producto;
            d.producto.p_nombre = item.p_nombre;
            d.p_facturado = item.precio;
            d.cantidad = Convert.ToInt32(txtCantidad.Text);
            

            obj.detalle.Add(d);
            obj.importe += d.cantidad * d.p_facturado;

            dgvDetalle.Rows.Add(new object[] { d.cantidad, d.producto.p_nombre, d.p_facturado });

            obj.importe = obj.CalcularTotal();

            txtImporte.Text = obj.importe.ToString();

            txtCantidad.Clear();
            cboProducto.SelectedIndex = -1;
            cboProducto.Focus();

        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {


            //aten.detalle.RemoveAt((int)dgvDetalle.CurrentRow.Index);
            //dgvDetalle.Rows.Remove(dgvDetalle.CurrentRow);
            //aten.importe -= d.cantidad * d.p_facturado;
            //txtImporte.Text = aten.importe.ToString();

        }

        private void InitializeDataGridView()
        {
            // Cree un DataGridView no vinculado declarando un recuento de columnas.
            dgvDetalle.ColumnCount = 3;
            dgvDetalle.ColumnHeadersVisible = true;

            // Configuramos la AutoGenerateColumns en false para que no se autogeneren las columnas
            dgvDetalle.AutoGenerateColumns = false;

            // Cambia el estilo de la cabecera de la grilla.
            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();

            columnHeaderStyle.BackColor = Color.Beige;
            columnHeaderStyle.Font = new Font("Verdana", 8, FontStyle.Bold);
            dgvDetalle.ColumnHeadersDefaultCellStyle = columnHeaderStyle;

            // Definimos el nombre de la columnas y el DataPropertyName que se asocia a DataSource
            dgvDetalle.Columns[0].Name = "Cantidad";
            dgvDetalle.Columns[0].DataPropertyName = "cantidad";
            // Definimos el ancho de la columna.
            dgvDetalle.Columns[1].Name = "Producto";
            dgvDetalle.Columns[1].DataPropertyName = "producto";

            dgvDetalle.Columns[2].Name = "Precio Unitario";
            dgvDetalle.Columns[2].DataPropertyName = "p_facturado";

    

            // Cambia el tamaño de la altura de los encabezados de columna.
            dgvDetalle.AutoResizeColumnHeadersHeight();

            // Cambia el tamaño de todas las alturas de fila para ajustar el contenido de todas las celdas que no sean de encabezado.
            dgvDetalle.AutoResizeRows(
                DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders);
        }

        private void cboCliente_SelectedValueChanged(object sender, EventArgs e)
        {
            
        }

        private void cboCliente_SelectionChangeCommitted(object sender, EventArgs e)
        {
          

            string cliente= cboCliente.SelectedItem.ToString();

           LlenarCombo(cboMascota, oSrvMas.MdeCli(cliente), "m_nombre", "id_mascota");

            
          
        }

        private void cboCliente_ValueMemberChanged(object sender, EventArgs e)
        {
           
        }

        private void cboCliente_DisplayMemberChanged(object sender, EventArgs e)
        {
           
        }

        private void cboCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
         
        }

    }
}
