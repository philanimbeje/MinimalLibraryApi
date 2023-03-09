using Microsoft.EntityFrameworkCore;
using MinimalLibraryApi;
using MinimalLibraryApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();


//books
app.MapGet("/book", async (DataContext context) =>
{
    var resp = await context.Books.ToListAsync();
    return Results.Ok(resp);
});

app.MapGet("/book/{id}", async (DataContext context, int id) =>
{
    var resp = await context.Books.FirstOrDefaultAsync(x => x.Id == id);

    if(resp == null)
    {
        return Results.NotFound("Book not found");
    }

    return Results.Ok(resp);
});

app.MapPost("/book", async (DataContext context, Book data) =>
{
    var author = await context.Authors.FirstOrDefaultAsync(x => x.Id == data.AuthorId);
    if(author  == null)
    {
        return Results.NotFound("Author not found");
    }

    data.Author = author;
    context.Add(data);
    await context.SaveChangesAsync();
    return Results.Ok("success");
});

app.MapPut("/book/{id}", async (DataContext context, Book data, int id) =>
{
    var book = await context.Books.FirstOrDefaultAsync(x => x.Id == id);
    if(book == null)
    {
        return Results.NotFound("book not found");
    }

    book.Title = data.Title;
    await context.SaveChangesAsync();
    return Results.Ok("success");
});

app.MapDelete("/book/{id}", async (DataContext context, int id) =>
{
    var book = await context.Books.FirstOrDefaultAsync(x => x.Id == id);
    if (book == null)
    {
        return Results.NotFound("book not found");
    }

    context.Remove(book);
    await context.SaveChangesAsync();
    return Results.Ok("success");
});




//authors
app.MapGet("/author", async (DataContext context) =>
{
    var resp = await context.Authors.ToListAsync();
    return Results.Ok(resp);
});

app.MapGet("/author/{id}", async(DataContext context, int id) =>
{
    var resp = await context.Authors.FirstOrDefaultAsync(x => x.Id == id);
    return Results.Ok(resp);
});

app.MapPost("/author", async(DataContext context, Author data) =>
{
    context.Add(data);
    await context.SaveChangesAsync();
    return Results.Ok("success");
});

app.MapPut("/author/{id}", async(DataContext context, Author data, int id) =>
{
    var author = await context.Authors.FirstOrDefaultAsync(x => x.Id == id);
    if (author == null)
    {
        return Results.NotFound("author not found");
    }

    author.Name = data.Name;
    author.Surname = data.Surname;
    await context.SaveChangesAsync();
    return Results.Ok("success");
});

app.MapDelete("/author/{id}", async(DataContext context, int id) =>
{
    var author = await context.Authors.FirstOrDefaultAsync(x => x.Id == id);
    if (author == null)
    {
        return Results.NotFound("author not found");
    }

    context.Remove(author);
    await context.SaveChangesAsync();
    return Results.Ok("success");
});

app.Run();
