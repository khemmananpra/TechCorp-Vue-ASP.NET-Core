# TechCorp - Employee Role Management System

A web-based employee role management system that provides CRUD (Create, Read, Update, and Delete) functionality for employee and organizational hierarchy management. The system enables administrators to assign positions, define reporting relationships, and manage role structures efficiently.

## Features

* Create employee records with assigned roles and job positions
* View employee information and organizational hierarchies
* Update employee details, reporting structures, and assigned roles
* Delete employee records when necessary
* Define supervisor-subordinate relationships
* Upload employee profile photos
* Capture employee signatures directly within the system

## Tech Stack

### Frontend

* Vue.js
* TypeScript

### Backend

* ASP.NET Core Web API
* C#
* Swagger (OpenAPI)

### Database

* Microsoft SQL Server

## Database Setup

Import the SQL script into SQL Server using SQL Server Management Studio (SSMS) to create the required database schema.

## API Documentation

Swagger UI is integrated into the ASP.NET Core Web API for API documentation and testing.

After running the backend application, the API documentation can be accessed at:

https://localhost:7025/swagger/index.html

## User Interface

### Role Management Page

## The main page displays a list of roles within the organization. Users can view role information, manage organizational hierarchies, and identify reporting relationships between employees.

<img width="1047" height="907" alt="Screenshot 2026-06-10 173126" src="https://github.com/user-attachments/assets/0e9c024f-4e65-4a8b-8783-f936fdaf664c" />

---
## This page allows users to create a new employee, assign positions, specify the supervisor, and define subordinates within the organizational hierarchy.

<img width="856" height="906" alt="Screenshot 2026-06-10 173518" src="https://github.com/user-attachments/assets/225e5bca-6f78-4e44-a293-fa15d12fbadf" />

---
## Choose your position in dropdown position

<img width="873" height="903" alt="Screenshot 2026-06-10 173528" src="https://github.com/user-attachments/assets/fe2e0ea9-24e4-487e-b83f-c07fe2243759" />

---
## and then upload your face image 

<img width="1429" height="912" alt="Screenshot 2026-06-10 173757" src="https://github.com/user-attachments/assets/1d69ac7f-5264-4c22-b26f-0fd00d2793fe" />

---
## Capture employee signatures directly through the system

<img width="867" height="905" alt="Screenshot 2026-06-10 173908" src="https://github.com/user-attachments/assets/898d54ca-a9b8-42d4-9da8-c0fa96175646" />

---



## Project Structure

```text
techcorp_vue.client/    # Frontend application
TechCorp_Vue.Server/           # ASP.NET Core Web API
TechCorp_SQL.sql/               # SQL scripts
```

## Purpose

The purpose of this project is to provide a simple and efficient CRUD-based employee management system that supports organizational hierarchy administration, role assignment, and reporting relationship management.


