# Backend de Bartify

Backend escrito en C# para la app web Bartify, proyecto de Integradora 2 en la UTRM 2026.
###Desarrollador:
Ofeck Megidish Ascencio

## Patches

### 0.4.0 - Sendbird

El registro ahora crea una cuenta de Sendbird al llamar el POST. También se agregaron endpoints para crear un chat de Sendbird y llamar a todas las urls de un usuario.

### 0.3.1 - Fix de JWT y DB

Se solucionó un problema en el que no se leía la UID de la cookie. Se arregló una consulta errónea al buscar un artículo.

### 0.3.0 - Setup para despliegue a Railway

Se eliminaron todos los obj y bin, los csproj, y se agregaron 2 Dockerfiles para que el proyecto funcione adecuadamente con Railway.

### 0.2.1 - JWT como Cookie

El JWT pasó de ser mandado al front a ser mandado como cookie.

### 0.2.0 - Integración de JWT

Ahora se crea un Token JWT al iniciar sesión, y se agarra la id del usuario del token en ciertos endpoints.

### 0.1.0 - Backend subido

Primera version del backend con endpoints de usuarios, articulos, categorias, ubicaciones, fotos (con servicio de Cloudinary externo).
