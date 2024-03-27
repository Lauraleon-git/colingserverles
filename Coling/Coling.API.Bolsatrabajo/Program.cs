using Coling.API.Bolsatrabajo.Contratos.Repositorio;
using Coling.API.Bolsatrabajo.Implementacion.Repositorio;
using Coling.API.Bolsatrabajo.Repositorio;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        services.AddScoped<ISolicitudRepositorio,SolicitudRepositorio>();
        services.AddScoped<IOfertaLaboralRepositorio, OfertaLaboralRepositorio>();
    })
    .Build();

host.Run();
