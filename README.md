## Banco Informático Kinal (BIK)

Esta documentación detalla la estructura, configuración y endpoints implementados hasta la fecha en el servidor administrativo (`server-admin`). El sistema está construido sobre **Node.js** con **Express** y **MongoDB**.

---

## 0. Requisitos del Sistema

Antes de comenzar, asegúrate de tener instalado lo siguiente:

* **Node.js:** v8
* **MongoDB:** Última versión estable
* **Docker:** v20.10+
* **Docker Compose:** v2.0+
* **pnpm:** 10.28.1 (Gestor de paquetes)
* **Git:** Para clonar el repositorio
* **C# .NET Core:** 6.0+ (para servicios de core-banking y auditoría)

---

## 1. Stack Tecnológico y Dependencias

El proyecto utiliza las siguientes tecnologías clave para su funcionamiento:

* **Runtime:** Node.js
* **Framework:** Express.js
* **Base de Datos:** MongoDB (con Mongoose como ODM)
* **Seguridad:**
* `argon2`: Para el hashing y verificación de contraseñas.
* `jsonwebtoken`: Para la generación y validación de tokens de sesión (JWT).
* `helmet`: Para la protección de cabeceras HTTP.

* **Validación:** `express-validator` para la sanitización y validación de datos de entrada.
* **Utilidades:** `cors`, `dotenv`, `morgan`.
* **Multer:** Para manejo de carga de archivos.
* **Cloudinary:** Para almacenamiento de archivos en la nube.
* **axios:** Para realizar peticiones HTTP.
* **uuid:** Para generación de identificadores únicos.
* **express-rate-limit:** Para limitar la tasa de peticiones.
* **node-cron:** Para tareas programadas.

---

## 2. Estructura de Carpetas del Proyecto

El repositorio está organizado de la siguiente manera:

```
BIK/
├── services/
│   ├── server-admin/              # Servidor administrativo (Node.js + Express + MongoDB)
│   │   ├── configs/               # Configuración (BD, seeding, CORS)
│   │   ├── middlewares/           # Middlewares (JWT, roles, validación)
│   │   ├── src/
│   │   │   ├── auth/              # Autenticación y registro
│   │   │   ├── users/             # Gestión de usuarios
│   │   │   ├── accounts/          # Gestión de cuentas bancarias
│   │   │   ├── deposits/          # Depósitos
│   │   │   ├── transactions/      # Transacciones y transferencias
│   │   │   ├── services/          # Catálogo de servicios pagables
│   │   │   └── utils/             # Utilidades (encrypt, JWT)
│   │   ├── index.js               # Punto de entrada
│   │   └── package.json           # Dependencias
│   │
│   ├── core-banking/              # Servicio de banca central (.NET Core)
│   │   ├── Controllers/           # Controladores de API
│   │   ├── Models/                # Modelos de datos
│   │   ├── Services/              # Lógica de negocio
│   │   ├── Data/                  # Contexto de base de datos
│   │   ├── Migrations/            # Migraciones de base de datos
│   │   └── Program.cs             # Configuración principal
│   │
│   ├── currency-service/          # Servicio de divisas (Node.js)
│   │   ├── index.js               # Punto de entrada
│   │   └── package.json
│   │
│   ├── audit-service/             # Servicio de auditoría (.NET Core)
│   │   ├── Controllers/
│   │   ├── Models/
│   │   ├── Services/
│   │   └── Program.cs
│
├── Postman/                       # Colección de peticiones para testing
├── docs/                          # Documentación adicional
├── clients/                       # Clientes (aplicaciones frontend)
├── docker-compose.yml             # Orquestación de contenedores
├── .dockerignore                  # Archivos a ignorar en imágenes Docker
├── .gitignore                     # Archivos a ignorar en Git
├── BIK.sln                        # Solución de Visual Studio
├── README.md                      # Este archivo
└── LICENSE                        # Licencia del proyecto
```

---

## 3. Configuración y Base de Datos

### Conexión a Base de Datos (`configs/db.js`)

El sistema gestiona la conexión a MongoDB con manejo de eventos para monitorear el estado (conectado, desconectado, error). Incluye una función de cierre controlado (`gracefulShutdown`) para terminar conexiones correctamente al cerrar el servidor.

### Seeding Inicial (`configs/admin.seed.js`)

Al iniciar la aplicación, se ejecuta un script automático que verifica la existencia de un administrador. Si no existe, crea el usuario base:

* **Username:** ADMINB
* **Password:** ADMINB (Cifrada con Argon2)
* **Rol:** ADMIN_ROLE

---

## 4. Cómo Clonar y Ejecutar el Programa

### Paso 1: Clonar el Repositorio

```bash
git clone https://github.com/gc130041/BIK.git
cd BIK
```

### Paso 2: Instalar Dependencias del Servidor Administrativo

```bash
cd services/server-admin
pnpm install
```

### Paso 3: Instalar Dependencias del Servicio de Divisas

```bash
cd ../currency-service
pnpm install
```

### Paso 4: Configurar Variables de Entorno

Crea un archivo `.env` en `services/server-admin/` con las siguientes variables:

```env
# Base de Datos
URI_MONGODB=mongodb://localhost:27017/BIK
DB_NAME=BIK

# JWT y Seguridad
SECRET_KEY=TuClaveSuperSecretaBIK2026
PORT=3001

# Cloudinary (Opcional, para manejo de archivos)
CLOUDINARY_NAME=tu_cloudinary_name
CLOUDINARY_API_KEY=tu_api_key
CLOUDINARY_API_SECRET=tu_api_secret

# Entorno
NODE_ENV=development
```

### Paso 5: Iniciar el Servidor Administrativo

```bash
cd services/server-admin
pnpm dev
```

El servidor estará disponible en: `http://localhost:3001`

### Paso 6: Iniciar el Servicio de Divisas (Opcional)

En otra terminal:

```bash
cd services/currency-service
pnpm start
```

### Paso 7: Iniciar los Servicios de .NET (Opcional)

Para el servicio de banca central y auditoría:

```bash
cd services/core-banking
dotnet restore
dotnet ef database update
dotnet run
```

---

## 5. Versiones Utilizadas

| Dependencia | Versión | Descripción |
|------------|---------|-------------|
| **Node.js** | 8 | Runtime de JavaScript |
| **Express.js** | 5.2.1 | Framework web |
| **Mongoose** | 9.1.5 | ODM para MongoDB |
| **MongoDB** | Última | Base de datos NoSQL |
| **pnpm** | 10.28.1 | Gestor de paquetes |
| **argon2** | 0.44.0 | Hash de contraseñas |
| **jsonwebtoken** | 9.0.3 | Gestión de JWT |
| **helmet** | 8.1.0 | Seguridad de headers HTTP |
| **cors** | 2.8.6 | CORS middleware |
| **express-validator** | 7.3.1 | Validación de datos |
| **morgan** | 1.10.1 | Logger de HTTP |
| **multer** | 2.0.2 | Carga de archivos |
| **cloudinary** | 2.9.0 | Almacenamiento en la nube |
| **axios** | 1.13.4 | Cliente HTTP |
| **uuid** | 13.0.0 | Generador de UUIDs |
| **express-rate-limit** | 8.2.1 | Límite de peticiones |
| **nodemon** | 3.1.11 | Auto-reload en desarrollo |
| **node-cron** | 4.2.1 | Tareas programadas |
| **.NET Core** | 6.0+ | Runtime para servicios C# |
| **Entity Framework Core** | Última | ORM para .NET |

---

## 6. Ubicación de las Rutas

Las rutas de la API están organizadas por módulos. Cada módulo se encuentra en:

```
services/server-admin/src/[módulo]/[módulo].routes.js
```

### Módulos Disponibles:

| Módulo | Ruta de Archivos | Base URL |
|--------|------------------|----------|
| **Autenticación** | `src/auth/` | `/BIK/v1/auth` |
| **Usuarios** | `src/users/` | `/BIK/v1/users` |
| **Cuentas** | `src/accounts/` | `/BIK/v1/accounts` |
| **Depósitos** | `src/deposits/` | `/BIK/v1/deposits` |
| **Transacciones** | `src/transactions/` | `/BIK/v1/transactions` |
| **Servicios** | `src/services/` | `/BIK/v1/services` |

**Base URL Completa:** `http://localhost:3001/BIK/v1`

---

## 7. Módulos de Seguridad (Middleware y Utils)

### Utilidades

* **Encriptación (`src/utils/encrypt.js`):** Contiene funciones para `encrypt` (hash) y `verifyPassword` (comprobación) usando Argon2.
* **JWT (`src/utils/jwt.js`):** Genera tokens firmados con una clave secreta, incluyendo el `uid`, `username` y `role` en el payload. Expiración configurada a 4 horas.

### Middlewares

* **Validar JWT (`middlewares/validate-jwt.js`):** Intercepta las peticiones, extrae el token del header `Authorization`, lo verifica y adjunta el usuario correspondiente a la `request`.
* **Validar Roles (`middlewares/validate-roles.js`):** Verifica si el usuario autenticado posee uno de los roles permitidos para acceder a la ruta.
* **Validar Campos (`middlewares/check-validators.js`):** Recolecta los errores generados por `express-validator` y responde con un estatus 400 si la validación falla.

---

## 8. Docker y Docker Compose

### ¿Qué es Docker Compose?

Docker Compose permite definir y ejecutar múltiples contenedores Docker como una sola aplicación. El archivo `docker-compose.yml` en la raíz del proyecto define todos los servicios necesarios.

### Comandos Básicos de Docker

#### Iniciar los Servicios en Segundo Plano

```bash
docker compose up -d
```

**¿Qué hace?** Descarga las imágenes, crea y inicia los contenedores en modo detached (sin bloquear la terminal).

---

#### Detener los Servicios

```bash
docker compose down
```

**¿Qué hace?** Detiene y elimina los contenedores, pero conserva los volúmenes de datos (base de datos).

---

#### Ver el Estado de los Contenedores

```bash
docker ps
```

**¿Qué hace?** Lista todos los contenedores que están actualmente en ejecución.

**Ejemplo de salida:**
```
CONTAINER ID   IMAGE              STATUS        PORTS
a1b2c3d4e5f6   mongodb:latest     Up 2 minutes  27017->27017/tcp
x7y8z9a0b1c2   node:16-alpine     Up 2 minutes  3001->3001/tcp
```

---

#### Detener y Eliminar Volúmenes (Limpieza Completa)

```bash
docker compose down -v
```

**¿Qué hace?** Detiene los contenedores y elimina también los volúmenes (datos de la base de datos). **¡Úsalo con cuidado, elimina los datos!**

---

### Otros Comandos Útiles de Docker Compose

#### Ver los Logs de los Servicios

```bash
docker compose logs
```

Para ver logs de un servicio específico:

```bash
docker compose logs server-admin
```

Para ver logs en tiempo real:

```bash
docker compose logs -f server-admin
```

---

#### Actualizar las Imágenes de los Servicios

```bash
docker compose pull
```

**¿Qué hace?** Descarga las versiones más recientes de las imágenes definidas en `docker-compose.yml`.

---

#### Reconstruir las Imágenes

```bash
docker compose build
```

**¿Qué hace?** Reconstruye las imágenes Docker a partir de los Dockerfiles si hay cambios en el código.

---

#### Ejecutar un Comando en un Contenedor en Ejecución

```bash
docker compose exec server-admin npm run dev
```

**¿Qué hace?** Ejecuta un comando dentro del contenedor `server-admin`.

---

#### Pausar y Reanudar Servicios

Pausar:
```bash
docker compose pause
```

Reanudar:
```bash
docker compose unpause
```

---

#### Eliminar Contenedores Detenidos

```bash
docker system prune
```

**¿Qué hace?** Elimina contenedores, redes e imágenes que no se estén usando.

---

### Flujo de Trabajo Típico con Docker Compose

1. **Iniciar el entorno por primera vez:**
   ```bash
   docker compose up -d
   ```

2. **Verificar que todo está corriendo:**
   ```bash
   docker ps
   ```

3. **Ver los logs para debugging:**
   ```bash
   docker compose logs -f
   ```

4. **Hacer cambios en el código y reconstruir (si es necesario):**
   ```bash
   docker compose up -d --build
   ```

5. **Cuando termines, detener todo:**
   ```bash
   docker compose down
   ```

6. **Limpieza completa (eliminar datos):**
   ```bash
   docker compose down -v
   ```

---

## 9. API Endpoints (Rutas y Funciones)


### 📋 Información General

* **Base URL:** `http://localhost:3001/BIK/v1`
* **Headers Comunes:**
* `Content-Type`: `application/json`
* `Authorization`: `Bearer <TU_TOKEN_JWT>` (Solo para rutas privadas)
---

Para obtener y aplicar el token de autenticación en Postman y así poder realizar peticiones a las rutas protegidas, sigue estos pasos:

### 1. Obtención del Token (Login)

Para generar un token válido, primero debes autenticarte con un usuario existente.

* **Método:** `POST`
* **URL:** `http://localhost:3001/BIK/v1/auth/login`
* **Body (JSON):** Envía las credenciales del usuario.
```json
{
    "email": "tu-correo@ejemplo.com",
    "password": "tu-password"
}

```


* **Respuesta:** El servidor te devolverá un objeto JSON que contiene una propiedad llamada `token`. **Copia ese valor** (sin las comillas).

---

### 2. Aplicación del Token en Peticiones Protegidas

Una vez que tengas el token, debes incluirlo en cada petición que lo requiera (marcadas con "✅ Token" en la documentación).

#### Opción A: Pestaña "Authorization" (Recomendado)

1. En Postman, selecciona la pestaña **Auth** o **Authorization**.
2. En el menú desplegable **Type**, selecciona **Bearer Token**.
3. En el campo de la derecha llamado **Token**, pega el código que copiaste anteriormente.

#### Opción B: Pestaña "Headers" (Manual)

Si prefieres hacerlo manualmente, ve a la pestaña **Headers** y agrega la siguiente entrada:

* **Key:** `Authorization`
* **Value:** `TU_TOKEN_AQUÍ`

---

### 3. Verificación

Si el token se aplicó correctamente, al intentar acceder a una ruta protegida como `/auth/me`, el servidor te devolverá la información del usuario en lugar de un error `401 Unauthorized` o `500`.

---

### 🔐 1. Autenticación (Auth)

*Gestión de acceso y perfiles.*

| Método | Endpoint Completo | Auth? | ¿Qué es el `:id`? | Descripción | Body (JSON) Sugerido |
| --- | --- | --- | --- | --- | --- |
| **POST** | `/auth/register` | ❌ No | N/A | Registrar un nuevo cliente. | `{"name": "Ana", "surname": "Lopez", "username": "analo", "email": "ana@mail.com", "password": "123456", "phone": "55554444"}` |
| **POST** | `/auth/login` | ❌ No | N/A | Iniciar sesión y obtener Token. | `{"email": "ana@mail.com", "password": "123456"}` |
| **GET** | `/auth/me` | ✅ Token | N/A | Obtener datos del perfil logueado. | *N/A* |

---

### 👤 2. Usuarios (Users)

*Gestión de usuarios del sistema (Requiere Rol ADMIN).*

| Método | Endpoint Completo | Auth? | ¿Qué es el `:id`? | Descripción | Body (JSON) Sugerido |
| --- | --- | --- | --- | --- | --- |
| **GET** | `/users` | ✅ Admin | N/A | Listar todos los usuarios. | *N/A* |
| **GET** | `/users/:id` | ✅ Admin | **ID del Usuario** | Ver detalle de un usuario. | *N/A* |
| **PUT** | `/users/:id` | ✅ Admin | **ID del Usuario** | Actualizar datos de usuario. | `{"name": "Ana María", "phone": "11223344"}` |
| **DELETE** | `/users/:id` | ✅ Admin | **ID del Usuario** | Eliminar (desactivar) usuario. | *N/A* |

---

### 💳 3. Cuentas (Accounts)

*Gestión de cuentas bancarias.*

| Método | Endpoint Completo | Auth? | ¿Qué es el `:id`? | Descripción | Body (JSON) Sugerido |
| --- | --- | --- | --- | --- | --- |
| **GET** | `/accounts` | ✅ Token | N/A | Listar todas las cuentas. | *N/A* |
| **GET** | `/accounts/:id` | ✅ Token | **ID de Cuenta** | Ver detalle de una cuenta. | *N/A* |
| **POST** | `/accounts/:id` | ✅ Token | **ID del Usuario** (Dueño) | Crear cuenta a un usuario específico. | `{"dpi": "1234567890101", "typeAcount": "Ahorro", "nameAccount": "Ahorro Navidad", "email": "ana@mail.com", "phoneNumber": "55554444"}` |
| **PUT** | `/accounts/:id` | ✅ Token | **ID de Cuenta** | Actualizar info de la cuenta. | `{"nameAccount": "Cuenta Principal"}` |
| **PUT** | `/accounts/:id/activate` | ✅ Token | **ID de Cuenta** | Activar una cuenta. | *N/A* |
| **PUT** | `/accounts/:id/desactivate` | ✅ Token | **ID de Cuenta** | Desactivar una cuenta. | *N/A* |

---

### 🛠️ 4. Servicios (Services)

*Catálogo de servicios pagables (Luz, Agua, etc.).*

| Método | Endpoint Completo | Auth? | ¿Qué es el `:id`? | Descripción | Body (JSON) Sugerido |
| --- | --- | --- | --- | --- | --- |
| **GET** | `/services` | ✅ Token | N/A | Listar servicios disponibles. | *N/A* |
| **POST** | `/services` | ✅ Token | N/A | Crear nuevo servicio en el sistema. | `{"nameService": "Pago de servicios", "typeService": "Internet", "numberAccountPay": "INT-9988", "methodPayment": "Bancaria", "amounth": 250}` |
| **GET** | `/services/:id` | ✅ Token | **ID de Servicio** | Ver un servicio específico. | *N/A* |
| **PUT** | `/services/:id` | ✅ Token | **ID de Servicio** | Editar servicio. | `{"amounth": 300}` |
| **PUT** | `/services/:id/:status` | ✅ Token | **ID de Servicio** y **Estado** | Cambiar estado (PENDING, COMPLETED, CANCELED). | *N/A* (El estado va en la URL, ej: `/services/ID/CANCELED`) |

---

### 💰 5. Depósitos (Deposits)

*Ingreso de dinero a cuentas.*

| Método | Endpoint Completo | Auth? | ¿Qué es el `:id`? | Descripción | Body (JSON) Sugerido |
|--------|------------------|-------|------------------|------------|----------------------|
| **POST** | `/deposits` | ✅ Token | N/A | **ADMIN:** Suma dinero (Ventanilla).<br>**CLIENT:** Transfiere de su cuenta a destino. | `{"accountId":"ID_CUENTA_DESTINO","amount":500,"description":"Abono"}` |
| **GET** | `/deposits/history/:accountId` | ✅ Token | ID de Cuenta | Ver historial de depósitos recibidos. | N/A |
| **GET** | `/deposits/:id` | ✅ Token | ID de Depósito | Ver detalle de un depósito. | N/A |

---

### 💸 6. Transacciones (Transactions)

*Movimientos de dinero (Transferencias y Pagos).*

| Método | Endpoint Completo | Auth? | ¿Qué es el `:id`? | Descripción | Body (JSON) Sugerido |
| --- | --- | --- | --- | --- | --- |
| **POST** | `/transactions/transfer` | ✅ Token | N/A | Transferencia entre cuentas. | `{"sourceAccount": "ID_CUENTA_ORIGEN", "destinationAccount": "ID_CUENTA_DESTINO", "amount": 100, "description": "Regalo"}` |
| **POST** | `/transactions/pay-service` | ✅ Token | N/A | Pagar un servicio del catálogo. | `{"sourceAccount": "ID_CUENTA_ORIGEN", "serviceId": "ID_DEL_SERVICIO", "amount": 250}` |
| **GET** | `/transactions/history/:accountId` | ✅ Token | **ID de Cuenta** | Historial de transacciones de una cuenta. | *N/A* |
| **GET** | `/transactions/:id` | ✅ Token | **ID de Transacción** | Ver detalle de una transacción. | *N/A* |

---

### 💡 Notas Importantes para el Frontend/QA

1. **IDs:** Cuando dice "ID", se refiere siempre al **`_id` de MongoDB** (cadena de 24 caracteres, ej: `65d1f2a...`), NO al número de cuenta o DPI.
2. **Roles:**
* Si usas el endpoint `/deposits` con un token de **ADMIN**, el dinero se crea.
* Si lo usas con token de **CLIENTE**, el dinero se descuenta de la cuenta del usuario logueado.

3. **Fechas:** Todas las fechas se generan automáticamente en el servidor.