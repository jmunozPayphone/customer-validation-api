using CustomerValidation.Api.Middleware;
using CustomerValidation.ApplicationCore;
using CustomerValidation.Infrastructure;

var builder = WebApplication.CreateBuilder(args);


// Controllers
builder.Services
    .AddControllers(c => c.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.ToString());
});

builder.Services.AddApplicationCore();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Run();