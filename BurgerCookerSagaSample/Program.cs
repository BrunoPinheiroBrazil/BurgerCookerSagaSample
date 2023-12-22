using BurgerCookerSagaSample.Services;
using MassTransit;
using System.Reflection;

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

	mt.AddConsumers(entryAssembly);
	mt.AddSagaStateMachines(entryAssembly);
	mt.AddSagas(entryAssembly);
	mt.AddActivities(entryAssembly);

	mt.UsingInMemory((context, cfg) =>
	{
		cfg.ConfigureEndpoints(context);
	});
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
