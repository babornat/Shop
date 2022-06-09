﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shop.Api.Data;
using Shop.Shared;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ShopDbContext>(
    opt => opt.UseSqlite("FileName=Shop.db")
    );
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(corsOptions => corsOptions.AddDefaultPolicy(policy =>
    policy.WithOrigins("https://localhost:7132")
    .WithMethods("GET", "POST", "PUT", "DELETE")
    .AllowAnyHeader()
));

var app = builder.Build();
app.UseCors();

app.MapGet("/", () => "Hellou");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/revize/{vyhledavanyRetezec}", (string vyhledavanyRetezec, ShopDbContext db, IMapper mapper) =>
{
    if (string.IsNullOrWhiteSpace(vyhledavanyRetezec)) return Results.Problem("Parametr musi byt neprazdny");
    var kdeJeRetezec = db.Revizes.Where(x => x.Name.Contains(vyhledavanyRetezec));
    return Results.Json(kdeJeRetezec);
});

app.MapPost("/revize", (RevizeModel prichoziModel,
    ShopDbContext db, IMapper mapper) =>
{
    prichoziModel.Id = Guid.Empty;//vynuluju id, db si idčka ošéfuje sama
    Revize ent = mapper.Map<Revize>(prichoziModel);//mapovaná na "databázový" typ
    db.Revizes.Add(ent);//přidání do db
    db.SaveChanges();//uložení db (v tuto chvíli se vytvoří id)

    return Results.Created("/revize", ent.Id);
});

app.MapGet("/vybaveni", (ShopDbContext db, IMapper mapper) =>
{

    var ents = db.Vybavenis.Include(x => x.Revizes);

    List<VybaveniModel> models = new();
    foreach (var ent in ents)
    {
        var vybModel = mapper.Map<VybaveniModel>(ent);
        vybModel.LastRevision = ent.Revizes.OrderByDescending(x => x.DateTime).FirstOrDefault()?.DateTime;
        models.Add(vybModel);
    }
    return Results.Json(models);
});

app.MapGet("/vybaveni/jensrevizi", (int c) =>
{
    //return seznam.Where(x=>!x.NeedsRevision);
});



app.MapGet("/vybaveni/{Id}", (Guid Id, ShopDbContext db, IMapper mapper) =>
 {
     var item = db.Vybavenis.Include(x=>x.Revizes).SingleOrDefault(x => x.Id == Id);
     if (item == null) return Results.NotFound("takováto entita neexistuje");
     return Results.Json(mapper.Map<VybaveniSRevizemaModel>(item));
 });

app.MapPost("/vybaveni", (VybaveniModel prichoziModel,
    ShopDbContext db, IMapper mapper) =>
{
    prichoziModel.Id = Guid.Empty;//vynuluju id, db si idčka ošéfuje sama
    Vybaveni ent = mapper.Map<Vybaveni>(prichoziModel);//mapovaná na "databázový" typ
    db.Vybavenis.Add(ent);//přidání do db
    db.SaveChanges();//uložení db (v tuto chvíli se vytvoří id)

    return Results.Created("/vybaveni", ent.Id);
});

app.MapPut("/vybaveni", (VybaveniModel prichoziModel, ShopDbContext db, IMapper mapper) =>
{
    var staryZaznam = db.Vybavenis.SingleOrDefault(x => x.Id == prichoziModel.Id);
    if (staryZaznam == null) return Results.NotFound("Tento záznam není v seznamu");
    mapper.Map(prichoziModel, staryZaznam);
    db.SaveChanges();
    return Results.Ok();
});


app.MapDelete("/vybaveni/{Id}", (Guid Id, ShopDbContext db, IMapper mapper) =>
 {
     var item = db.Vybavenis.SingleOrDefault(x => x.Id == Id);
     if (item == null)
         return Results.NotFound("Tato položka nebyla nalezena!!");
     db.Remove(item);
     db.SaveChanges();
     return Results.Ok();
 }
);


app.Run();

//record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
//{
//    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
//}