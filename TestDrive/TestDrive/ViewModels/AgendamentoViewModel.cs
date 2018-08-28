using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using TestDrive.Models;
using Xamarin.Forms;

namespace TestDrive.ViewModels
{
    public class AgendamentoViewModel : BaseViewModel
    {
        const string URL_POST_AGENDAMENTO =  "http://aluracar.herokuapp.com/salvaragendamento";
        public Agendamento Agendamento { get; set; }

        public string Nome
        {
            get
            {
                return Agendamento.Nome;
            }
            set
            {
                Agendamento.Nome = value;
                OnPropertyChanged();
                ((Command)AgendarCommand).ChangeCanExecute();
            }
        }
        public string Telefone
        {
            get
            {
                return Agendamento.Telefone;
            }
            set
            {
                Agendamento.Telefone = value;
                OnPropertyChanged();
                ((Command)AgendarCommand).ChangeCanExecute();
            }
        }
        public string Email
        {
            get
            {
                return Agendamento.Email;
            }
            set
            {
                Agendamento.Email = value;
                OnPropertyChanged();
                ((Command)AgendarCommand).ChangeCanExecute();
            }
        }

        public DateTime DataAgendamento
        {
            get
            {
                return Agendamento.DataAgendamento;
            }
            set
            {
                Agendamento.DataAgendamento = value;
            }
        }

        public TimeSpan HoraAgendamento
        {
            get
            {
                return Agendamento.HoraAgendamento;
            }
            set
            {
                Agendamento.HoraAgendamento = value;
            }
        }
        public AgendamentoViewModel(Veiculo veiculo)
        {
            this.Agendamento = new Agendamento();
            this.Agendamento.Veiculo = veiculo;

            AgendarCommand = new Command(() =>
            {
                MessagingCenter.Send<Agendamento>(this.Agendamento, "Agendamento");
            }, () => {
                return !string.IsNullOrEmpty(Nome) && !string.IsNullOrEmpty(Telefone) && !string.IsNullOrEmpty(Email);
            });
        }

        public ICommand AgendarCommand { get; set; }

        public async void SalvarAgendamento()
        {
            HttpClient client = new HttpClient();

            var dataHoraAgendamento = new DateTime(
                DataAgendamento.Year, DataAgendamento.Month, DataAgendamento.Day, HoraAgendamento.Hours, HoraAgendamento.Minutes, HoraAgendamento.Seconds);

            var json = JsonConvert.SerializeObject(new {
                nome = Nome,
                fone = Telefone,
                email = Email,
                carro = Agendamento.Veiculo.Nome,
                preco = Agendamento.Veiculo.Preco,
                DataAgendamento = dataHoraAgendamento
            });

            var conteudo = new StringContent(json, Encoding.UTF8, "application/json"); 

            var resposta = await client.PostAsync(URL_POST_AGENDAMENTO, conteudo);

            if (resposta.IsSuccessStatusCode)
            {
                MessagingCenter.Send<Agendamento>(this.Agendamento, "SucessoAgendamento");
            }
            else
            {
                MessagingCenter.Send<ArgumentException>(new ArgumentException(), "FalhaAgendamento");
            }
        }
    }
}
