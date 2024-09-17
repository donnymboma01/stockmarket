using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class StockRepository : IStockRepository
{
    private readonly ApplicationDBContext _context;

    public StockRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<List<Stock>> GetAllStocksAsync()
    {
        // return await _context.Stocks.ToListAsync();
        // Include permet d'ajouter les commentaires dans la liste des stocks
        return await _context.Stocks.Include(c => c.Comments).ToListAsync();
    }

    public async Task<Stock?> GetStockByIdAsync(int id)
    {
        return await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Stock> CreateStockAsync(Stock stock)
    {
        await _context.Stocks.AddAsync(stock);
        await _context.SaveChangesAsync();
        return stock;
    }

    public async Task<Stock?> DeleteStockAsync(int id)
    {
        var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
        if (stock == null)
        {
            return null;
        }

        _context.Stocks.Remove(stock);
        await _context.SaveChangesAsync();
        return stock;
    }

    public async Task<Stock?> UpdateStockAsync(int id, UpdateStockRequestDto stockDto)
    {
        var existingStock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
        if (existingStock == null)
        {
            return null;
        }

        existingStock.Symbol = stockDto.Symbol;
        existingStock.CompanyName = stockDto.CompanyName;
        existingStock.Purchase = stockDto.Purchase;
        existingStock.LastDividend = stockDto.LastDividend;
        existingStock.Industry = stockDto.Industry;
        existingStock.MarketCap = stockDto.MarketCap;

        await _context.SaveChangesAsync();
        return existingStock;
    }

    public async Task<bool> StockExists(int id)
    {
        return await _context.Stocks.AnyAsync(x => x.Id == id);
    }
}