using api.Dtos.Stock;
using api.Helpers;
using api.Models;

namespace api.Interfaces;

public interface IStockRepository
{
    Task<List<Stock>> GetAllStocksAsync(QueryObject queryObject);
    Task<Stock?> GetStockByIdAsync(int id); // ? because FirstOrDefault can be NULL
    Task<Stock> CreateStockAsync(Stock stock);
    Task<Stock?> UpdateStockAsync(int id, UpdateStockRequestDto stockRequestDto);
    Task<Stock?> DeleteStockAsync(int id);
    Task<bool> StockExists(int id);
}