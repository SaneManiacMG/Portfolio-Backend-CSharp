using Portfolio.Backend.Csharp.Configs;
using Portfolio.Backend.Csharp.Interfaces;
using Portfolio.Backend.Csharp.Models.Entities;

namespace Portfolio.Backend.Csharp.Repositories
{
#nullable disable
    public class LoginRepository : ILoginRepository
    {
        private readonly PortfolioDbContext _dbContext;
        public LoginRepository(PortfolioDbContext portfolioDbContext) {
            _dbContext = portfolioDbContext;        
        }

        public async Task<Login> CreateNewUserAsync(Login authentication)
        {
            _dbContext.Add(authentication);
            await _dbContext.SaveChangesAsync();
            return authentication;
        }

        public async Task<Login> DeleteUserAsync(Login authentication)
        {
            _dbContext.LoginDetails.Remove(authentication);
            await _dbContext.SaveChangesAsync();

            return authentication;
        }

        public async Task<Login> GetUserByIdAsync(string userId)
        {
            return await _dbContext.LoginDetails.FindAsync(userId);
        }

        public async Task<Login> UpdateUserAsync(Login authentication)
        {
            authentication.DateModified = DateTime.Now;
            _dbContext.Update(authentication);
            await _dbContext.SaveChangesAsync();

            return authentication;
        }
    }
}
