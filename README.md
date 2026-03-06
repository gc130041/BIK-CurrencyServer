# BIK Currency Service

Microservicio desarrollado en **Node.js** encargado de proporcionar el **tipo de cambio actualizado** y permitir la **conversión de divisas** para las operaciones financieras del sistema BIK.

---

## Configuración de Entorno (.env)

Crea un archivo `.env` dentro de la carpeta `currency-service` con las siguientes variables:

```env
PORT=3002
MONGODB_URI=mongodb://mongo-db:27017/bik_currencies
````

---

## Ejecución del Proyecto

Para iniciar este servicio junto con todo el ecosistema:

1. Sitúate en la **raíz del proyecto principal**.
2. Ejecuta:

```bash
docker compose up -d --build
```

El contenedor se conectará automáticamente a su base de datos **MongoDB** y el servicio estará disponible en:

```
http://localhost:3002
```

---

## Uso de la Colección de Postman

1. Abre la colección del proyecto en **Postman**.
2. Verifica que la variable:

```
currency_base_url
```

tenga el valor:

```
http://localhost:3002
```

3. Accede a la sección correspondiente a **Currency Service**.

Desde ahí podrás probar la **conversión de divisas**, enviando:

* **Monto a convertir**
* **Código de moneda de origen**
* **Código de moneda de destino**