using Portfolio.Backend.Csharp.Interfaces;

namespace Portfolio.Backend.Csharp.Services
{
    public class SequenceGenerator : ISequenceGenerator
    {
        public string UserIdSequenceGenerator()
        {
            string formattedUserId = DateTimeFormatter() + RandomNumbers();
            return formattedUserId;
        }

        private string RandomNumbers()
        {
            Random random = new Random();
            int randomNumber = random.Next(1000, 9999);
            return randomNumber.ToString();
        }

        private string DateTimeFormatter()
        {
            string DateTimePattern = "yyMMddhhmmss";
            DateTime dateTime = DateTime.Now;
            return dateTime.ToString(DateTimePattern);
        }
    }
}
