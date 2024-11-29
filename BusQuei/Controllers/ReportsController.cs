using Microsoft.AspNetCore.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Linq;
using BusQuei.Context;
using BusQuei.Models;

namespace BusQuei.Controllers
{
    public class ReportsController : Controller
    {
        private readonly AppDbContext _context;

        public ReportsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GeneratePdf(string reportType)
        {
            using var memoryStream = new MemoryStream();
            var document = new Document(PageSize.A4, 10, 10, 10, 10);
            PdfWriter.GetInstance(document, memoryStream);

            document.Open();

            document.Add(new Paragraph($"Relatório: {reportType.ToUpper()}"));
            document.Add(new Paragraph($"Data de Geração: {DateTime.Now:dd/MM/yyyy HH:mm}\n"));

            switch (reportType.ToLower())
            {
                case "bus":
                    AddBusReportContent(document);
                    break;
                case "routes":
                    AddRoutesReportContent(document);
                    break;
                case "maintenance":
                    AddMaintenanceReportContent(document);
                    break;
                default:
                    document.Add(new Paragraph("Tipo de relatório desconhecido."));
                    break;
            }

            document.Close();

            var fileBytes = memoryStream.ToArray();
            return File(fileBytes, "application/pdf", $"relatorio-{reportType}.pdf");
        }

        private void AddBusReportContent(Document document)
        {
            var buses = _context.Buses
                .Select(bus => new
                {
                    bus.LicensePlate,
                    bus.Model,
                    bus.Capacity,
                    bus.Status,
                    RouteName = bus.Route != null ? bus.Route.Name : "Sem rota"
                })
                .ToList();

            document.Add(new Paragraph("\n"));

            var table = new PdfPTable(5) { WidthPercentage = 100 };
            table.SetWidths(new float[] { 15, 25, 10, 15, 35 });

            table.AddCell("Placa");
            table.AddCell("Modelo");
            table.AddCell("Capacidade");
            table.AddCell("Status");
            table.AddCell("Rota");

            foreach (var bus in buses)
            {
                table.AddCell(bus.LicensePlate);
                table.AddCell(bus.Model);
                table.AddCell(bus.Capacity.ToString());
                table.AddCell(bus.Status);
                table.AddCell(bus.RouteName);
            }

            document.Add(table);
        }

        private void AddRoutesReportContent(Document document)
        {
            var routes = _context.Routes
                .Select(route => new
                {
                    route.Name,
                    route.Departure,
                    route.Arrival,
                    DepartureTime = route.DepartureTime.ToString(@"hh\:mm"),
                    ArrivalTime = route.ArrivalTime.ToString(@"hh\:mm")
                })
                .ToList();

            document.Add(new Paragraph("\n"));

            var table = new PdfPTable(5) { WidthPercentage = 100 };
            table.SetWidths(new float[] { 20, 25, 25, 15, 15 });

            table.AddCell("Nome da Rota");
            table.AddCell("Ponto de Partida");
            table.AddCell("Ponto de Chegada");
            table.AddCell("Hora de Partida");
            table.AddCell("Hora de Chegada");

            foreach (var route in routes)
            {
                table.AddCell(route.Name);
                table.AddCell(route.Departure);
                table.AddCell(route.Arrival);
                table.AddCell(route.DepartureTime);
                table.AddCell(route.ArrivalTime);
            }

            document.Add(table);
        }

        private void AddMaintenanceReportContent(Document document)
        {
            var maintenances = _context.Maintenances
                .Select(maintenance => new
                {
                    MaintenanceDate = maintenance.Date.ToString("dd/MM/yyyy"),
                    maintenance.Type,
                    maintenance.Status,
                    BusPlate = maintenance.Bus != null ? maintenance.Bus.LicensePlate : "Sem ônibus",
                    maintenance.Remarks
                })
                .ToList();

            document.Add(new Paragraph("\n"));

            var table = new PdfPTable(5) { WidthPercentage = 100 };
            table.SetWidths(new float[] { 20, 20, 20, 20, 20 });

            table.AddCell("Data");
            table.AddCell("Tipo");
            table.AddCell("Status");
            table.AddCell("Ônibus");
            table.AddCell("Observações");

            foreach (var maintenance in maintenances)
            {
                table.AddCell(maintenance.MaintenanceDate);
                table.AddCell(maintenance.Type);
                table.AddCell(maintenance.Status);
                table.AddCell(maintenance.BusPlate);
                table.AddCell(maintenance.Remarks ?? "Nenhuma");
            }

            document.Add(table);
        }
    }
}
