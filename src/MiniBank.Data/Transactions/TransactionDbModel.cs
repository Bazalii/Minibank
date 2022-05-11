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
                builder.ToTable("transactions");
                builder.HasKey(dbModel => dbModel.Id).HasName("pk_transaction");
            }
        }
    }
}