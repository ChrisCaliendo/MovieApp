using MovieApp.Models;

namespace MovieApp.Interfaces
{
    public interface IBingeRepository
    {
        //Read Methods
        ICollection<Binge> GetPublicBinges();
        bool DoesBingeExist(int bingeId);
        bool DoesBingeHaveShow(int bingeId, int showId);
        Binge GetBinge(int bingeId);
        ICollection<Show> GetShowsInBinge(int bingeId);
        ICollection<Tag> GetTagsInBinge(int bingeId);
        int GetBingeTimespan(int bingeId);
        int GetUnknownTimespans(int bingeId);

        //Edit Methods
        bool CreateBinge(Binge binge, int authorId);
        bool AddShowToBinge(int bingeId, int showId);
        bool RemoveShowFromBinge(int bingeId, int showId);
        bool UpdateBinge(Binge binge);
        bool DeleteBinge(Binge binge);
        bool DeleteBinges(List<Binge> binges);
        
        bool Save();
    }
}
