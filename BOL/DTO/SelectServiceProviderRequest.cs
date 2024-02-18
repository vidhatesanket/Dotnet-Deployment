namespace BOL;
public class SelectServiceProviderRequest
    {
        public int UserID { get; set; }
        public string? ServiceProviderUsername { get; set; }
        public bool IsSelected { get; set; }
    }