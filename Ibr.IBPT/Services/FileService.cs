using System.Globalization;
using System.Text;

namespace Ibr.IBPT.Services;

public static class FileService
{
    private static readonly CultureInfo _cultureInfo = new CultureInfo("pt-BR");
    private static readonly List<string> UFs = new List<string>()
    {
        "RO", "AC", "AM", "RR", "PA", "AP", "TO", "MA", "PI", "CE", "RN", "PB", "PE",
        "AL", "SE", "BA", "MG", "ES", "RJ", "SP", "PR", "SC", "RS", "MS", "MT", "GO", "DF"
    };

    public static string[] GetLinesToInsert(IFormFile file)
    {
        byte[] fileBytes;
        using (var ms = new MemoryStream())
        {
            file.CopyTo(ms);
            fileBytes = ms.ToArray();
        }

        var utf8 = new UTF8Encoding();
        var text = utf8.GetString(fileBytes);
        return text.Split('\n');
    }

    public static string[] CleanColumns(string[] columns)
    {
        for (var i = 0; i < columns.Length; i++)
            columns[i] = columns[i].Trim().Replace("\"", "").Replace("\r", "");

        return columns;
    }

    public static bool UfIsValid(string uf) => UFs.Contains(uf);

    public static DateTime GetDate(string date)
    {
        var dateTime = DateTime.Parse(date, _cultureInfo);
        return SetKindUtc(dateTime);
    }

    private static DateTime SetKindUtc(DateTime dateTime)
    {
        if (dateTime.Kind == DateTimeKind.Utc) { return dateTime; }
        return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
    }
}