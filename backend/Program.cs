using Bogus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "QuoteAPI";
    config.Title = "QuoteAPI v1";
    config.Version = "v1";
});

builder.Services.AddCors(options =>
    {
        options.AddPolicy("MyCorsPolicy",
            policy =>
            {
                policy.WithOrigins("http://localhost:4200") // Replace with your frontend origin
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
    });

var app = builder.Build();

app.UseCors("MyCorsPolicy"); // Apply the defined CORS policy
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

var todoItems = app.MapGroup("/api/quotes");
todoItems.MapGet("/", GetAllQuotes);
app.Run();

static async Task<IResult> GetAllQuotes()
{
    var quoteFaker = new Faker<Quote>()
            // Define rules for each property
            .RuleFor(u => u.Phrase, f => f.Lorem.Sentence())
            .RuleFor(u => u.Author, f => f.Name.FullName())
            .RuleFor(u => u.Tags, f => f.Lorem.Words(f.Random.Number(2, 5)));

    // Generate an array of 10 fake User objects
    Quote[] fakeQuotes = quoteFaker.Generate(10).ToArray();
    await Task.Yield();
    return TypedResults.Ok(fakeQuotes);
}
