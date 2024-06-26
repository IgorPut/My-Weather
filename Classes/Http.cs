﻿using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace My_Weather.Classes
{
    internal class Http
    {
        public HttpResponseMessage response;

        static readonly HttpClient client = new HttpClient();

        public async Task
    Translate(string myText, string to, string from = "")
        {
            //HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://nlp-translation.p.rapidapi.com/v1/translate?text={myText}&to={to}&from={from}"),
                Headers =
                            {
                                { "X-RapidAPI-Key", "0e70cb33a7msh356a0987e3ae692p1b01c7jsn521f6e826d87" },
                                { "X-RapidAPI-Host", "nlp-translation.p.rapidapi.com" },
                            },
            };

            response = await client.SendAsync(request);
        }

        //public async void GetAsync<T>(Uri uri)
        //{
        //    using (var httpClient = new HttpClient())
        //    {
        //        var httpResponseMessage = await httpClient.GetAsync(uri);
        //        var content = await httpResponseMessage.Content.ReadAsStringAsync();

        //        return content;
        //    }
        //}
    }
}
