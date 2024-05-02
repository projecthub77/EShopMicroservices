
using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

//Registrazione servizi e aggiunta al container
builder.Services.AddSwaggerGen();

builder.Services.AddCarter();

var assembly = typeof(Program).Assembly;

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("connStr")!);
}).UseLightweightSessions();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//Configurazione pipeline richieste http
app.MapCarter();

app.UseExceptionHandler(options => { });

app.UseRouting();

app.Run();


//app.UseExceptionHandler(exc_handler =>              // Gestione eccezioni a livello globale.
//{
//    exc_handler.Run(async context =>
//    {
//        var exc = context.Features.Get<IExceptionHandlerFeature>()?.Error;
//        if (exc == null)
//            return;

//        var problemDetails = new ProblemDetails
//        {
//            Title = exc.Message,
//            Status = StatusCodes.Status500InternalServerError,
//            Detail = exc.StackTrace
//        };

//        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();//
//        logger.LogError(exc, exc.Message);                                          //
//                                                                                    //  Si deve anche registrare l'eccezione per ulteriori analisi e debug;
//        context.Response.StatusCode = StatusCodes.Status500InternalServerError;     //  
//        context.Response.ContentType = "application/problem+json";                  //

//        await context.Response.WriteAsJsonAsync(problemDetails);

//      });
//});