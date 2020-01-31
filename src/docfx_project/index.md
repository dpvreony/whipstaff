# This is the **HOMEPAGE**.
Refer to [Markdown](http://daringfireball.net/projects/markdown/) for how to write markdown files.
## Quick Start Notes:
1. Add images to the *images* folder if the file is referencing an image.

The following diagram details the basic flow of a HTTP GET request.

### Layer 1 Application Host

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
"Web Server" -> "Audit.NET" : "Record Request + Response"
"Web Server" -> "Http Client" : RESPONSE

```

### Layer 3 controller

```plantUml

"Controller" -> "Authorization Manager" : "Request Based Authorization"
"Controller" -> "Model Validation" : GET
"Controller" -> "Mediator" : "GET QUERY"
"Controller" -> "Authorization Manager" : "Resource Based Authorization"

```


### Layer 4 CQRS

#### Request

A request can be either a Command or a Query. A Command is typically a request for an asynchronous action (i.e. a transaction for which the requestor doesn't need to wait) for trace flow this library is designed to return a unique id.

A query will typically be a read of data so is a **synchronous** message flow. The design of this library intends for Response Data Transfer Objects to be used.

```plantUml

"Mediator" -> "Request Handler" : "CQRS REQUEST"
"Request Handler" -> "Business Logic" : "CQRS REQUEST"
"Business Logic" -> "Request Handler" : "LOGIC RESPONSE"
"Request Handler" -> "Mediator" : "QUERY RESPONSE"

```

#### Post-Event Notifications to SignalR

TODO

#### Post-Event Notifications to MessageBus

TODO
