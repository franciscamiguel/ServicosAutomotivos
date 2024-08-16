using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ServiçosAutomotivos
{
    /// <summary>
    /// Lógica interna para TelaConsulta.xaml
    /// </summary>
    public partial class TelaConsulta : Window
    {
        private ServicoDatabase servicoDatabase;

        public TelaConsulta()
        {
            InitializeComponent();
            servicoDatabase = new ServicoDatabase(); // Inicializa o banco de dados

            AtualizarListaServicos();
        }

        private void AtualizarListaServicos()
        {
            var servicos = servicoDatabase.ObterServicos();
            lstServicos.ItemsSource = servicos;
        }

        private void Adicionar_Click(object sender, RoutedEventArgs e)
        {
            TelaCadastro telaCadastro = new TelaCadastro();
            telaCadastro.Show();
            this.Close();
        }

        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            var servicos = servicoDatabase.ObterServicos();
            string termoBusca = txtBusca.Text.ToLower();
            var resultados = servicos.Where(s => s.Nome.ToLower().Contains(termoBusca)).ToList();
            lstServicos.ItemsSource = resultados;
        }

        private void lstServicos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstServicos.SelectedItem is Servico servicoSelecionado)
            {
                MessageBox.Show($"Nome: {servicoSelecionado.Nome}\n" +
                                $"Custo: {servicoSelecionado.Custo}\n" + 
                                $"Descrição: {servicoSelecionado.Descricao}\n" +
                                $"Tempo Estimado: {servicoSelecionado.TempoEstimado}");
            }
        }
    }
}
