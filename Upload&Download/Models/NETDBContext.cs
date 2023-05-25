using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Upload_Download.Models
{
    public partial class NETDBContext : DbContext
    {
        public NETDBContext()
        {
        }

        public NETDBContext(DbContextOptions<NETDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DocStore> DocStores { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=PC0337;Database=NETDB;Trusted_Connection=True;Encrypt=False;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DocStore>(entity =>
            {
                entity.HasKey(e => e.DocId)
                    .HasName("PK_DOC_STORE")
                    .IsClustered(false);

                entity.ToTable("DocStore");

                entity.HasIndex(e => e.DocName, "IDX_NAME")
                    .IsClustered();

                entity.Property(e => e.DocId).HasColumnName("DocID");

                entity.Property(e => e.ContentType).HasMaxLength(100);

                entity.Property(e => e.DocName)
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.InsertionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
