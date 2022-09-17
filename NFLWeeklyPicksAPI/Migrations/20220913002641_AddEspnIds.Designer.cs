﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NFLWeeklyPicksAPI;

#nullable disable

namespace NFLWeeklyPicksAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220913002641_AddEspnIds")]
    partial class AddEspnIds
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "e946f55f-260a-4788-a601-ff2265b05adf",
                            ConcurrencyStamp = "678a1439-697b-462e-b4d8-47365371c5c8",
                            Name = "User",
                            NormalizedName = "USER"
                        },
                        new
                        {
                            Id = "ab861aa4-a7bb-4912-ab27-6442dcfe57cb",
                            ConcurrencyStamp = "3b666bb8-e434-49e2-92bd-106b3bd8fbc4",
                            Name = "Administrator",
                            NormalizedName = "ADMINISTRATOR"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("NFLWeeklyPicksAPI.Models.Entities.Competitions", b =>
                {
                    b.Property<int>("CompetitionsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CompetitionsId"), 1L, 1);

                    b.Property<int>("AwayTeamId")
                        .HasColumnType("int");

                    b.Property<string>("AwayTeamScoreUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("EspnCompetitionId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("GameDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("GameName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("HomeTeamId")
                        .HasColumnType("int");

                    b.Property<string>("HomeTeamScoreUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Odds")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SeasonWeeksId")
                        .HasColumnType("int");

                    b.HasKey("CompetitionsId");

                    b.HasIndex("AwayTeamId");

                    b.HasIndex("HomeTeamId");

                    b.HasIndex("SeasonWeeksId");

                    b.ToTable("Competitions");
                });

            modelBuilder.Entity("NFLWeeklyPicksAPI.Models.Entities.PickTypes", b =>
                {
                    b.Property<int>("PickTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PickTypeId"), 1L, 1);

                    b.Property<int>("PickType")
                        .HasColumnType("int");

                    b.HasKey("PickTypeId");

                    b.ToTable("PickTypes");
                });

            modelBuilder.Entity("NFLWeeklyPicksAPI.Models.Entities.Season", b =>
                {
                    b.Property<int>("SeasonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SeasonId"), 1L, 1);

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("SeasonId");

                    b.ToTable("Seasons");
                });

            modelBuilder.Entity("NFLWeeklyPicksAPI.Models.Entities.SeasonWeeks", b =>
                {
                    b.Property<int>("SeasonWeeksId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SeasonWeeksId"), 1L, 1);

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("SeasonId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("WeekDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WeekNumber")
                        .HasColumnType("int");

                    b.HasKey("SeasonWeeksId");

                    b.HasIndex("SeasonId");

                    b.ToTable("SeasonWeeks");
                });

            modelBuilder.Entity("NFLWeeklyPicksAPI.Models.Entities.Teams", b =>
                {
                    b.Property<int>("TeamsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TeamsId"), 1L, 1);

                    b.Property<string>("Abbreviation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EspnTeamId")
                        .HasColumnType("int");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LogoURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nickname")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TeamsId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("NFLWeeklyPicksAPI.Models.Entities.TeamSeasonRecords", b =>
                {
                    b.Property<int>("TeamSeasonRecordsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TeamSeasonRecordsId"), 1L, 1);

                    b.Property<string>("Record")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SeasonId")
                        .HasColumnType("int");

                    b.Property<int>("TeamId")
                        .HasColumnType("int");

                    b.HasKey("TeamSeasonRecordsId");

                    b.HasIndex("SeasonId");

                    b.HasIndex("TeamId");

                    b.ToTable("TeamSeasonRecords");
                });

            modelBuilder.Entity("NFLWeeklyPicksAPI.Models.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RefreshTokenExpiryTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("NFLWeeklyPicksAPI.Models.Entities.UserPickLineItems", b =>
                {
                    b.Property<int>("UserPickLineItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserPickLineItemId"), 1L, 1);

                    b.Property<long>("CompetitionId")
                        .HasColumnType("bigint");

                    b.Property<int?>("CompetitionsId")
                        .HasColumnType("int");

                    b.Property<int>("PickPoints")
                        .HasColumnType("int");

                    b.Property<int>("PickTeamId")
                        .HasColumnType("int");

                    b.Property<int>("PickTypeId")
                        .HasColumnType("int");

                    b.Property<int>("UserPickId")
                        .HasColumnType("int");

                    b.HasKey("UserPickLineItemId");

                    b.HasIndex("CompetitionsId");

                    b.HasIndex("PickTypeId");

                    b.HasIndex("UserPickId");

                    b.ToTable("UserPickLineItems");
                });

            modelBuilder.Entity("NFLWeeklyPicksAPI.Models.Entities.UserPicks", b =>
                {
                    b.Property<int>("UserPicksId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserPicksId"), 1L, 1);

                    b.Property<bool>("IsPaid")
                        .HasColumnType("bit");

                    b.Property<int>("Season")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Week")
                        .HasColumnType("int");

                    b.HasKey("UserPicksId");

                    b.ToTable("UserPicks");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("NFLWeeklyPicksAPI.Models.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("NFLWeeklyPicksAPI.Models.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NFLWeeklyPicksAPI.Models.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("NFLWeeklyPicksAPI.Models.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NFLWeeklyPicksAPI.Models.Entities.Competitions", b =>
                {
                    b.HasOne("NFLWeeklyPicksAPI.Models.Entities.Teams", "AwayTeam")
                        .WithMany()
                        .HasForeignKey("AwayTeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NFLWeeklyPicksAPI.Models.Entities.Teams", "HomeTeam")
                        .WithMany()
                        .HasForeignKey("HomeTeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NFLWeeklyPicksAPI.Models.Entities.SeasonWeeks", "SeasonWeeks")
                        .WithMany("Competitions")
                        .HasForeignKey("SeasonWeeksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AwayTeam");

                    b.Navigation("HomeTeam");

                    b.Navigation("SeasonWeeks");
                });

            modelBuilder.Entity("NFLWeeklyPicksAPI.Models.Entities.SeasonWeeks", b =>
                {
                    b.HasOne("NFLWeeklyPicksAPI.Models.Entities.Season", "Season")
                        .WithMany("Weeks")
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Season");
                });

            modelBuilder.Entity("NFLWeeklyPicksAPI.Models.Entities.TeamSeasonRecords", b =>
                {
                    b.HasOne("NFLWeeklyPicksAPI.Models.Entities.Season", "Season")
                        .WithMany()
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NFLWeeklyPicksAPI.Models.Entities.Teams", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Season");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("NFLWeeklyPicksAPI.Models.Entities.UserPickLineItems", b =>
                {
                    b.HasOne("NFLWeeklyPicksAPI.Models.Entities.Competitions", "Competition")
                        .WithMany()
                        .HasForeignKey("CompetitionsId");

                    b.HasOne("NFLWeeklyPicksAPI.Models.Entities.PickTypes", "PickType")
                        .WithMany()
                        .HasForeignKey("PickTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NFLWeeklyPicksAPI.Models.Entities.UserPicks", "UserPick")
                        .WithMany("PickLineItems")
                        .HasForeignKey("UserPickId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Competition");

                    b.Navigation("PickType");

                    b.Navigation("UserPick");
                });

            modelBuilder.Entity("NFLWeeklyPicksAPI.Models.Entities.Season", b =>
                {
                    b.Navigation("Weeks");
                });

            modelBuilder.Entity("NFLWeeklyPicksAPI.Models.Entities.SeasonWeeks", b =>
                {
                    b.Navigation("Competitions");
                });

            modelBuilder.Entity("NFLWeeklyPicksAPI.Models.Entities.UserPicks", b =>
                {
                    b.Navigation("PickLineItems");
                });
#pragma warning restore 612, 618
        }
    }
}
