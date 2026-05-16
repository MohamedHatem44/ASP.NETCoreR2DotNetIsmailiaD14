# ASP.NETCoreR2DotNetIsmailiaD14

# 🔷 ASP.NET Core Web API Identity – JWT Authentication & Authorization & Role Management (.NET 9)

## 📌 Project Overview

This project is an **ASP.NET Core Web API** that demonstrates:

- ASP.NET Core Identity
- JWT Authentication
- Role-Based Authorization
- Policy-Based Authorization
- Entity Framework Core
- SQL Server
- OpenAPI Documentation
- Secure API Development

The application allows users to:

- Register accounts
- Login using JWT Tokens
- Access protected endpoints
- Create roles
- Use authorization policies

---

# 🏗 Project Structure

```text
ASP.NETCoreD14
│
├── Controllers
│   ├── AuthController.cs
│   ├── RolesController.cs
│   ├── TestAuthController.cs
│   └── TestOptionsController.cs
│
├── Data
│   ├── Context
│   │   └── AppDbContext.cs
│   │
│   └── Models
│       ├── ApplicationUser.cs
│       └── ApplicationRole.cs
│
├── DTOs
│   ├── Auth
│   │   ├── RegisterDto.cs
│   │   ├── LoginDto.cs
│   │   └── TokenDto.cs
│   │
│   └── Role
│       └── RoleCreateDto.cs
│
├── Settings
│   └── JwtSettings.cs
│
└── Program.cs
```

---

# ⚙ Technologies Used

- ASP.NET Core Web API
- Entity Framework Core
- ASP.NET Core Identity
- JWT Bearer Authentication
- SQL Server
- OpenAPI
- Scalar API Reference

---

# 🔐 Authentication vs Authorization

## Authentication

Authentication means:

> "Who are you?"

The API validates the JWT token and identifies the user.

---

## Authorization

Authorization means:

> "What are you allowed to do?"

The API checks:

- Roles
- Claims
- Policies

---

# 🗄 Database Context

## AppDbContext

```csharp
public class AppDbContext :
    IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
}
```

The project uses:

- `ApplicationUser`
- `ApplicationRole`
- ASP.NET Core Identity Tables

---

# 👤 ApplicationUser

```csharp
public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }

    public string LastName { get; set; }
}
```

Extends `IdentityUser`.

---

# 🛡 ApplicationRole

```csharp
public class ApplicationRole : IdentityRole
{
    public string? Desciption { get; set; }
}
```

Extends `IdentityRole`.

---

# ⚙ JWT Settings

## JwtSettings.cs

```csharp
public class JwtSettings
{
    public string Issuer { get; set; }

    public string Audience { get; set; }

    public int DurationInMinutes { get; set; }

    public string SecretKey { get; set; }
}
```

---

# 📄 appsettings.json Example

```json
"JwtSettings": {
  "Issuer": "Issuer",
  "Audience": "Audience",
  "DurationInMinutes": 600,
  "SecretKey": "BASE64_SECRET_KEY"
}
```

---

# 🚀 Program.cs Configuration

---

# Add Controllers

```csharp
builder.Services.AddControllers();
```

---

# Configure Database

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SDMVCDay14"));
});
```

---

# Configure JWT Settings

```csharp
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));
```

---

# Configure Identity

```csharp
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
```

Registers:

- UserManager
- RoleManager
- SignInManager
- Identity Services

---

# Configure Identity Options

```csharp
builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;

    options.User.RequireUniqueEmail = true;

    options.Password.RequireNonAlphanumeric = false;

    options.Password.RequiredLength = 4;
});
```

---

# 🔑 JWT Authentication

## Configure Authentication

```csharp
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
        JwtBearerDefaults.AuthenticationScheme;

    options.DefaultChallengeScheme =
        JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters =
        new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings.Issuer,

            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Convert.FromBase64String(jwtSettings.SecretKey))
        };
});
```

---

# 🧠 What Happens During Authentication?

1. Client sends JWT token
2. API validates token
3. User claims are extracted
4. User becomes authenticated
5. Authorization policies are applied

---

# 🔒 Authorization Policies

## AdminOnly Policy

```csharp
options.AddPolicy("AdminOnly", policy =>
    policy.RequireRole("Admin"));
```

---

## Claim-Based Policy

```csharp
options.AddPolicy("Employee", policy =>
    policy.RequireClaim("EmployeeNumber"));
```

---

## Custom Policy

```csharp
options.AddPolicy("Over18", policy =>
    policy.RequireAssertion(context =>
        context.User.HasClaim(
            c => c.Type == "Age"
            && int.Parse(c.Value) >= 18)));
```

---

# 📦 DTOs

---

## RegisterDto

```csharp
public class RegisterDto
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }
}
```

---

## UserLoginDto

```csharp
public record UserLoginDto(
    string Email,
    string Password
);
```

---

## TokenDto

```csharp
public record TokenDto
(
    string AccessToken,
    int DurationInMinutes,
    string TokenType = "Bearer"
);
```

---

# 👨‍💻 AuthController

Base Route:

```text
api/Auth
```

---

# 📝 Register Endpoint

## Endpoint

```http
POST /api/Auth/Register
```

---

## Request Body

```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "JohnDoe@gmail.com",
  "userName": "JohnDoe",
  "password": "1234"
}
```

---

## Logic

1. Validate ModelState
2. Create ApplicationUser
3. Save user using Identity
4. Assign Admin role
5. Return success response

---

# 🔑 Login Endpoint

## Endpoint

```http
POST /api/Auth/Login
```

---

## Request Body

```json
{
  "email": "JohnDoe@gmail.com",
  "password": "1234"
}
```

---

## Login Process

1. Find user by email
2. Check password
3. Create claims
4. Generate JWT token
5. Return token

---

# 🧾 Claims

Claims added inside token:

```csharp
claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));

claims.Add(new Claim(ClaimTypes.Name, user.UserName));

claims.Add(new Claim(ClaimTypes.Email, user.Email));
```

Roles are also added:

```csharp
claims.Add(new Claim(ClaimTypes.Role, role));
```

---

# 🔐 Generate JWT Token

```csharp
var jwt = new JwtSecurityToken(
    issuer: _jwtSettings.Issuer,
    audience: _jwtSettings.Audience,
    claims: claims,
    signingCredentials: signingCredentials,
    expires: expiryDateTime
);
```

---

# 🛡 Protected Endpoints

## TestAuthController

Base Route:

```text
api/TestAuth
```

---

## Authenticated Endpoint

```csharp
[Authorize]
```

Endpoint:

```http
GET /api/TestAuth/V01
```

Requires valid JWT token.

---

## AdminOnly Endpoint

```csharp
[Authorize(Policy = "AdminOnly")]
```

Endpoint:

```http
GET /api/TestAuth/V02
```

Requires:

- Valid JWT
- Admin Role

---

# 👑 RolesController

Base Route:

```text
api/Roles
```

---

# Create Role Endpoint

```http
POST /api/Roles
```

Example:

```json
{
  "name": "Admin"
}
```

---

# 🧪 TestOptionsController

Used to test reading configuration using:

```csharp
IOptions<JwtSettings>
```

---

# 🔄 Middleware Pipeline

```csharp
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
```

---

# ⚠ Important Notes

## Authentication MUST come before Authorization

✅ Correct:

```csharp
app.UseAuthentication();

app.UseAuthorization();
```

❌ Wrong:

```csharp
app.UseAuthorization();

app.UseAuthentication();
```

---

# 📚 HTTP Status Codes Used

| Status Code | Meaning |
|---|---|
| 200 | OK |
| 400 | Bad Request |
| 401 | Unauthorized |
| 403 | Forbidden |
| 404 | Not Found |

---

# ▶ Running the Project

## 1️⃣ Restore Packages

```bash
dotnet restore
```

---

## 2️⃣ Create Migration

```bash
Add-Migration InitialCreate
```

---

## 3️⃣ Update Database

```bash
Update-Database
```

---

## 4️⃣ Run Project

```bash
dotnet run
```

---

# 🧪 Testing JWT Authentication

## Step 1 — Register User

```http
POST /api/Auth/Register
```

---

## Step 2 — Login

```http
POST /api/Auth/Login
```

Copy returned token.

---

## Step 3 — Use Token

Inside Swagger / Postman:

```text
Authorization: Bearer YOUR_TOKEN
```

---

# 🧠 Key Concepts Demonstrated

- JWT Authentication
- ASP.NET Core Identity
- Claims
- Roles
- Policy-Based Authorization
- Authentication Middleware
- Authorization Middleware
- Secure APIs
- Entity Framework Core
- OpenAPI

---

# 👨‍💻 Author

Mohamed Hatem  
Software Engineer

---