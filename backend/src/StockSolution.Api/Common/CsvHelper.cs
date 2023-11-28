using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace StockSolution.Api.Common;
public class CsvHelper<T>
{
    public static byte[] GerarCsv(List<T> entidades)
    {
        using (var memoryStream = new MemoryStream())
        using (var writer = new StreamWriter(memoryStream))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(entidades);
            writer.Flush();

            var content = memoryStream.ToArray();

            return content;
        }
    }
}
