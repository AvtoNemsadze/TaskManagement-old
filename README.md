# TaskManagement.API
  TaskManagement.API is a RESTful API that provides a flexible and efficient way to manage tasks and projects. 
  Built using ASP.NET Core and MS SQL, this API allows users to create, retrieve, update, and delete tasks while ensuring secure authentication and authorization mechanisms.

# Features
•	Task CRUD Operations: Perform CRUD (Create, Read, Update, Delete) operations on tasks. <br />
•	User Authentication: Secure user authentication using JSON Web Tokens (JWT) for enhanced security. <br />
•	Role-Based Authorization: Implement role-based access control, ensuring only authorized users can perform certain actions. <br />
The API supports the following roles for users: Tester, Developer, TeamLead, ProductManager, Designer and System roles: Admin and SuperAdmin. <br />

•	Refresh Token Mechanism: Implement a token refresh mechanism for a seamless user experience. <br />
•	Customizable Task Attributes: Define task attributes such as title, description, due date, and status. <br />
•	Flexible Project Structure: The API follows a modular and organized project structure for easy maintenance and scalability.

# User Registration
  The TaskManagement.API provides a user-friendly and secure user registration process, 
  allowing individuals to create accounts and gain access to the powerful features of the application. 
  Overview of the registration process: <br />
  
Step 1: Account Information
Users start by navigating to the registration page, where they are prompted to provide
essential account information. This includes:
•	<strong>Username:</strong> Users choose a unique username that will identify them within the system.
•	<strong>Email Address:</strong> A valid email address is required for communication and account recovery.
•	<strong>Password:</strong> Users create a strong password to secure their account.

# Authentication and Authorization
  The API uses JSON Web Tokens (JWT) for authentication and supports role-based authorization. 
  Users can log in, receive access and refresh tokens, and use these tokens to access protected endpoints.

# User Roles and Authorization
  The TaskManagement.API utilizes role-based authorization to control access to various parts of the application.
  There are two categories of roles: system roles and basic roles.

  1. System Roles <br />
   • Admin Role - Admins can perform administrative tasks, manage users' roles, and have full access to all tasks. <br />
   • SuperAdmin Role - The SuperAdmin role has the highest level of access and control.
     SuperAdmins can manage system-wide settings, configurations, and perform advanced administrative tasks.
     
  3. Basic Roles <br />
     • This Include: Tester, Developer, Designer, TeamLead and ProductManager Roles - These roles define the level of access and control users have within the application. 

# Technologies Used
  • ASP.NET Core 7 <br />
  • Entitiy Framework Core <br />
  • MS SQL

# API Endpoints
  • POST /api/task: Create a new task. <br />
  • GET /api/task: Retrieve a list of all tasks. <br />
  • GET /api/task/{id}: Retrieve details of a specific task. <br />
  • PUT /api/task/{id}: Update the details of a task. <br />
  • DELETE /api/task/{id}: Delete a task. <br />
  • POST /api/auth/register: Register a new user. <br />
  • POST /api/auth/login: Authenticate a user and generate access and refresh tokens. <br />
  • POST /api/auth/refresh-token: Generate a new access token using a valid refresh token. <br />

# Get Started 
  Follow these steps to set up and run the TaskManagement.API project locally:
  1. Clone the repository: git clone `https://github.com/AvtoNemsadze/TaskManagement.git`
  2. Navigate to the project directory: `cd TaskManagement.API`
  3. Install dependencies: `dotnet restore`
  4. Configure the database connection in `appsettings.json`
  5. Apply database migrations: `dotnet ef database update`
  6. Run the application: `dotnet run`
