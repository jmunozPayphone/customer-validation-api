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
POST /api/customers/assess-risk
Content-Type: application/json

{
    "name": "George Bluth",
    "document": "0102030405",
    "txAmount": 800.00
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

## Consideraciones adicionales

* La primera fase no persiste datos, todas las consultas van al proveedor HTTP simulado.
* El diseño está preparado para **extenderse fácilmente**: persistencia en DB, cache, logging, manejo de errores.
* El score generado es determinístico, pero simulado; en un entorno real, se reemplazaría por un proveedor de crédito real.

---

Si quieres, puedo armar también **una versión lista para GitHub** con **secciones de instalación, configuración de API Key, pruebas unitarias y ejemplos de CQRS**, que quede profesional y lista para entregar o presentar.

¿Quieres que haga eso?
