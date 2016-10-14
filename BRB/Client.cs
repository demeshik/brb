
namespace BRB
{
    public class Client
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string AccCode { get; set; }
        public string Cash { get; set; }
        public bool Block { get; set; }
        public Client(int id, string name, string login, string password, string acccode, string cash, bool block)
        {
            this.ID = id;
            this.Name = name;
            this.Login = login;
            this.Password = password;
            this.AccCode = acccode;
            this.Cash = cash;
            this.Block = block;
        }
        public Client() { }
    }
}
