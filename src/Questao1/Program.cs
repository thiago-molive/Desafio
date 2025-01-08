using Questao1.Domain;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Questao1;

class Program
{
    static void Main(string[] args)
    {
        ContaBancaria conta;

        var numero = GetValidLong("Entre o número da conta: ");
        var titular = GetValidString("Entre o titular da conta: ");
        char resp = GetValidChar("Haverá depósito inicial (s/n)? ");

        if (resp == 's' || resp == 'S')
        {
            var depositoInicial = GetValidDecimal("Entre o valor de depósito inicial: ");
            conta = ContaBancaria.Create(numero, titular, depositoInicial);
        }
        else
        {
            conta = ContaBancaria.Create(numero, titular);
        }

        Console.WriteLine();
        Console.WriteLine("Dados da conta:");
        Console.WriteLine(conta);

        Console.WriteLine();
        var quantia = GetValidDecimal("Entre um valor para depósito: ");
        conta.Deposito(quantia);
        Console.WriteLine("Dados da conta atualizados:");
        Console.WriteLine(conta);

        Console.WriteLine();
        quantia = GetValidDecimal("Entre um valor para saque: ");
        conta.Saque(quantia);
        Console.WriteLine("Dados da conta atualizados:");
        Console.WriteLine(conta);

        /* Output expected:
        Exemplo 1:

        Entre o número da conta: 5447
        Entre o titular da conta: Milton Gonçalves
        Haverá depósito inicial(s / n) ? s
        Entre o valor de depósito inicial: 350.00

        Dados da conta:
        Conta 5447, Titular: Milton Gonçalves, Saldo: $ 350.00

        Entre um valor para depósito: 200
        Dados da conta atualizados:
        Conta 5447, Titular: Milton Gonçalves, Saldo: $ 550.00

        Entre um valor para saque: 199
        Dados da conta atualizados:
        Conta 5447, Titular: Milton Gonçalves, Saldo: $ 347.50

        Exemplo 2:
        Entre o número da conta: 5139
        Entre o titular da conta: Elza Soares
        Haverá depósito inicial(s / n) ? n

        Dados da conta:
        Conta 5139, Titular: Elza Soares, Saldo: $ 0.00

        Entre um valor para depósito: 300.00
        Dados da conta atualizados:
        Conta 5139, Titular: Elza Soares, Saldo: $ 300.00

        Entre um valor para saque: 298.00
        Dados da conta atualizados:
        Conta 5139, Titular: Elza Soares, Saldo: $ -1.50
        */
    }

    static long GetValidLong(string prompt)
    {
        long result;
        while (true)
        {
            Console.Write(prompt);
            if (long.TryParse(Console.ReadLine(), out result) && result > 0)
                break;
            Console.WriteLine("Valor inválido. Por favor, tente novamente.");
        }
        return result;
    }

    static string GetValidString(string prompt)
    {
        string result;
        while (true)
        {
            Console.Write(prompt);
            result = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(result) && Regex.IsMatch(result, "^[A-Za-zÀ-ÖØ-öø-ÿ]+(?: [A-Za-zÀ-ÖØ-öø-ÿ]+)+$"))
                break;
            Console.WriteLine("Valor inválido. Por favor, tente novamente.");
        }
        return result;
    }

    static char GetValidChar(string prompt)
    {
        char result;
        while (true)
        {
            Console.Write(prompt);
            if (char.TryParse(Console.ReadLine(), out result) && (result == 's' || result == 'S' || result == 'n' || result == 'N'))
                break;
            Console.WriteLine("Valor inválido. Por favor, tente novamente.");
        }
        return result;
    }

    static decimal GetValidDecimal(string prompt)
    {
        decimal result;
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine().Replace(',', '.');
            if (decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
                break;
            Console.WriteLine("Valor inválido. Por favor, tente novamente.");
        }
        return result;
    }
}
