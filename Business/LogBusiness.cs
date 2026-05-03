using Ludo.Database;
using Ludo.Models;

namespace Ludo.Business
{
    public class LogBusiness
    {
        private LudoDbContext dbContext { get; set; }
        public LogBusiness(LudoDbContext db)
        {
            dbContext = db;
        }

        public void Add(Log log)
        {
            dbContext.Add(log);
            dbContext.SaveChanges();
        }
    }
}
