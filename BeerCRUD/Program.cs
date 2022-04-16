using BeerCRUD.Data;
using BeerCRUD.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDB>(options => options.UseInMemoryDatabase("BeerLog"));
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
var serviceScopeFactory = app.Services.GetService<IServiceScopeFactory>();
using (var scope = serviceScopeFactory.CreateScope())
{
    var context = (ApplicationDB)scope.ServiceProvider.GetService(typeof(ApplicationDB));
    AddInitialData(context);
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


static async void AddInitialData(ApplicationDB _dbContext)
{
    var beerList = new List<Beer>()
    {
        new Beer()
        {
            Name = "Alfa",
            Type = "Blonde"
        },
        new Beer()
        {
            Name = "Paulaner",
            Type = "Weiss"
        },
        new Beer()
        {
            Name = "Kaizer",
            Type = "Pils"
        },
    };
    _dbContext.Beers.AddRange(beerList);
    await _dbContext.SaveChangesAsync();
}