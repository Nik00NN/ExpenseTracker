﻿using ExpenseTracker.Core.Models;

namespace ExpenseTracker.API.DTOs.Transactions
{
    public class TransactionDTO
    {
        public Guid Id { get; set; } 

        public string Description { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        public bool IsRecurrent { get; set; }

        public TransactionType TransactionType { get; set; }
        public TransactionDTO()
        {

        }

        public TransactionDTO(Transaction transaction)
        {
            Id = transaction.Id;
            Description = transaction.Description;
            Amount = transaction.Amount;
            Date = transaction.Date;
            IsRecurrent = transaction.IsRecurrent;
            TransactionType = transaction.TransactionType;
        }
    }
}
