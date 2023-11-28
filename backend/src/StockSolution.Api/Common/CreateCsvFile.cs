using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public class CsvWriter<T>
{
    private readonly CsvConfiguration _csvConfig;

    public CsvWriter(CsvConfiguration csvConfig = null)
    {
        _csvConfig = csvConfig ?? new CsvConfiguration(CultureInfo.InvariantCulture);
    }

    public MemoryStream WriteRecords(IEnumerable<T> records)
    {
        var memoryStream = new MemoryStream();

        using (var writer = new StreamWriter(memoryStream, leaveOpen: true))
        using (var csv = new CsvWriter(writer, _csvConfig))
        {
            csv.WriteRecords(records);
        }

        memoryStream.Position = 0;
        return memoryStream;
    }
}
