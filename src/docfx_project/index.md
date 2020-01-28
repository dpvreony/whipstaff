# This is the **HOMEPAGE**.
Refer to [Markdown](http://daringfireball.net/projects/markdown/) for how to write markdown files.
## Quick Start Notes:
1. Add images to the *images* folder if the file is referencing an image.

The following diagram details the basic flow of a HTTP GET request.

```plantUml

Http Client -> Web Server : GET
Web Server -> Authentication Handler : GET
Web Server -> Controller : GET
Web Server -> Request Based Access Control : GET
Controller -> Model Validation : GET
Controller -> Mediator : GET QUERY
Mediator -> Query Handler : GET QUERY
Controller -> Resource Based Access Control
Web Server -> Audit.NET

```
