using UserRegistrationAPI.Data;


var builder = WebApplication.CreateBuilder(args);


// register class
builder.Services.AddScoped<UserDatabaseAccess>(provider =>
    new UserDatabaseAccess(
        builder.Configuration.GetConnectionString("UserSignUpSignIn")!));


// Register controllers
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Client", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("Client");

app.MapControllers();


app.Run();
