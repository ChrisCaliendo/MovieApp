using MovieApp.Models;

namespace MovieApp.Interfaces
{
    public interface IBingeRepository
    {
        //Read Methods
        ICollection<Binge> GetPublicBinges();
        bool doesBingeExist(int bingeId);
        Binge GetBinge(int bingeId);
        ICollection<Show> GetShowsInBinge(int bingeId);
        int GetBingeTimespan(int bingeId);
        int GetUnknownTimespans(int bingeId);

        //Edit Methods
        bool CreateBinge(Binge binge);
        bool Save();
    }
}
