namespace WebAppCore.BLL.Models
{
    public class PasswordModel
    {
        public PasswordModel(string salt, string hash)
        {
            PassportSalt = salt;
            PassportHash = hash;
        }

        public string PassportSalt { get; }

        public string PassportHash { get; }
    }
}