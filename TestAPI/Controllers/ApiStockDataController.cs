﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using YahooFinanceApi;

namespace TestAPI.Controllers
{
    [Produces("application/json")]
    public class ApiStockDataController : ControllerBase
    {
        [Route("~/api/ApiStockData/{ticker}/{start}/{end}/{period}")]
        [HttpGet]
        public async Task<List<StockPriceModel>> GetStockData(string ticker, string start,
             string end, string period)
        {
            var p = Period.Daily;
            if (period.ToLower() == "weekly") p = Period.Weekly;
            else if (period.ToLower() == "monthly") p = Period.Monthly;
            var startDate = DateTime.Parse(start);
            var endDate = DateTime.Parse(end);

            var hist = await Yahoo.GetHistoricalAsync(ticker, startDate, endDate, p);

            List<StockPriceModel> models = new List<StockPriceModel>();
            foreach (var r in hist)
            {
                models.Add(new StockPriceModel
                {
                    Ticker = ticker,
                    Date = r.DateTime.ToString("yyyy-MM-dd"),
                    Open = r.Open,
                    High = r.High,
                    Low = r.Low,
                    Close = r.Close,
                    AdjustedClose = r.AdjustedClose,
                    Volume = r.Volume
                });
            }
            return models;
        }
    }
}