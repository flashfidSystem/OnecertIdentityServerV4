namespace OnecertApiV1.Exceptions
{
    public class CustomValidationException : Exception
    {
        public CustomValidationException(string errorMessage) : base(errorMessage)
        {

        }

        public CustomValidationException(string errorMessage, Exception innerException) : base(errorMessage, innerException)
        {

        }
    }
}
