using System.ComponentModel.DataAnnotations;

namespace E_Search.UI.Models;

public sealed class NewsEntity
{
    public int ID { get; set; }
    public string Title { get; set; }
    public string TitleDisplay => Title.Length < 10 ? Title : Title.Substring(0, 20);
    public string Agency { get; set; }


    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; }

    public string Summary { get; set; }
    public string SummaryDisplay => Summary.Length < 20 ? Summary: Summary.Substring(0, 20);

    public string MainImageURL { get; set; }
    public string ArticleBody { get; set; }
    public string ArticleBodyDisplay => ArticleBody.Length < 30 ? ArticleBody: ArticleBody.Substring(0, 30);



}
