using JhaRepository;
using JhaServer.Config;
using JhaServices.Twitter;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.ConfigureDependentServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

//start the tweetConsumerServices
//NOTE... in a production environment this would be run in a separate process/app
var tweetConsumerService = new TweetConsumerService(
	app.Configuration[ConfigKeys.Twitter.BearerToken],
	app.Services.GetRequiredService<ITweetStatisticsRepository>(),
	app.Services.GetRequiredService<ILogger<TweetConsumerService>>());

await tweetConsumerService.Run();

//for this exercise, we allow anyone
app.UseCors(policy => policy
				.AllowAnyHeader()
				.AllowAnyMethod()
				.SetIsOriginAllowed((host) => true));

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
