using Coling.API.Curriculum.Contratos.Repositorio;
using Coling.API.Curriculum.Implementacion.Repositorio;
using Coling.API.Curriculum.Modelo;
using Coling.Shared;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddTransient<IInstitucionRepositorio, InstitucionRepositorio>();
        services.AddTransient<IProfesionRepositorio, ProfesionRepositorio>();
        services.AddTransient<IEstudiosRepositorio, EstudiosRepositorio>();
        services.AddTransient<IExperienciaLaboralRepositorio, ExperienciaLaboralRepositorio>();

    })
    .Build();

host.Run();
