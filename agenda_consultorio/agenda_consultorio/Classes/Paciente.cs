public class Paciente
{
    public Paciente() { }

    public string CPF { get; private set; }
    public string Nome { get; private set; }
    public DateTime DataNascimento { get; private set; }
    public int Idade { get; private set; }

    public Paciente(string cpf, string nome, DateTime dataNascimento)
    {
        if (nome.Length < 5) throw new ArgumentException("Erro: Nome deve ter pelo menos 5 caracteres.");

        // Calcular a idade dentro do construtor
        int idade = DateTime.Now.Year - dataNascimento.Year;
        if (DateTime.Now < dataNascimento.AddYears(idade)) // Verifica se o aniversário já passou no ano atual
            idade--;

        if (idade < 13)
        {
            throw new ArgumentException("Erro: Paciente deve ter pelo menos 13 anos.");
        }


        CPF = cpf;
        Nome = nome;
        DataNascimento = dataNascimento;
        Idade = idade; 
    }
}
