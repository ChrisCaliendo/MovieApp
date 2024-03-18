using MovieApp.Models;

namespace MovieApp.Interfaces
{
    public interface IBingeRepository
    {
        ICollection<Tag> GetPublicBinges();
        bool doesBingeExist(int bingeId);
        Binge GetBinge(int bingeId);
        ICollection<Show> GetShowsInBinge(int bingeId);
        int GetBingeTimespan(int bingeId);
        bool GetUnknownTimespans(int bingeId);
    }
}
