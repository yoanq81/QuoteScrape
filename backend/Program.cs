var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "QuoteAPI";
    config.Title = "QuoteAPI v1";
    config.Version = "v1";
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "QuoteAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

app.MapGet("/", () => "Hello World!");

var todoItems = app.MapGroup("/quotes");
todoItems.MapGet("/", GetAllQuotes);
app.Run();

// <snippet_handlers>
// <snippet_getalltodos>
static async Task<IResult> GetAllQuotes()
{
    await Task.Yield();
    Quote[] result = [];
    return TypedResults.Ok(result);
}

