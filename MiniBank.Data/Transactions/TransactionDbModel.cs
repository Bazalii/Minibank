using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MiniBank.Data.Transactions
{
    public class TransactionDbModel
    {
        public Guid Id { get; set; }

        public double AmountOfMoney { get; set; }

        public Guid WithdrawalAccount { get; set; }

        public Guid ReplenishmentAccount { get; set; }
        
        internal class Map : IEntityTypeConfiguration<TransactionDbModel>
        {
            public void Configure(EntityTypeBuilder<TransactionDbModel> builder)
            {
                builder.HasKey(dbModel => dbModel.Id).HasName("pk_transaction");
                builder.Property(dbModel => dbModel.Id).HasColumnName("id");
                builder.Property(dbModel => dbModel.AmountOfMoney).HasColumnName("amount_of_money");
                builder.Property(dbModel => dbModel.WithdrawalAccount).HasColumnName("withdrawal_account");
                builder.Property(dbModel => dbModel.ReplenishmentAccount).HasColumnName("replenishment_account");
            }
        }
    }
}