using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using SQLitePCL;


namespace ServiçosAutomotivos
{
    
    public class ServicoDatabase
    {
        private string connectionString = "Data Source=servicos.db";

        public ServicoDatabase()
        {
            // Inicializar SQLitePCL
            Batteries.Init();

            // Cria o banco de dados e a tabela, caso ainda não existam
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string tableCommand = "CREATE TABLE IF NOT EXISTS Servicos (" +
                                      "Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                      "Nome TEXT, " +
                                      "Descricao TEXT, " +
                                      "Custo REAL, " +
                                      "TempoEstimado INTEGER)";

                var createTable = new SqliteCommand(tableCommand, connection);
                createTable.ExecuteNonQuery();
            }
        }

        // Método para adicionar um serviço ao banco de dados
        public void AdicionarServico(Servico servico)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var insertCommand = connection.CreateCommand();
                insertCommand.CommandText = "INSERT INTO Servicos (Nome, Descricao, Custo, TempoEstimado) VALUES (@Nome, @Descricao, @Custo, @TempoEstimado)";
                insertCommand.Parameters.AddWithValue("@Nome", servico.Nome);
                insertCommand.Parameters.AddWithValue("@Descricao", servico.Descricao);
                insertCommand.Parameters.AddWithValue("@Custo", servico.Custo);
                insertCommand.Parameters.AddWithValue("@TempoEstimado", servico.TempoEstimado);

                insertCommand.ExecuteNonQuery();
            }
        }

        // Método para buscar todos os serviços no banco de dados
        public List<Servico> ObterServicos()
        {
            var servicos = new List<Servico>();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var selectCommand = connection.CreateCommand();
                selectCommand.CommandText = "SELECT * FROM Servicos";

                using (var reader = selectCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        servicos.Add(new Servico
                        {
                            Nome = reader.GetString(1),
                            Descricao = reader.GetString(2),
                            Custo = reader.GetDecimal(3),
                            TempoEstimado = TimeSpan.FromMinutes(reader.GetInt32(4))
                        });
                    }
                }
            }

            return servicos;
        }
    }
}
