using api.Data;
using api.Dtos.Stock;
using api.Helpers;
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

    public async Task<List<Stock>> GetAllStocksAsync(QueryObject queryObject)
    {
        // return await _context.Stocks.ToListAsync();
        // Include permet d'ajouter les commentaires dans la liste des stocks
        var stocks = _context.Stocks.Include(c => c.Comments).AsQueryable();
        if (!string.IsNullOrWhiteSpace(queryObject.CompanyName))
        {
            stocks = stocks.Where(s => s.CompanyName.Contains(queryObject.CompanyName));
        }

        if (!string.IsNullOrWhiteSpace(queryObject.Symbol))
        {
            stocks = stocks.Where(s => s.Symbol.Contains(queryObject.Symbol));
        }

        if (!string.IsNullOrWhiteSpace(queryObject.SortBy))
        {
            if (queryObject.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
            {
                stocks = queryObject.IsDescending
                    ? stocks.OrderByDescending(s => s.Symbol)
                    : stocks.OrderBy(s => s.Symbol);
            }
        }

        var skipNumber = (queryObject.PageNumber - 1) * queryObject.PageSize;

        // return await _context.Stocks.Include(c => c.Comments).ToListAsync();
        return await stocks.Skip(skipNumber).Take(queryObject.PageSize).ToListAsync();
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