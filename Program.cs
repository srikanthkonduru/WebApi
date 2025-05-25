using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

string appConfigConnectionString = builder.Configuration["ConnectionString:connection"];
string endPoint = builder.Configuration["ConnectionString:endPoint"];

//Using Connections string 
//builder.Configuration.AddAzureAppConfiguration(options =>
//{

//	options.Connect(appConfigConnectionString)
//		   .Select("*") // Load all keys
//		   .ConfigureRefresh(refreshOptions =>
//		   {
//			   // Optional: Configure dynamic refresh
//			   refreshOptions.Register("Settings:Sentinel", refreshAll: true)
//							 .SetCacheExpiration(TimeSpan.FromMinutes(5));
//		   });
//});

//Using EndPoints and Azure Default Credentials and added Secret Managet in asp.net core

builder.Configuration.AddAzureAppConfiguration(options =>
{
	var token = new DefaultAzureCredential();
	options.Connect(new Uri(endPoint), token)
		   .Select("*") // Load all keys
		   .ConfigureRefresh(refreshOptions =>
		   {
			   // Optional: Configure dynamic refresh
			   refreshOptions.Register("Settings:Sentinel", refreshAll: true)
							 .SetCacheExpiration(TimeSpan.FromMinutes(5));
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
