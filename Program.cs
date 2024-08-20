using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Clase base Publicacion
public abstract class Publicacion
{
    public string Titulo { get; set; }
    public string Autor { get; set; }
    public string ISBN { get; set; }
    public int AñoPublicacion { get; set; }

    public Publicacion(string titulo, string autor, string isbn, int añoPublicacion)
    {
        Titulo = titulo;
        Autor = autor;
        ISBN = isbn;
        AñoPublicacion = añoPublicacion;
    }

    public abstract void MostrarInformacion();
}

// Clase derivada: Libro
public class Libro : Publicacion, IPrestable
{
    public int NumeroPaginas { get; set; }
    public bool Prestado { get; private set; }

    public Libro(string titulo, string autor, string isbn, int añoPublicacion, int numeroPaginas)
        : base(titulo, autor, isbn, añoPublicacion)
    {
        NumeroPaginas = numeroPaginas;
        Prestado = false;
    }

    public override void MostrarInformacion()
    {
        Console.WriteLine($"Libro: {Titulo}, Autor: {Autor}, ISBN: {ISBN}, Año: {AñoPublicacion}, Páginas: {NumeroPaginas}");
    }

    public void Prestar()
    {
        if (!Prestado)
        {
            Prestado = true;
            Console.WriteLine($"{Titulo} ha sido prestado.");
        }
        else
        {
            Console.WriteLine($"{Titulo} ya está prestado.");
        }
    }

    public void Devolver()
    {
        if (Prestado)
        {
            Prestado = false;
            Console.WriteLine($"{Titulo} ha sido devuelto.");
        }
        else
        {
            Console.WriteLine($"{Titulo} no estaba prestado.");
        }
    }

    public override string ToString()
    {
        return $"{Titulo}|{Autor}|{ISBN}|{AñoPublicacion}|{NumeroPaginas}|{Prestado}";
    }

    public static Libro FromString(string data)
    {
        var parts = data.Split('|');
        var libro = new Libro(parts[0], parts[1], parts[2], int.Parse(parts[3]), int.Parse(parts[4]));
        libro.Prestado = bool.Parse(parts[5]);
        return libro;
    }
}

// Clase derivada: Revista
public class Revista : Publicacion
{
    public int NumeroVolumenes { get; set; }

    public Revista(string titulo, string autor, string isbn, int añoPublicacion, int numeroVolumenes)
        : base(titulo, autor, isbn, añoPublicacion)
    {
        NumeroVolumenes = numeroVolumenes;
    }

    public override void MostrarInformacion()
    {
        Console.WriteLine($"Revista: {Titulo}, Autor: {Autor}, ISBN: {ISBN}, Año: {AñoPublicacion}, Volúmenes: {NumeroVolumenes}");
    }
}

// Clase derivada: DVD
public class DVD : Publicacion
{
    public int Duracion { get; set; }

    public DVD(string titulo, string autor, string isbn, int añoPublicacion, int duracion)
        : base(titulo, autor, isbn, añoPublicacion)
    {
        Duracion = duracion;
    }

    public override void MostrarInformacion()
    {
        Console.WriteLine($"DVD: {Titulo}, Autor: {Autor}, ISBN: {ISBN}, Año: {AñoPublicacion}, Duración: {Duracion} minutos");
    }
}

// Interfaz IPrestable
public interface IPrestable
{
    void Prestar();
    void Devolver();
}

// Clase Usuario
public class Usuario
{
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public int NumeroSocio { get; set; }

    public Usuario(string nombre, string apellido, int numeroSocio)
    {
        Nombre = nombre;
        Apellido = apellido;
        NumeroSocio = numeroSocio;
    }

    public void MostrarInformacion()
    {
        Console.WriteLine($"Usuario: {Nombre} {Apellido}, Número de Socio: {NumeroSocio}");
    }

    public override string ToString()
    {
        return $"{Nombre}|{Apellido}|{NumeroSocio}";
    }

    public static Usuario FromString(string data)
    {
        var parts = data.Split('|');
        return new Usuario(parts[0], parts[1], int.Parse(parts[2]));
    }
}

// Clase principal de gestión de biblioteca
public class Biblioteca
{
    private List<Libro> libros;
    private List<Usuario> usuarios;
    private const string archivoLibros = "libros.txt";
    private const string archivoUsuarios = "usuarios.txt";

    public Biblioteca()
    {
        libros = new List<Libro>();
        usuarios = new List<Usuario>();
        CargarLibros();
        CargarUsuarios();
    }

    // Métodos para gestionar libros
    public void AgregarLibro()
    {
        Console.WriteLine("Ingrese los datos del libro:");
        Console.Write("Título: ");
        string titulo = Console.ReadLine();
        Console.Write("Autor: ");
        string autor = Console.ReadLine();
        Console.Write("ISBN: ");
        string isbn = Console.ReadLine();
        Console.Write("Año de publicación: ");
        int añoPublicacion = int.Parse(Console.ReadLine());
        Console.Write("Número de páginas: ");
        int numeroPaginas = int.Parse(Console.ReadLine());

        Libro libro = new Libro(titulo, autor, isbn, añoPublicacion, numeroPaginas);
        libros.Add(libro);

        Console.WriteLine($"Libro '{titulo}' agregado a la biblioteca.");
        GuardarLibros();
    }

    public void ModificarLibro()
    {
        Console.Write("Ingrese el ISBN del libro a modificar: ");
        string isbn = Console.ReadLine();
        Libro libro = libros.FirstOrDefault(l => l.ISBN == isbn);
        if (libro != null)
        {
            Console.WriteLine("Ingrese los nuevos datos del libro:");
            Console.Write("Nuevo Título: ");
            libro.Titulo = Console.ReadLine();
            Console.Write("Nuevo Autor: ");
            libro.Autor = Console.ReadLine();
            Console.Write("Nuevo Año de publicación: ");
            libro.AñoPublicacion = int.Parse(Console.ReadLine());
            Console.Write("Nuevo Número de páginas: ");
            libro.NumeroPaginas = int.Parse(Console.ReadLine());

            Console.WriteLine($"Libro con ISBN {isbn} modificado.");
            GuardarLibros();
        }
        else
        {
            Console.WriteLine($"No se encontró un libro con ISBN {isbn}.");
        }
    }

    public void EliminarLibro()
    {
        Console.Write("Ingrese el ISBN del libro a eliminar: ");
        string isbn = Console.ReadLine();
        Libro libro = libros.FirstOrDefault(l => l.ISBN == isbn);
        if (libro != null)
        {
            libros.Remove(libro);
            Console.WriteLine($"Libro '{libro.Titulo}' eliminado de la biblioteca.");
            GuardarLibros();
        }
        else
        {
            Console.WriteLine($"No se encontró un libro con ISBN {isbn}.");
        }
    }

    public void BuscarLibro()
    {
        Console.Write("Ingrese el título, autor o ISBN del libro a buscar: ");
        string criterio = Console.ReadLine();
        var resultados = libros.Where(l => l.Titulo.Contains(criterio) || l.Autor.Contains(criterio) || l.ISBN.Contains(criterio)).ToList();
        if (resultados.Any())
        {
            Console.WriteLine("Resultados de la búsqueda:");
            foreach (var libro in resultados)
            {
                libro.MostrarInformacion();
            }
        }
        else
        {
            Console.WriteLine("No se encontraron libros que coincidan con el criterio de búsqueda.");
        }
    }

    // Métodos para gestionar usuarios
    public void AgregarUsuario()
    {
        Console.WriteLine("Ingrese los datos del usuario:");
        Console.Write("Nombre: ");
        string nombre = Console.ReadLine();
        Console.Write("Apellido: ");
        string apellido = Console.ReadLine();
        Console.Write("Número de socio: ");
        int numeroSocio = int.Parse(Console.ReadLine());

        Usuario usuario = new Usuario(nombre, apellido, numeroSocio);
        usuarios.Add(usuario);

        Console.WriteLine($"Usuario '{nombre} {apellido}' agregado a la biblioteca.");
        GuardarUsuarios();
    }

    public void ModificarUsuario()
    {
        Console.Write("Ingrese el Número de socio del usuario a modificar: ");
        int numeroSocio = int.Parse(Console.ReadLine());
        Usuario usuario = usuarios.FirstOrDefault(u => u.NumeroSocio == numeroSocio);
        if (usuario != null)
        {
            Console.WriteLine("Ingrese los nuevos datos del usuario:");
            Console.Write("Nuevo Nombre: ");
            usuario.Nombre = Console.ReadLine();
            Console.Write("Nuevo Apellido: ");
            usuario.Apellido = Console.ReadLine();

            Console.WriteLine($"Usuario con Número de Socio {numeroSocio} modificado.");
            GuardarUsuarios();
        }
        else
        {
            Console.WriteLine($"No se encontró un usuario con Número de Socio {numeroSocio}.");
        }
    }

    public void EliminarUsuario()
    {
        Console.Write("Ingrese el Número de socio del usuario a eliminar: ");
        int numeroSocio = int.Parse(Console.ReadLine());
        Usuario usuario = usuarios.FirstOrDefault(u => u.NumeroSocio == numeroSocio);
        if (usuario != null)
        {
            usuarios.Remove(usuario);
            Console.WriteLine($"Usuario '{usuario.Nombre} {usuario.Apellido}' eliminado de la biblioteca.");
            GuardarUsuarios();
        }
        else
        {
            Console.WriteLine($"No se encontró un usuario con Número de Socio {numeroSocio}.");
        }
    }

    // Métodos para gestionar préstamos
    public void PrestarLibro()
    {
        Console.Write("Ingrese el ISBN del libro a prestar: ");
        string isbn = Console.ReadLine();
        Console.Write("Ingrese el Número de socio del usuario: ");
        int numeroSocio = int.Parse(Console.ReadLine());
        Libro libro = libros.FirstOrDefault(l => l.ISBN == isbn);
        Usuario usuario = usuarios.FirstOrDefault(u => u.NumeroSocio == numeroSocio);
        if (libro != null && usuario != null)
        {
            libro.Prestar();
            Console.WriteLine($"Libro '{libro.Titulo}' prestado al usuario '{usuario.Nombre} {usuario.Apellido}'.");
            GuardarLibros();
        }
        else
        {
            Console.WriteLine($"No se pudo realizar el préstamo. Verifique el ISBN del libro y el número de socio.");
        }
    }

    public void DevolverLibro()
    {
        Console.Write("Ingrese el ISBN del libro a devolver: ");
        string isbn = Console.ReadLine();
        Console.Write("Ingrese el Número de socio del usuario: ");
        int numeroSocio = int.Parse(Console.ReadLine());
        Libro libro = libros.FirstOrDefault(l => l.ISBN == isbn);
        Usuario usuario = usuarios.FirstOrDefault(u => u.NumeroSocio == numeroSocio);
        if (libro != null && usuario != null)
        {
            libro.Devolver();
            Console.WriteLine($"Libro '{libro.Titulo}' devuelto por el usuario '{usuario.Nombre} {usuario.Apellido}'.");
            GuardarLibros();
        }
        else
        {
            Console.WriteLine($"No se pudo realizar la devolución. Verifique el ISBN del libro y el número de socio.");
        }
    }

    // Métodos para guardar y cargar datos
    private void GuardarLibros()
    {
        using (StreamWriter writer = new StreamWriter(archivoLibros))
        {
            foreach (var libro in libros)
            {
                writer.WriteLine(libro.ToString());
            }
        }
    }

    private void GuardarUsuarios()
    {
        using (StreamWriter writer = new StreamWriter(archivoUsuarios))
        {
            foreach (var usuario in usuarios)
            {
                writer.WriteLine(usuario.ToString());
            }
        }
    }

    private void CargarLibros()
    {
        if (File.Exists(archivoLibros))
        {
            using (StreamReader reader = new StreamReader(archivoLibros))
            {
                string linea;
                while ((linea = reader.ReadLine()) != null)
                {
                    libros.Add(Libro.FromString(linea));
                }
            }
        }
    }

    private void CargarUsuarios()
    {
        if (File.Exists(archivoUsuarios))
        {
            using (StreamReader reader = new StreamReader(archivoUsuarios))
            {
                string linea;
                while ((linea = reader.ReadLine()) != null)
                {
                    usuarios.Add(Usuario.FromString(linea));
                }
            }
        }
    }

    // Menú principal
    public void Menu()
    {
        int opcion;
        do
        {
            Console.WriteLine("\nSistema de Gestión de Biblioteca");
            Console.WriteLine("1. Agregar Libro");
            Console.WriteLine("2. Modificar Libro");
            Console.WriteLine("3. Eliminar Libro");
            Console.WriteLine("4. Buscar Libro");
            Console.WriteLine("5. Agregar Usuario");
            Console.WriteLine("6. Modificar Usuario");
            Console.WriteLine("7. Eliminar Usuario");
            Console.WriteLine("8. Prestar Libro");
            Console.WriteLine("9. Devolver Libro");
            Console.WriteLine("0. Salir");
            Console.Write("Seleccione una opción: ");
            opcion = int.Parse(Console.ReadLine());

            switch (opcion)
            {
                case 1:
                    AgregarLibro();
                    break;
                case 2:
                    ModificarLibro();
                    break;
                case 3:
                    EliminarLibro();
                    break;
                case 4:
                    BuscarLibro();
                    break;
                case 5:
                    AgregarUsuario();
                    break;
                case 6:
                    ModificarUsuario();
                    break;
                case 7:
                    EliminarUsuario();
                    break;
                case 8:
                    PrestarLibro();
                    break;
                case 9:
                    DevolverLibro();
                    break;
                case 0:
                    GuardarLibros();
                    GuardarUsuarios();
                    Console.WriteLine("Saliendo del sistema...");
                    break;
                default:
                    Console.WriteLine("Opción no válida, intente nuevamente.");
                    break;
            }
        } while (opcion != 0);
    }
}

// Clase principal del programa
class Program
{
    static void Main()
    {
        Biblioteca biblioteca = new Biblioteca();
        biblioteca.Menu();

        // Pausa para evitar el cierre inmediato de la consola
        Console.WriteLine("Presiona Enter para cerrar...");
        Console.ReadLine();
    }
}
