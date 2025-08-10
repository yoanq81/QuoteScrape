using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;

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
                policy.WithOrigins(["http://localhost:4200"]) // Replace with your frontend origin
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
    List<Quote> quotes = [];
    var web = new HtmlWeb();
    // loading the target web page 
    var document = web.Load("https://quotes.toscrape.com");
    // selecting all HTML product elements from the current page 
    var quoteHTMLElements = document.QuerySelectorAll("div.quote");

    foreach (var quoteHTMLElement in quoteHTMLElements)
    {
        // scraping the interesting data from the current HTML element 
        var phrase = HtmlEntity.DeEntitize(quoteHTMLElement.QuerySelector("span.text").InnerText);
        var author = HtmlEntity.DeEntitize(quoteHTMLElement.QuerySelector("small.author").InnerText);
        var tagHTMLElements = quoteHTMLElement.QuerySelectorAll("div.tags > a");
        List<string> tags = [];
        foreach (var tagHTMLElement in tagHTMLElements)
        {
            tags.Add(HtmlEntity.DeEntitize(tagHTMLElement.InnerText));
        }
        // instancing a new Product object 
        var quote = new Quote() { Phrase = phrase, Author = author, Tags = [.. tags] };
        // adding the object containing the scraped data to the list 
        quotes.Add(quote);
    }

    await Task.Yield();
    return TypedResults.Ok(quotes.ToArray());
}
