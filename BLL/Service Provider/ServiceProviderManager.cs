namespace BLL;
using BOL;
using DAL;

public static class ServiceProviderManager{

    public static ServiceProvider ValidateServiceProvider(string username,string password){
        return DBManager.ValidateServiceProvider(username,password);
    }
    public static bool RegisterServiceProvider(ServiceProvider serviceProvider){
        return DBManager.RegisterServiceProvider(serviceProvider);
    }

    public static List<UserRequirementWithUserData> GetUserRequirements(string skills){
        return DBManager.GetUserRequirementsWithUserInfo(skills);
    }
    public static ServiceProvider GetServiceProviderById(int ServiceProviderID){
        return DBManager.GetServiceProviderByID(ServiceProviderID);
    }

    public static List<ServiceProviderViewStatus> GetServiceProviderData(string serviceProviderUsername){
        return DBManager.GetServiceProviderData(serviceProviderUsername);
    }
}
