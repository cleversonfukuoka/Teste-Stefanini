namespace Questao5.Application
{
    public class BaseResponse
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public ErrorType ErrorType { get; set; }
    }
}
