## **Requerimientos**

La API debe recibir un listado de registros almacenarlos en la base de datos, iniciar
un proceso en segundo plano con un id unico y retornar como respuesta el
identificador del proceso.
El proceso en segundo plano debe generar el registro de estado del proceso en la
base de datos y comenzar a procesar cada registro, por cada registro debe registrar
su estado en la base de datos y enviar a sap la informacion, con el resultado del
envio debe ir actualizandose el estado del registro. Despues de procesar todos los
registros debe actualizarse el estado del proceso.
Adicionalmente debe crearse un metodo para consultar el estado del proceso por su
id unico. y otro metodo para consultar por el campo Reference el estado de un
registro.

## Requisitos

- [Docker](https://www.docker.com/) (para ejecutar SQL Server en un contenedor)
- [ASP.NET Core 7](https://dotnet.microsoft.com/download/dotnet/7.0)
- [Hangfire](https://www.hangfire.io/)
- [CSV Helper](https://joshclose.github.io/CsvHelper/)

## Configuración

1. **Base de Datos SQL Server:**

   - Ejecuta el siguiente comando Docker para iniciar un contenedor de SQL Server:
     ```bash
     docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=TuContraseña123' -p 1433:1433 --name sql_server_container -d mcr.microsoft.com/mssql/server
     ```
   - Actualiza la cadena de conexión en `appsettings.json` con la información de tu servidor SQL Server.

2. **Ejecución de la Aplicación:**
   - Ejecuta tu aplicación ASP.NET Core.

## **Endpoints**

ContabilidadController
Este controlador maneja las operaciones relacionadas con la contabilidad, como la creación de asientos contables y la consulta de procesos y registros.

Constructor
csharp
Copy code
public ContabilidadController(ISAPService SAPService, IContabilidadRepository contabilidad, ILogger<ContabilidadController> logger)
Parámetros:

SAPService: Una instancia de la interfaz ISAPService que proporciona servicios relacionados con SAP.
contabilidad: Una instancia de la interfaz IContabilidadRepository que proporciona operaciones de repositorio para la contabilidad.
logger: Una instancia de ILogger<ContabilidadController> para el registro de eventos.
Métodos

1. CrearAsientosJSON
   csharp
   Copy code
   [HttpPost]
   public async Task<ActionResult<CrearAsientosSuccessResponse>> CrearAsientosJSON([FromBody] List<AsientoContableDTO> asientos)
   Descripción:
   Crea asientos contables utilizando el servicio SAP y almacena los resultados en la base de datos.

Parámetros:

asientos: Lista de objetos AsientoContableDTO que representan los asientos contables.
Devuelve:

ActionResult<CrearAsientosSuccessResponse>: Resultado de la creación de asientos. 2. ConsultarProceso
csharp
Copy code
[HttpGet("Proceso/{id}")]
public async Task<ActionResult<ProcesoDTO>> ConsultarProceso(Guid id)
Descripción:
Consulta un proceso por ID.

Parámetros:

id: Identificador único del proceso.
Devuelve:

ActionResult<ProcesoDTO>: Resultado de la consulta del proceso. 3. ConsultarRegistro
csharp
Copy code
[HttpGet("Registro/{id}")]
public async Task<ActionResult<RegistroDTO>> ConsultarRegistro(Guid id)
Descripción:
Consulta un registro por ID.

Parámetros:

id: Identificador único del registro.
Devuelve:

ActionResult<RegistroDTO>: Resultado de la consulta del registro. 4. ProcesarCSV
csharp
Copy code
[HttpPost("ProcesarCSV")]
public async Task<ActionResult<List<AsientoContableDTO>>> ProcesarCSV([FromForm] IFormFile file)
Descripción:
Procesa un archivo CSV enviado en el cuerpo de la solicitud y crea asientos contables.

Parámetros:

file: Archivo CSV enviado en la solicitud.
Devuelve:

ActionResult<List<AsientoContableDTO>>: Resultado del procesamiento del archivo CSV.
Excepciones:

Devuelve un error 400 si el archivo no se proporciona.
Devuelve un error 500 si hay un problema al procesar el archivo CSV. 5. LeerCSV (Método Privado)
csharp
Copy code
private async Task<List<AsientoContableDTO>> LeerCSV(IFormFile file)
Descripción:
Lee y procesa el contenido de un archivo CSV.

Parámetros:

file: Archivo CSV.
Devuelve:

Task<List<AsientoContableDTO>>: Lista de objetos AsientoContableDTO obtenidos del archivo CSV.
Excepciones:

Lanza una excepción si hay un error al procesar el archivo CSV.
Esta documentación puede ser utilizada en el archivo README.md de tu proyecto para proporcionar información sobre el uso del controlador ContabilidadController.
