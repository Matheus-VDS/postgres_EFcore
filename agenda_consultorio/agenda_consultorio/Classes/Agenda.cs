/*using System;
using System.Collections.Generic;
using System.Linq;

public class Agenda
{
    private List<Paciente> pacientes = new List<Paciente>();
    private List<Consulta> consultas = new List<Consulta>();

    public void AdicionarPaciente(Paciente paciente)
    {
        if (pacientes.Any(p => p.CPF == paciente.CPF))
            throw new ArgumentException("Erro: CPF já cadastrado.");

        pacientes.Add(paciente);
        Console.WriteLine("Paciente cadastrado com sucesso!");
    }

    public void RemoverPaciente(string cpf)
    {
        var paciente = pacientes.FirstOrDefault(p => p.CPF == cpf);
        if (paciente == null) throw new ArgumentException("Erro: Paciente não cadastrado.");
        if (consultas.Any(c => c.CPF == cpf && c.DataConsulta >= DateTime.Now.Date))
            throw new InvalidOperationException("Erro: Paciente possui consulta futura agendada.");

        consultas.RemoveAll(c => c.CPF == cpf);
        pacientes.Remove(paciente);
        Console.WriteLine("Paciente excluído com sucesso!");
    }

    public void AgendarConsulta(string cpf, DateTime dataConsulta, TimeSpan horaInicial, TimeSpan horaFinal)
    {
        // Verifica se o CPF existe no cadastro
        var paciente = pacientes.Find(p => p.CPF == cpf);
        if (paciente == null)
        {
            throw new Exception("Erro: paciente não cadastrado.");
        }

        // Valida a data da consulta no formato DDMMAAAA
        if (dataConsulta < DateTime.Today ||
            (dataConsulta == DateTime.Today && horaInicial <= DateTime.Now.TimeOfDay))
        {
            throw new Exception("Erro: o agendamento deve ser para um horário futuro.");
        }

        // Verifica se hora final é maior que hora inicial
        if (horaFinal <= horaInicial)
        {
            throw new Exception("Erro: hora final deve ser maior que a hora inicial.");
        }

        // Verifica se o paciente já tem um agendamento futuro
        var agendamentoExistente = consultas.Find(c => c.CPF == cpf && c.DataConsulta > DateTime.Today);
        if (agendamentoExistente != null)
        {
            throw new Exception("Erro: o paciente já tem um agendamento futuro.");
        }

        // Verifica se o agendamento não está sobrepondo outro
        foreach (var consulta in consultas)
        {
            if (consulta.DataConsulta == dataConsulta &&
                ((horaInicial >= consulta.HoraInicial && horaInicial < consulta.HoraFinal) ||
                 (horaFinal > consulta.HoraInicial && horaFinal <= consulta.HoraFinal)))
            {
                throw new Exception("Erro: o horário está sobrepondo outro agendamento.");
            }
        }

        // Verifica se a hora inicial e final são válidas (de 15 em 15 minutos)
        if (horaInicial.Minutes % 15 != 0 || horaFinal.Minutes % 15 != 0)
        {
            throw new Exception("Erro: a hora deve ser múltiplo de 15 minutos.");
        }

        // Verifica se o horário de agendamento está dentro do horário de funcionamento
        if (horaInicial < new TimeSpan(8, 0, 0) || horaFinal > new TimeSpan(19, 0, 0))
        {
            throw new Exception("Erro: o horário de agendamento deve estar entre 08:00 e 19:00.");
        }

        // Agendamento válido, cria uma nova consulta
        var novaConsulta = new Consulta(cpf, dataConsulta, horaInicial, horaFinal);
        consultas.Add(novaConsulta);
        Console.WriteLine("Agendamento realizado com sucesso!");
    }


    public void ListarPacientes(bool ordenarPorCPF = true)
    {
        var listaPacientes = ordenarPorCPF ? pacientes.OrderBy(p => p.CPF) : pacientes.OrderBy(p => p.Nome);
        foreach (var paciente in listaPacientes)
        {
            Console.WriteLine($"CPF: {paciente.CPF} | Nome: {paciente.Nome} | Data Nascimento: {paciente.DataNascimento:dd/MM/yyyy} | Idade: {paciente.Idade}");
            var consultaFutura = consultas.FirstOrDefault(c => c.CPF == paciente.CPF && c.DataConsulta >= DateTime.Now.Date);
            if (consultaFutura != null)
            {
                Console.WriteLine($"   Agendado para: {consultaFutura.DataConsulta:dd/MM/yyyy} - {consultaFutura.HoraInicial:hh\\:mm} às {consultaFutura.HoraFinal:hh\\:mm}");
            }
        }
    }

    public void ListarAgenda(DateTime? dataInicial = null, DateTime? dataFinal = null)
    {
        var agendaFiltrada = consultas.Where(c => (!dataInicial.HasValue || c.DataConsulta >= dataInicial.Value) &&
                                                  (!dataFinal.HasValue || c.DataConsulta <= dataFinal.Value))
                                      .OrderBy(c => c.DataConsulta).ThenBy(c => c.HoraInicial);

        foreach (var consulta in agendaFiltrada)
        {
            var paciente = pacientes.FirstOrDefault(p => p.CPF == consulta.CPF);
            if (paciente != null)
            {
                Console.WriteLine($"Data: {consulta.DataConsulta:dd/MM/yyyy} | Início: {consulta.HoraInicial:hh\\:mm} | Fim: {consulta.HoraFinal:hh\\:mm} | Nome: {paciente.Nome} | Nascimento: {paciente.DataNascimento:dd/MM/yyyy}");
            }
        }
    }

    public void CancelarAgendamento(string cpf, DateTime dataConsulta, TimeSpan horaInicial)
    {
        var consulta = consultas.Find(c => c.CPF == cpf && c.DataConsulta == dataConsulta && c.HoraInicial == horaInicial);

        if (consulta == null)
        {
            throw new Exception("Erro: agendamento não encontrado.");
        }

        if (consulta.DataConsulta < DateTime.Today ||
           (consulta.DataConsulta == DateTime.Today && consulta.HoraInicial < DateTime.Now.TimeOfDay))
        {
            throw new Exception("Erro: apenas consultas futuras podem ser canceladas.");
        }

        consultas.Remove(consulta);
        Console.WriteLine("Agendamento cancelado com sucesso!");
    }
}*/

using Microsoft.EntityFrameworkCore;
using System.Linq;

public class Agenda
{
    private readonly AgendaContext _context;

    public Agenda(AgendaContext context)
    {
        _context = context;
    }

    public void AdicionarPaciente(Paciente paciente)
    {
        try
        {
            if (_context.Pacientes.Any(p => p.CPF == paciente.CPF))
                throw new ArgumentException("Erro: CPF já cadastrado.");

            _context.Pacientes.Add(paciente);
            _context.SaveChanges();
            Console.WriteLine("Paciente cadastrado com sucesso!");
        }
        catch (DbUpdateException ex)
        {
            // Log detalhado do erro
            Console.WriteLine($"Erro ao salvar paciente: {ex.InnerException?.Message}");
            throw;
        }
    }

    public void RemoverPaciente(string cpf)
    {
        var paciente = _context.Pacientes.FirstOrDefault(p => p.CPF == cpf);
        if (paciente == null) throw new ArgumentException("Erro: Paciente não cadastrado.");

        var consultasFuturas = _context.Consultas.Any(c => c.CPF == cpf && c.DataConsulta >= DateTime.Now.Date);
        if (consultasFuturas)
            throw new InvalidOperationException("Erro: Paciente possui consulta futura agendada.");

        _context.Consultas.RemoveRange(_context.Consultas.Where(c => c.CPF == cpf));
        _context.Pacientes.Remove(paciente);
        _context.SaveChanges();
        Console.WriteLine("Paciente excluído com sucesso!");
    }

    public void AgendarConsulta(string cpf, DateTime dataConsulta, TimeSpan horaInicial, TimeSpan horaFinal)
    {
        var paciente = _context.Pacientes.Find(cpf);
        if (paciente == null)
        {
            throw new Exception("Erro: paciente não cadastrado.");
        }

        if (dataConsulta < DateTime.Today ||
            (dataConsulta == DateTime.Today && horaInicial <= DateTime.Now.TimeOfDay))
        {
            throw new Exception("Erro: o agendamento deve ser para um horário futuro.");
        }

        if (horaFinal <= horaInicial)
        {
            throw new Exception("Erro: hora final deve ser maior que a hora inicial.");
        }

        var agendamentoExistente = _context.Consultas
            .FirstOrDefault(c => c.CPF == cpf && c.DataConsulta > DateTime.Today);
        if (agendamentoExistente != null)
        {
            throw new Exception("Erro: o paciente já tem um agendamento futuro.");
        }

        var consultasSobrepostas = _context.Consultas
            .Any(c => c.DataConsulta == dataConsulta &&
                   ((horaInicial >= c.HoraInicial && horaInicial < c.HoraFinal) ||
                    (horaFinal > c.HoraInicial && horaFinal <= c.HoraFinal)));

        if (consultasSobrepostas)
        {
            throw new Exception("Erro: o horário está sobrepondo outro agendamento.");
        }

        if (horaInicial.Minutes % 15 != 0 || horaFinal.Minutes % 15 != 0)
        {
            throw new Exception("Erro: a hora deve ser múltiplo de 15 minutos.");
        }

        if (horaInicial < new TimeSpan(8, 0, 0) || horaFinal > new TimeSpan(19, 0, 0))
        {
            throw new Exception("Erro: o horário de agendamento deve estar entre 08:00 e 19:00.");
        }

        var novaConsulta = new Consulta(cpf, dataConsulta, horaInicial, horaFinal);
        _context.Consultas.Add(novaConsulta);
        _context.SaveChanges();
        Console.WriteLine("Agendamento realizado com sucesso!");
    }

    public void ListarPacientes(bool ordenarPorCPF = true)
    {
        var listaPacientes = ordenarPorCPF
            ? _context.Pacientes.OrderBy(p => p.CPF).ToList() // Dados carregados na memória
            : _context.Pacientes.OrderBy(p => p.Nome).ToList();

        foreach (var paciente in listaPacientes)
        {
            Console.WriteLine($"CPF: {paciente.CPF} | Nome: {paciente.Nome} | Data Nascimento: {paciente.DataNascimento:dd/MM/yyyy} | Idade: {paciente.Idade}");

            var consultaFutura = _context.Consultas
                .Where(c => c.CPF == paciente.CPF && c.DataConsulta >= DateTime.UtcNow.Date)
                .OrderBy(c => c.DataConsulta)
                .FirstOrDefault();

            if (consultaFutura != null)
            {
                Console.WriteLine($"   Agendado para: {consultaFutura.DataConsulta:dd/MM/yyyy} - {consultaFutura.HoraInicial:hh\\:mm} às {consultaFutura.HoraFinal:hh\\:mm}");
            }
        }
    }


    public void ListarAgenda(DateTime? dataInicial = null, DateTime? dataFinal = null)
    {
        var agendaFiltrada = _context.Consultas
            .Where(c => (!dataInicial.HasValue || c.DataConsulta >= dataInicial.Value) &&
                        (!dataFinal.HasValue || c.DataConsulta <= dataFinal.Value))
            .OrderBy(c => c.DataConsulta)
            .ThenBy(c => c.HoraInicial)
            .ToList();

        foreach (var consulta in agendaFiltrada)
        {
            var paciente = _context.Pacientes.FirstOrDefault(p => p.CPF == consulta.CPF);
            if (paciente != null)
            {
                Console.WriteLine($"Data: {consulta.DataConsulta:dd/MM/yyyy} | Início: {consulta.HoraInicial:hh\\:mm} | Fim: {consulta.HoraFinal:hh\\:mm} | Nome: {paciente.Nome} | Nascimento: {paciente.DataNascimento:dd/MM/yyyy}");
            }
        }
    }

    public void CancelarAgendamento(string cpf, DateTime dataConsulta, TimeSpan horaInicial)
    {
        var consulta = _context.Consultas
            .FirstOrDefault(c => c.CPF == cpf &&
                                 c.DataConsulta == dataConsulta &&
                                 c.HoraInicial == horaInicial);

        if (consulta == null)
        {
            throw new Exception("Erro: agendamento não encontrado.");
        }

        if (consulta.DataConsulta < DateTime.Today ||
           (consulta.DataConsulta == DateTime.Today && consulta.HoraInicial < DateTime.Now.TimeOfDay))
        {
            throw new Exception("Erro: apenas consultas futuras podem ser canceladas.");
        }

        _context.Consultas.Remove(consulta);
        _context.SaveChanges();
        Console.WriteLine("Agendamento cancelado com sucesso!");
    }
}
