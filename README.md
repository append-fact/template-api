# Sistema de Gestión de Turnos Genérico - API

## **Descripción del Proyecto**  
Esta es la API principal para el sistema de gestión de turnos genérico. Facilita la administración de agendas, la creación y reserva de turnos, y la gestión de usuarios y organizaciones. Es adaptable a cualquier rubro, como salud, consultoría, servicios profesionales, entre otros.

---

## **Características Principales**  
- **Gestión de Agendas y Turnos**: Configuración dinámica de horarios de atención y generación automática de turnos.  
- **Autenticación Segura**: Implementada con **ASP.NET Identity** y tokens JWT.  
- **Manejo de Roles**: Profesionales, administradores y usuarios finales con permisos específicos.  
- **Flexibilidad**: Adaptable a organizaciones con múltiples profesionales.  
- **Estados Personalizables**: Gestión dinámica de estados para turnos, clientes y proveedores.

---

## **Tecnologías Utilizadas**  
- **Lenguaje**: C#  
- **Framework**: ASP.NET Core 8  
- **Base de Datos**: SQL Server  
- **ORM**: Entity Framework Core  
- **Seguridad**: Autenticación con JWT y ASP.NET Identity  
- **Gestión de Versiones**: Git y Azure DevOps  
- **CI/CD**: Azure DevOps Pipelines

---

## **Requisitos Previos**  
Antes de ejecutar la API, asegúrate de tener instalado:  
- **.NET SDK 8.0**  
- **SQL Server**  
- **Azure CLI** *(opcional, para despliegues)*  

---

## **Configuración del Proyecto**  

1. **Clona el repositorio**:  
   ```bash
   git clone https://appendOrganization@dev.azure.com/appendOrganization/Turnos/_git/turnos-backend
  ```
