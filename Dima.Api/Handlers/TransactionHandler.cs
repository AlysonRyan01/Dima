using Dima.Api.Data;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers;

public class TransactionHandler(AppDbContext context) : ITransactionHandler
{
    public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
    {
        try
        {
            var transaction = new Transaction
            {
                Title = request.Title,
                Type = request.Type,
                Amount = request.Amount,
                CategoryId = request.CategoryId,
                PaidOrReceivedAt = request.PaidOrReceivedAt
            };
            
            await context.Transactions.AddAsync(transaction);
            await context.SaveChangesAsync();
            
            return new Response<Transaction?>(transaction, 201, "Transacao criada com sucesso!");
        }
        catch
        {
            return new Response<Transaction?>(null, 500, "Falha ao criar uma transacao.");
        }
    }

    public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
    {
        try
        {
            var transaction = await context
                .Transactions
                .FirstOrDefaultAsync(x => x.Id == request.Id);
            
            if(transaction == null)
                return new Response<Transaction?>(null, 404, "Transacao nao encontrada");
            
            transaction.Title = request.Title;
            transaction.Type = request.Type;
            transaction.Amount = request.Amount;
            transaction.CategoryId = request.CategoryId;
            transaction.PaidOrReceivedAt = request.PaidOrReceivedAt;

            context.Transactions.Update(transaction);
            await context.SaveChangesAsync();
            
            return new Response<Transaction?>(transaction, 201, "Transacao atualizada com sucesso!");
        }
        catch 
        {
            return new Response<Transaction?>(null, 500, "Falha ao criar uma transacao.");
        }
    }

    public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
    {
        try
        {
            var transaction = await context
                .Transactions
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);
            
            if (transaction == null)
                return new Response<Transaction?>(null, 404, "Transacao nao encontrada");
            
            context.Transactions.Remove(transaction);
            await context.SaveChangesAsync();
            
            return new Response<Transaction?>(transaction, 201, "Transacao removida com sucesso!");
        }
        catch
        {
            return new Response<Transaction?>(null, 500, "Falha ao remover a transacao!");
        }
    }

    public async Task<PagedResponse<List<Transaction>?>> GetAllAsync(GetAllTransactionRequest request)
    {
        try
        {
            var query = context
                .Transactions
                .AsNoTracking()
                .Where(x => x.UserId == request.UserId)
                .OrderBy(x => x.Title);
            
            var transactions = await query.Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();
            
            var count = await query.CountAsync();
            
            return new PagedResponse<List<Transaction>?>(transactions, count, request.PageNumber, request.PageSize);
        }
        catch
        {
            return new PagedResponse<List<Transaction>?>(null, 500, "Falha ao obter as transacoes!");
        }
    }

    public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
    {
        try
        {
            var transaction = await context
                .Transactions
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && x.UserId == request.UserId);
            
            if (transaction == null)
                return new Response<Transaction?>(null, 404, "Transacao nao encontrada");
            
            return new Response<Transaction?>(transaction);
        }
        catch
        {
            return new Response<Transaction?>(null, 500, "Falha ao carregar a transacao!");

        }
    }
}