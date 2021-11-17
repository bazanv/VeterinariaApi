
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

namespace VeterinariaFront.Presentacion.TipoF
{
    public partial class frmABMTipo : Form
    {

        private FormMode formMode = FormMode.nuevo;

      

        private readonly TipoService oService;

        private Tipos oSelected;

        public frmABMTipo()
        {
            InitializeComponent();
           
            oService = new TipoService();
           
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
                        this.Text = "Nuevo Tipo de Mascota";
                        break;
                    }
                case FormMode.modificar:
                    {
                        this.Text = "Actualizar Tipo de Mascota";
                        // Recuperar usuario seleccionado en la grilla 
                        MostrarDatos();
                        txtNombre.Enabled = true;                     

                        break;
                    }

                case FormMode.eliminar:
                    {
                        MostrarDatos();
                        this.Text = "Eliminar Tipo de Mascota";
                        txtNombre.Enabled = false;                    

                        break;
                    }
            }
        }

        public void InicializarFormulario(FormMode op, Tipos objSelected)
        {
            formMode = op;
            oSelected = objSelected;
        }

        private void MostrarDatos()
        {
            if (oSelected != null)
            {
                txtNombre.Text = oSelected.t_descrip;
                                              
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
                                var oTipo = new Tipos();
                                oTipo.t_descrip = txtNombre.Text;

                                if (MessageBox.Show("Seguro que desea registrar Tipo de Mascota?", "Aviso", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                                {
                                    string url = "https://localhost:44365/api/Tipo/Crear";
                                    string JSON = JsonConvert.SerializeObject(oTipo);
                                    var resultado = await SingletonHttp.GetInstancia().PostAsync(url, JSON);
                                    var res = JsonConvert.DeserializeObject<bool>(resultado);


                                    //if (oService.Crear(oTipo))
                                    if(res)
                                    {
                                        MessageBox.Show("Tipo de Mascota registado!", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        this.Close();
                                    }
                                }                           
                            }
                            else
                                MessageBox.Show("Tipo de Mascota encontrado!. Ingrese un nombre diferente", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
                        }
                        break;
                    }

                case FormMode.modificar:
                    {
                        if (ValidarCampos())
                        {
                            oSelected.t_descrip = txtNombre.Text;


                            if (MessageBox.Show("Seguro que desea actualizar Tipo de Mascota?", "Aviso", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) 
                            {
                                string url = "https://localhost:44365/api/Tipo/Actualizar";
                                string JSON = JsonConvert.SerializeObject(oSelected);
                                var resultado = await SingletonHttp.GetInstancia().PostAsync(url, JSON);
                                var res = JsonConvert.DeserializeObject<bool>(resultado);

                                //if (oService.Actualizar(oSelected)) 
                                if (res)
                                {
                                    MessageBox.Show("Tipo de Mascota actualizado!", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    this.Dispose();
                                }
                                else
                                    MessageBox.Show("Error al actualizar el Tipo de Mascota!", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }


                        break;
                    }

                case FormMode.eliminar:
                    {
                        if (MessageBox.Show("Seguro que desea deshabilitar el Tipo de Mascota seleccionado?", "Aviso", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            string url = "https://localhost:44365/api/Tipo/Eliminar";
                            string JSON = JsonConvert.SerializeObject(oSelected);
                            var resultado = await SingletonHttp.GetInstancia().PostAsync(url, JSON);
                            var res = JsonConvert.DeserializeObject<bool>(resultado);

                            //if (oService.Eliminar(oSelected))
                            if(res)
                            {
                                MessageBox.Show("Tipo de Mascota Desabilitado", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Close();
                            }
                            else
                                MessageBox.Show("Error al desabilitar Tipo de Mascota!", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
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


    }
}
