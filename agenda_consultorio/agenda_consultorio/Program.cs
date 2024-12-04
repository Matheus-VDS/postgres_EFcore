using System;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;


namespace agenda_consultorio
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Configuração do banco de dados
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AgendaContext>();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

            using (var context = new AgendaContext(optionsBuilder.Options))
            {
                // Garante que o banco de dados seja criado
                context.Database.EnsureCreated();

                var agenda = new Agenda(context);

                // Restante do código original do Program.Main()
                int opcaoMenuPrincipal;

                do
                {
                    Console.Clear();
                    Console.WriteLine("Menu Principal");
                    Console.WriteLine("1-Cadastro de pacientes");
                    Console.WriteLine("2-Agenda");
                    Console.WriteLine("3-Fim");
                    Console.Write("Escolha uma opção: ");
                    opcaoMenuPrincipal = int.Parse(Console.ReadLine());

                    switch (opcaoMenuPrincipal)
                    {
                        case 1:
                            MenuCadastroPacientes(agenda);
                            break;
                        case 2:
                            MenuAgenda(agenda);
                            break;
                        case 3:
                            Console.WriteLine("Fim de programa");
                            break;
                        default:
                            Console.WriteLine("Opção inválida.");
                            break;
                    }
                } while (opcaoMenuPrincipal != 3);
            }
        }

        private static void MenuCadastroPacientes(Agenda agenda)
        {
            int opcaoCadastro;
            do
            {
                Console.Clear();
                Console.WriteLine("Menu de Cadastro de Pacientes");
                Console.WriteLine("1-Cadastrar novo paciente");
                Console.WriteLine("2-Excluir paciente");
                Console.WriteLine("3-Listar pacientes (ordenado por CPF)");
                Console.WriteLine("4-Listar pacientes (ordenado por nome)");
                Console.WriteLine("5-Voltar p/ menu principal");
                Console.Write("Escolha uma opção: ");
                opcaoCadastro = int.Parse(Console.ReadLine());

                switch (opcaoCadastro)
                {
                    case 1:
                        // Cadastro de paciente
                        try
                        {
                            Console.Write("CPF: ");
                            string cpf = Console.ReadLine();
                            Console.Write("Nome: ");
                            string nome = Console.ReadLine();
                            Console.Write("Data de nascimento (dd/MM/yyyy): ");
                            DateTime dataNascimento = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                            var novoPaciente = new Paciente(cpf, nome, dataNascimento);
                            agenda.AdicionarPaciente(novoPaciente);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case 2:
                        // Exclusão de paciente
                        try
                        {
                            Console.Write("CPF do paciente a excluir: ");
                            string cpf = Console.ReadLine();
                            agenda.RemoverPaciente(cpf);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case 3:
                        agenda.ListarPacientes(true); // Ordenado por CPF
                        break;

                    case 4:
                        agenda.ListarPacientes(false); // Ordenado por nome
                        break;

                    case 5:
                        break;

                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }
                Console.WriteLine("Pressione qualquer tecla para continuar...");
                Console.ReadKey();
            } while (opcaoCadastro != 5);
        }

        private static void MenuAgenda(Agenda agenda)
        {
            int opcaoAgenda;
            do
            {
                Console.Clear();
                Console.WriteLine("Menu da Agenda");
                Console.WriteLine("1-Agendar consulta");
                Console.WriteLine("2-Cancelar agendamento");
                Console.WriteLine("3-Listar agenda");
                Console.WriteLine("4-Voltar p/ menu principal");
                Console.Write("Escolha uma opção: ");
                opcaoAgenda = int.Parse(Console.ReadLine());

                switch (opcaoAgenda)
                {
                    case 1:
                        // Agendamento de consulta
                        try
                        {
                            Console.Write("CPF do paciente: ");
                            string cpf = Console.ReadLine();
                            Console.Write("Data da consulta (dd/MM/yyyy): ");
                            DateTime dataConsulta = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            Console.Write("Hora inicial (HHmm): ");
                            TimeSpan horaInicial = TimeSpan.ParseExact(Console.ReadLine(), "hhmm", CultureInfo.InvariantCulture);
                            Console.Write("Hora final (HHmm): ");
                            TimeSpan horaFinal = TimeSpan.ParseExact(Console.ReadLine(), "hhmm", CultureInfo.InvariantCulture);
                            agenda.AgendarConsulta(cpf, dataConsulta, horaInicial, horaFinal);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case 2:
                        // Cancelamento de agendamento
                        try
                        {
                            Console.Write("CPF do paciente: ");
                            string cpf = Console.ReadLine();
                            Console.Write("Data da consulta (dd/MM/yyyy): ");
                            DateTime dataConsulta = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            Console.Write("Hora inicial da consulta (HHmm): ");
                            TimeSpan horaInicial = TimeSpan.ParseExact(Console.ReadLine(), "hhmm", CultureInfo.InvariantCulture);

                            agenda.CancelarAgendamento(cpf, dataConsulta, horaInicial);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case 3:
                        Console.Write("Apresentar a agenda T-Toda ou P-Periodo (T/P): ");
                        string tipo = Console.ReadLine();
                        if (tipo.ToUpper() == "P")
                        {
                            Console.Write("Data inicial (dd/MM/yyyy): ");
                            DateTime dataInicial = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            Console.Write("Data final (dd/MM/yyyy): ");
                            DateTime dataFinal = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            agenda.ListarAgenda(dataInicial, dataFinal);
                        }
                        else
                        {
                            agenda.ListarAgenda();
                        }
                        break;

                    case 4:
                        break;

                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }
                Console.WriteLine("Pressione qualquer tecla para continuar...");
                Console.ReadKey();
            } while (opcaoAgenda != 4);
        }

    }
}