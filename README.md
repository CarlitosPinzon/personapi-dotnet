# personapi-dotnet

Laboratorio 1 de Arquitectura de Software. Monolito web en ASP.NET Core MVC que implementa el CRUD del modelo de datos `persona_db` usando el patron DAO (repositorios) sobre SQL Server. Expone vistas Razor para el usuario y una API REST documentada con Swagger (OpenAPI 3).

## Stack

| Componente | Version |
|---|---|
| .NET | 10.0 (LTS) |
| ASP.NET Core MVC | 10.0 |
| Entity Framework Core | 10.0.14 |
| SQL Server | 2025 Express |
| Swashbuckle (Swagger UI / OpenAPI 3) | 10.3.1 |

## Modelo de datos

- **persona** (`cc` PK, `nombre`, `apellido`, `genero` M/F, `edad`)
- **profesion** (`id` PK, `nom`, `des`)
- **estudios** (`id_prof` + `cc_per` PK compuesta, `fecha`, `univer`). Relaciona persona con profesion (N:M).
- **telefono** (`num` PK, `oper`, `duenio` FK a persona)

SQL Server no tiene tipo `ENUM`, por eso `genero` se define como `CHAR(1)` con una restriccion `CHECK (genero IN ('M','F'))`. El tipo `TEXT` esta obsoleto en SQL Server y se reemplazo por `VARCHAR(MAX)`.

## Estructura del proyecto

```
personapi-dotnet/
├── docker-compose.yml          # SQL Server + carga de scripts + aplicacion
├── scripts/
│   ├── ddl.sql                 # Creacion de la base de datos y tablas
│   └── dml.sql                 # Datos de prueba
└── personapi-dotnet/
    ├── Controllers/            # Controladores MVC (vistas)
    │   └── Api/                # Controladores REST
    ├── Interfaces/             # Contratos de los repositorios (DAO)
    ├── Repositories/           # Implementacion de los DAO con EF Core
    ├── Models/
    │   ├── Entities/           # Entidades y DbContext generados con Scaffold-DbContext
    │   └── Dtos/               # Objetos de transferencia y validaciones
    ├── Views/                  # Vistas Razor
    ├── Program.cs
    └── appsettings.json
```

## Configuracion del ambiente

### Opcion A: Windows (Visual Studio 2026 + SQL Server Express)

1. Instalar **SQL Server 2025 Express** (modo Basico) y **SQL Server Management Studio 22**.
2. Instalar **Visual Studio Community 2026** con las cargas de trabajo:
   - Desarrollo ASP.NET y web
   - Almacenamiento y procesamiento de datos
   - Plantillas de proyecto y elementos de .NET Framework
   - Caracteristicas avanzadas de ASP.NET
3. Abrir SSMS, conectarse a `localhost\SQLEXPRESS` y ejecutar en orden `scripts/ddl.sql` y `scripts/dml.sql`.
4. Cambiar la cadena de conexion en `personapi-dotnet/appsettings.json`:

   ```json
   "ConnectionStrings": {
     "PersonaDb": "Server=localhost\\SQLEXPRESS;Database=persona_db;Trusted_Connection=True;TrustServerCertificate=True"
   }
   ```

5. Abrir `personapi-dotnet.slnx` en Visual Studio y ejecutar con el perfil `http`.

### Opcion B: Linux / macOS / Windows con Docker

Requisitos: Docker y .NET 10 SDK.

1. Levantar SQL Server 2025 y cargar los scripts:

   ```bash
   docker compose up -d db db-init
   ```

   El servicio `db-init` ejecuta `ddl.sql` y `dml.sql` y termina. La cadena de conexion que trae `appsettings.json` ya apunta a `localhost,1433` con el usuario `sa`.

2. Si se prefiere cargar los scripts manualmente:

   ```bash
   docker exec -i persona-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'Arqui2026*Lab' -C < scripts/ddl.sql
   docker exec -i persona-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'Arqui2026*Lab' -C < scripts/dml.sql
   ```

## Generacion de entidades (Database First)

Las clases de `Models/Entities` se generaron a partir de la base de datos ya creada.

En Visual Studio (Consola del Administrador de paquetes):

```powershell
Scaffold-DbContext "Server=localhost\SQLEXPRESS;Database=persona_db;Trusted_Connection=True;TrustServerCertificate=true" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models/Entities
```

Con la CLI de .NET (equivalente, usando la cadena de `appsettings.json`):

```bash
dotnet tool install --global dotnet-ef
cd personapi-dotnet
dotnet ef dbcontext scaffold "Name=ConnectionStrings:PersonaDb" Microsoft.EntityFrameworkCore.SqlServer -o Models/Entities --context PersonaDbContext --force
```

## Compilacion

```bash
dotnet restore
dotnet build
```

## Ejecucion local

```bash
cd personapi-dotnet
dotnet run --launch-profile http
```

- Vistas: http://localhost:5207
- Swagger UI: http://localhost:5207/swagger
- Documento OpenAPI: http://localhost:5207/swagger/v1/swagger.json

## Despliegue con Docker Compose

Levanta la base de datos, ejecuta los scripts y publica la aplicacion en un contenedor:

```bash
docker compose up -d --build
```

- Vistas: http://localhost:5080
- Swagger UI: http://localhost:5080/swagger

Para detener y borrar todo (incluido el volumen de datos):

```bash
docker compose down -v
```

## Endpoints REST

| Recurso | Metodo | Ruta |
|---|---|---|
| Personas | GET, POST | `/api/personas` |
| | GET, PUT, DELETE | `/api/personas/{cc}` |
| Profesiones | GET, POST | `/api/profesiones` |
| | GET, PUT, DELETE | `/api/profesiones/{id}` |
| Estudios | GET, POST | `/api/estudios` |
| | GET, PUT, DELETE | `/api/estudios/{idProf}/{ccPer}` |
| Telefonos | GET, POST | `/api/telefonos` |
| | GET, PUT, DELETE | `/api/telefonos/{num}` |

Codigos de respuesta: `200` consulta, `201` creado, `204` actualizado/eliminado, `400` datos invalidos, `404` no existe, `409` llave duplicada.

Ejemplo:

```bash
curl -X POST http://localhost:5207/api/personas \
  -H "Content-Type: application/json" \
  -d '{"cc":1060606060,"nombre":"Mateo","apellido":"Diaz","genero":"M","edad":24}'
```

## Notas

- Al eliminar una persona se eliminan tambien sus telefonos y estudios. Al eliminar una profesion se eliminan los estudios asociados.
- La contrasena de `sa` incluida en los archivos es solo para el ambiente local del laboratorio.
