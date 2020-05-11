namespace Backend_Dis_App.Models
{
    public class ResetPasswordModel
    {
        public string EmailAddress { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
