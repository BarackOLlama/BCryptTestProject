using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Linq;
using System.Windows.Input;

namespace BCryptTest.WPF.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private string _message;
        public AccountVM Account { get; set; }
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                base.RaisePropertyChanged();
            }
        }
        public ICommand VerifyLoginCommand { get; set; }
        public MainViewModel()
        {
            VerifyLoginCommand = new RelayCommand(VerifyLogin);
            Message = "";
            Account = new AccountVM();
        }

        public void VerifyLogin()
        {
            string username;
            string password;
            string salt;

            using(var context = new BCryptTestEntities())
            {
                username = Account.Username;
                password = Account.Password;
                //check whether there's there's any such username in the database.
                var findSalt = context.Accounts.Where(e => e.username == username).FirstOrDefault();
                salt = findSalt?.salt;

                //short circuit
                if(salt == null)
                {
                    Message = "No such username/password combination found.";
                    return;
                }

                //get the salted version of the user input with the salt from the account in the database
                string saltedPW = BCrypt.Net.BCrypt.HashPassword(password, salt);

                //check whether there's any account in the database with that username/password combination
                var findUser = context.Accounts.Where(e => e.username == username && e.password == saltedPW).FirstOrDefault();

                if(findUser == null)
                {
                    Message = "No such username/password combination found.";
                }else
                {
                    Message = "Login succesful!";
                    //save the fact that the user has logged in in a variable.
                }

            }
        }

        public void CreateNewAccount()
        {
            //for demonstrative purposes

            string username = "gebruikersnaam";
            string password = "wachtwoord";
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string saltedPW = BCrypt.Net.BCrypt.HashPassword(password, salt);

            using (var context = new BCryptTestEntities())
            {
                //You do not ever save the plaintext (var password) in the database, only the salted
                context.Accounts.Add(new Account() {Id = 0, username = username, password = saltedPW, salt = salt });
                context.SaveChanges();
            }
            
        }
    }
}
