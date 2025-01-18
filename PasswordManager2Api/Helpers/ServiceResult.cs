namespace PasswordManager2Api.Helpers
{
    public class ServiceResult
    {
        public bool IsSuccess { get; }
        public string Message { get; }

        public ServiceResult(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
    }
}
