using System;

namespace PsTest
{
    /// <summary>
    /// Represents a color formatter which formats colors
    /// for a PowerShell host.
    /// </summary>
    public class PowerShellColorFormatter : IColorFormatter
    {
        private readonly IRunspace _runspace;

        /// <summary>
        /// Get the saved foreground color.
        /// </summary>
        public virtual string SavedForegroundColor { get; private set; }

        /// <summary>
        /// Get the saved background color.
        /// </summary>
        public virtual string SavedBackgroundColor { get; private set; }

        /// <summary>
        /// Initializes a new instance of the PowerShell color formatter
        /// with the specified runtime.
        /// </summary>
        /// <param name="runspace">
        /// The runtime to use for formatting instructions.
        /// </param>
        public PowerShellColorFormatter(IRunspace runspace)
        {
            if (runspace == null)
            {
                throw new ArgumentNullException("runspace");
            }
            _runspace = runspace;
        }

        /// <summary>
        /// Get the runspace to use for formatting instructions.
        /// </summary>
        internal IRunspace Runspace { get { return _runspace; } }

        private string GetBackgroundColor()
        {
            const string command = "[string]$Host.UI.RawUI.BackgroundColor";
            return (string)Runspace
                .CreateNestedPipeline(command, false).Invoke()[0]
                .BaseObject;
        }

        private string GetForegroundColor()
        {
            const string command = "[string]$Host.UI.RawUI.ForegroundColor";
            return (string)Runspace
                .CreateNestedPipeline(command, false).Invoke()[0]
                .BaseObject;
        }

        /// <summary>
        /// Sets the foreground and background color to the specified parameters.
        /// </summary>
        /// <param name="foregroundColor">
        /// The foreground color.
        /// </param>
        /// <param name="backgroundColor">
        /// The background color.
        /// </param>
        public void SetColor(string foregroundColor, string backgroundColor)
        {
            var fgCommand = string.Format(
                "$Host.UI.RawUI.ForegroundColor = '{0}'",
                foregroundColor
            );
            Runspace.CreateNestedPipeline(fgCommand, false).Invoke();

            var bgCommand = string.Format(
                "$Host.UI.RawUI.BackgroundColor = '{0}'",
                backgroundColor
            );
            Runspace.CreateNestedPipeline(bgCommand, false).Invoke();
        }

        /// <summary>
        /// Saves the current foreground and background color for a future call to ResetColor.
        /// </summary>
        public void SaveColor()
        {
            SavedForegroundColor = GetForegroundColor();
            SavedBackgroundColor = GetBackgroundColor();
        }

        /// <summary>
        /// Reset the foreground and background color to their saved values.
        /// </summary>
        public void ResetColor()
        {
            SetColor(SavedForegroundColor, SavedBackgroundColor);
        }
    }
}
