namespace HospitalAPI.Hospital.Domain
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message) { }

    }
}
