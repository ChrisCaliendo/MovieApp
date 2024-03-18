using MovieApp.Interfaces;
using MovieApp.Models;

namespace MovieApp.Repositories
{
    public class BingeRepository : IBingeRepository
    {
        public bool doesBingeExist(int bingeId)
        {
            throw new NotImplementedException();
        }

        public Binge GetBinge(int bingeId)
        {
            throw new NotImplementedException();
        }

        public int GetBingeTimespan(int bingeId)
        {
            throw new NotImplementedException();
        }

        public ICollection<Tag> GetPublicBinges()
        {
            throw new NotImplementedException();
        }

        public ICollection<Show> GetShowsInBinge(int bingeId)
        {
            throw new NotImplementedException();
        }

        public bool GetUnknownTimespans(int bingeId)
        {
            throw new NotImplementedException();
        }
    }
}
