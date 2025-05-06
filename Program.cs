var builder = WebApplication.CreateBuilder(args);

string appConfigConnectionString = builder.Configuration["ConnectionString:endPoint"];
builder.Configuration.AddAzureAppConfiguration(options =>
{
	options.Connect(appConfigConnectionString)
		   .Select("*") // Load all keys or filter with Select("App:*")
		   .ConfigureRefresh(refreshOptions =>
		   {
			   refreshOptions.Register("Settings:Sentinel", refreshAll: true)
							 .SetCacheExpiration(TimeSpan.FromSeconds(5));
		   });
});
builder.Services.AddAzureAppConfiguration();
// Add services to the container.

builder.Services.AddControllers();
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
app.UseAzureAppConfiguration();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
