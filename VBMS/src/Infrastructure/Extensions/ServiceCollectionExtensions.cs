using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using VBMS.Application.Interfaces.Repositories;
using VBMS.Application.Interfaces.Services.Storage;
using VBMS.Application.Interfaces.Services.Storage.Provider;
using VBMS.Application.Interfaces.Serialization.Serializers;
using VBMS.Application.Serialization.JsonConverters;
using VBMS.Infrastructure.Repositories;
using VBMS.Infrastructure.Services.Storage;
using VBMS.Application.Serialization.Options;
using VBMS.Infrastructure.Services.Storage.Provider;
using VBMS.Application.Serialization.Serializers;

namespace VBMS.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructureMappings(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddTransient(typeof(IRepositoryAsync<,>), typeof(RepositoryAsync<,>))
                .AddTransient<IPartnerRepository, PartnerRepository>()
                .AddTransient<IPartnerTypeRepository, PartnerTypeRepository>()
                .AddTransient<IVehicleRepository, VehicleRepository>()
                .AddTransient<IVehicleTypeRepository, VehicleTypeRepository>()
                .AddTransient<IProductTestRepository, ProductTestRepository>()
                .AddTransient<IBrandTestRepository, BrandTestRepository>()
                .AddTransient<IDocumentRepository, DocumentRepository>()
                .AddTransient<IDocumentTypeRepository, DocumentTypeRepository>()
                .AddTransient(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        }

        public static IServiceCollection AddExtendedAttributesUnitOfWork(this IServiceCollection services)
        {
            return services
                .AddTransient(typeof(IExtendedAttributeUnitOfWork<,,>), typeof(ExtendedAttributeUnitOfWork<,,>));
        }

        public static IServiceCollection AddServerStorage(this IServiceCollection services)
            => AddServerStorage(services, null);

        public static IServiceCollection AddServerStorage(this IServiceCollection services, Action<SystemTextJsonOptions> configure)
        {
            return services
                .AddScoped<IJsonSerializer, SystemTextJsonSerializer>()
                .AddScoped<IStorageProvider, ServerStorageProvider>()
                .AddScoped<IServerStorageService, ServerStorageService>()
                .AddScoped<ISyncServerStorageService, ServerStorageService>()
                .Configure<SystemTextJsonOptions>(configureOptions =>
                {
                    configure?.Invoke(configureOptions);
                    if (!configureOptions.JsonSerializerOptions.Converters.Any(c => c.GetType() == typeof(TimespanJsonConverter)))
                        configureOptions.JsonSerializerOptions.Converters.Add(new TimespanJsonConverter());
                });
        }
    }
}