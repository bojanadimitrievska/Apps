using ClosedXML.Excel;
using EShopAdminApp.Models;
using GemBox.Document;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EShopAdminApp.Controllers
{
    public class OrderController : Controller
    {
        public OrderController()
        {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }
        public IActionResult Index()
        {
            HttpClient client = new HttpClient();
            string URL = "https://localhost:44336/api/admin/GetAllActiveOrders";
            HttpResponseMessage response = client.GetAsync(URL).Result;
            var data = response.Content.ReadAsAsync<List<Order>>().Result;
            return View(data);
        }
        public IActionResult GetOrderDetails(int id)
        {
            HttpClient client = new HttpClient();
            string URL = "https://localhost:44336/api/admin/GetOrderDetails";
            var model = new
            {
                Id = id
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;
            var data = response.Content.ReadAsAsync<Order>().Result;
            return View(data);
        }
        public FileResult SavePdf(int id)
        {
            HttpClient client = new HttpClient();
            string URL = "https://localhost:44336/api/admin/GetOrderDetails";
            var model = new
            {
                Id = id
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;
            var result = response.Content.ReadAsAsync<Order>().Result;
            var directoryPath = Directory.GetCurrentDirectory();
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");

            var document = DocumentModel.Load(templatePath);
            document.Content.Replace("{{OrderNumber}}", result.Id.ToString());
            document.Content.Replace("{{UserName}}", result.OrderBy.Username);

            StringBuilder sb = new StringBuilder();
            double totalPrice = 0.0;
            foreach (var item in result.Tickets)
            {
                totalPrice += item.Quantity * item.Ticket.TicketPrice;
                sb.AppendLine(item.Ticket.MovieName + ", quantitty: " + item.Quantity + ", price: " + item.Ticket.TicketPrice);
            }

            document.Content.Replace("{{TicketList}}", sb.ToString());
            document.Content.Replace("{{TotalPrice}}", totalPrice.ToString());

            var stream = new MemoryStream();
            document.Save(stream, new PdfSaveOptions());
            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ExportInvoice.pdf");

        }
        //[HttpGet]
        // public FileContentResult ExportAllOrders()
        //{

        //    string fileName = "Orders.xlsx";
        //    string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        //    using(var workBook = new XLWorkbook())
        //    {
        //        IXLWorksheet worksheet = workBook.Worksheets.Add("All Orders");

        //        worksheet.Cell(1, 1).Value = "Order Id";
        //        worksheet.Cell(1, 2).Value = "Customer Email";

        //        HttpClient client = new HttpClient();
        //        string URL = "https://localhost:44336/api/admin/GetAllActiveOrders";
        //        HttpResponseMessage response = client.GetAsync(URL).Result;
        //        var data = response.Content.ReadAsAsync<List<Order>>().Result;

        //        for(int i = 1; i <= data.Count(); i++)
        //        {
        //            var item = data[i - 1];

        //            worksheet.Cell(i + 1, 1).Value = item.Id.ToString();
        //            worksheet.Cell(i + 1, 2).Value = item.OrderBy.Email;

        //            for(int p = 0; p< item.Tickets.Count(); p++)
        //            {
        //                if(item.Tickets.ElementAt(p).Ticket.MovieName.Equals("zlaten dab"))
        //                {
        //                    worksheet.Cell(1, p + 3).Value = "Ticket-" + (p + 1);
        //                    worksheet.Cell(i + 1, p + 3).Value = item.Tickets.ElementAt(p).Ticket.MovieName;
        //                }

        //            }
        //        }

        //        using(var stream = new MemoryStream())
        //        {
        //            workBook.SaveAs(stream);
        //            var content = stream.ToArray();

        //            return File(content, contentType, fileName);
        //        }

        //    }
        //}
        [HttpGet]
        public FileContentResult ExportAllOrders()
        {

            string fileName = "Orders.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workBook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workBook.Worksheets.Add("All Orders");

                

                HttpClient client = new HttpClient();
                string URL = "https://localhost:44336/api/admin/GetAllActiveOrders";
                HttpResponseMessage response = client.GetAsync(URL).Result;
                var data = response.Content.ReadAsAsync<List<Order>>().Result;

                for (int i = 1; i <= data.Count(); i++)
                {
                    var has = false;
                    var item = data[i - 1];

                    for(int k = 0; k<item.Tickets.Count(); k++)
                    {
                        if (item.Tickets.ElementAt(k).Ticket.MovieGenre.Equals("Action"))
                        {
                            has = true;
                        }
                    }

                    if (has)
                    {
                        worksheet.Cell(1, 1).Value = "Order Id";
                        worksheet.Cell(1, 2).Value = "Customer Email";
                        worksheet.Cell(i + 1, 1).Value = item.Id.ToString();
                        worksheet.Cell(i + 1, 2).Value = item.OrderBy.Email;

                        for (int p = 0; p < item.Tickets.Count(); p++)
                        {
                            
                                worksheet.Cell(1, p + 3).Value = "Movie-" + (p + 1);
                                worksheet.Cell(i + 1, p + 3).Value = item.Tickets.ElementAt(p).Ticket.MovieName+"-"+item.Tickets.ElementAt(p).Quantity+" tickets.";
                            

                        }
                    }
                }

                using (var stream = new MemoryStream())
                {
                    workBook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content, contentType, fileName);
                }

            }
        }
    }
}
