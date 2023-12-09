using CsvHelper;
using CsvHelper.Configuration;
using FakeUserDataGenerator.Data;
using FakeUserDataGenerator.Models;
using System.Globalization;
using System.Text;

namespace FakeUserDataGenerator.Services;

public interface ICsvService
{
    Task<Report> GenerateCsv(IEnumerable<User> userData);
}
public class CsvService : ICsvService
{
    public async Task<Report> GenerateCsv(IEnumerable<User> userData)
    {
        var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Encoding = Encoding.UTF8,
        };
        await using var memoryStream = new MemoryStream();
        await using var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);
        await using var csvWriter = new CsvWriter(streamWriter, csvConfig);

        await csvWriter.WriteRecordsAsync(userData);
        await streamWriter.FlushAsync();

        var report = new Report
        {
            Content = memoryStream.ToArray(),
            Type = ReportConstants.CsvFileType,
            Filename = ReportConstants.CsvReportFileName
        };

        return report;
    }
}

