# Customer Risk Validation Service

## Descripción del proyecto

Este proyecto implementa un **servicio de validación de riesgo crediticio de clientes**, con el objetivo de evaluar si un cliente puede realizar una transacción según su score crediticio.

Actualmente, el servicio se encuentra en **una primera fase de desarrollo**, que consiste en consultar un **proveedor HTTP simulado** para obtener la información del score crediticio.

La arquitectura del proyecto está basada en **tres capas**:

1. **ApplicationCore**: Contiene los casos de uso, entidades, value objects, interfaces y la lógica de negocio.
2. **Infrastructure**: Implementa la integración con servicios externos (HTTP Providers), persistencia futura y cache.
3. **Presentation (API)**: Exposición de endpoints RESTful para la validación de riesgo.

---

## Funcionalidades implementadas

* Consulta de **score crediticio** de un cliente vía un **servicio HTTP externo** (por ahora usando Reqres API como simulador).
* Generación de un **score simulado** a partir de los datos del cliente (`id`, `email`, `first_name`, `last_name`).
* Retorno de un **resultado de riesgo** basado en reglas de negocio:

| Score                 | Condición de aprobación    |
| --------------------- | -------------------------- |
| ≥ 700                 | Aprobado                   |
| 500–699               | Aprobado si monto < $1,000 |
| < 500                 | Rechazado                  |
| Sin respuesta del API | Rechazado por seguridad    |

* Implementación de la lógica con **CQRS usando MediatR**, incluyendo Query (`GetCustomerCreditScoreQuery`) y Handler (`GetCustomerCreditScoreQueryHandler`).
* Uso de **HttpClientFactory** para consumir el proveedor HTTP, con configuración centralizada de timeout y base URL.

---

## Fases futuras

### Fase 2: Persistencia y cache

1. **Persistencia del score crediticio**

   * Evitar múltiples llamadas al proveedor externo.
   * Mantener los datos del score en base de datos para futuras consultas.

2. **Cache temporal de resultados**

   * Controlar reintentos o solicitudes duplicadas.
   * Configurar cache por cliente con una expiración de aproximadamente **5 minutos**.
   * Esto reduce la carga sobre el proveedor externo y mejora tiempos de respuesta.

---

## Arquitectura del proyecto

```
Presentation (API)
    └─ Exposición de endpoints REST (validación de riesgo)
    
ApplicationCore
    ├─ Casos de uso (AssessCustomerRisk)
    ├─ Entidades y Value Objects (CustomerScore, TransactionAmount)
    ├─ Interfaces de servicios externos (ICreditScoreProvider)
    └─ Lógica de negocio (Reglas de riesgo, generación de score)
    
Infrastructure
    ├─ Implementación de servicios HTTP (CreditScoreProvider)
    ├─ Configuración de HttpClientFactory
    ├─ Persistencia futura (DB)
    └─ Cache futura (in-memory / distributed)
```

---

## Ejemplo de uso (API)

**Request:**

```http
POST /api/customers/risk-assessment
Content-Type: application/json

{
    "name": "Jesus David",
    "documentNumber": "0986589647",
    "transactionAmount": 1500
}
```

**Response:**

```json
{
    "status": "Approved"
}
```

---

## Tecnologías utilizadas

* **.NET 7 / C# 11**
* **MediatR** (CQRS)
* **HttpClientFactory** para consumo HTTP
* **Newtonsoft.Json** para deserialización JSON
* Arquitectura **3 capas** (ApplicationCore, Infrastructure, Presentation)

---

### Por qué priorizamos la **consulta directa al proveedor** en la primera fase

1. **Objetivo inicial:**
   La primera fase del proyecto está diseñada para **validar que la integración con el proveedor externo funciona** y que podemos obtener correctamente el score crediticio.

2. **Evitar complejidad temprana:**
   Si implementamos persistencia y cache desde el inicio, complicamos el flujo de pruebas y desarrollo, porque tendríamos que simular DB, cache, expiraciones, etc. Al hacer la **consulta directa**, podemos enfocarnos en la lógica de negocio y reglas de riesgo primero.

3. **Determinismo de datos:**
   Consultar directamente nos asegura que siempre obtenemos los datos más recientes del proveedor, lo cual es importante para **probar las reglas de riesgo** con distintos valores de score.

4. **Preparación para la fase 2:**
   Una vez validada la integración, la **persistencia y cache** se agregan para optimizar el rendimiento y evitar múltiples llamadas al API externo. 

---

En pocas palabras: **primero garantizamos la funcionalidad y la exactitud de la consulta**, y luego optimizamos con persistencia y cache.

---

## Configuración de la aplicación (`appsettings.json`)

La aplicación requiere ciertos settings clave para funcionar correctamente, especialmente la integración con el proveedor de **Credit Score**.

Ejemplo de `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "CreditScoreProvider": {
    "BaseAddress": "https://reqres.in",
    "ApiKey": ""
  }
}
```

### Importancia de los settings

* **Logging:**
  Define el nivel de detalle de los logs. Por defecto, se registran **información general** y advertencias de ASP.NET Core. Es útil para depuración y monitoreo.

* **CreditScoreProvider.BaseAddress:**
  URL base del proveedor externo de score crediticio. Es fundamental que esté correcta para que las consultas HTTP funcionen.

* **CreditScoreProvider.ApiKey:**
  Este valor es **clave para la autenticación** con el proveedor externo.

  * Debe ser **asignado correctamente** antes de ejecutar la aplicación.
  * Sin el ApiKey, las llamadas al API externo fallarán con `Unauthorized`.
  * Se recomienda mantener el ApiKey en **variables de entorno** o en un **secret manager** en entornos de producción, en lugar de dejarlo directamente en `appsettings.json`.

> ⚠️ **Nota:** No se podrá obtener el score crediticio ni validar el riesgo si el `ApiKey` está vacío o incorrecto.

---

## Consideraciones adicionales

* La primera fase no persiste datos, todas las consultas van al proveedor HTTP simulado.
* El diseño está preparado para **extenderse fácilmente**: persistencia en DB, cache, logging, manejo de errores.
* El score generado es determinístico, pero simulado; en un entorno real, se reemplazaría por un proveedor de crédito real.