# Whipcords

Whipcords is helper library aimed at simplifying the development of ASP.NET Core applications by combining and leveraging the power of other packages whilst providing a template of use.
Below gives a breakdown of the structure of the library and the features in use.

### Layer 1 Application Host

The application host is how the .NET runtime loads and configures your web app. The work done here is target and environment specific.

| Target | Description |
| --- | --- |
| Blazor server side app | ? |
| Blazor WASM app | ? |
| MVC app | ? |
| WebAPI app | ? |


| Feature | Environment | Description |
| --- | --- | --- |
| Audit.NET | All | Records the request and response. Useful for request level security and diagnostic auditing. |
| Mediator | All | Handles the dispatching of Command\Query requests. |
| Swashbuckle | Non-Live | Records the request and response. Useful for request level security and diagnostic auditing. |


### Layer 2 pipeline

The pipeline focuses on how a client request is recieved and executed by the web server.
As part of the pipeline we use the following features:

| Feature | Description |
| --- | --- |
| Audit.NET | Records the request and response. Useful for request level security and diagnostic auditing. |

The following diagram gives a simplified flow of the request from the client to the controller.

```plantUml

"Http Client" -> "Web Server" : GET
"Web Server" -> "Authentication Handler" : GET
"Web Server" -> "Controller" : GET
"Controller" -> "Web Server" : RESPONSE
"Web Server" -> "Audit.NET" : Record Request + Response
"Web Server" -> "Http Client" : RESPONSE

```

### Layer 3 controller

```plantUml

"Controller" -> "Authorization Manager" : Request Based Authorization
"Controller" -> "Model Validation" : GET
"Controller" -> "Mediator" : GET QUERY
"Controller" -> "Authorization Manager" : Resource Based Authorization

```


### Layer 4 CQRS

A request can be either a Command or a Query. A Command is typically a request for an **asynchronous** action (i.e. a transaction for which the requestor doesn't need to wait) for trace flow this library is designed to return a unique id.

A query will typically be a read of data so is a **synchronous** message flow. The design of this library intends for Response Data Transfer Objects to be used.

```plantUml

"Mediator" -> "Request Pre-Processor" : CQRS REQUEST
"Mediator" -> "Request Handler" : CQRS REQUEST
"Request Handler" -> "Business Logic" : CQRS REQUEST
"Business Logic" -> "Request Handler" : LOGIC RESPONSE
"Request Handler" -> "Mediator" : QUERY RESPONSE
"Mediator" -> "Request Post-Processor" : QUERY RESPONSE

```

#### Post-Event Notifications to SignalR

TODO

#### Post-Event Notifications to MessageBus

TODO

### Layer 5 Domain Logic
