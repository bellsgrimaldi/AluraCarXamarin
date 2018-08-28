using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using TestDrive.Models;
using Xamarin.Forms;

namespace TestDrive.ViewModels
{
    public class LoginViewModel
    {
        private string usuario;

        public string Usuario
        {
            get { return usuario; }
            set {
                usuario = value;
                //avisar o botão da mudança no valor
                ((Command)EntrarCommand).ChangeCanExecute();
            }
        }

        private string senha;

        public string Senha
        {
            get { return senha; }
            set {
                senha = value;
                ((Command)EntrarCommand).ChangeCanExecute();
            }
        }

        public ICommand EntrarCommand { get; set; }

        public LoginViewModel()
        {
            EntrarCommand = new Command(async() =>
            {
                var loginService = new LoginService();
                await loginService.FazerLogin(new Login(Usuario, Senha));
            }, () => 
            {
                return !string.IsNullOrEmpty(Usuario) && !string.IsNullOrEmpty(Senha);
            });
        }

    }
}
