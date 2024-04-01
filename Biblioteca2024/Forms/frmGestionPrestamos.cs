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
    public partial class frmGestionPrestamos : Form
    {
        public frmGestionPrestamos()
        {
            InitializeComponent();
        }


        //Carga de datos al momento en que se cargue la pestaña
        private void frmGestionPrestamos_Load(object sender, EventArgs e)
        {
            cargarDatos();
        }

        //Carga de datos de prestamo
        private void cargarDatos()
        {
            Biblioteca2024Entities oBiblioteca2024Entities = new Biblioteca2024Entities();
            var prestamosRegistrados = oBiblioteca2024Entities.Prestamos.ToList();

            dgvListaPrestamos.DataSource = prestamosRegistrados;
        }

        //Utilizando el evento cargarDatos lo implementamos al boton Limpiar para refrescar la data de la Lista
        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            cargarDatos();
        }


        //Evento para crear el prestamo en la bd
        private void btnCrearPrestamo_Click(object sender, EventArgs e)
        {
            string estadoLibro = "";
            Biblioteca2024Entities oBiblioteca2024Entities = new Biblioteca2024Entities();

            int id = GeneradorId.SiguienteId();
            int idLibro = int.Parse(txtIdLibro.Text);
            int idUsuario = int.Parse(txtIdUsuario.Text);   
            DateTime fPrestamo = dtpPrestamo.Value;
            DateTime fDevolucion = dtpDevolucion.Value;

            var rev = from libro in oBiblioteca2024Entities.Libros
                      where libro.Id == idLibro
                      select new
                      {
                          libro.Estado,
                      };

            if (rev.Any())
            {
                var Libro = rev.First();

                estadoLibro = Libro.Estado.ToString();
            } else
            {
                MessageBox.Show("Estado del libro no encontrado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (estadoLibro.Equals("Disponible"))
            {
                Prestamos nuevoPrestamo = new Prestamos
                {
                    Id = id,
                    Id_Libro = idLibro,
                    Id_Usuario = idUsuario,
                    Fecha_prestamo = fPrestamo,
                    Fecha_devolucion = fDevolucion,
                };

                oBiblioteca2024Entities.Prestamos.Add(nuevoPrestamo);
                oBiblioteca2024Entities.SaveChanges();


                //Cambio del dato de Disponible a Prestado --
                // Encuentra el libro en la base de datos
                var libro = oBiblioteca2024Entities.Libros.FirstOrDefault(u => u.Id == idLibro);
                
                // Actualiza el libro con el valor prestado
                libro.Estado = "Prestado";

                // Guarda los cambios en la base de datos
                oBiblioteca2024Entities.SaveChanges();
                }
                else
                {
                    MessageBox.Show("El libro ya fue prestado a otro usuario", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            cargarDatos();
            }


        private class GeneradorId
        {
            public static int SiguienteId()
            {
                Biblioteca2024Entities oBiblioteca2024Entities = new Biblioteca2024Entities();
                var ultimoId = (from prestamo in oBiblioteca2024Entities.Prestamos
                                orderby prestamo.Id descending
                                select prestamo.Id).FirstOrDefault();

                return ++ultimoId;
            }

        }
    }
}
