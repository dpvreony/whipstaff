---
title: Home
layout: default
---
# DHGMS ASP.NET Core Contrib

![Build status](https://ci.appveyor.com/api/projects/status/jk9v57hxjj0mi6t4?svg=true)

## Mission Statement

To provide a series of helpers to aid in the development of solutions using ASP.NET core.

## Introduction

ASP.NET Core Contrib is a project that aims to give reusable components to reduce development time. It is currently aimed at the following types of objects:

* Commands (Interfaces and Classes)
* Command Factory (Interfaces and Classes)
* Queries (Interfaces and Classes)
* Query Factories (Interfaces and Classes)
* Request DTO POCO Objects (Classes)
* Response DTO POCO Objects (Classes)
* Web Api Controllers (Classes)

The project came out of the Nucleotide project where patterns were being established that weren't specific to roslyn code generation. One of the aims of that project is while it is generating code to reduce development burden it needs to to avoid duplicating code.

## Credits

* https://github.com/AArnott/CodeGeneration.Roslyn

## Getting Started

### Pre-requisites

You will need:
* Visual Studio 2017
* A project using netstandard 2.0 upward

### Get the packages

` Install-Package Dhgms.AspNetCoreContrib.Abstractions `
` Install-Package Dhgms.AspNetCoreContrib.Controllers `

### Get Coding

** There is an option to use Nucleotide to generate code for this library, but will be treated as out of scope for this to make the usage principles for this library understandle in their own right. **

#### Create a controller

The controllers use the following concepts:

* Allows utilisation of ASP.NET core depedency injection but doesn't actually care how classes are instantiated.
* Use the CQRS pattern
* The CQRS pattern has auditing in mind hence tracking IClaimsPrincipal
* Uses the ASP.NET core logging interface
* Uses MediatR for dispatching Commands and Queries
* Uses the ASP.NET core Authorization Service for user and resource authorization
* Allow for controllers that allow purely List\Get methods (Query Controllers)
* Allow for controllers that extend the Query Controllers for full CRUD (CRUD Controllers)

The controllers are generic heavy and break the SonarQube recommended limit of 2 generic arguments. This is to allow the inheriting implementations to be as flexible as possible while avoid vague implementations of queries\commands etc. Alternative designs will be considered if proposed.

##### Read\List capable MVC controller

** TODO **

##### synchronous CRUD capable MVC controller

** TODO **

##### Read\List capable API controller

** TODO **

##### synchronous CRUD capable API controller

** TODO **

##### asyncronous CRUD capable API controller

** TODO **

##### Read\List capable MVC controller with API controller for synchronous CRUD

** TODO **

##### Read\List capable MVC controller with API controller and SignalR hub for asynchronous CRUD and notifications

** TODO **