namespace OnecertApiV1.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string errorMessage) : base(errorMessage)
        {

        }

        public NotFoundException(string errorMessage, Exception innerException) : base(errorMessage, innerException)
        {

        }
    }
}
