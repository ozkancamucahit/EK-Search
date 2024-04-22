namespace E_Search.UI.Models;

public sealed class HighlightedContent
{
    /// <summary>
    /// Gets or sets the line number.
    /// </summary>
    public int LineNo { get; set; }

    /// <summary>
    /// Gets or sets the line content.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the flag, if this line needs to be highlighted.
    /// </summary>
    public bool IsHighlight { get; set; }
}
