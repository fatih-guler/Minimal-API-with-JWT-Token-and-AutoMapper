var builder = WebApplication.CreateBuilder(args);

// IZipService defined for Dependency Injection
builder.Services.AddScoped<IZipService, ZipService>();


builder.Services.AddDataProtection();
builder.Services.AddCors();

var ctx = new DataContext();

// Hashing Configurations
var servicerProvider = builder.Services.BuildServiceProvider();
var _provider = servicerProvider.GetService<IDataProtectionProvider>();

var protector = _provider.CreateProtector(builder.Configuration["Protector_Key"]);

// Auto Mapper Configurations
var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MyAutoMapper(protector));
});
IMapper autoMapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(autoMapper);

// JWT Authentication/Authorization Configurations
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Issuer"],
        ValidAudience = builder.Configuration["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SigningKey"]))
    };
}) ;


// ITokenService defined for Dependency Injection
builder.Services.AddSingleton<ITokenService>(new TokenService(builder));

builder.Services.AddEndpointsApiExplorer();
// Swagger Integration
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

app.UseAuthorization();
app.UseAuthentication();

app.UseCors(p =>
{
    p.AllowAnyOrigin();
    p.WithMethods("GET");
    p.AllowAnyHeader();
});

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1"));
}

// There is no authorization for this method
app.MapGet("/GetRoles", (Func<List<Role>>)(() => new()
{
    new(1, "Admin", 1),
    new(2, "User", 1),
    new(3, "Worker", 1)
}));

app.MapGet("/GetTop5UserPermissions", async (IZipService service) =>
{
    var ctx = new DataContext();
    var userList = ctx.Users.Select(s => s.Name).ToList();
    var roleList = ctx.Roles.Select(s => s.Name).ToList();
    var actionList = ctx.Actions.Select(s => s.ActionNumberTotal).ToList();

    return service.ZipResult(userList.ToList(), roleList, actionList);
});

app.MapPost("/InsertUser", async (UserViewModel user) =>
{
    var model = autoMapper.Map<User>(user);
    ctx.Users.Add(model);
    return new OkResult();
});

// This method requires authorization
app.MapGet("/GetAllUsersByID/{name}",[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async (string name) =>
{
    var model = ctx.Users.Where(u => u.Name.Contains(name)).ToList();
    var result = autoMapper.Map<List<UserViewModel>>(model);

    return result;

});

app.MapPost("/login", [AllowAnonymous] async (HttpContext http, ITokenService tokenService, Login login) => { 
    if(!string.IsNullOrEmpty(login.UserName) &&
       !string.IsNullOrEmpty(login.Password))
    {
        var userModel = ctx.Users.Where(u => u.Username == login.UserName && u.Password == login.Password).FirstOrDefault();
        if(userModel == null)
        {
            http.Response.StatusCode = 401;
            await http.Response.WriteAsJsonAsync(new {Message = "You are not authorized!"});
            return;
        }
        var token = tokenService.GetToken(userModel);
        await http.Response.WriteAsJsonAsync(new { token = token});
    }
});

app.Run();
