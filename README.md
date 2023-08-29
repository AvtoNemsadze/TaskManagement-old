# TaskManagement.API
  TaskManagement.API is a RESTful API that provides a flexible and efficient way to manage tasks and projects. 
  Built using ASP.NET Core and MS SQL, this API allows users to create, retrieve, update, and delete tasks while ensuring secure authentication and authorization mechanisms.

#Features
  •	Task CRUD Operations: Perform CRUD (Create, Read, Update, Delete) operations on tasks.
  •	User Authentication: Secure user authentication using JSON Web Tokens (JWT) for enhanced security.
  •	Role-Based Authorization: Implement role-based access control, ensuring only authorized users can perform certain actions. 
    The API supports the following roles for users: Tester, Developer, TeamLead, ProductManager, Designer and system roles - Admin and SuperAdmin
  •	Refresh Token Mechanism: Implement a token refresh mechanism for a seamless user experience.
  •	Customizable Task Attributes: Define task attributes such as title, description, due date, and status.
  •	Flexible Project Structure: The API follows a modular and organized project structure for easy maintenance and scalability.

# Authentication and Authorization
  The API uses JSON Web Tokens (JWT) for authentication and supports role-based authorization. 
  Users can log in, receive access and refresh tokens, and use these tokens to access protected endpoints.

# User Roles and Authorization
  The TaskManagement.API utilizes role-based authorization to control access to various parts of the application.
  There are two categories of roles: system roles and basic roles.

  1. System Roles
   • Admin Role - Admins can perform administrative tasks, manage users' roles, and have full access to all tasks.
   • SuperAdmin Role - The SuperAdmin role has the highest level of access and control.
     SuperAdmins can manage system-wide settings, configurations, and perform advanced administrative tasks.
     
  3. Basic Roles
     • This Include: Tester, Developer, Designer, TeamLead and ProductManager Roles - These roles define the level of access and control users have within the application. 

# Technologies Used
  • ASP.NET Core 7
  • Entitiy Framework Core
  • MS SQL

#API Endpoints
  • POST /api/task: Create a new task.
  • GET /api/task: Retrieve a list of all tasks.
  • GET /api/task/{id}: Retrieve details of a specific task.
  • PUT /api/task/{id}: Update the details of a task.
  • DELETE /api/task/{id}: Delete a task.
  • POST /api/auth/register: Register a new user.
  • POST /api/auth/login: Authenticate a user and generate access and refresh tokens.
  • POST /api/auth/refresh-token: Generate a new access token using a valid refresh token.
