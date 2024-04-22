using E_Search.UI.Models;
using Nest;

namespace E_Search.UI.Extensions;

public static class ElasticSearchExtensions
{
    public static void AddElasticSearch(this IServiceCollection services,
                                        IConfiguration configuration)
    {

        var url = configuration["ELKConfiguration:Uri"] ?? 
            throw new ArgumentNullException(nameof(configuration), "ELASTIC URL Not found");

        var defaultIndex = configuration["ELKConfiguration:index"];

        var settings = new ConnectionSettings(new Uri(url))
            .PrettyJson()
            .DefaultIndex(defaultIndex);

        AddDefaultMappings(settings);

        var client = new ElasticClient(settings);
        services.AddSingleton<IElasticClient>(client);
        CreateIndex(client, defaultIndex);


    }

    private static void AddDefaultMappings(ConnectionSettings settings)
    {
        settings.DefaultMappingFor<NewsEntity>(nd =>
         nd
         .Ignore(n => n.Agency)
         //.Ignore(n => n.Date)
         .Ignore(n => n.MainImageURL)


        );
    }
    private static void CreateIndex(IElasticClient client, string indexName)
    {
        client
            .Indices
            .Create(indexName, i => i.Map<NewsEntity>(x => x.AutoMap()));
    }



}
