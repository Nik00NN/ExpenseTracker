﻿using System.Data.SqlClient;
using Dapper;

using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Data.Repositories
{
    public class TransactionsRespository : ITransactionsRepository
    {
        private readonly string _connectionString;
        private static string TableName => "[Transactions]";

        public TransactionsRespository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            using var conn = new SqlConnection(_connectionString);

            string query = @"
                    SELECT t.*, c.*, pc.*
                    FROM Transactions t
                    LEFT JOIN Categories c ON t.CategoryId = c.Id
                    LEFT JOIN Categories pc ON c.ParentCategoryId = pc.Id";

            var transactionsList = new List<Transaction>();

            var transactions = await conn.QueryAsync<Transaction, Category, Category, Transaction>(
                query,
                (transaction, category, parentCategory) =>
                {
                    if (parentCategory != null)
                    {
                        category.ParentCategory = parentCategory;
                    }
                    transaction.Category = category;

                    transactionsList.Add(transaction);
                    return transaction;
                },
                splitOn: "Id");

            return transactionsList;
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByTypeAsync(TransactionType transactionType)
        {
            using var conn = new SqlConnection(_connectionString);

            var query = @"
                    SELECT t.*, c.*, pc.*
                    FROM Transactions t
                    LEFT JOIN Categories c ON t.CategoryId = c.Id
                    LEFT JOIN Categories pc ON c.ParentCategoryId = pc.Id
                    WHERE t.TransactionType = @TransactionType";


            var transactions = await conn.QueryAsync<Transaction, Category, Category, Transaction>(query, (transaction, category, parentCategory) =>
            {
                if (parentCategory != null)
                {
                    category.ParentCategory = parentCategory;
                }
                transaction.Category = category;

                return transaction;
            }, new { TransactionType = transactionType }, splitOn: "Id"
            );
            return transactions;
        }

        public async Task<Transaction> GetTransactionByIdAsync(Guid transactionId)
        {
            using var conn = new SqlConnection(_connectionString);

            var query = @"
                    SELECT t.*, c.*, pc.*
                    FROM Transactions t
                    LEFT JOIN Categories c ON t.CategoryId = c.Id
                    LEFT JOIN Categories pc ON c.ParentCategoryId = pc.Id
                    WHERE t.Id = @TransactionId";

            var transaction = await conn.QueryAsync<Transaction, Category, Category, Transaction>(query, (transaction, category, parentCategory) =>
            {
                if (parentCategory != null)
                {
                    category.ParentCategory = parentCategory;
                }
                transaction.Category = category;

                return transaction;
            },
                new { TransactionId = transactionId }, splitOn: "Id"
            );

            return transaction.FirstOrDefault();
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByCategoryIdAsync(Guid categoryId)
        {
            using var conn = new SqlConnection(_connectionString);

            var query = $"SELECT * FROM {TableName} WHERE CategoryId = @CategoryId";

            return await conn.QueryAsync<Transaction>(query, new { CategoryId = categoryId });
        }
        public async Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(int userId)
        {
            using var conn = new SqlConnection(_connectionString);

            var query = @"
                    SELECT t.*, c.*, pc.*
                    FROM Transactions t
                    LEFT JOIN Categories c ON t.CategoryId = c.Id
                    LEFT JOIN Categories pc ON c.ParentCategoryId = pc.Id
                    WHERE t.UserId = @UserId";

            var transactions = await conn.QueryAsync<Transaction, Category, Category, Transaction>(query, (transaction, category, parentCategory) =>
            {
                if (parentCategory != null)
                {
                    category.ParentCategory = parentCategory;
                }
                transaction.Category = category;

                return transaction;
            }, new { UserId = userId }, splitOn: "Id"
             );
            return transactions;

        }

        public async Task<Guid> CreateTransactionAsync(Transaction transaction)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = $@"INSERT INTO {TableName} 
                              (Id, Description, Amount, Date, IsRecurrent, TransactionType, CategoryId, UserId)
                          VALUES (@Id, @Description,@Amount, @Date, @IsRecurrent, @TransactionType, @CategoryId, @UserId)";

            await connection.ExecuteAsync(query, transaction);

            return transaction.Id;
        }

        public async Task<bool> DeleteTransactionAsync(Guid transactionId)
        {
            using var conn = new SqlConnection(_connectionString);

            var query = $"DELETE FROM {TableName} WHERE Id = @Id;";

            var affectedRows = await conn.ExecuteAsync(query, new { Id = transactionId });

            return affectedRows == 1;
        }

        public async Task<Transaction?> UpdateTransactionAsync(Transaction transaction)
        {
            using var conn = new SqlConnection(_connectionString);

            var query = $@"UPDATE {TableName}
                              SET 
                                  Description = @Description,
                                  Amount = @Amount,
                                  Date = @Date,
                                  IsRecurrent = @IsRecurrent,
                                  TransactionType = @TransactionType

                          WHERE Id = @Id";

            var result = await conn.ExecuteAsync(query, transaction);

            if (result == 0)
            {
                return null;
            }

            return transaction;
        }
    }
}
