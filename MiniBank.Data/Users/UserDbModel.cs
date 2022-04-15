using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniBank.Core.Domains.BankAccounts;
using MiniBank.Data.BankAccounts;

namespace MiniBank.Data.Users
{
    public class UserDbModel
    {
        public Guid Id { get; set; }

        public string Login { get; set; }

        public string Email { get; set; }

        public virtual List<BankAccountDbModel> BankAccounts { get; set; }

        internal class Map : IEntityTypeConfiguration<UserDbModel>
        {
            public void Configure(EntityTypeBuilder<UserDbModel> builder)
            {
                builder.ToTable("users");
                builder.HasKey(dbModel => dbModel.Id).HasName("pk_user");
            }
        }
    }
}