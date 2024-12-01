using UploadFileProject.DataAccessLayer;

var builder = WebApplication.CreateBuilder(args);

// Register Swagger services
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<IUploadFileDL, UploadFileDL>(); // Dependency Injection
builder.Services.AddEndpointsApiExplorer();               // Enable Minimal APIs in Swagger
builder.Services.AddSwaggerGen();                         // Add Swagger generation


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Enable Swagger UI
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

/*var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();*/

/*using UploadFileProject.DataAccessLayer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IUploadFileDL, UploadFileDL>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
        app.UseSwaggerUI();
        app.MapOpenApi();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSwagger();

app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
    {
        *//*        options.SerializeAsV2 = true;
        *//*
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });

app.MapControllers();

app.Run();
*/