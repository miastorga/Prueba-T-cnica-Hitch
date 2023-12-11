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

## Uso

1. **Procesar Asientos Contables desde CSV:**

   - Utiliza el endpoint `POST /api/Contabilidad/ProcesarCSV` para cargar y procesar archivos CSV.

2. **Crear Asientos Contables JSON:**

   - Utiliza el endpoint `POST /api/Contabilidad` para enviar asientos contables en formato JSON.

3. **Consultar Estado de Proceso:**
   - Utiliza los endpoints `GET /api/Contabilidad/Proceso/{id}` y `GET /api/Contabilidad/Registro/{id}` para consultar el estado de un proceso y un registro, respectivamente.

Esta documentación puede ser utilizada en el archivo README.md de tu proyecto para proporcionar información sobre el uso del controlador ContabilidadController.
