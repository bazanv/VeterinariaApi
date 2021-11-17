
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

namespace VeterinariaFront.Presentacion.MascotaF
{
    public partial class frmABMMascota : Form
    {

        private FormMode formMode = FormMode.nuevo;
              
        private readonly MascotaService oService;
        private readonly ClienteService oSrvCli;
        private readonly TipoService oSrvTipo;

        private Mascotas oSelected;

        public frmABMMascota()
        {
            InitializeComponent();
           
            oService = new MascotaService();
            oSrvCli = new ClienteService();
            oSrvTipo = new TipoService();


        }

        public enum FormMode
        {
            nuevo,          // Alta
            eliminar = 99,  // Baja
            modificar      //Modificación
            
        }


        private async  void frmABMUsuario_Load(System.Object sender, System.EventArgs e)
        {
            this.CenterToParent();

            //LlenarCombo(cboCliente, oSrvCli.ObtenerTodos(), "c_nombre", "id_cliente");
            //LlenarCombo(cboTipo, oSrvTipo.ObtenerTodos(), "t_descrip", "id_tipo");

            string url = "https://localhost:44365/api/Cliente/ConsultarTodos";
            var resultado = await SingletonHttp.GetInstancia().GetAsync(url);
            var Clie = JsonConvert.DeserializeObject<List<Clientes>>(resultado);

            string url2 = "https://localhost:44365/api/Tipo/ConsultarTodos";
            var resultado2 = await SingletonHttp.GetInstancia().GetAsync(url2);
            var tip = JsonConvert.DeserializeObject<List<Tipos>>(resultado2);

            LlenarCombo(cboCliente, Clie, "c_nombre", "id_cliente");
            LlenarCombo(cboTipo, tip, "t_descrip", "id_tipo");


            switch (formMode)
            {
                case FormMode.nuevo:
                    {
                        this.Text = "Nueva Mascota";
                        break;
                    }
                case FormMode.modificar:
                    {
                        this.Text = "Actualizar Mascota";
                        // Recuperar usuario seleccionado en la grilla 
                        MostrarDatos();
                        txtNombre.Enabled = true;                     

                        break;
                    }

                case FormMode.eliminar:
                    {
                        MostrarDatos();
                        this.Text = "Eliminar Mascota";
                        txtNombre.Enabled = false;
                        txtEdad.Enabled = false;
                        cboCliente.Enabled = false;
                        cboTipo.Enabled = false;

                        break;
                    }
            }
        }

        public void InicializarFormulario(FormMode op, Mascotas objSelected)
        {
            formMode = op;
            oSelected = objSelected;
        }

        private void MostrarDatos()
        {
            if (oSelected != null)
            {
                txtNombre.Text = oSelected.m_nombre;
                txtEdad.Text = oSelected.edad.ToString();
                cboTipo.Text = oSelected.tipo.t_descrip;
                cboCliente.Text = oSelected.cliente.c_nombre;   
                                              
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
                                var obj = new Mascotas();
                                obj.m_nombre = txtNombre.Text;
                                obj.edad = Convert.ToInt32(txtEdad.Text);
                                
                                obj.tipo = new Tipos();
                                obj.tipo.id_tipo = (int)cboTipo.SelectedValue;

                                obj.cliente = new Clientes();
                                obj.cliente.id_cliente=(int)cboCliente.SelectedValue;



                                if (MessageBox.Show("Seguro que desea registrar nueva Mascota?", "Aviso", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                                {
                                    string url = "https://localhost:44365/api/Mascota/Crear";
                                    string JSON = JsonConvert.SerializeObject(obj);
                                    var resultado = await SingletonHttp.GetInstancia().PostAsync(url, JSON);
                                    var res = JsonConvert.DeserializeObject<bool>(resultado);

                                    //if (oService.Crear(obj))
                                    if(res)
                                    {
                                        MessageBox.Show("Mascota registrada!", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        this.Close();
                                    }
                                }                           
                            }
                            else
                                MessageBox.Show("Mascota encontrada!. Ingrese un nombre diferente", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
                        }
                        break;
                    }

                case FormMode.modificar:
                    {
                        if (ValidarCampos())
                        {
                            oSelected.m_nombre = txtNombre.Text;
                            oSelected.edad = Convert.ToInt32(txtEdad.Text);

                            oSelected.tipo = new Tipos();
                            oSelected.tipo.id_tipo = (int)cboTipo.SelectedValue;

                            oSelected.cliente = new Clientes();
                            oSelected.cliente.id_cliente = (int)cboCliente.SelectedValue;


                            if (MessageBox.Show("Seguro que desea actualizar Mascota?", "Aviso", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) 
                            {
                                string url = "https://localhost:44365/api/Mascota/Actualizar";
                                string JSON = JsonConvert.SerializeObject(oSelected);
                                var resultado = await SingletonHttp.GetInstancia().PostAsync(url, JSON);
                                var res = JsonConvert.DeserializeObject<bool>(resultado);

                                //if (oService.Actualizar(oSelected)) 
                                if(res)
                                {
                                    MessageBox.Show("Mascota actualizada!", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    this.Dispose();
                                }
                                else
                                    MessageBox.Show("Error al actualizar Mascota!", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }


                        break;
                    }

                case FormMode.eliminar:
                    {
                        if (MessageBox.Show("Seguro que desea deshabilitar Mascota seleccionada?", "Aviso", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            string url = "https://localhost:44365/api/Mascota/Eliminar";
                            string JSON = JsonConvert.SerializeObject(oSelected);
                            var resultado = await SingletonHttp.GetInstancia().PostAsync(url, JSON);
                            var res = JsonConvert.DeserializeObject<bool>(resultado);

                            //if (oService.Eliminar(oSelected))
                            if (res)
                            {
                                MessageBox.Show("Mascota Desabilitada", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Close();
                            }
                            else
                                MessageBox.Show("Error al desabilitar Mascota!", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            if (txtEdad.Text == string.Empty || Convert.ToInt32(txtEdad.Text)<=0)
            {
                txtEdad.BackColor = Color.Red;
                txtEdad.Focus();
                return false;
            }
            else
                txtEdad.BackColor = Color.White;

            if (cboTipo.SelectedValue == null)
            {
                cboTipo.BackColor = Color.Red;

                cboTipo.Focus();
                return false;
            }
            cboTipo.BackColor = Color.White;

            if (cboCliente.SelectedValue == null)
            {
                cboCliente.BackColor = Color.Red;

                cboCliente.Focus();
                return false;
            }
            cboCliente.BackColor = Color.White;

            return true;
        }

        private bool ExisteUsuario()
        {
            return oService.Obtener(txtNombre.Text) != null;
            
        }

        
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





    }
}
