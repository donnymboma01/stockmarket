using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("/api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {

        // private readonly ApplicationDBContext _context; // plus besoin d'appeler ceci dans le constructeur.
        private readonly IStockRepository _stockRepository;

        public StockController(IStockRepository stockRepository)
        {
            // _context = context;
            _stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject queryObject)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            // var stocks = await _context.Stocks.ToListAsync(); --> ancienne version, maintenant les couches sont separées.
            var stocks = await _stockRepository.GetAllStocksAsync(queryObject);
            var stockDto = stocks.Select(s => s.ToStockDto());
            return Ok(stockDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var stock = await _stockRepository.GetStockByIdAsync(id);
            if (stock == null)
            {
                return NotFound();
            }

            return Ok(stock.ToStockDto());
            // var stock = await _context.Stocks.FindAsync(id);
            //
            // if (stock == null)
            // {
            //     return NotFound();
            // }
            //
            // return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var stockModel = stockDto.ToStockFromCreateDTO(); // Ne change pas !
            // await _context.Stocks.AddAsync(stockModel);
            // await _context
            //     .SaveChangesAsync(); // méthode très importante car sans cette ligne, l'objet n'est pas sauvegardé dans la base de données/
            await _stockRepository.CreateStockAsync(stockModel);
            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id :int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto stockDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            // var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            var stockModel = await _stockRepository.UpdateStockAsync(id, stockDto);

            if (stockModel == null)
            {
                return NotFound();
            }

            // stockModel.Symbol = stockDto.Symbol;
            // stockModel.CompanyName = stockDto.CompanyName;
            // stockModel.Purchase = stockDto.Purchase;
            // stockModel.LastDividend = stockDto.LastDividend;
            // stockModel.Industry = stockDto.Industry;
            // stockModel.MarketCap = stockDto.MarketCap;

            // _context.SaveChanges();
            return Ok(stockModel.ToStockDto());
        }

        [HttpDelete]
        [Route("{id :int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            // var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            var stockModel = await _stockRepository.DeleteStockAsync(id);

            if (stockModel == null)
            {
                return NotFound();
            }

            return NoContent(); // ceci est le succes dans le cadre de la suppression.

            // Remove n'est pas une fonction awaitable ou asynchrone. Pourquoi? je ne sais pas.
            // _context.Stocks.Remove(stockModel);
            // await _context.SaveChangesAsync();
        }
    }
}