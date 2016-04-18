using System;
using System.Diagnostics;
using System.Linq;
using CannedBytes.CommandLineGui.Schema;
using CannedBytes.CommandLineGui.Schema.Version1;

namespace CannedBytes.CommandLineGui
{
    class ToolExecutionHost : IDisposable
    {
        private ITextOutput _output;
        private readonly Process _process = new Process();
        private readonly ToolInfo _tool;

        public ToolExecutionHost(ToolInfo tool, ITextOutput ouput)
        {
            _tool = tool;
            _output = ouput;

            AutoDispose = true;
        }

        public bool AutoDispose { get; set; }

        public void BeginExecute(string commandline)
        {
            string executable = GetExecutablePath();

            _process.StartInfo = CreateStartInfo(executable, commandline);
            _process.EnableRaisingEvents = true;
            _process.OutputDataReceived += new DataReceivedEventHandler(Process_OutputDataReceived);
            _process.ErrorDataReceived += new DataReceivedEventHandler(Process_ErrorDataReceived);
            _process.Exited += new System.EventHandler(Process_Exited);

            _output.Write(executable + " " + commandline, false);
            _output.Write(string.Empty, false);

            if (_process.Start())
            {
                _process.BeginOutputReadLine();
                _process.BeginErrorReadLine();
            }
            else
            {
                _output.Write("Process could not be started.", true);
            }
        }

        private string GetExecutablePath()
        {
            return Environment.ExpandEnvironmentVariables(_tool.ToolExecutablePath);
        }

        private string GetExitCodeMessage()
        {
            int code = _process.ExitCode;

            ExitCode exitCode = null;

            if (_tool.Tool.ExitCodes != null)
            {
                exitCode = (from ec in _tool.Tool.ExitCodes
                            where ec.Value == code
                            select ec).FirstOrDefault();
            }

            var msg = "Exit Code: ";

            if (exitCode != null)
            {
                msg = string.Format("{0}{1} ({2})", msg, exitCode.Description, code);
            }
            else
            {
                msg += code.ToString();
            }

            return msg;
        }

        void Process_Exited(object sender, System.EventArgs e)
        {
            _output.Write(string.Empty, false);

            _output.Write(GetExitCodeMessage(), false);

            if (AutoDispose)
            {
                Dispose();
            }
        }

        void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            _output.Write(e.Data, true);
        }

        void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            _output.Write(e.Data, false);
        }

        private ProcessStartInfo CreateStartInfo(string executable, string commandline)
        {
            var startInfo = new ProcessStartInfo(executable, commandline);

            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.CreateNoWindow = true;

            return startInfo;
        }

        public void Dispose()
        {
            _process.Dispose();
        }
    }
}