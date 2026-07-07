using Application.CQRS.Queries.Reportes;
using Application.CQRS.Core;
using Application.DTOS.Reportes;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ReportesController : ControllerBase {
    private readonly IDispatcher _dispatcher;
    public ReportesController(IDispatcher dispatcher) => _dispatcher = dispatcher;

    // GET api/reportes?desde=2026-01-01&hasta=2026-06-30
    [HttpGet]
    [ProducesResponseType(typeof(ReportesDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(
        [FromQuery] DateTime? desde,
        [FromQuery] DateTime? hasta,
        CancellationToken ct) {
        var d = desde ?? DateTime.Today.AddMonths(-1);
        var h = hasta ?? DateTime.Today;
        var result = await _dispatcher.QueryAsync(new ObtenerReportesQuery(d, h), ct);
        return Ok(result);
    }

    // GET api/reportes/distribucion
    [HttpGet("distribucion")]
    [ProducesResponseType(typeof(ReporteDistribucionDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDistribucion(CancellationToken ct) {
        var result = await _dispatcher.QueryAsync(new ObtenerDistribucionQuery(), ct);
        return Ok(result);
    }

    // GET api/reportes/distribucion/excel
    [HttpGet("distribucion/excel")]
    public async Task<IActionResult> GetDistribucionExcel(CancellationToken ct) {
        var data = await _dispatcher.QueryAsync(new ObtenerDistribucionQuery(), ct);

        using var wb = new XLWorkbook();

        // ── Hoja 1: Por técnico ───────────────────────────────────────────
        var wsTec = wb.AddWorksheet("Por Técnico");
        string[] headersTec = ["Técnico", "Total", "Resueltos", "Cerrados", "Pendientes", "T. prom. resolución (min)"];
        for (int i = 0; i < headersTec.Length; i++)
            wsTec.Cell(1, i + 1).Value = headersTec[i];

        int row = 2;
        foreach (var t in data.PorTecnico) {
            wsTec.Cell(row, 1).Value = t.NombreTecnico;
            wsTec.Cell(row, 2).Value = t.Total;
            wsTec.Cell(row, 3).Value = t.Resueltos;
            wsTec.Cell(row, 4).Value = t.Cerrados;
            wsTec.Cell(row, 5).Value = t.Pendientes;
            if (t.TiempoPromedioResolucionMinutos.HasValue)
                wsTec.Cell(row, 6).Value = Math.Round(t.TiempoPromedioResolucionMinutos.Value, 1);
            else
                wsTec.Cell(row, 6).Value = "—";
            row++;
        }

        EstiloHeader(wsTec, headersTec.Length, data.PorTecnico.Count + 1);
        wsTec.Columns().AdjustToContents();

        // ── Hoja 2: Por sede ──────────────────────────────────────────────
        var wsSede = wb.AddWorksheet("Por Sede");
        string[] headersSede = ["Sede", "Tipo", "Total", "Resueltos", "Cerrados", "Pendientes"];
        for (int i = 0; i < headersSede.Length; i++)
            wsSede.Cell(1, i + 1).Value = headersSede[i];

        row = 2;
        foreach (var s in data.PorSede) {
            wsSede.Cell(row, 1).Value = s.NombreSede;
            wsSede.Cell(row, 2).Value = s.TipoSede;
            wsSede.Cell(row, 3).Value = s.Total;
            wsSede.Cell(row, 4).Value = s.Resueltos;
            wsSede.Cell(row, 5).Value = s.Cerrados;
            wsSede.Cell(row, 6).Value = s.Pendientes;
            row++;
        }

        EstiloHeader(wsSede, headersSede.Length, data.PorSede.Count + 1);
        wsSede.Columns().AdjustToContents();

        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        ms.Position = 0;

        var filename = $"distribucion-tickets-{DateTime.Today:yyyy-MM-dd}.xlsx";
        return File(ms.ToArray(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            filename);
    }

    private static void EstiloHeader(IXLWorksheet ws, int cols, int lastDataRow) {
        var header = ws.Range(1, 1, 1, cols);
        header.Style.Font.Bold = true;
        header.Style.Fill.BackgroundColor = XLColor.FromHtml("#FF6B35");
        header.Style.Font.FontColor = XLColor.White;

        if (lastDataRow > 1)
            ws.Range(2, 1, lastDataRow, cols).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
    }
}
