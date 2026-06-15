# Control de Inventario - Prueba Técnica CCL

Este proyecto es una solución integral para la gestión de inventarios, compuesta por una API robusta en .NET y una interfaz de usuario moderna en Angular.

## Requisitos Previos

Antes de comenzar, asegúrese de tener instalado:
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (versión LTS recomendada)
- [Angular 19 CLI](https://angular.dev/tools/cli)

## Instrucciones de Ejecución Local

> [!IMPORTANT]
> **Orden de ejecución:** Es fundamental iniciar primero el **Backend** y verificar que esté corriendo antes de lanzar el **Frontend**. Esto asegura que el servicio de autenticación y los endpoints de productos estén disponibles cuando la aplicación Angular intente conectarse.

### 1. Configuración del Backend (.NET)

Navegue al directorio del servidor y ejecute la aplicación:

```bash
cd Backend/CCL_Inventario_BE
dotnet restore
dotnet run
```
La API estará disponible para recibir peticiones (generalmente en `http://localhost:5000` o similar).

### 2. Configuración del Frontend (Angular)

En una nueva terminal, navegue al directorio del cliente e instale las dependencias necesarias:

```bash
cd Frontend/CCL_Inventario_FE
npm install
ng serve
```
Una vez finalizada la compilación, abra su navegador en `http://localhost:4200/`.

## Ejecución de Pruebas Unitarias

### Backend (xUnit)
Para validar la lógica de negocio y los controladores del servidor:
```bash
cd Backend/CCL_Inventario.Tests
dotnet test
```

### Frontend (Vitest)
Para ejecutar las pruebas de los componentes y servicios de Angular:
```bash
cd Frontend/CCL_Inventario_FE
ng test
```

## Credenciales de Acceso
Para probar el inicio de sesión, puede utilizar las siguientes credenciales preconfiguradas:
- **Usuario:** `admin`
- **Contraseña:** `admin123`

### Autenticación JWT y Seguridad
Se ha implementado un esquema de seguridad basado en **JWT (JSON Web Token)** para proteger los endpoints de la API. 

**Razón de la implementación:**
Siguiendo los requerimientos de la prueba, la autenticación se realiza mediante **credenciales fijas en memoria**. El uso de JWT en este escenario permite:
1.  **Simular un entorno real:** Aunque las credenciales no provengan de una base de datos de usuarios, el flujo de obtención y validación del token es idéntico al de una aplicación productiva.
2.  **Desacoplamiento:** El frontend gestiona el estado de la sesión mediante el token, permitiendo que la API sea stateless.
3.  **Protección de Endpoints:** Garantiza que solo las peticiones con un token válido (firmado por el servidor) puedan registrar movimientos o consultar el inventario.

Este enfoque demuestra la capacidad de asegurar una arquitectura moderna entre .NET 9 y Angular 19.
