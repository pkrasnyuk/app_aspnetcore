namespace WebAppCore.BLL.Helpers
{
    public sealed class Role
    {
        public static readonly Role Admin = new Role(1, "Admin");
        public static readonly Role User = new Role(2, "User");
        public static readonly Role Guest = new Role(3, "Guest");
        private readonly string _name;
        private readonly int _value;

        private Role(int value, string name)
        {
            _name = name;
            _value = value;
        }

        public override string ToString()
        {
            return _name;
        }
    }
}