-- Crear la tabla Usuarios
CREATE TABLE Usuarios (
    Id INT PRIMARY KEY,
    Nombre VARCHAR(255),
    CorreoElectronico VARCHAR(255),
    Contraseña VARCHAR(255),
    Rol VARCHAR(255)
);

-- Crear la tabla Libros
CREATE TABLE Libros (
    Id INT PRIMARY KEY,
    Titulo VARCHAR(255),
    Autor VARCHAR(255),
    Genero VARCHAR(255),
    Estado VARCHAR(255),
	Imagen VARBINARY(MAX)
);

-- Crear la tabla Prestamos
CREATE TABLE Prestamos (
    Id INT PRIMARY KEY,
    Id_Libro INT FOREIGN KEY REFERENCES Libros(Id),
    Id_Usuario INT FOREIGN KEY REFERENCES Usuarios(Id),
    Fecha_prestamo DATE,
    Fecha_devolucion DATE
);

