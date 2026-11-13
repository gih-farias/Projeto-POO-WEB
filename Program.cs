var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();   // Permite descobrir os endpoints dos controllers
builder.Services.AddSwaggerGen();             // Gera a documentação Swagger

builder.Services.AddSingleton<IMaterialRepository, MaterialRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
    );
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();                         // Publica o JSON da documentação
    app.UseSwaggerUI();                       // Publica a página do Swagger em /swagger
}

app.UseHttpsRedirection();
app.UseCors("PermitirFrontend");
app.UseAuthorization();
app.MapControllers();                         // Liga as rotas dos controllers

app.Run();