# TaskManagement API
  TaskManagement.API is a RESTful API that provides a flexible and efficient way to manage tasks and projects. 
  Built using ASP.NET Core and MS SQL, this API allows users to create, retrieve, update, and delete tasks while ensuring secure authentication and authorization mechanisms.

# Features
•	<strong>Task CRUD Operations:</strong> Perform CRUD (Create, Read, Update, Delete) operations on tasks. <br />
•	<strong>User Authentication:</strong> Secure user authentication using JSON Web Tokens (JWT) for enhanced security. <br />
•	<strong>Role-Based Authorization:</strong> Implement role-based access control, ensuring only authorized users can perform certain actions. <br />
The API supports the following roles for users: Tester, Developer, TeamLead, ProductManager, Designer and System roles: Admin and SuperAdmin. <br />

•	<strong>Refresh Token Mechanism:</strong> Implement a token refresh mechanism for a seamless user experience. <br />
•	<strong>Comment Functionality:</strong> Allow users to create, update, and delete comments associated with tasks. <br />
•	<strong>Real-time notifications:</strong> Task creators receive instant updates when a comment is added to one of their tasks. <br />
•	<strong>Customizable Task Attributes:</strong> Define task attributes such as title, description, due date, and status. <br />
•	<strong>Task Attachments: </strong> Attach files or documents to tasks, providing additional context or resources. <br />
•	<strong>Team Management:</strong> Create, view, update, and delete teams. Assign members to teams and associate tasks with teams.
    Retrieve a list of teams with member details.<br />
  
•	<strong>Update User Information:</strong> Allow users to update their personal information, including first name, last name, username, user role etc.<br />
•	<strong>Change Password:</strong> Enable users to change their passwords securely, including verification of the current password.

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

 <strong>1. System Roles</strong> <br />
   • Admin Role - Admins can perform administrative tasks, manage users' roles, and have full access to all tasks. <br />
   • SuperAdmin Role - The SuperAdmin role has the highest level of access and control.
     SuperAdmins can manage system-wide settings, configurations, and perform advanced administrative tasks.
     
  <strong>2. Basic Roles</strong> <br />
     • This Include: Tester, Developer, Designer, TeamLead and ProductManager Roles - These roles define the level of access and control users have within the application. 

# Technologies Used
  • ASP.NET Core 7 <br />
  • Entitiy Framework Core <br />
  • MS SQL

 # API Endpoints
  <strong>Auth</strong> <br />
  • POST /api/auth/register: Register a new user. <br />
  • POST /api/auth/login: Authenticate a user and generate access and refresh tokens. <br />
  • POST /api/auth/refresh-token: Generate a new access token using a valid refresh token. <br />
  • Post /api/auth/seed-roles  <br />
  • Post /api/auth/make-admin  <br />
  • Post /api/auth/make-super-admin <br />

 <strong>Token</strong> <br />
  • POST /api/Token/refresh-token: Generate a new access token using a valid refresh token.<br />
  • POST /api/Token/revoke-token: Revoke a refresh token (when user logs out).

  <strong>User</strong> <br />
  • GET /api/user: Retrieve a list of all users. <br />
  • GET /api/user/{id}: Retrieve details of a specific user. <br />
  • DELETE /api/user/{id}: Delete a user. <br />
  • Put /api/user/Update/{userId}: Update user information. <br />
  • Put /api/user/changePassword/{userId}: Change user password. <br />
  
   <strong>Task</strong> <br />
  • POST /api/task: Create a new task. <br />
  • GET /api/task: Retrieve a list of all tasks. <br />
  • GET /api/task/{id}: Retrieve details of a specific task. <br />
  • PUT /api/task/{id}: Update the details of a task. <br />
  • DELETE /api/task/{id}: Delete a task. <br />

  <strong>Team</strong> <br />
  • POST /api/team: Create a new team with a unique name. <br />
  • GET /api/team: Retrieve a list of all teams along with their member details<br />
  • DELETE /api/team/{teamId}: Delete a team and remove its association with members. <br />
      Tasks assigned to the team will also be disassociated.


  <strong>Comments</strong> <br />
  • POST /api/comment: Create a new comment. <br />
  • GET /api/comment: Retrieve a list of all comments. <br />
  • GET /api/comment/{commentId}: Retrieve details of a specific comment. <br />
  • PUT /api/comment/{commentId}: Update the details of a comment. <br />
  • DELETE /api/comment/{commentId}: Delete a comment. <br />

# Get Started 
  Follow these steps to set up and run the TaskManagement.API project locally:
  1. Clone the repository: git clone `https://github.com/AvtoNemsadze/TaskManagement.git`
  2. Navigate to the project directory: `cd TaskManagement.API`
  3. Install dependencies: `dotnet restore`
  4. Configure the database connection in `appsettings.json`
  5. Apply database migrations: `dotnet ef database update`
  6. Run the application: `dotnet run`
