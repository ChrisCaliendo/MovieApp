﻿using MovieApp.Models;

namespace MovieApp.Interfaces
{
    public interface ITagRepository
    {
        //Read Methods

        ICollection<Tag> GetTags();
        Tag GetTag(int id);
        Tag GetTag(string name);
        bool DoesTagExist(int id);
        bool DoesTagExist(string name);
        ICollection<Show> GetShowsWithTag(int tagId);
        
        //Edit Methods
        bool CreateTag(Tag tag);
        bool UpdateTag(Tag tag);
        bool DeleteTag(Tag tag);
        bool RemoveTagFromAllShows(int tagId);
        bool Save();

    }
}
