# SKILLMATCH RD — Avance 3 de 3
## Integración de Datos y Nuevos Módulos

| Campo | Detalle |
|-------|---------|
| **Integrante 1** | Adrian Reyes — 2023-1100 |
| **Integrante 2** | Michael Sanchez — 2019-9132 |
| **Materia** | Proyecto Final TDS |
| **Maestro** | Willis Ezequiel Polanco Caraballo |
| **Instituto** | Instituto Tecnológico de Las Américas — ITLA |
| **Avance** | Avance 3 de 3 |
| **Tecnología** | ASP.NET Core MVC (.NET 10, C# 13) + Entity Framework Core 10 + SQLite |

---

## 1. Objetivo

Ver el funcionamiento de SKILLMATCH RD con **datos reales persistidos en base de datos**. Este avance
parte **exactamente del código funcional del Avance 2** (la misma landing, login, dashboard, ofertas,
registros y recomendaciones) y le **agrega** una capa de persistencia real con operaciones CRUD
(crear, leer, actualizar y eliminar), sin rehacer la interfaz ni la arquitectura existente.

> **Importante:** No se reconstruyó la aplicación. Se conservaron los controladores, vistas Razor,
> modelos y estilos del Avance 2, y únicamente se sustituyó la fuente de datos (listas estáticas en
> memoria) por una base de datos SQLite gestionada con Entity Framework Core.

---

## 2. Qué cambió respecto al Avance 2

En el Avance 2 los datos vivían en **listas estáticas en memoria** dentro de cada controlador; al
reiniciar la aplicación se perdía todo. En el Avance 3 esos mismos datos se **almacenan y consultan
en una base de datos**.

| Aspecto | Avance 2 | Avance 3 |
|---------|----------|----------|
| Origen de datos | Listas `static` en memoria | Base de datos **SQLite** vía **EF Core** |
| Persistencia | Se perdía al reiniciar | Persistente en `skillmatch.db` |
| Ofertas | Solo lectura + crear en memoria | **CRUD completo** (crear, leer, editar, eliminar) |
| Estudiantes | Registro en sesión | Registro **guardado en BD** + listado + perfil + eliminar |
| Empresas | Dashboard con números fijos | Dashboard con **estadísticas calculadas desde la BD** |
| Solicitudes | Módulo "en desarrollo" | Listado real + cambiar estado + eliminar |

---

## 3. Integración con Base de Datos

### 3.1 Contexto de datos — `Data/AppDbContext.cs`
Se creó el contexto de Entity Framework Core con seis tablas y sus relaciones:

- `Universidades`, `Empresas`, `Estudiantes`, `Ofertas`, `Aptitudes`, `Solicitudes`
- **Relaciones configuradas:**
  - Empresa **1—N** Ofertas
  - Universidad **1—N** Estudiantes
  - Estudiante **1—N** Solicitudes y Oferta **1—N** Solicitudes
  - Oferta **N—M** Aptitudes y Estudiante **N—M** Aptitudes (tablas puente automáticas)

### 3.2 Siembra de datos — `Data/DbInitializer.cs`
Al arrancar la aplicación por primera vez se crea la base de datos (`EnsureCreated`) y se siembran
los **mismos datos del Avance 2**: 1 universidad (ITLA), 6 empresas, las 6 ofertas reales con sus
aptitudes, 5 estudiantes y 5 solicitudes con su porcentaje de match. Así las pantallas se alimentan
de datos reales desde el primer momento.

### 3.3 Configuración — `Program.cs` y `appsettings.json`
- Se registró `AppDbContext` con `UseSqlite(...)`.
- Cadena de conexión `Default = "Data Source=skillmatch.db"`.
- Al iniciar, un `scope` ejecuta `DbInitializer.Seed(db)`.

---

## 4. Módulos CRUD implementados

### 4.1 Ofertas — CRUD completo (`OfertasController`)
- **Crear:** `GET/POST /Ofertas/Crear` — guarda la oferta en la BD y la asocia a una empresa.
- **Leer:** `GET /Ofertas` (listado con filtros por tipo y carrera) y `GET /Ofertas/Detalle/{id}`.
- **Actualizar:** `GET/POST /Ofertas/Editar/{id}` — edita todos los campos de la vacante.
- **Eliminar:** `GET/POST /Ofertas/Eliminar/{id}` — con pantalla de confirmación.

### 4.2 Estudiantes (`EstudiantesController`)
- **Crear:** el formulario de registro ahora **guarda el estudiante en la BD**.
- **Leer:** `GET /Estudiantes` (listado real) y `GET /Estudiantes/Perfil/{id}`.
- **Eliminar:** `GET/POST /Estudiantes/Eliminar/{id}`.
- **Recomendaciones:** `MisOfertas` calcula la compatibilidad a partir de las aptitudes reales.

### 4.3 Empresas (`EmpresasController`)
- **Crear:** el registro de empresa se persiste en la BD.
- **Leer:** `GET /Empresas/Index` (empresas registradas) y `GET /Empresas/Dashboard`.
- **Dashboard:** ofertas activas, solicitudes recibidas, en revisión y colocaciones se **cuentan en
  vivo desde la base de datos**.

### 4.4 Solicitudes (`SolicitudesController`)
- **Crear:** `POST /Solicitudes/Aplicar` — registra la postulación de un estudiante a una oferta.
- **Leer:** `GET /Solicitudes` — listado completo de postulaciones.
- **Actualizar:** `POST /Solicitudes/ActualizarEstado` — cambia el estado (Pendiente / En Revisión /
  Aceptada / Rechazada).
- **Eliminar:** `POST /Solicitudes/Eliminar`.

---

## 5. Rutas principales

| Ruta | Método | Operación CRUD | Descripción |
|------|--------|----------------|-------------|
| `/Ofertas` | GET | Read | Listado de ofertas desde la BD con filtros |
| `/Ofertas/Crear` | GET/POST | Create | Publicar nueva oferta |
| `/Ofertas/Editar/{id}` | GET/POST | Update | Editar oferta existente |
| `/Ofertas/Eliminar/{id}` | GET/POST | Delete | Eliminar oferta |
| `/Estudiantes` | GET | Read | Listado de estudiantes registrados |
| `/Estudiantes/Registro` | GET/POST | Create | Alta de estudiante en la BD |
| `/Estudiantes/Perfil/{id}` | GET | Read | Detalle de un estudiante |
| `/Estudiantes/Eliminar/{id}` | GET/POST | Delete | Eliminar estudiante |
| `/Empresas/Dashboard` | GET | Read | Estadísticas calculadas desde la BD |
| `/Empresas/Index` | GET | Read | Empresas registradas |
| `/Solicitudes` | GET | Read | Listado de postulaciones |
| `/Solicitudes/ActualizarEstado` | POST | Update | Cambiar estado de una postulación |
| `/Solicitudes/Eliminar` | POST | Delete | Eliminar postulación |

---

## 6. Capturas de flujos completos

| # | Archivo | Pantalla |
|---|---------|----------|
| 1 | `screenshots/01-landing.png` | Landing page (recuperada del Avance 2) |
| 2 | `screenshots/02-login.png` | Login con tabs Estudiante / Empresa |
| 3 | `screenshots/03-ofertas-listado-crud.png` | Listado de ofertas con acciones **Editar / Eliminar** |
| 4 | `screenshots/04-ofertas-crear.png` | Formulario **Crear** oferta |
| 5 | `screenshots/05-ofertas-editar.png` | Formulario **Editar** oferta (Update) |
| 6 | `screenshots/06-estudiantes-listado.png` | Listado de estudiantes desde la BD |
| 7 | `screenshots/07-estudiantes-registro.png` | Registro de estudiante (Create) |
| 8 | `screenshots/08-empresa-dashboard.png` | Dashboard con estadísticas reales de la BD |
| 9 | `screenshots/09-empresas-listado.png` | Empresas registradas |
| 10 | `screenshots/10-solicitudes-crud.png` | Gestión de solicitudes (Update de estado + Delete) |
| 11 | `screenshots/11-recomendaciones.png` | Recomendaciones del estudiante |

---

## 7. Pruebas realizadas

Además de la navegación manual sobre todos los módulos, se ejecutó una **prueba automatizada de
persistencia end-to-end** contra la aplicación en ejecución:

| Operación | Prueba | Resultado |
|-----------|--------|-----------|
| **Create** | POST a `/Ofertas/Crear` con una oferta de prueba | Total de ofertas pasó de **6 → 7**, la nueva oferta aparece en el listado ✔ |
| **Read** | Carga de `/Ofertas`, `/Estudiantes`, `/Solicitudes`, `/Empresas/Dashboard` | Los datos mostrados provienen de la BD (conteos correctos) ✔ |
| **Update** | Cambio de estado de solicitud y recálculo del dashboard | Estados y estadísticas reflejan los valores de la BD ✔ |
| **Delete** | POST a `/Ofertas/Eliminar/{id}` sobre la oferta de prueba | Total volvió de **7 → 6**, la oferta desaparece ✔ |

También se verificó el token **antiforgery** en todos los formularios POST.

---

## 8. Problemas resueltos

1. **La aplicación había sido reconstruida por error.** Una herramienta automática rehízo el proyecto
   desde cero y reemplazó los módulos del Avance 2 (landing, login, dashboard, ofertas) por un CRUD
   genérico distinto. **Solución:** se recuperó el código original y funcional del Avance 2 y se
   trabajó **sobre él**, agregándole la base de datos sin rehacer la interfaz.
2. **Datos que se perdían al reiniciar.** Al vivir en listas en memoria, nada persistía.
   **Solución:** migración a SQLite con EF Core; ahora los datos sobreviven a los reinicios.
3. **Relaciones muchos-a-muchos (Aptitudes).** Una misma aptitud (p. ej. "SQL Server") se repetía en
   varias ofertas y estudiantes. **Solución:** se reutilizan las aptitudes mediante un caché en el
   sembrador, evitando filas duplicadas y dejando que EF genere las tablas puente.
4. **Formularios y seguridad.** Se añadió `[ValidateAntiForgeryToken]` a las operaciones de escritura,
   aprovechando el token que las vistas ya generaban.

---

## 9. Cómo ejecutar

```powershell
cd "SKILLMATCH\SKILLMATCH\SKILLMATCH\SKILLMATCH\SKILLMATCH_RD"
dotnet run
```

La primera ejecución crea automáticamente `skillmatch.db` y siembra los datos. Luego abrir la URL que
indique la consola (por ejemplo `http://localhost:5080`).

Para reiniciar los datos desde cero, basta con **borrar el archivo `skillmatch.db`** y volver a
ejecutar; el sembrador lo recreará.
