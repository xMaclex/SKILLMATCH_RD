using SKILLMATCH_RD.Models;
using SKILLMATCH_RD.Models.Enums;

namespace SKILLMATCH_RD.Data;

// Siembra inicial de datos (Avance 3).
// Son los mismos datos que en el Avance 2 vivian como listas estaticas en los
// controladores; ahora se insertan en la base de datos SQLite la primera vez que
// arranca la aplicacion, de modo que las vistas se alimentan de datos reales.
public static class DbInitializer
{
    public static void Seed(AppDbContext db)
    {
        db.Database.EnsureCreated();

        if (db.Ofertas.Any())
            return; // ya sembrado

        // ---- Universidad ----
        var itla = new Universidad
        {
            Nombre = "Instituto Tecnologico de Las Americas",
            Tipo = "Instituto Tecnico Superior",
            Ciudad = "Santo Domingo Este"
        };
        db.Universidades.Add(itla);

        // ---- Aptitudes (reutilizables) ----
        var cache = new Dictionary<string, Aptitud>();
        Aptitud Apt(string nombre, string categoria = "General")
        {
            if (!cache.TryGetValue(nombre, out var a))
            {
                a = new Aptitud { Nombre = nombre, Categoria = categoria };
                cache[nombre] = a;
                db.Aptitudes.Add(a);
            }
            return a;
        }

        // ---- Empresas ----
        var techCorp   = new Empresa { Nombre = "TechCorp RD",    Sector = "Tecnologia", Ciudad = "Santo Domingo",        Email = "rrhh@techcorp.do",     Telefono = "809-555-0101" };
        var bancoDig   = new Empresa { Nombre = "Banco Digital RD", Sector = "Finanzas",  Ciudad = "Santiago",             Email = "empleos@bancodigital.do", Telefono = "809-555-0102" };
        var cyberShield= new Empresa { Nombre = "CyberShield RD",  Sector = "Ciberseguridad", Ciudad = "Santo Domingo",   Email = "talento@cybershield.do", Telefono = "809-555-0103" };
        var creativa   = new Empresa { Nombre = "CreativaStudio",  Sector = "Diseno",    Ciudad = "Santo Domingo",        Email = "hola@creativastudio.do", Telefono = "809-555-0104" };
        var dataCenter = new Empresa { Nombre = "DataCenter RD",   Sector = "Tecnologia", Ciudad = "Santiago",            Email = "jobs@datacenter.do",   Telefono = "809-555-0105" };
        var retailMax  = new Empresa { Nombre = "RetailMax RD",    Sector = "Retail",    Ciudad = "San Pedro de Macoris", Email = "rrhh@retailmax.do",    Telefono = "809-555-0106" };
        db.Empresas.AddRange(techCorp, bancoDig, cyberShield, creativa, dataCenter, retailMax);

        // ---- Ofertas (las 6 del Avance 2) ----
        var ofertas = new List<Oferta>
        {
            new()
            {
                Titulo = "Desarrollador .NET Junior",
                Descripcion = "Buscamos estudiante con conocimientos basicos de C# y SQL Server para unirse a nuestro equipo de desarrollo interno.",
                Tipo = TipoOferta.Pasantia, CarreraRequerida = "Desarrollo de Software",
                SemestreMinimo = 3, Modalidad = "Hibrido", Activa = true,
                FechaPublicacion = DateTime.Now.AddDays(-3), Empresa = techCorp,
                AptitudesRequeridas = new() { Apt("C#", "Programacion"), Apt("SQL Server", "Base de Datos"), Apt("HTML/CSS", "Web") }
            },
            new()
            {
                Titulo = "Tecnico de Soporte IT",
                Descripcion = "Apoyo en soporte tecnico nivel 1 y 2, gestion de incidencias y mantenimiento de equipos.",
                Tipo = TipoOferta.Empleo, CarreraRequerida = "Redes y Telecomunicaciones",
                SemestreMinimo = 4, Modalidad = "Presencial", Activa = true,
                FechaPublicacion = DateTime.Now.AddDays(-7), Empresa = bancoDig,
                AptitudesRequeridas = new() { Apt("Windows Server", "Sistemas"), Apt("Redes", "Infraestructura"), Apt("Active Directory", "Sistemas") }
            },
            new()
            {
                Titulo = "Analista de Seguridad Informatica",
                Descripcion = "Monitoreo de sistemas, analisis de vulnerabilidades y apoyo en implementacion de politicas de seguridad.",
                Tipo = TipoOferta.Pasantia, CarreraRequerida = "Seguridad Informatica",
                SemestreMinimo = 5, Modalidad = "Remoto", Activa = true,
                FechaPublicacion = DateTime.Now.AddDays(-1), Empresa = cyberShield,
                AptitudesRequeridas = new() { Apt("Kali Linux", "Seguridad"), Apt("Firewall", "Seguridad"), Apt("SIEM", "Seguridad") }
            },
            new()
            {
                Titulo = "Disenador UI/UX Junior",
                Descripcion = "Creacion de wireframes, prototipos y diseno de interfaces responsivas para aplicaciones web.",
                Tipo = TipoOferta.Empleo, CarreraRequerida = "Diseno Grafico",
                SemestreMinimo = 3, Modalidad = "Remoto", Activa = true,
                FechaPublicacion = DateTime.Now.AddDays(-5), Empresa = creativa,
                AptitudesRequeridas = new() { Apt("Figma", "Diseno"), Apt("Adobe XD", "Diseno"), Apt("CSS", "Web") }
            },
            new()
            {
                Titulo = "Administrador de Bases de Datos",
                Descripcion = "Gestion, optimizacion y respaldo de bases de datos SQL Server y PostgreSQL en ambiente productivo.",
                Tipo = TipoOferta.Empleo, CarreraRequerida = "Desarrollo de Software",
                SemestreMinimo = 6, Modalidad = "Presencial", Activa = true,
                FechaPublicacion = DateTime.Now.AddDays(-2), Empresa = dataCenter,
                AptitudesRequeridas = new() { Apt("SQL Server", "Base de Datos"), Apt("PostgreSQL", "Base de Datos"), Apt("T-SQL", "Base de Datos") }
            },
            new()
            {
                Titulo = "Pasante de Soporte Tecnico",
                Descripcion = "Apoyo al departamento de IT en instalacion de software, configuracion de equipos y atencion a usuarios.",
                Tipo = TipoOferta.Pasantia, CarreraRequerida = "Soporte Tecnico",
                SemestreMinimo = 2, Modalidad = "Presencial", Activa = true,
                FechaPublicacion = DateTime.Now.AddDays(-10), Empresa = retailMax,
                AptitudesRequeridas = new() { Apt("Windows 11", "Sistemas"), Apt("Office 365", "Ofimatica"), Apt("Comunicacion", "Blanda") }
            }
        };
        db.Ofertas.AddRange(ofertas);

        // ---- Estudiantes (los candidatos del dashboard del Avance 2) ----
        var estudiantes = new List<Estudiante>
        {
            new() { Nombre = "Carlos",  Apellido = "Mejia",     Email = "carlos.mejia@itla.edu.do",  Matricula = "2022-0451", Carrera = "Desarrollo de Software",     SemestreActual = 6, BuscaPasantia = true, Universidad = itla, Aptitudes = new() { Apt("C#"), Apt("SQL Server"), Apt("HTML/CSS") } },
            new() { Nombre = "Maria",   Apellido = "Torres",    Email = "maria.torres@itla.edu.do",  Matricula = "2022-0788", Carrera = "Desarrollo de Software",     SemestreActual = 5, BuscaEmpleo = true,   Universidad = itla, Aptitudes = new() { Apt("C#"), Apt("SQL Server") } },
            new() { Nombre = "Jose",    Apellido = "Ramirez",   Email = "jose.ramirez@itla.edu.do",  Matricula = "2021-1120", Carrera = "Redes y Telecomunicaciones", SemestreActual = 7, BuscaEmpleo = true,   Universidad = itla, Aptitudes = new() { Apt("Redes"), Apt("Windows Server") } },
            new() { Nombre = "Ana",     Apellido = "Rodriguez", Email = "ana.rodriguez@itla.edu.do", Matricula = "2022-0333", Carrera = "Seguridad Informatica",      SemestreActual = 6, BuscaPasantia = true, Universidad = itla, Aptitudes = new() { Apt("Kali Linux"), Apt("Firewall") } },
            new() { Nombre = "Luis",    Apellido = "Fernandez", Email = "luis.fernandez@itla.edu.do", Matricula = "2023-0210", Carrera = "Desarrollo de Software",    SemestreActual = 4, BuscaPasantia = true, Universidad = itla, Aptitudes = new() { Apt("C#"), Apt("HTML/CSS") } }
        };
        db.Estudiantes.AddRange(estudiantes);

        db.SaveChanges(); // asigna Ids reales

        // ---- Solicitudes (postulaciones con % de match del Avance 2) ----
        var netJunior = ofertas[0];
        var soporteIt = ofertas[1];
        var seguridad = ofertas[2];
        db.Solicitudes.AddRange(
            new Solicitud { Estudiante = estudiantes[0], Oferta = netJunior, PorcentajeMatch = 92, Estado = EstadoSolicitud.EnRevision },
            new Solicitud { Estudiante = estudiantes[1], Oferta = netJunior, PorcentajeMatch = 87, Estado = EstadoSolicitud.Pendiente },
            new Solicitud { Estudiante = estudiantes[2], Oferta = soporteIt, PorcentajeMatch = 81, Estado = EstadoSolicitud.EnRevision },
            new Solicitud { Estudiante = estudiantes[3], Oferta = seguridad, PorcentajeMatch = 78, Estado = EstadoSolicitud.Aceptada },
            new Solicitud { Estudiante = estudiantes[4], Oferta = netJunior, PorcentajeMatch = 74, Estado = EstadoSolicitud.Pendiente }
        );

        db.SaveChanges();
    }
}
