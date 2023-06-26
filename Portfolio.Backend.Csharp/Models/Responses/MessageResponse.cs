namespace Portfolio.Backend.Csharp.Models.Responses
{
    public class MessageResponse
    {
        public MessageResponse(string response)
        {
            Response = response;
        }

        public string Response { get; set; }
    }
}
