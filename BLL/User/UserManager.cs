namespace BLL;
using BOL;
using DAL;

public static class UserManager{
    public static User ValidateUser(string username, string password){
        return DBManager.IsUserPresent(username, password);
    }
   public static bool RegisterUser(User userData){
        return DBManager.RegisterUser(userData);
    }

    public static int AddUserRequirement(int userId, string skills, string wages, string address, string date)
    {
        return DBManager.InsertUserRequirement(userId, skills, wages, address, date);
    }
    public static bool AddFeedback(Feedback feedback)
    {
        return DBManager.GiveFeedback(feedback);
    }

}
