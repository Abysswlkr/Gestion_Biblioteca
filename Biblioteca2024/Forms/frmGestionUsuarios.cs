using Biblioteca2024.Models;
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
    public partial class frmGestionUsuarios : Form
    {
        public frmGestionUsuarios()
        {
            InitializeComponent();
        }

        private void frmGestionUsuarios_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void dgvListaUsuarios_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Obtén el ID del usuario que se está editando
            int idUsuario = (int)dgvListaUsuarios.Rows[e.RowIndex].Cells["Id"].Value;

            Biblioteca2024Entities oBiblioteca2024Entities = new Biblioteca2024Entities();

            // Encuentra el usuario en la base de datos
            var usuario = oBiblioteca2024Entities.Usuarios.FirstOrDefault(u => u.Id == idUsuario);

            if (usuario != null)
            {
                // Actualiza el usuario con los nuevos valores
                usuario.Nombre = dgvListaUsuarios.Rows[e.RowIndex].Cells["Nombre"].Value.ToString();
                usuario.CorreoElectronico = dgvListaUsuarios.Rows[e.RowIndex].Cells["CorreoElectronico"].Value.ToString();
                usuario.Rol = dgvListaUsuarios.Rows[e.RowIndex].Cells["Rol"].Value.ToString();
                usuario.Contraseña = Encrypt.GetSHA256(dgvListaUsuarios.Rows[e.RowIndex].Cells["Contraseña"].Value.ToString());

                // Guarda los cambios en la base de datos
                oBiblioteca2024Entities.SaveChanges();
                CargarDatos();
            }
        }

        private void btnCrearUsuario_Click(object sender, EventArgs e)
        {
            Biblioteca2024Entities oBiblioteca2024Entities = new Biblioteca2024Entities();

            
            int id = GeneradorId.SiguienteId();
            string nombre = txtNombre.Text;
            string correo = txtCorreo.Text;
            string rol = txtRol.Text;

            if (rol != "Administrador" && rol != "Usuario")
            {
                MessageBox.Show("Rol de usuario no identificado, intentelo nuevamente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string contrasena = Encrypt.GetSHA256(txtContrasena.Text);
            string rContrasena = Encrypt.GetSHA256(txtRepetirContrasena.Text);

            if (contrasena != rContrasena)
            {
                MessageBox.Show("La contraseña no es la misma, intentelo nuevamente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            Usuarios nuevoUsuario = new Usuarios
            {
                Id = id,
                Nombre = nombre,
                CorreoElectronico = correo,
                Rol = rol,
                Contraseña = contrasena,
            };

            oBiblioteca2024Entities.Usuarios.Add(nuevoUsuario);

            oBiblioteca2024Entities.SaveChanges();
            CargarDatos();
        }

        public void CargarDatos()
        {
            Biblioteca2024Entities oBiblioteca2024Entities = new Biblioteca2024Entities();
            var usuariosRegistrados = oBiblioteca2024Entities.Usuarios.ToList();

            dgvListaUsuarios.DataSource = usuariosRegistrados;
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            Biblioteca2024Entities oBiblioteca2024Entities = new Biblioteca2024Entities();
            var usuariosRegistrados = oBiblioteca2024Entities.Usuarios.ToList();

            dgvListaUsuarios.DataSource = usuariosRegistrados;
        }

        private class GeneradorId
        {
            public static int SiguienteId() { 
            Biblioteca2024Entities oBiblioteca2024Entities = new Biblioteca2024Entities();
            var ultimoId = (from usuario in oBiblioteca2024Entities.Usuarios
                           orderby usuario.Id descending
                           select usuario.Id).FirstOrDefault();
            
            return ++ultimoId;
            }

        }
    }
}
