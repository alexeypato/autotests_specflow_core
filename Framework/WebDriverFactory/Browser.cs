using System.ComponentModel;

namespace Framework.WebDriverFactory
{
    public enum Browser
    {
        [Description("Chrome")]
        Chrome,
        [Description("Edge")]
        Edge,
        [Description("Firefox")]
        Firefox,
        [Description("IE")]
        IE,
        [Description("Safari")]
        Safari
    }
}
