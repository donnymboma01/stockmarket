using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("/api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        #region attributes

        private readonly ApplicationDBContext _context;

        #endregion

        public StockController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stocks = await _context.Stocks.ToListAsync();
            var stockDto = stocks.Select(s => s.ToStockDto());
            return Ok(stockDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stock = await _context.Stocks.FindAsync(id);

            if (stock == null)
            {
                return NotFound();
            }

            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDTO();
            await _context.Stocks.AddAsync(stockModel);
            await _context
                .SaveChangesAsync(); // méthode très importante car sans cette ligne, l'objet n'est pas sauvegardé dans la base de données/
            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto stockDto)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);

            if (stockModel == null)
            {
                return NotFound();
            }

            stockModel.Symbol = stockDto.Symbol;
            stockModel.CompanyName = stockDto.CompanyName;
            stockModel.Purchase = stockDto.Purchase;
            stockModel.LastDividend = stockDto.LastDividend;
            stockModel.Industry = stockDto.Industry;
            stockModel.MarketCap = stockDto.MarketCap;

            _context.SaveChanges();
            return Ok(stockModel.ToStockDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);

            if (stockModel == null)
            {
                return NotFound();
            }

            // Remove n'est pas une fonction awaitable ou asynchrone. Pourquoi? je ne sais pas.
            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();
            return NoContent(); // ceci est le succes dans le cadre de la suppression.
        }
    }
}