Actúa como un Arquitecto de Software Senior especializado en Cloud Computing y .NET. 

Necesito desarrollar un Microservicio de Envío de Emails altamente escalable y resiliente utilizando .NET 10 (C# 14) y un modelo de arquitectura orientada a eventos.

### Requerimientos Técnicos Obligatorios:
1. **Infraestructura de Mensajería:** Azure Service Bus.
2. **Framework de Comunicación:** MassTransit (para abstraer la complejidad de Service Bus y manejar retries).
3. **Motor de Envío:** MailKit (SMTP profesional).
4. **Resiliencia:** Configurar una "Retry Policy" exponencial en MassTransit (3 reintentos) y manejo de Dead Letter Queues (DLQ) para mensajes fallidos.
5. **Logging:** Serilog con sinks para Consola y salida estructurada.

### Arquitectura del Código:
- **Framework:** .NET 10 Worker Service (`net10.0`).
- **Patrón de Diseño:** Clean Architecture simplificada (Separación de contratos, lógica de negocio y consumidores).
- **Inyección de Dependencias:** Uso estricto de interfaces para desacoplamiento.
- **Configuración:** Implementar "Options Pattern" con `IOptions<T>` para validar y leer las credenciales de Azure y SMTP.

### Por favor, genera los siguientes archivos con código completo y listo para producción:

1. `EmailService.Worker.csproj`: Configurado para .NET 10 con los paquetes necesarios (MassTransit.AzureServiceBusCore, MailKit, Serilog.AspNetCore).
2. `appsettings.json`: Estructura clara para `ServiceBusOptions` (ConnectionString) y `SmtpOptions` (Host, Port, User, Pass, SenderEmail).
3. `Contracts/SendEmailRequest.cs`: Usando C# 14 Primary Constructors para el mensaje que viajará por la cola.
4. `Services/IEmailService.cs` y `Services/MailKitEmailService.cs`: Lógica de envío usando MailKit, manejando correctamente `CancellationToken` y `async/await`.
5. `Consumers/EmailRequestConsumer.cs`: Consumidor de MassTransit que procesa la cola. Debe incluir logs detallados sobre el inicio y fin del procesamiento.
6. `Program.cs`: El corazón del servicio. Aquí debes:
   - Configurar Serilog.
   - Registrar las dependencias.
   - Configurar MassTransit para usar Azure Service Bus (`UsingAzureServiceBus`).
   - Configurar el `ReceiveEndpoint` para que MassTransit cree la cola automáticamente si no existe.

### Notas adicionales: 
- Asegúrate de usar las nuevas características de rendimiento de .NET 10 si aplican.
- El código debe ser "Production Ready", con manejo de excepciones y disposición correcta de recursos (usando `using` statements).
- Incluye comentarios breves explicando por qué usas cada configuración de Azure Service Bus.


Cada vez que te pida un paso o algo lo tendras que explicar detalladamente, no solo el código, sino también el por qué de cada decisión técnica.