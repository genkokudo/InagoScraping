using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Scraping.Model
{
    class BaseModel : BindableBase
    {
        private string url = "https://inagoflyer.appspot.com/btcmac";
        public string Url
        {
            get { return url; }
            set { SetProperty(ref url, value); }
        }

        private string display = String.Empty;
        public string Display
        {
            get { return display; }
            set { SetProperty(ref display, value); }
        }

        private IWebDriver chrome;


        #region AngleSharpで取得（没）
        public async void GetDataByAngleSharp()
        {
            //指定したサイトのHTMLをストリームで取得する
           var doc = default(IHtmlDocument);
            using (var client = new HttpClient())
            using (var stream = await client.GetStreamAsync(new Uri(url)))
            {
                // AngleSharp.Parser.Html.HtmlParserオブジェクトにHTMLをパースさせる
                var parser = new HtmlParser();
                doc = await parser.ParseAsync(stream);
            }

           // ちゃんと上からアクセスするとこうなる
           //var containerElement = doc.GetElementsByClassName("container");
           // foreach (var container in containerElement)
           // {
           //     var contentsElement = container.GetElementsByClassName("contents");
           //     foreach (var contents in contentsElement)
           //     {
           //         var priceBoardsElement = contents.GetElementsByClassName("price_board_reverse");
           //         foreach (var priceBoards in priceBoardsElement)
           //         {
           //             var volumePrateElement = priceBoards.GetElementsByClassName("flex_box volumePrate");
           //             foreach (var volumePrate in volumePrateElement)
           //             {
           //                 var priceBoardElement = volumePrate.GetElementsByClassName("priceBoard");
           //                 foreach (var priceBoard in priceBoardElement)
           //                 {
           //                     var sellVolElement = priceBoard.GetElementsByClassName("boardname");
           //                     var results = sellVolElement.Select(n =>
           //                     {
           //                         Console.WriteLine(n.TextContent);
           //                         var title = n.TextContent.Trim();
           //                         return new { title };
           //                     });

           //                     results.ToList().ForEach(item =>
           //                     {
           //                         Display = item.title;
           //                     });
           //                 }
           //             }
           //         }
           //     }
           // }

            //IDを指定したら一気に取得できる
           var containerElement = doc.GetElementById("sellVolumePerMeasurementTime");
            Display = containerElement.TextContent.Trim();
        }
        #endregion

        public void DisplayData()
        {
            if (url != string.Empty)
            {
                if (chrome == null)
                {
                    // 初回なら、WebDriverを初期化する
                    ChromeOptions options = new ChromeOptions();
                    options.AddArgument("--headless");  // ヘッドレスモードとなり、ブラウザの立ち上げがなくなる
                    chrome = new ChromeDriver(options)
                    {
                        Url = url
                    };
                }
                
                var element = chrome.FindElement(By.Id("buyVolumePerMeasurementTime"));
                Display = element.Text;
            }
        }
    }
}
