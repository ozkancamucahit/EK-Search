using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using E_Search.UI.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nest;
using System.Text.Json;

namespace E_Search.UI.Pages.News;

public class IndexModel : PageModel
{
    #region FIELDS
    private readonly IElasticClient elasticClient;

    [BindProperty(SupportsGet = true)]
    public string? Title { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Summary { get; set; } 
    
    [BindProperty(SupportsGet = true)]
    public string? ArticleBody { get; set; } 
    #endregion

    public IndexModel(IElasticClient elasticClient)
    {
        this.elasticClient = elasticClient;
    }

    public IList<NewsEntity> News { get; set; } =  new List<NewsEntity>();


    public async Task OnPostAsync()
    {
        if(!String.IsNullOrWhiteSpace(ArticleBody))
            await DataSearch(ArticleBody);

    }

    private async Task DataSearch(string keyword)
    {
        var results = await elasticClient.SearchAsync<NewsEntity>(
            sd =>
            sd
            .Query(
                qc => qc.MultiMatch(
                    md =>
                    md
                    .Fields(fd => 
                        fd.Field(news => news.ArticleBody)
                        .Field(news => news.Title)
                        .Field(news => news.Summary)
                    )
                    .Query(ArticleBody)
                )
            )
            .Size(100)
        );

        News = (from hits in results.Hits
                        select hits.Source).ToList();
    }


}
