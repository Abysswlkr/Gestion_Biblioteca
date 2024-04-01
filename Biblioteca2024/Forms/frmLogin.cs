using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Biblioteca2024.Forms
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            if ((MessageBox.Show("Esta seguro de salir?", "Cerrar", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK))
            {
                Close();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string sPass = Encrypt.GetSHA256(txtContrasena.Text.Trim());

            using (Models.Biblioteca2024Entities db = new Models.Biblioteca2024Entities()) 
            {
                //QUERY CON LINQ (LINQ RECUERDA QUE ES UN METALENGUAJE DE SQL TE SIRVE TANTO PARA HCAER QUERYS A LA BASE DE DATOS COMO PARA ARREGLOS)
                var lst = from usuario in db.Usuarios
                          where usuario.CorreoElectronico == txtCorreo.Text 
                          && usuario.Contraseña == sPass
                          select usuario;

                if (lst.Count() > 0)
                {
                    this.Hide();
                    frmPrincipal frm = new frmPrincipal();
                    frm.FormClosed += (s, args) => this.Close();
                    frm.Show();
                } else
                {
                    MessageBox.Show("Usuario no existe");
                }
            }
        }
    }
}
