using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ServiçosAutomotivos
{
    /// <summary>
    /// Interação lógica para TelaCadastro.xam
    /// </summary>
    public partial class TelaCadastro : Window
    {
        private ServicoDatabase servicoDatabase;

        public TelaCadastro()
        {
            InitializeComponent();
            servicoDatabase = new ServicoDatabase(); // Inicializa o banco de dados
        }

        private void Salvar_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarCampos())
            {
                decimal custo;
                TimeSpan tempoEstimado;

                // Remover qualquer ponto (.) do texto
                custo = Convert.ToDecimal(txtCusto.Text.Replace(".", ",").Trim());

                // Tenta converter txtTempo.Text para TimeSpan
                if (!TimeSpan.TryParse(txtTempo.Text, out tempoEstimado))
                {
                    MessageBox.Show("Por favor, insira um valor válido para o tempo estimado. Exemplo: 45 para 45 minutos.");
                    return;
                }

                var novoServico = new Servico
                {
                    Nome = txtNome.Text,
                    Descricao = txtDescricao.Text,
                    Custo = custo,
                    TempoEstimado = tempoEstimado
                };

                servicoDatabase.AdicionarServico(novoServico); // Salva no banco de dados

                // Opcional: Abrir a tela de consulta após salvar
                TelaConsulta telaConsulta = new TelaConsulta();
                telaConsulta.Show();
                this.Close();
            }
        }

        private void IrParaTelaConsulta_Click(object sender, RoutedEventArgs e)
        {
            TelaConsulta telaConsulta = new TelaConsulta();
            telaConsulta.Show(); // Mostra a Tela de Consulta
            this.Close(); // Fecha a Tela de Cadastro, se desejado
        }

        private bool ValidarCampos()
        {

            // Verifica se os campos estão preenchidos corretamente
            if (string.IsNullOrWhiteSpace(txtNome.Text)){
                MessageBox.Show("Por favor, preencha o Nome.");
                return false;
            }
            var servicos = servicoDatabase.ObterServicos();
            string nome = txtNome.Text.ToLower();
            var resultados = servicos.Where(s => s.Nome.ToLower().Contains(nome)).ToList();
            if(resultados != null && resultados.Count > 0)
            {
                MessageBox.Show("Já Existe o Serviço: " + txtNome.Text);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDescricao.Text))
            {
                MessageBox.Show("Por favor, preencha a Descrição.");
                return false;
            }
            
            // Expressão regular para verificar se o texto contém apenas números e vírgula
            Regex regex = new Regex(@"^\d+(,\d{1,2})?$");
            if (!regex.IsMatch(txtCusto.Text))
            {
                MessageBox.Show("Por favor, insira um valor válido para o custo apenas com números e vírgula.");
                return false;
            }
            if(!TimeSpan.TryParse(txtTempo.Text, out _))
            {
                MessageBox.Show("Por favor, insira um valor válido para o tempo estimado. Exemplo: 45 para 45 minutos.");
                return false;
            }

            return true;
        }
    }

    
}
