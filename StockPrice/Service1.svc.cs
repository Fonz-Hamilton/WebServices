using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace StockPrice {
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1 {
        private string jsonString;      // needed to pass the string around the methods
        private string ticker = "";     // ticker taken from user

        public string GetStockPrice(string ticker) {
            string stockPrice;
            this.ticker = ticker;
            Task dummy;                 // soley for waiting

            dummy = GetJsonData();      // gets assigned the method task
            dummy.Wait();               // waits until it is completed      
            stockPrice = ParseStockPrice(jsonString);

            return stockPrice;
        }

        private void SetString(string s) {
            jsonString = s;
        }
        private string GetString() {
            return jsonString;
        }

        /** fetches data from API source
        ** No input
        ** manipulates fields, needs to be waited on so it can fetch data
        **/
        private async Task GetJsonData() {
            var body = "";
            var client = new HttpClient();
            var request = new HttpRequestMessage {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://yh-finance.p.rapidapi.com/market/v2/get-quotes?region=US&symbols=" + ticker),
                Headers =
                {
        { "x-rapidapi-host", "yh-finance.p.rapidapi.com" },
        { "x-rapidapi-key", "f28fc2c10fmsh2b419450cffeeffp18cc87jsn350b48a44481" },
    },
            };
            using (var response = await client.SendAsync(request)) {
                response.EnsureSuccessStatusCode();
                body = await response.Content.ReadAsStringAsync();

                SetString(body);
            }
        }

        /** Parses the JSON string for stock price
        ** Input is string
        ** Returns string
        **/
        private string ParseStockPrice(string s) {

            string priceString = "";
            int last = s.LastIndexOf("regularMarketPrice");

            //Console.WriteLine("the index is like: " + last);
            // find the money
            for (int i = last + 20; i < s.Length; i++) {

                if (s[i] != ',') {
                    priceString += s[i];
                }
                else {
                    i = s.Length;
                }
            }

            //Console.WriteLine("The price is: " + priceString);
            return priceString;
        }

    }
}
