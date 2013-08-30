namespace PsTest
{
    /// <summary>
    /// Represents a color formatter which formats colors.
    /// </summary>
    public interface IColorFormatter
    {
        /// <summary>
        /// Sets the foreground and background color to the specified parameters.
        /// </summary>
        /// <param name="foregroundColor">
        /// The foreground color.
        /// </param>
        /// <param name="backgroundColor">
        /// The background color.
        /// </param>
        void SetColor(string foregroundColor, string backgroundColor);

        /// <summary>
        /// Saves the current foreground and background color for a future call to ResetColor.
        /// </summary>
        void SaveColor();

        /// <summary>
        /// Reset the foreground and background color to their saved values.
        /// </summary>
        void ResetColor();
    }
}