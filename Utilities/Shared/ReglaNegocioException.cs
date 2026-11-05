namespace Utilities.Shared
{
    public class ReglaNegocioException : Exception
    {
        public ReglaNegocioException(string message, Exception? innerException = null)
            : base(message, innerException)
        {
        }
    }
}
