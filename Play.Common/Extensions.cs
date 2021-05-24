using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Common.IRepository;
using Play.Catalog.Service.Settings;
using Play.Common.Entities;
using Play.Common.Repository;
using Newtonsoft.Json;

namespace Play.Common
{
    public static class Extensions
    {
        public static IServiceCollection AddMongo(this IServiceCollection services)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));


            services.AddSingleton(serviceProvider =>
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                var serviceSettings = JsonConvert.DeserializeObject<ServiceSettings>(configuration.GetSection(nameof(ServiceSettings)).Value.ToJson());
                var mongodbSettigns = JsonConvert.DeserializeObject<MongoDbSettings>(configuration.GetSection(nameof(MongoDbSettings)).Value.ToJson());
                var mongoClient = new MongoClient(mongodbSettigns.ConnectionString);
                return mongoClient.GetDatabase(serviceSettings.ServiceName);
            });

            return services;
        }

        public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName) where T : IEntity
        {
            services.AddTransient<IMongoRepository<T>, MongoRepository<T>>(serviceProvider =>
            {
                var database = serviceProvider.GetService<IMongoDatabase>();
                return new MongoRepository<T>(database, collectionName);
            });

            return services;
        }
    }
}
