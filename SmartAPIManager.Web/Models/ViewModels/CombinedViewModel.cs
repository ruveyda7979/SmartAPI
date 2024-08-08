namespace SmartAPIManager.Web.Models.ViewModels
{
    public class CombinedViewModel
    {
        public LoginViewModel Login { get; set; } = new LoginViewModel();
        public RegisterViewModel Register { get; set; } = new RegisterViewModel();
    }
}
