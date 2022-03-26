using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MiniBank.Data.ConvertingServices.HttpClients.Models
{
    public class ExchangeRateResponse
    {
        public DateTime Date { get; set; }

        [JsonPropertyName("Valute")] 
        public Dictionary<string, ValueItem> Currencies { get; set; }
    }

    public class ValueItem
    {
        [JsonPropertyName("ID")] 
        public string Id { get; set; }

        public string NumCode { get; set; }

        public double Value { get; set; }
    }
}