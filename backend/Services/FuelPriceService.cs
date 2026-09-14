using System.Globalization;
using System.Text.Json;
using backend.Models;

namespace backend.Services;

public class FuelPriceService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public FuelPriceService(
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<FuelPrice> GetLatestJetFuelPriceAsync()
    {
        string? apiKey =
            _configuration["Eia:ApiKey"];

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException(
                "EIA API key is not configured."
            );
        }

        string url =
            "https://api.eia.gov/v2/petroleum/pri/spt/data/" +
            $"?api_key={Uri.EscapeDataString(apiKey)}" +
            "&frequency=daily" +
            "&data[0]=value" +
            "&facets[series][]=EER_EPJK_PF4_RGC_DPG" +
            "&sort[0][column]=period" +
            "&sort[0][direction]=desc" +
            "&length=1";

        using HttpResponseMessage response =
            await _httpClient.GetAsync(url);

        string json =
            await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(
                $"Unable to retrieve jet fuel price from EIA. " +
                $"HTTP {(int)response.StatusCode}: {json}"
            );
        }

        using JsonDocument document =
            JsonDocument.Parse(json);

        JsonElement root =
            document.RootElement;

        if (
            !root.TryGetProperty("response", out JsonElement responseElement)
            ||
            !responseElement.TryGetProperty("data", out JsonElement data)
            ||
            data.ValueKind != JsonValueKind.Array
            ||
            data.GetArrayLength() == 0
        )
        {
            throw new InvalidOperationException(
                "EIA returned no jet fuel price data."
            );
        }

        JsonElement latest =
            data[0];

        string period =
            latest.TryGetProperty(
                "period",
                out JsonElement periodElement
            )
                ? periodElement.GetString() ?? ""
                : "";

        if (
            !latest.TryGetProperty(
                "value",
                out JsonElement valueElement
            )
        )
        {
            throw new InvalidOperationException(
                "EIA response did not contain a fuel price."
            );
        }

        double pricePerGallon;

        if (valueElement.ValueKind == JsonValueKind.Number)
        {
            pricePerGallon =
                valueElement.GetDouble();
        }
        else
        {
            string value =
                valueElement.GetString() ?? "";

            if (!double.TryParse(
                value,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out pricePerGallon))
            {
                throw new InvalidOperationException(
                    $"Unable to parse EIA jet fuel price: '{value}'."
                );
            }
        }

        return new FuelPrice
        {
            PricePerGallon =
                pricePerGallon,

            Date =
                period,

            Source =
                "U.S. EIA Gulf Coast Jet Fuel Spot Price"
        };
    }
}