using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using BIK.CoreBanking.Interfaces;
using BIK.CoreBanking.DTOs;

namespace BIK.CoreBanking.Services
{
    public class AuditServiceClient : IAuditServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _auditUrl;

        public AuditServiceClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _auditUrl = configuration["AuditServiceUrl"] ?? throw new Exception("URL de auditoría no configurada.");
        }

        public async Task LogActivityAsync(AuditLogDto log)
        {
            try
            {
                await _httpClient.PostAsJsonAsync(_auditUrl, log);
            }
            catch
            {

                Console.WriteLine("Advertencia: No se pudo conectar con el servicio de auditoría.");
            }
        }
    }
}