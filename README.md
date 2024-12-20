# UserManagementService

Este proyecto corresponde al Taller 2 para la asignatura Arquitectura de Sistemas.

## Requerimientos

- **[ASP.NET Core 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)** 
- **[Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)** 
- **[Postman](https://www.postman.com/downloads/)** para probar la API
- **[pgAdmin4](https://www.pgadmin.org/download/pgadmin-4-windows/)** para probar la Base de Datos
- **[PostgreSQL](https://www.postgresql.org/download/)** para probar la Base de Datos

## Clonar el Repositorio

Clona el repositorio con el siguiente comando:

```bash
git clone https://github.com/Hvmnn/UserManagementService.git
```

## Restaurar el Proyecto

Después de clonar el repositorio, navega a la carpeta del proyecto y restaura los paquetes de NuGet:

```bash
cd .\UserManagementService\
dotnet restore
```

## Configurar la Base de Datos

Estan las migraciones listas, pero se tiene que probar la base de datos en pgAdmin4. 
Para esto se necesita descargar y tener corriendo el servicio de PostgreSQL.

## Ejecutar la Aplicación

Para ejecutar la aplicación, utiliza el siguiente comando:

```bash
dotnet run
```

Esto iniciará el servidor en `http://localhost:5171`.
