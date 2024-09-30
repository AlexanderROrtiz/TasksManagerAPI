# Task Management API

## Descripci�n del Proyecto  Back-end

El sistema de gesti�n de tareas de DVP permite la autenticaci�n de usuarios y la gesti�n de roles, facilitando la asignaci�n y seguimiento de tareas para empleados, supervisores y administradores. Esta API RESTful est� construida con .NET 8 y utiliza Angular para el front-end.

## Arquitectura 

El proyecto est� estructurado en microservicios, con las siguientes capas:

- **API**: Maneja las solicitudes y respuestas HTTP.
- **Application**: Contiene la l�gica de negocio y orquestaci�n de las operaciones.
- **Domain**: Define las entidades y las reglas de negocio.
- **Infrastructure**: Implementa la comunicaci�n con la base de datos y servicios externos.

## Requerimientos

- **Back-end**:
  - .NET 8
  - Entity Framework Core
  - SQL Server
  - JWT para autenticaci�n

## C�mo ejecutar el proyecto localmente

1. **Clonar el repositorio**:
   ```bash
   git clone https://github.com/tu_usuario/TaskManagement.git
   cd TaskManagement

2. **Ajustar la cadena de conexion ConnectionStrings**
3. **Realizar la migracion correspondiente en la capa de Infrastructure**
4. **Probar el api con postman o swagger**
5. **Tiene un modelo de pruebas unitarias con Moq y XUnit**

## Contacto

Cualquier duda o inquietud sobre el proyecto estare atento al correo: raortiz61@misena.edu.co

�Gracias por visitar este proyecto!
