using Microsoft.VisualBasic.FileIO;

namespace Swipeclock;

public class CsvHelper
{
    public static List<string> GetSiteIdsFromCsv(string csvFilePath)
    {
        var siteIds = new List<string>();

        using var parser = new TextFieldParser(csvFilePath);

        parser.TextFieldType = FieldType.Delimited;
        parser.SetDelimiters(",");
        parser.HasFieldsEnclosedInQuotes = true;

        if (parser.EndOfData)
        {
            return siteIds;
        }

        string[]? headers = parser.ReadFields();

        if (headers == null)
        {
            return siteIds;
        }

        int siteIdIndex = Array.FindIndex(
            headers,
            h => string.Equals(h?.Trim(), "Site Code", StringComparison.OrdinalIgnoreCase)
        );

        if (siteIdIndex == -1)
        {
            throw new InvalidOperationException("CSV file does not contain a SiteID column.");
        }

        while (!parser.EndOfData)
        {
            string[]? fields = parser.ReadFields();

            if (fields == null || siteIdIndex >= fields.Length)
            {
                continue;
            }

            string siteId = fields[siteIdIndex].Trim();

            if (!string.IsNullOrWhiteSpace(siteId))
            {
                siteIds.Add(siteId);
            }
        }

        return siteIds;
    }
}