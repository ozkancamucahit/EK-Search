namespace E_Search.UI.Models;

public enum SortOrderEnum
{
    /// <summary>
    /// Ascending.
    /// </summary>
    Ascending = 1,

    /// <summary>
    /// Descending.
    /// </summary>
    Descending = 2
}

public sealed class SortField
{
    /// <summary>
    /// Gets or sets the Sort Field.
    /// </summary>
    public required string Field { get; set; }

    /// <summary>
    /// Gets or sets the Sort Order.
    /// </summary>
    public required SortOrderEnum Order { get; set; } = SortOrderEnum.Ascending;
}
