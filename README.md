# TaskManagement.API
  TaskManagement.API is a RESTful API that provides a flexible and efficient way to manage tasks and projects. 
  Built using ASP.NET Core and MS SQL, this API allows users to create, retrieve, update, and delete tasks while ensuring secure authentication and authorization mechanisms.

# Features
•	<strong>Task CRUD Operations:</strong> Perform CRUD (Create, Read, Update, Delete) operations on tasks. <br />
•	<strong>User Authentication:</strong> Secure user authentication using JSON Web Tokens (JWT) for enhanced security. <br />
•	<strong>Role-Based Authorization:</strong> Implement role-based access control, ensuring only authorized users can perform certain actions. <br />
<strong>The API supports the following roles for users:</strong> Tester, Developer, TeamLead, ProductManager, Designer and System roles: Admin and SuperAdmin. <br />

•	<strong>Refresh Token Mechanism:</strong> Implement a token refresh mechanism for a seamless user experience. <br />
•	<strong>Customizable Task Attributes:</strong> Define task attributes such as title, description, due date, and status. <br />
•	<strong>Flexible Project Structure:</strong> The API follows a modular and organized project structure for easy maintenance and scalability.

# User Registration
  The TaskManagement.API provides a user-friendly and secure user registration process, 
  allowing individuals to create accounts and gain access to the powerful features of the application. <br />
  Overview of the registration process: <br />
  
<strong>Step 1: Account Information</strong> <br />
Users start by navigating to the registration page, where they are prompted to provide
essential account information. This includes: <br />
•	<strong>Username:</strong> Users choose a unique username that will identify them within the system. <br />
•	<strong>Email Address:</strong> A valid email address is required for communication and account recovery. <br />
•	<strong>Password:</strong> Users create a strong password to secure their account.

<strong>Step 2: Profile Details</strong> <br />
After providing account information, users are guided to enter their profile details.
This step includes: <br />
•	<strong>First Name and Last Name:</strong> Users enter their full names for personalized identification. <br />
•	<strong>User Role:</strong> Users select their designated role (e.g., Tester, Developer, Team Lead, Product Manager, Designer) from the available options. This role defines their responsibilities and permissions within the application. <br />

<strong>Step 3: Account Creation</strong> <br />
  With the necessary information submitted, users proceed to create their account.
  During this step: <br />
  • User-provided data is validated to ensure accuracy and prevent errors. <br />
  • Passwords are securely hashed to protect user credentials. <br />
  • Account creation prompts users with clear instructions and validation messages. <br />

 # Role-Based Access
  The registration process ensures that each user is assigned a specific role that aligns with their responsibilities. This role         
  determines the tasks and features users can access within the application, creating a tailored experience that enhances collaboration 
  and productivity.
  
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

  • GET /api/user: Retrieve a list of all users. <br />
  • GET /api/user/{id}: Retrieve details of a specific user. <br />
  • DELETE /api/user/{id}: Delete a user. <br />

# Get Started 
  Follow these steps to set up and run the TaskManagement.API project locally:
  1. Clone the repository: git clone `https://github.com/AvtoNemsadze/TaskManagement.git`
  2. Navigate to the project directory: `cd TaskManagement.API`
  3. Install dependencies: `dotnet restore`
  4. Configure the database connection in `appsettings.json`
  5. Apply database migrations: `dotnet ef database update`
  6. Run the application: `dotnet run`
