namespace Portfolio.Backend.Csharp.Models.Responses
{
    public class LoginResponse
    {
        public LoginResponse(string response)
        {
            Response = response;
        }

        public string Response { get; set; }
    }
}
