
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

namespace VeterinariaFront.Presentacion.ClienteF
{
    public partial class frmABMCliente : Form
    {

        private FormMode formMode = FormMode.nuevo;

      

        private readonly ClienteService oClienteService;

        private Clientes oClienteSelected;

        public frmABMCliente()
        {
            InitializeComponent();
           
            oClienteService = new ClienteService();
           
        }

        public enum FormMode
        {
            nuevo,          // Alta
            eliminar = 99,  // Baja
            modificar      //Modificación
            
        }


        private void frmABMUsuario_Load(System.Object sender, System.EventArgs e)
        {
            this.CenterToParent();

            switch (formMode)
            {
                case FormMode.nuevo:
                    {
                        this.Text = "Nuevo Cliente";
                        break;
                    }
                case FormMode.modificar:
                    {
                        this.Text = "Actualizar Cliente";
                        // Recuperar usuario seleccionado en la grilla 
                        MostrarDatos();
                        txtNombre.Enabled = true;
                        rbtMasculino.Enabled = true;
                        rbtFemenino.Enabled = true;                       

                   
                        break;
                    }

                case FormMode.eliminar:
                    {
                        MostrarDatos();
                        this.Text = "Eliminar Cliente";
                        txtNombre.Enabled = false;
                        rbtMasculino.Enabled = false;
                        rbtFemenino.Enabled = false;                      

                        break;
                    }
            }
        }

        public void InicializarFormulario(FormMode op, Clientes clienteSelected)
        {
            formMode = op;
            oClienteSelected = clienteSelected;
        }

        private void MostrarDatos()
        {
            if (oClienteSelected != null)
            {
                txtNombre.Text = oClienteSelected.c_nombre;

                if (oClienteSelected.sexo) rbtMasculino.Checked = true;
                else rbtFemenino.Checked = true;
        
                              
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
                            if  (ExisteUsuario() == false)
                            {
                                var oCliente = new Clientes();
                                oCliente.c_nombre = txtNombre.Text;
                                if (rbtMasculino.Checked) oCliente.sexo = true;
                                else oCliente.sexo = false;

                                if (MessageBox.Show("Seguro que desea registrar Cliente?", "Aviso", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                                {

                                    string url = "https://localhost:44365/api/Cliente/Crear";
                                    string JSON = JsonConvert.SerializeObject(oCliente);
                                    var resultado = await SingletonHttp.GetInstancia().PostAsync(url, JSON);
                                    var res = JsonConvert.DeserializeObject<bool>(resultado);


                                    // if (oClienteService.CrearCliente(oCliente))
                                    if (res)
                                    {
                                        MessageBox.Show("Cliente insertado!", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        this.Close();
                                    }
                                }                           
                            }
                            else
                                MessageBox.Show("Nombre de Cliente encontrado!. Ingrese un nombre diferente", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
                        }
                        break;
                    }

                case FormMode.modificar:
                    {
                        if (ValidarCampos())
                        {
                            oClienteSelected.c_nombre = txtNombre.Text;

                            if (rbtMasculino.Checked) oClienteSelected.sexo = true;
                            else oClienteSelected.sexo = false;

                            if (MessageBox.Show("Seguro que desea actualizar Cliente?", "Aviso", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) 
                            { 
                                if (oClienteService.ActualizarCliente(oClienteSelected)) 
                                {
                                    MessageBox.Show("Cliente actualizado!", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    this.Dispose();
                                }
                                else
                                    MessageBox.Show("Error al actualizar el usuario!", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }


                        break;
                    }

                case FormMode.eliminar:
                    {
                        if (MessageBox.Show("Seguro que desea deshabilitar el Cliente seleccionado?", "Aviso", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                        {


                            if (oClienteService.EliminarCliente(oClienteSelected))
                            {
                                MessageBox.Show("Cliente Desabilitado", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Close();
                            }
                            else
                                MessageBox.Show("Error al desabilitar Cliente!", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        break;
                    }
            }
        }

        private bool ValidarCampos()
        {
            // campos obligatorios
            if (txtNombre.Text == string.Empty)
            {
                txtNombre.BackColor = Color.Red;
                txtNombre.Focus();
                return false;
            }
            else
                txtNombre.BackColor = Color.White;
            

            if (!rbtMasculino.Checked) 
            {   
                if (!rbtFemenino.Checked)
                {
                    MessageBox.Show("Seleccione Sexo!", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            

            return true;
        }

        private bool ExisteUsuario()
        {
            return oClienteService.ObtenerCliente(txtNombre.Text) != null;
            
        }

        
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
