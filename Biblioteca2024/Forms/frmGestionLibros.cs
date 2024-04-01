using Biblioteca2024.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Biblioteca2024.Forms
{
    public partial class frmGestionLibros : Form
    {
        //Variable de instancia
        private byte[] imagenBytes;
        public frmGestionLibros()
        {
            InitializeComponent();
        }

        private void frmGestionLibros_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void btnCrearPrestamo_Click(object sender, EventArgs e)
        {
            Biblioteca2024Entities oBiblioteca2024Entities = new Biblioteca2024Entities();


            int id = GeneradorId.SiguienteId();
            string titulo = txtTitulo.Text;
            string autor = txtAutor.Text;
            string genero = cbGenero.Text;
            string estado = cbEstado.Text;
            byte[] imagen = imagenBytes;


            Libros nuevoLibro = new Libros
            {
                Id = id,
                Titulo = titulo,
                Autor = autor,
                Genero = genero,
                Estado = estado,
                Imagen = imagen,
                
            };

            oBiblioteca2024Entities.Libros.Add(nuevoLibro);

            oBiblioteca2024Entities.SaveChanges();
            CargarDatos();
        }

        private void btnAbrirImagen_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "C:\\";

                openFileDialog.Filter = "Archivos de imágenes|*.jpg;*.jpeg;*.png;*.bmp|Todos los archivos (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string rutaImagen = openFileDialog.FileName;

                        imagenBytes = File.ReadAllBytes(rutaImagen);

                        txtDireccionImagen.Text = rutaImagen;


                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show($"Error al abrir el archivo: {ex.Message}");
                    }
                }

            }
        }

        public void CargarDatos()
        {
            Biblioteca2024Entities oBiblioteca2024Entities = new Biblioteca2024Entities();
            var librosIngresados = oBiblioteca2024Entities.Libros.ToList();

            dgvListaLibros.DataSource = librosIngresados;
        }

        private class GeneradorId
        {
            public static int SiguienteId()
            {
                Biblioteca2024Entities oBiblioteca2024Entities = new Biblioteca2024Entities();
                var ultimoId = (from libro in oBiblioteca2024Entities.Libros
                                orderby libro.Id descending
                                select libro.Id).FirstOrDefault();

                return ++ultimoId;
            }

        }

        private void dgvListaLibrosEdit_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            // Obtén el ID del libro que se está editando
            int idLibro = (int)dgvListaLibros.Rows[e.RowIndex].Cells["Id"].Value;

            Biblioteca2024Entities oBiblioteca2024Entities = new Biblioteca2024Entities();

            // Encuentra el libro en la base de datos
            var libro = oBiblioteca2024Entities.Libros.FirstOrDefault(u => u.Id == idLibro);

            if ((MessageBox.Show("Seguro de editar este dato?", "Edición de libro", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) ==
                                DialogResult.OK))
            {
                if (libro != null)
                {
                    // Actualiza el el libro con los nuevos valores
                    libro.Titulo = dgvListaLibros.Rows[e.RowIndex].Cells["Titulo"].Value.ToString();
                    libro.Autor = dgvListaLibros.Rows[e.RowIndex].Cells["Autor"].Value.ToString();
                    libro.Genero = dgvListaLibros.Rows[e.RowIndex].Cells["Genero"].Value.ToString();
                    libro.Estado = dgvListaLibros.Rows[e.RowIndex].Cells["Estado"].Value.ToString();

                    // Guarda los cambios en la base de datos
                    oBiblioteca2024Entities.SaveChanges();
                    CargarDatos();
                }
                CargarDatos();
            }


        }
    }
}
