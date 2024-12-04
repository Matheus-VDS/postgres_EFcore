using System;

public class Consulta
{
    public Consulta() { }

    public string CPF { get; private set; }
    public DateTime DataConsulta { get; private set; }
    public TimeSpan HoraInicial { get; private set; }
    public TimeSpan HoraFinal { get; private set; }

    public Consulta(string cpf, DateTime dataConsulta, TimeSpan horaInicial, TimeSpan horaFinal)
    {
        if (horaInicial < TimeSpan.FromHours(8) || horaFinal > TimeSpan.FromHours(19))
            throw new ArgumentException("Erro: O horário de consulta deve estar entre 08:00 e 19:00.");
        if (horaInicial >= horaFinal)
            throw new ArgumentException("Erro: Hora final deve ser maior que hora inicial.");

        CPF = cpf;
        DataConsulta = dataConsulta;
        HoraInicial = horaInicial;
        HoraFinal = horaFinal;
    }
}
