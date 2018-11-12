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
                var findSalt = context.Accounts.Where(e => e.username == username).FirstOrDefault();
                salt = findSalt?.salt;

                //short circuit
                if(salt == null)
                {
                    Message = "No such username/password combination found.";
                    return;
                }

                string saltedPW = BCrypt.Net.BCrypt.HashPassword(password, salt);

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
            //purely for demonstrative purposes
            //
        }
    }
}
