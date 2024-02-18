namespace BOL;
public class User{
    public int UserID{get;set;}
    public string NameFirst{get;set;}
    public string NameLast{get;set;}
    public string Username{get;set;}
    public string Password{get;set;}
    public string PhoneNumber{get;set;}
    public string Address{get;set;}
   
    
    public User(){
        Console.WriteLine("Default constructor");
    }
    public User(string namefirst,string namelast,string username,string password,string phonenumber, string address){
        this.NameFirst=namefirst;
        this.NameLast=namelast;
        this.Username=username;
        this.Password=password;
        this.PhoneNumber=phonenumber;
        this.Address=address;
    }
}