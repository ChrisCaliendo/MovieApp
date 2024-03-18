namespace MovieApp.Interfaces
{
    public interface IUserRepository
    {
        
        bool doesUserExist(string username);

        bool isEmailConfirmed(int userId);


    }
}
