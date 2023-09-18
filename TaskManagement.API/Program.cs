using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using TaskManagement.API.Core.DataAccess;
using TaskManagement.API.Core.Hubs;
using TaskManagement.API.Core.Interface;
using TaskManagement.API.Core.OtherObjects;
using TaskManagement.API.Core.Services;


var builder = WebApplication.CreateBuilder(args);

// Add Authentication and JwtBearer
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
        };
    });


// Add services to the container.
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddAutoMapper(typeof(AutoMapperConfigProfile));

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("local"));
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOrSuperAdminPolicy", policy =>
    {
        policy.RequireRole(SystemRoles.ADMIN, SystemRoles.SUPERADMIN);
    });
});


builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<TaskSeedData>();

//add SignalR
builder.Services.AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbInitializer = services.GetRequiredService<TaskSeedData>();
    var authService = services.GetRequiredService<IAuthService>();

    // Seed Roles
    authService.SeedRolesAsync().Wait();

    // Seed Task Data
    dbInitializer.SeedData();
}
app.MapHub<CommentHub>("commentHub");

app.Run();


//protected override void Up(MigrationBuilder migrationBuilder)
//{
//    migrationBuilder.AddColumn<int>(
//        name: "CreatorId",
//        table: "Teams",
//        type: "int",
//        nullable: true);

//    migrationBuilder.CreateIndex(
//        name: "IX_Teams_CreatorId",
//        table: "Teams",
//        column: "CreatorId");

//    migrationBuilder.AddForeignKey(
//        name: "FK_Teams_Users_CreatorId",
//        table: "Teams",
//        column: "CreatorId",
//        principalTable: "Users",
//        principalColumn: "Id");
//}

///// <inheritdoc />
//protected override void Down(MigrationBuilder migrationBuilder)
//{
//    migrationBuilder.DropForeignKey(
//        name: "FK_Teams_Users_CreatorId",
//        table: "Teams");

//    migrationBuilder.DropIndex(
//        name: "IX_Teams_CreatorId",
//        table: "Teams");

//    migrationBuilder.DropColumn(
//        name: "CreatorId",
//        table: "Teams");
//}