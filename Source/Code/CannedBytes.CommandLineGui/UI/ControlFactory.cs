using System.Windows.Forms;

namespace CannedBytes.CommandLineGui.UI
{
    /// <summary>
    /// A static factory class for creating common (non-wpf) controls.
    /// </summary>
    static class ControlFactory
    {
        /// <summary>
        /// Creates a new instance of the <see cref="FolderBrowserDialog"/>.
        /// </summary>
        /// <param name="title">The title of the dialog.</param>
        /// <returns>Never returns null.</returns>
        public static FolderBrowserDialog CreateFolderBrowserDialog(string title)
        {
            var fbd = new FolderBrowserDialog();

            fbd.Description = title;
            fbd.ShowNewFolderButton = true;

            return fbd;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="SaveFileDialog"/>.
        /// </summary>
        /// <param name="title">The title of the dialog.</param>
        /// <param name="fileFilter">A specification of the file filters to use.</param>
        /// <returns>Never returns null.</returns>
        public static SaveFileDialog CreateSaveFileDialog(string title, string fileFilter)
        {
            var sfd = new SaveFileDialog();

            sfd.AddExtension = true;
            sfd.AutoUpgradeEnabled = true;
            sfd.CheckFileExists = false;
            sfd.CheckPathExists = true;
            sfd.DereferenceLinks = true;
            sfd.Filter = fileFilter;
            sfd.FilterIndex = 0;
            //sfd.InitialDirectory = Environment.CurrentDirectory;
            sfd.RestoreDirectory = false;
            sfd.Title = title;
            sfd.ValidateNames = true;
            sfd.OverwritePrompt = true;

            return sfd;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OpenFileDialog"/>.
        /// </summary>
        /// <param name="title">The title of the dialog.</param>
        /// <param name="fileFilter">A specification of the file filters to use.</param>
        /// <returns>Never returns null.</returns>
        public static OpenFileDialog CreateOpenFileDialog(string title, string fileFilter)
        {
            var ofd = new OpenFileDialog();

            ofd.AddExtension = true;
            ofd.AutoUpgradeEnabled = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.DereferenceLinks = true;
            ofd.Filter = fileFilter;
            ofd.FilterIndex = 0;
            //ofd.InitialDirectory = Environment.CurrentDirectory;
            ofd.Multiselect = false;
            ofd.RestoreDirectory = false;
            ofd.Title = title;
            ofd.ValidateNames = true;

            return ofd;
        }
    }
}