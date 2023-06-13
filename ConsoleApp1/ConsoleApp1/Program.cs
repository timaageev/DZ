using System.Collections.Generic;
using System.Linq;

public class Client
{
    public int Year { get; set; }
    public int Duration { get; set; }
}

public class Program
{
    public static void Main()
    {
        List<Client> clients = new List<Client>()
        {
             new Client { Year = 2021, Duration = 100 },
             new Client { Year = 2022, Duration = 200 },
             new Client { Year = 2021, Duration = 150 },
             new Client { Year = 2023, Duration = 300 },
             new Client { Year = 2022, Duration = 250 }
         };

        int maxDurationYear = GetYearWithMaxDuration(clients);
        int maxDuration = clients.Where(c => c.Year == maxDurationYear).Sum(c => c.Duration);

        Console.WriteLine(maxDurationYear);
        Console.WriteLine(maxDuration);

        string[] text = new string[] {
            "Текст представлен в виде массива строк, слова в которых разделены пробелами и знаками",
            "препинания (переносов нет). Написать метод, который по заданному тексту и целому числу",
            "возвращает список слов, длина которых не менее заданного числа, отсортированных алфавитном",
            "порядке. Слова в списке должны быть приведены к нижнему регистру и не должны повторяться.",
            "Метод должен быть написан с использованием только одного оператора"
        };

        int number = 5;

        List<string> result = GetWords(text, number);

        Console.WriteLine(string.Join(", ", result));
    }

    static List<string> GetWords(string[] text, int number)
    {
        return text.SelectMany(line => line.Split(new char[] { ' ', ',', '.', '(', ')' }, StringSplitOptions.RemoveEmptyEntries)) 
                   .Select(word => word.ToLower()) 
                   .Where(word => word.Length >= number) 
                   .Distinct() 
                   .OrderBy(word => word) 
                   .ToList(); 
    }
    public static int GetYearWithMaxDuration(List<Client> clients)
    {
        var groupedByYear = clients.GroupBy(c => c.Year);
        int maxDurationYear = groupedByYear
        .OrderByDescending(g => g.Sum(c => c.Duration))
        .ThenBy(g => g.Key)
        .First().Key;

        return maxDurationYear;
    }
}