namespace CannedBytes.CommandLineGui
{
    interface ITextOutput
    {
        void Write(string text, bool isError);
    }
}