using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillSync.Core.Entities;

namespace SkillSync.Infrastructure.Data.Configurations;

public class SwapRequestConfiguration : IEntityTypeConfiguration<SwapRequest>
{
    public void Configure(EntityTypeBuilder<SwapRequest> builder)
    {
        builder.Property(s => s.Status).HasConversion<string>();

        builder.HasOne(s => s.Requester)
            .WithMany()
            .HasForeignKey(s => s.RequesterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.Receiver)
            .WithMany()
            .HasForeignKey(s => s.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.OfferedSkill)
            .WithMany()
            .HasForeignKey(s => s.OfferedSkillId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.RequestedSkill)
            .WithMany()
            .HasForeignKey(s => s.RequestedSkillId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
