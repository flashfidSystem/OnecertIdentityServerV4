namespace FlexyBill.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string errorMessage) : base(errorMessage)
        {
            
        }

        public ValidationException(string errorMessage, Exception innerException) : base(errorMessage, innerException)
        {

        }
    }
}
