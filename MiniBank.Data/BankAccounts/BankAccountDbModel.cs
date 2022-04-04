using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniBank.Core.Enums;
using MiniBank.Data.Users;

namespace MiniBank.Data.BankAccounts
{
    public class BankAccountDbModel
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public virtual UserDbModel User { get; set; }

        public double AmountOfMoney { get; set; }

        public Currencies CurrencyCode { get; set; }

        public bool IsOpened { get; set; }

        public DateTime OpenDate { get; set; }

        public DateTime? CloseDate { get; set; }

        internal class Map : IEntityTypeConfiguration<BankAccountDbModel>
        {
            public void Configure(EntityTypeBuilder<BankAccountDbModel> builder)
            {
                builder.HasKey(dbModel => dbModel.Id).HasName("pk_bank_account");
                builder.Property(dbModel => dbModel.Id).HasColumnName("id");
                builder.Property(dbModel => dbModel.UserId).HasColumnName("user_id");
                builder.Property(dbModel => dbModel.AmountOfMoney).HasColumnName("amount_of_money");
                builder.Property(dbModel => dbModel.CurrencyCode).HasColumnName("currency_code");
                builder.Property(dbModel => dbModel.IsOpened).HasColumnName("is_opened");
                builder.Property(dbModel => dbModel.OpenDate).HasColumnName("open_date");
                builder.Property(dbModel => dbModel.CloseDate).HasColumnName("close_date");
                builder.HasOne(dbModel => dbModel.User)
                    .WithMany(dbModel => dbModel.BankAccounts)
                    .HasForeignKey(dbModel => dbModel.UserId);
            }
        }
    }
}