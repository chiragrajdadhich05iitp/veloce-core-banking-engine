using CoreLedger.Models;
using CoreLedger.Services;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connection string fallback
var connectionString = "mongodb://localhost:27017";
var mongoClient = new MongoClient(connectionString);
var mongoDatabase = mongoClient.GetDatabase("CoreLedger");

// Direct service instantiation
builder.Services.AddSingleton<IAccountsService>(new AccountsService(mongoDatabase));
builder.Services.AddSingleton<IAuthService>(new AuthService(mongoDatabase));
builder.Services.AddSingleton<IGoalsService>(new GoalsService(mongoDatabase));
builder.Services.AddSingleton<ITagsService>(new TagsService(mongoDatabase));
builder.Services.AddSingleton<ITransactionsService>(new TransactionsService(mongoDatabase));
builder.Services.AddSingleton<IUsersService>(new UsersService(mongoDatabase));

builder.Services.AddCors();

var app = builder.Build();

app.UseCors(policy => policy
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();