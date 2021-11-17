
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

namespace VeterinariaFront.Presentacion.UsuarioF
{
    public partial class frmABMUsuario : Form
    {

        private FormMode formMode = FormMode.nuevo;

     
        private readonly UsuarioService oUsuarioService;

        private Usuarios oUsuarioSelected;

        public frmABMUsuario()
        {
            InitializeComponent();
           
            oUsuarioService = new UsuarioService();
           
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
                        this.Text = "Nuevo Usuario";
                        break;
                    }
                case FormMode.modificar:
                    {
                        this.Text = "Actualizar Usuario";
                        // Recuperar usuario seleccionado en la grilla 
                        MostrarDatos();
                        txtNombre.Enabled = true;
                        txtEmail.Enabled = true;                       
                        txtPassword.Enabled = true;
                        txtConfirmarPass.Enabled = true;
                   
                        break;
                    }

                case FormMode.eliminar:
                    {
                        MostrarDatos();
                        this.Text = "Eliminar Usuario";
                        txtNombre.Enabled = false;
                        txtEmail.Enabled = false;                       
                        txtPassword.Enabled = false;                      
                        txtConfirmarPass.Enabled = false;
                        break;
                    }
            }
        }

        public void InicializarFormulario(FormMode op, Usuarios usuarioSelected)
        {
            formMode = op;
            oUsuarioSelected = usuarioSelected;
        }

        private void MostrarDatos()
        {
            if (oUsuarioSelected != null)
            {
                txtNombre.Text = oUsuarioSelected.usuario;
                txtEmail.Text = oUsuarioSelected.email;
                txtPassword.Text = oUsuarioSelected.password;
                txtConfirmarPass.Text = txtPassword.Text;
                              
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
                                var oUsuario = new Usuarios();
                                oUsuario.usuario = txtNombre.Text;
                                oUsuario.password = txtPassword.Text;
                                oUsuario.email = txtEmail.Text;
                                if (MessageBox.Show("Seguro que desea registrar usuario?", "Aviso", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                                {
                                    string url = "https://localhost:44365/api/Usuario/CrearUsuario";
                                    string JSON = JsonConvert.SerializeObject(oUsuario);
                                    var resultado = await SingletonHttp.GetInstancia().PostAsync(url, JSON);
                                    var res = JsonConvert.DeserializeObject<bool>(resultado);


                                    //if (oUsuarioService.CrearUsuario(oUsuario))
                                    if (res)
                                    {
                                        MessageBox.Show("Usuario insertado!", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        this.Close();
                                    }
                                }                           
                            }
                            else
                                MessageBox.Show("Nombre de usuario encontrado!. Ingrese un nombre diferente", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                        break;
                    }

                case FormMode.modificar:
                    {
                        if (ValidarCampos())
                        {
                            oUsuarioSelected.usuario = txtNombre.Text;
                            oUsuarioSelected.password = txtPassword.Text;
                            oUsuarioSelected.email = txtEmail.Text;

                            if (MessageBox.Show("Seguro que desea actualizar usuario?", "Aviso", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) 
                            {
                                string url = "https://localhost:44365/api/Usuario/ActualizarUsuario";
                                string JSON = JsonConvert.SerializeObject(oUsuarioSelected);
                                var resultado = await SingletonHttp.GetInstancia().PostAsync(url, JSON);
                                var res = JsonConvert.DeserializeObject<bool>(resultado);

                                //if (oUsuarioService.ActualizarUsuario(oUsuarioSelected))
                                if(res)
                                {
                                    MessageBox.Show("Usuario actualizado!", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        if (MessageBox.Show("Seguro que desea deshabilitar el usuario seleccionado?", "Aviso", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            string url = "https://localhost:44365/api/Usuario/EliminarUsuario";
                            string JSON = JsonConvert.SerializeObject(oUsuarioSelected);
                            var resultado = await SingletonHttp.GetInstancia().PostAsync(url, JSON);
                            var res = JsonConvert.DeserializeObject<bool>(resultado);

                            //if (oUsuarioService.EliminarUsuario(oUsuarioSelected))
                            if(res)
                            {
                                MessageBox.Show("Usuario Desabilitado", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Close();
                            }
                            else
                                MessageBox.Show("Error al desabilitar usuario!", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            if (txtPassword.Text == string.Empty)
            {
                txtPassword.BackColor = Color.Red;
                txtPassword.Focus();
                return false;
            }
            else
                txtPassword.BackColor = Color.White;

            if (txtConfirmarPass.Text == string.Empty)
            {
                txtConfirmarPass.BackColor = Color.Red;
                txtConfirmarPass.Focus();
                return false;
            }
            else
                txtConfirmarPass.BackColor = Color.White;

            

            if (txtPassword.Text!= txtConfirmarPass.Text)
            {
                MessageBox.Show("Contraseña y Repetir Contranseña son diferentes!", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            

            return true;
        }

        private bool ExisteUsuario()
        {
            return oUsuarioService.ObtenerUsuario(txtNombre.Text) != null;
        }

        
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
