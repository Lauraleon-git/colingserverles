using Coling.API.Curriculum.Contratos.Repositorio;
using Coling.API.Curriculum.Implementacion.Repositorio;
using Coling.API.Curriculum.Modelo;
using Coling.Shared;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddScoped<IInstitucionRepositorio, InstitucionRepositorio>();
        services.AddScoped<IProfesionRepositorio, ProfesionRepositorio>();
        services.AddScoped<IEstudiosRepositorio, EstudiosRepositorio>();
        services.AddScoped<IExperienciaLaboralRepositorio, ExperienciaLaboralRepositorio>();
       
    })

    .Build();

host.Run();
