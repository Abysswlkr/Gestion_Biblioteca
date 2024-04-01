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
    public partial class frmDetallesLibro : Form
    {
        public frmDetallesLibro()
        {
            InitializeComponent();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string userInput = txtBuscar.Text.Trim();

            if(int.TryParse(userInput, out int idLibro))
            {
                BuscarPorId(idLibro);

            } else
            {
                BuscarPorNombre(userInput);
            }

        }

        private void BuscarPorId(int id)
        {
            //Llamamos a la base de datos
            Biblioteca2024Entities bbe = new Biblioteca2024Entities();

            //Query
            var consultaLibro = from libro in bbe.Libros
                                where
                                libro.Id == id
                                select new
                                {
                                    libro.Id,
                                    libro.Titulo,
                                    libro.Autor,
                                    libro.Genero,
                                    libro.Estado,
                                    libro.Imagen,
                                };

            //Asignación de valores a los textbox
            if (consultaLibro.Any())
            {
                var Libro = consultaLibro.First();

                txtIdLibro.Text = Libro.Id.ToString();
                txtTitulo.Text = Libro.Titulo.ToString();
                txtAutor.Text = Libro.Autor.ToString();
                txtGenero.Text = Libro.Genero.ToString();
                txtEstado.Text = Libro.Estado.ToString();
                byte[] imagen = Libro.Imagen.ToArray();

                using (MemoryStream ms = new MemoryStream(imagen))
                {
                    pbImagen.Image = Image.FromStream(ms);
                }

            } else
            {
                MessageBox.Show("No se encuentra ningún libro con el ID especificado", "Error", 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void BuscarPorNombre(string titulo)
        {
            //Llamamos a la base de datos
            Biblioteca2024Entities bbe = new Biblioteca2024Entities();

            //Query
            var consultaLibro = from libro in bbe.Libros
                                where
                                libro.Titulo == titulo
                                select new
                                {
                                    libro.Id,
                                    libro.Titulo,
                                    libro.Autor,
                                    libro.Genero,
                                    libro.Estado,
                                    libro.Imagen,
                                };

            //Asignación de valores a los textbox
            if (consultaLibro.Any())
            {
                var Libro = consultaLibro.First();

                txtIdLibro.Text = Libro.Id.ToString();
                txtTitulo.Text = Libro.Titulo.ToString();
                txtAutor.Text = Libro.Autor.ToString();
                txtGenero.Text = Libro.Genero.ToString();
                txtEstado.Text = Libro.Estado.ToString();
                byte[] imagen = Libro.Imagen.ToArray();

                using(MemoryStream ms = new MemoryStream(imagen))
                {
                    pbImagen.Image = Image.FromStream(ms);
                }
                

            }
            else
            {
                MessageBox.Show("No se encuentra ningún libro con el Titulo especificado", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            txtBuscar.Text = string.Empty;
            txtIdLibro.Text = string.Empty;
            txtTitulo.Text= string.Empty;
            txtAutor.Text= string.Empty;
            txtGenero.Text = string.Empty;
            txtEstado.Text = string.Empty;
            pbImagen.Image = null;

        }
    }
}
