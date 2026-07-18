# SKILLMATCH RD — Avance 5
## Consolidación del sistema de matchmaking, persistencia y experiencia de usuario

| Campo | Detalle |
|-------|---------|
| **Integrante 1** | Adrian Reyes — 2023-1100 |
| **Integrante 2** | Michael Sanchez — 2019-9132 |
| **Materia** | Proyecto Final TDS |
| **Maestro** | Willis Ezequiel Polanco Caraballo |
| **Instituto** | Instituto Tecnológico de Las Américas — ITLA |
| **Avance** | Avance 5 |
| **Tecnología** | ASP.NET Core MVC (.NET 10, C# 13) + Entity Framework Core 10 + SQLite |

---

## 1. Objetivo

Documentar los cambios y mejoras incorporados en el proyecto SKILLMATCH RD como parte del Avance 5, enfocándose en la consolidación de la lógica de negocio, la persistencia real en base de datos, la experiencia de usuario y la organización del sistema.

Este avance parte del funcionamiento ya consolidado del proyecto y lo fortalece con:

- mejores flujos de registro y sesión,
- gestión de ofertas, estudiantes, empresas y solicitudes con datos reales,
- recomendaciones automatizadas basadas en aptitudes,
- y una estructura más completa para la navegación y uso del sistema.

---

## 2. Qué se agregó en este avance

### 2.1 Gestión real de usuarios y sesión
Se incorporó un flujo de autenticación y sesión básico que permite identificar al usuario que entra al sistema y redirigirlo según su tipo:

- Estudiante
- Empresa

Esto se logra a través del controlador AuthController, que gestiona:

- inicio de sesión,
- almacenamiento de datos de sesión,
- cierre de sesión,
- y redirección a las vistas correspondientes.

### 2.2 Registro y persistencia de estudiantes
El módulo de estudiantes ahora permite:

- registrar nuevos estudiantes en la base de datos,
- almacenar información básica del perfil,
- subir documentos como CV y portafolio,
- ver el perfil en una vista específica,
- y eliminar estudiantes registrados.

Además, el proceso de registro guarda los datos en SQLite usando EF Core, de forma que no se pierden al reiniciar la aplicación.

### 2.3 Gestión completa de ofertas
El módulo de ofertas quedó fortalecido con un flujo CRUD completo:

- crear oferta,
- listar ofertas,
- ver detalle,
- editar oferta,
- eliminar oferta.

Las ofertas ahora se gestionan con datos reales, con asociación a una empresa y con filtros por tipo y carrera.

### 2.4 Gestión de solicitudes de los estudiantes
Se consolidó el módulo de postulaciones para que un estudiante pueda postular a una oferta y que la empresa pueda ver el estado de esas postulaciones.

Las funcionalidades implementadas incluyen:

- enviar una postulación,
- visualizar las solicitudes en el sistema,
- cambiar el estado de una solicitud,
- y eliminar postulaciones cuando sea necesario.

### 2.5 Dashboard y panel de empresas
El módulo de empresas incorpora ahora un panel con estadísticas reales calculadas sobre la base de datos:

- total de ofertas activas,
- total de solicitudes,
- solicitudes en revisión,
- colocaciones aceptadas,
- y candidatos destacados por match.

Esto convierte el dashboard en una herramienta más útil para la gestión de talento.

### 2.6 Recomendaciones inteligentes
El sistema incluye un mecanismo de recomendaciones para estudiantes, basado en la coincidencia entre las aptitudes del perfil del usuario y las aptitudes requeridas de las ofertas.

Esto permite mostrar propuestas más relevantes y ordenadas por compatibilidad, mejorando la experiencia de usuario.

---

## 3. Arquitectura del avance 5

### 3.1 Controladores principales
Se trabajó con los siguientes controladores:

- AuthController: autenticación y sesión.
- HomeController: navegación principal y página de inicio.
- EstudiantesController: registro, perfil, recomendaciones y gestión de estudiantes.
- EmpresasController: registro, listado y dashboard de empresas.
- OfertasController: CRUD de ofertas.
- SolicitudesController: gestión de postulaciones.

### 3.2 Modelos principales
Los modelos principales del sistema son:

- Estudiante
- Empresa
- Oferta
- Solicitud
- Aptitud
- Universidad
- Enums para estado de solicitud, tipo de oferta y tipo de usuario

### 3.3 Persistencia
La base de datos se gestiona mediante SQLite y Entity Framework Core, con un contexto central llamado AppDbContext.

La aplicación usa:

- SQLite como motor de base de datos,
- EF Core para mapear modelos y relaciones,
- y un inicializador de datos (DbInitializer) para sembrar información al iniciar.

---

## 4. Funcionalidades principales implementadas

### 4.1 Inicio de sesión
Se agregó un flujo de login con:

- selección de tipo de usuario,
- redirección según el tipo,
- y almacenamiento de datos en sesión.

### 4.2 Registro de estudiante
El formulario de registro permite:

- ingresar datos personales,
- escoger carrera y semestre,
- subir CV y portafolio,
- y guardar todo en la base de datos.

### 4.3 Gestión de ofertas
La aplicación permite:

- publicar nuevas vacantes,
- listar ofertas activas,
- filtrar por tipo y carrera,
- editar información existente,
- y eliminar ofertas.

### 4.4 Postulaciones
Los estudiantes pueden postular a ofertas y el sistema las registra con información de match y estado.

### 4.5 Dashboard empresarial
Las empresas pueden ver un panel real con métricas de sus postulaciones y ofertas.

### 4.6 Recomendaciones para estudiantes
Las recomendaciones se calculan a partir de conocer las aptitudes del estudiante y las requeridas por la oferta.

---

## 5. Rutas principales del sistema

| Ruta | Función principal |
|------|-------------------|
| `/` | Landing page |
| `/Auth/Login` | Inicio de sesión |
| `/Auth/Logout` | Cierre de sesión |
| `/Estudiantes/Registro` | Registro de estudiante |
| `/Estudiantes` | Listado de estudiantes |
| `/Estudiantes/Perfil/{id}` | Perfil de estudiante |
| `/Estudiantes/MisOfertas` | Recomendaciones de ofertas |
| `/Estudiantes/MisSolicitudes` | Solicitudes del estudiante |
| `/Empresas/Registro` | Registro de empresa |
| `/Empresas/Dashboard` | Panel de empresa |
| `/Empresas` | Empresas registradas |
| `/Ofertas` | Listado de ofertas |
| `/Ofertas/Crear` | Crear oferta |
| `/Ofertas/Editar/{id}` | Editar oferta |
| `/Ofertas/Eliminar/{id}` | Eliminar oferta |
| `/Solicitudes` | Listado de postulaciones |
| `/Solicitudes/ActualizarEstado` | Cambiar estado |
| `/Solicitudes/Eliminar` | Eliminar postulación |

---

## 6. Mejoras logradas respecto a versiones anteriores

1. Se mejoró la lógica de navegación y autenticación.
2. Se logró una mayor integración entre módulos.
3. Se consolidó la persistencia real con base de datos SQLite.
4. Se incorporó un sistema de recomendaciones más útil.
5. Se mejoró la experiencia de usuario de estudiantes y empresas.
6. Se dejó una estructura más clara para futuras mejoras y extensiones.

---

## 7. Estado del proyecto

El proyecto se encuentra en un estado funcional y con una base sólida para continuar con nuevas mejoras, como por ejemplo:

- autenticación con usuarios reales,
- recuperación de contraseña,
- filtros avanzados,
- panel administrativo,
- notificaciones,
- y mejoras visuales del frontend.

---

## 8. Cómo ejecutar

```powershell
cd "SKILLMATCH\SKILLMATCH\SKILLMATCH_RD"
dotnet run
```

La aplicación queda disponible en la URL indicada por la consola, normalmente en un puerto local como `http://localhost:5105`.

---

## 9. Conclusión

El Avance 5 consolidó SKILLMATCH RD como una aplicación más completa, con módulos integrados, persistencia real, recomendaciones y una experiencia de usuario más cercana a un sistema funcional de matchmaking para estudiantes y empresas.
