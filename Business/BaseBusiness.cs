using Ludo.Database;

namespace Ludo.Business
{
    public class BaseBusiness
    {
        internal LudoDbContext dbContext { get; private set; }
        public LogBusiness logBusiness { get; set; }
        public BaseBusiness(LudoDbContext db)
        {
            dbContext = db;
            this.logBusiness = new LogBusiness(db);
        }
    }
}
