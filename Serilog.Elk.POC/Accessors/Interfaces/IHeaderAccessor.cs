namespace Serilog.Elk.POC.Accessors.Interfaces
{
    public interface IHeaderAccessor
    {
        /// <summary>
        /// Gets a value in a header in context.
        /// </summary>
        /// <param name="key">Key used to find a value in context header.</param>
        string GetValue(string key);
    }
}
