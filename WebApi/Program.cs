using EasyResult;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    //.AddJsonOptions(opt=> opt.JsonSerializerOptions.PropertyNamingPolicy = null)
    .AddEasyResult(option => 
    {
        option.SuccessDefaultMessage = "عملیات با موفقیت انجام شد!";
        option.UnhandledExceptionStatusCode = System.Net.HttpStatusCode.BadGateway;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseExceptionMiddleware();
app.MapControllers();

app.Run();
