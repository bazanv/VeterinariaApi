﻿
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VeterinariaBack.AccesoDatos;
using VeterinariaBack.Dominio;
using VeterinariaBack.Servicios;
using VeterinariaFront.HttpSingleton;

namespace VeterinariaFront.Presentacion
{
    public partial class frmLogin : Form
    {
        
        private IService servicio;

        public string UsuarioLogueado { get; internal set; }

        public frmLogin()
        {
            //Se inicializan los controles del formulario, si se elimina el formulario se inicia vacio (sin controles ).
            InitializeComponent();
            servicio = new UsuarioService();
            
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            // Terminamos la aplicacion dado que el usuario no inicio sesion.
            Environment.Exit(0);
        }

        private async void btnIngresar_Click(object sender, EventArgs e)
        {
            //Validamos que se haya ingresado un usuario.
            if ((txtUsuario.Text == ""))
            {
                MessageBox.Show("Se debe ingresar un usuario.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //Validamos que se haya ingresado una password
            if ((txtPassword.Text == ""))
            {
                MessageBox.Show("Se debe ingresar una contraseña.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
                  
            //string url = "https://localhost:44365/api/Usuario/ObtenerUsuario";
        

            //string uJson = JsonConvert.SerializeObject(txtUsuario.Text.ToString());
            //var respuesta= await SingletonHttp.GetInstancia().PostAsync(url, uJson);
            //var usr = JsonConvert.DeserializeObject<Usuarios>(respuesta);

            var usr = servicio.ValidarUsuario(txtUsuario.Text, txtPassword.Text);
            //Controlamos que las creadenciales sean las correctas. 
            if (usr != null && usr.password== txtPassword.Text)
            {
                // Login OK
                frmPrincipal formulario = new frmPrincipal();
                formulario.uLog = usr;
                formulario.ShowDialog();
             
                this.Close();
            }
            else
            {
                //Limpiamos el campo password, para que el usuario intente ingresar un usuario distinto.
                txtPassword.Text = "";
                // Enfocamos el cursor en el campo password para que el usuario complete sus datos.
                txtPassword.Focus();
                //Mostramos un mensaje indicando que el usuario/password es invalido.
                MessageBox.Show("Debe ingresar usuario y/o contraseña válidos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

          
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            //Mostramos el formulario al centro del formulario padre.
            this.CenterToParent();
        }
    }

}
