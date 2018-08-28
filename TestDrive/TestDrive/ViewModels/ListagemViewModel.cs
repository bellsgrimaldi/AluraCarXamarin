using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TestDrive.Models;
using Xamarin.Forms;

namespace TestDrive.ViewModels
{
    public class ListagemViewModel : BaseViewModel
    {
        const string URL_GET_VEICULOS = "http://aluracar.herokuapp.com/";

        public ObservableCollection<Veiculo> Veiculos { get; set; }

        private bool loading;

        public bool Loading
        {
            get { return loading; }
            set {
                loading = value;
                OnPropertyChanged();
            }
        }


        Veiculo veiculoSelecionado;
        public Veiculo VeiculoSelecionado {
            get {
                return veiculoSelecionado;
            }
            set
            {
                veiculoSelecionado = value;
                if(value != null)
                    MessagingCenter.Send(veiculoSelecionado, "VeiculoSelecionado");
            }
        }
        public ListagemViewModel()
        {
            this.Veiculos = new ObservableCollection<Veiculo>();
        }

        public async Task GetVeiculos()
        {
            Loading = true;
            HttpClient cliente = new HttpClient();
            var resultado = await cliente.GetStringAsync(URL_GET_VEICULOS);

            var veiculosJson = JsonConvert.DeserializeObject<VeiculoJson[]>(resultado);

            foreach (var veiculoJson in veiculosJson)
            {
                this.Veiculos.Add(new Veiculo {
                    Nome = veiculoJson.nome, 
                    Preco = veiculoJson.preco
                });
            }
            Loading = false;
        }
    }

    class VeiculoJson
    {
        public string nome { get; set; }
        public int preco { get; set; }
    }
}
