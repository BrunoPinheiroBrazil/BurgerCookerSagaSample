using BurgerCookerSagaSample.Consumers;
using BurgerCookerSagaSample.Events;
using BurgerCookerSagaSample.Services;
using BurgerCookerSagaSample.StateMachine;
using MassTransit;
using System.Reflection;
using static MassTransit.Logging.OperationName;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICookBurgerService, CookBurgerService>();

builder.Services.AddMassTransit(mt =>
{
	mt.SetKebabCaseEndpointNameFormatter();

	mt.SetInMemorySagaRepositoryProvider();

	var entryAssembly = Assembly.GetEntryAssembly();

  mt.AddConsumer<CookBurgerConsumer>();
  mt.AddSagaStateMachine<BurgerCookerStateMachine, BurgerCookerState>()
        .InMemoryRepository();

  //mt.AddSagas(entryAssembly);
	mt.AddActivities(entryAssembly);

  mt.UsingRabbitMq((context, cfg) =>
  {
		cfg.ConfigureEndpoints(context);
    cfg.Host(new Uri("rabbitmq://localhost//TESTS"), h =>
    {
      h.Username("guest");
      h.Password("guest");

      //No SSL for local connection
      //h.UseSsl(c =>
      //{
      //  c.Protocol = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls13;
      //});
    });
  });

  mt.AddRequestClient<BurgerCookerOrderedEvent>(RequestTimeout.After(s:30));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


public partial class Program { }