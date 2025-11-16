using Application.Interfaces.IEvenSeat;
using Application.Interfaces.IEventSector;
using Application.Interfaces.ITicket;
using Application.Interfaces.ITicketSeat;
using Application.Interfaces.ITicketSector;
using Application.Interfaces.ITicketStatus;
using Infrastructure.Commands;
using Infrastructure.Data;
using Infrastructure.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infrastructure.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
       public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Base de datos
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // MediatR
            var applicationAssembly = Assembly.Load("Application");
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

            // servicios de dominio
            services.AddScoped<ITicketCommand, TicketCommand>();
            services.AddScoped<ITicketQuery, TicketQuery>();
            services.AddScoped<ITicketStatusQuery, TicketStatusQuery>();

            services.AddScoped<IEventSeatCommand, EventSeatCommand>();
            services.AddScoped<IEventSeatQuery, EventSeatQuery>();

            services.AddScoped<IEventSectorQuery, EventSectorQuery>();
            services.AddScoped<IEventSectorCommand, EventSectorCommand>();

            services.AddScoped<ITicketSeatCommand, TicketSeatCommand>();
            services.AddScoped<ITicketSeatQuery, TicketSeatQuery>();

            services.AddScoped<ITicketSectorCommand, TicketSectorCommand>();
            services.AddScoped<ITicketSectorQuery, TicketSectorQuery>();


            // cliente HTTP, conexión con servicio externo

            return services;
        }
    }
}
