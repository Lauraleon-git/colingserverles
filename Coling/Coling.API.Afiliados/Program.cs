using Coling.API.Afiliados;
using Coling.API.Afiliados.Contratos;
using Coling.API.Afiliados.Implementacion;
using Coling.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        var configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
           .AddEnvironmentVariables()
           .Build();
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddDbContext<Contexto>(options => options.UseSqlServer(
                     configuration.GetConnectionString("cadenaConexion")));
        services.AddTransient<IPersonaLogic, PersonaLogic>();
        services.AddTransient<ITipoSocialLogic, TipoSocialLogic>();
        services.AddTransient<IIdiomaLogic, IdiomaLogic>();
        services.AddTransient<IAfiliadoLogic,AfiliadoLogic>();
        services.AddTransient<IAfiliadoIdiomaLogic, AfiliadoIdiomaLogic>();
        services.AddTransient<IDireccionLogic, DireccionLogic>();
        services.AddTransient<ITelefonoLogic, TelefonoLogic>();
        services.AddTransient<IPersonaTipoSocialLogic, PersonaTipoSocialLogic>();
        services.AddTransient<IProfesionAfiliadoLogic, ProfesionAfiliadoLogic>();
    })
    .Build();

host.Run();
