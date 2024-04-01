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
    public partial class frmPrincipal : Form
    {
        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            cargarDatos();
        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmGestionUsuarios gUsuarios = new frmGestionUsuarios();
            gUsuarios.Show();
        }

        private void prestamosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmGestionPrestamos gPrestamos = new frmGestionPrestamos();
            gPrestamos.Show();  
        }

        private void librosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmGestionLibros gLibros = new frmGestionLibros();
            gLibros.Show();
        }

        private void detallesDeLibrosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDetallesLibro dLibros = new frmDetallesLibro();
            dLibros.Show();
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            cargarDatos();
        }

        private void cargarDatos()
        {
            Biblioteca2024Entities oBiblioteca2024Entities = new Biblioteca2024Entities();

            int usuarioEspecificoId = 3;

            var consultaPrestamoUsuario = (from prestamo in oBiblioteca2024Entities.Prestamos
                                           join usuario in oBiblioteca2024Entities.Usuarios
                                           on prestamo.Id_Usuario equals usuario.Id
                                           join libro in oBiblioteca2024Entities.Libros
                                           on prestamo.Id_Libro equals libro.Id
                                           where usuario.Id == usuarioEspecificoId
                                           select new
                                           {
                                               libro.Titulo,
                                               prestamo.Fecha_prestamo,
                                               prestamo.Fecha_devolucion
                                           }).ToList();

            dgvDatosPrestamoUsuario.DataSource = consultaPrestamoUsuario;

        }

        private void frmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if((MessageBox.Show("Esta seguro de salir?", "Cerrar", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)) 
                {
                    e.Cancel = true;
                }
        }
    }
}
