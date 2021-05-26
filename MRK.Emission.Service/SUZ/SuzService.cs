
using CRPTAuthLib;
using CRPTAuthLib.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MRK.Emission.Domain.Enums;
using MRK.Emission.Domain.Helpers;
using MRK.Emission.Domain.Models;
using MRK.Emission.Domain.Models.SUZ;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MRK.Emission.Service.SUZ
{
    public class SuzService : ISuzService
    {
        private AuthData _authData;
        private readonly ILogger<SuzService> _logger;
        private readonly SuzServiceSettings _settings;
        private readonly IHttpClientFactory _clientFactory;

        public SuzService(ILogger<SuzService> logger,
            IOptions<SuzServiceSettings> settings,
            IHttpClientFactory clientFactory)
        {
            _settings = settings.Value;
            _logger = logger;
            _clientFactory = clientFactory;
        }

        public bool Auth()
        {
            if (!string.IsNullOrEmpty(_authData?.token))
                return true;

            var authLib = new AuthLib();

            var regEndpoing = $"{_settings.Host}/auth/key";
            var authEndpoint = $"{_settings.Host}/auth/simpleSignIn/{_settings.OmsConnectionId}";

            _authData = authLib.Reg(regEndpoing);

            if (!string.IsNullOrWhiteSpace(_authData.error_message))
                _logger.LogError(_authData.error_message);

            _authData = authLib.Auth(_authData, authEndpoint, _settings.SertNum);

            if (!string.IsNullOrWhiteSpace(_authData.error_message))
                _logger.LogError(_authData.error_message);

            return !string.IsNullOrEmpty(_authData?.token);
        }

        public async Task<bool> PingAsync(string clientName, CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"{_settings.EmissionUrl}/ping?omsId={_settings.SuzOmsId}");

            request.Headers.Add("clientToken", _authData.token);

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request, cancellationToken);

            return response.IsSuccessStatusCode;
        }

        public async Task<List<OrderDocumentLine>> SendDocLinesAsync(List<OrderDocumentLine> docLines, CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Post,
                $"{_settings.EmissionUrl}/orders?omsId={_settings.SuzOmsId}");

            request.Headers.Add("clientToken", _authData.token);

            var client = _clientFactory.CreateClient();

            Order order = new Order();
            List<OrderProduct> products = new List<OrderProduct>();

            foreach (OrderDocumentLine docLine in docLines)
            {
                OrderProduct product = new OrderProduct
                {
                    gtin = docLine.gtin,
                    quantity = docLine.qty
                };

                products.Add(product);
            }

            order.products = products;

            string ser = JsonSerializer.Serialize(order);
            request.Content = new StringContent(ser,
                Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request, cancellationToken);

            string res = "";

            if (response != null)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                TextReader reader = new StreamReader(responseStream);
                res = reader.ReadToEnd();

                if (response.IsSuccessStatusCode)
                {
                    OrderResponse ordRes = JsonSerializer.Deserialize
                            <OrderResponse>(res);

                    docLines.ForEach(l =>
                    {
                        l.orderId = ordRes.orderId;
                        l.orderDate = DateTime.Now;
                        l.documentLineStatus = DocumentLineStatus.CREATED;
                        l.orderResponse = ordRes;
                    });
                }

            }

            return docLines;
        }

        public async Task<(OrderDocumentLine, List<CisInfo>)> GetCodesAsync(OrderDocumentLine docLine, int codesCount, CancellationToken cancellationToken = default)
        {
            List<CisInfo> resp = new List<CisInfo>();

            string queryString = $"{_settings.EmissionUrl}/codes?";
            queryString += $"omsId={_settings.SuzOmsId}";
            queryString += $"&orderId={docLine.orderId}";
            queryString += $"&gtin={docLine.gtin}";
            queryString += $"&quantity={codesCount}";

            string lastBlockId = string.IsNullOrEmpty(docLine.lastBlockId) ? "0" : docLine.lastBlockId;
            queryString += $"&lastBlockId={lastBlockId}";

            var request = new HttpRequestMessage(HttpMethod.Get, queryString);

            request.Headers.Add("clientToken", _authData.token);

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request, cancellationToken);

            string res = "";

            if (response != null)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                TextReader reader = new StreamReader(responseStream);
                res = reader.ReadToEnd();

                if (response.IsSuccessStatusCode)
                {
                    GtinCodes codes = JsonSerializer.Deserialize<GtinCodes>(res);

                    if (codes != null)
                    {
                        docLine.lastBlockId = codes.blockId;

                        codes.codes.ForEach(c =>
                        {
                            CisInfo cis = new CisInfo
                            {
                                code = StringHelper.StringToBase64(c.Replace("\\u001D", "\\\\u001D")),
                                orderId = docLine.orderId,
                                gtin = docLine.gtin,
                                cisStatus = CISStatus.EMITTED,
                                clientName = docLine.clientName,
                                cis = StringHelper.StringToBase64(c.Substring(0, 31))
                            };

                            resp.Add(cis);
                        });
                    }

                    return (docLine, resp);
                }
            }

            return (docLine, resp);
        }

        public async Task<BufferInfo> GetBufferInfoAsync(OrderDocumentLine docLine, CancellationToken cancellationToken = default)
        {
            BufferInfo resp = null;

            var request = new HttpRequestMessage(HttpMethod.Get,
                $"{_settings.EmissionUrl}/buffer/status?omsId={_settings.SuzOmsId}&gtin={docLine.gtin}&orderId={docLine.orderId}");

            request.Headers.Add("clientToken", _authData.token);

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request, cancellationToken);

            string res = "";

            if (response != null)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                TextReader reader = new StreamReader(responseStream);
                res = reader.ReadToEnd();

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        resp = JsonSerializer.Deserialize<BufferInfo>(res);
                        return resp;
                    }
                    catch(Exception ex)
                    {
                        _logger.LogError($"Error during deserialize GetBufferInfoAsync response: {ex.Message} original response: {res}");
                    }
                }
            }

            return resp;
        }

        public async Task<bool> CloseOrderAsync(OrderDocumentLine docLine, CancellationToken cancellationToken = default)
        {
            string queryString = $"{_settings.EmissionUrl}/buffer/close?";
            queryString += $"omsId={_settings.SuzOmsId}";
            queryString += $"&orderId={docLine.orderId}";
            queryString += $"&gtin={docLine.gtin}";

            string lastBlockId = string.IsNullOrEmpty(docLine.lastBlockId) ? "0" : docLine.lastBlockId;
            queryString += $"&lastBlockId={lastBlockId}";

            var request = new HttpRequestMessage(HttpMethod.Post, queryString);
            request.Headers.Add("clientToken", _authData.token);

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request, cancellationToken);

            return response.IsSuccessStatusCode;
        }
    }
}
