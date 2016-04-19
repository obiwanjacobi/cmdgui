using System;

namespace CannedBytes.CommandLineGui.Model
{
    class HelpInfo
    {
        public HelpInfo(string helpCmd, string helpUrl)
        {
            this.HelpCmd = helpCmd;
            this.HelpUrl = helpUrl;

            IsHelpCmdEmpty = String.IsNullOrEmpty(helpCmd);
            IsHelpUrlEmpty = String.IsNullOrEmpty(helpUrl);
            IsEmpty = (IsHelpCmdEmpty && IsHelpUrlEmpty);
        }

        public string HelpCmd { get; set; }

        public string HelpUrl { get; set; }

        public bool IsHelpCmdEmpty { get; set; }

        public bool IsHelpUrlEmpty { get; set; }

        public bool IsEmpty { get; set; }
    }
}