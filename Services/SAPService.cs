using System.Text;
using System.Text.Json;
using AsientosContrablesApi.Models;

namespace AsientosContrablesApi.Services
{
  public class SAPService : ISAPService
  {
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<SAPService> _logger;

    public SAPService(IHttpClientFactory clientFactory, ILogger<SAPService> logger)
    {
      _clientFactory = clientFactory;
      _logger = logger;
    }

    public async Task<List<DataModel>> CrearAsientos(List<AsientoContableDTO> asientos)
    {
      _logger.LogInformation("************Asientos {asientos}", asientos);
      var successResponses = new List<DataModel>();

      try
      {
        var httpClient = _clientFactory.CreateClient("SAPApi");

        foreach (var asiento in asientos)
        {
          _logger.LogInformation("La solicitud fue exitosa. Respuesta: {Response}", asiento.Reference);

          var jsonContent = new StringContent(JsonSerializer.Serialize(asiento), Encoding.UTF8, "application/json");

          var response = await httpClient.PostAsync("JournalEntries", jsonContent);
          _logger.LogInformation("*********AQUI {a}", response);
          if (response.IsSuccessStatusCode)
          {
            var contentStream = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("La solicitud fue exitosa. Respuesta: {Response}", contentStream);// me devulve los dos numeros
            var sapApiResponse = JsonSerializer.Deserialize<SAPApiResponse>(contentStream);

            int docEntry = sapApiResponse.Data.DocEntry;
            int docNum = sapApiResponse.Data.DocNum;
            _logger.LogInformation("entry num: {entry}", docEntry);
            _logger.LogInformation("doc num: {num}", docNum);

            successResponses.Add(new DataModel
            {
              DocEntry = docEntry,
              DocNum = docNum
            });
          }
          else
          {
            var contentStream = await response.Content.ReadAsStringAsync();

            _logger.LogError("Error en la solicitud. Estado HTTP: {StatusCode}", response.StatusCode);
            _logger.LogError("Error en la solicitud. Estado HTTP: {StatusCode}", contentStream);

            successResponses.Add(new DataModel
            {
              DocEntry = 0,
              DocNum = 0
            });
          }
        }
      }
      catch (Exception ex)
      {
        _logger.LogError("Ocurrio un error inesperado {ex}", ex);

        successResponses.Add(new DataModel
        {
          DocEntry = 0,
          DocNum = 0
        });
      }
      return successResponses;
    }
  }
}