﻿// <auto-generated />
using System;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20210810112733_addExpTable_CoreAbilities")]
    partial class addExpTable_CoreAbilities
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Infrastructure.Models.Card", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnName("create_time")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasColumnType("varchar(30) CHARACTER SET utf8mb4")
                        .HasMaxLength(30);

                    b.Property<int>("Order")
                        .HasColumnName("order")
                        .HasColumnType("int");

                    b.Property<int>("ResumeId")
                        .HasColumnName("resume_id")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .HasColumnName("text")
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnName("type")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnName("update_time")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UserId")
                        .HasColumnName("user_id")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("pk_cards");

                    b.HasIndex("ResumeId")
                        .HasName("ix_cards_resume_id");

                    b.HasIndex("UserId")
                        .HasName("ix_cards_user_id");

                    b.ToTable("cards");
                });

            modelBuilder.Entity("Infrastructure.Models.Card_Experience", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int");

                    b.Property<int>("CardId")
                        .HasColumnName("card_id")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnName("create_time")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("ExperienceId")
                        .HasColumnName("experience_id")
                        .HasColumnType("int");

                    b.Property<bool>("ShowFeedback")
                        .HasColumnName("show_feedback")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("ShowPosition")
                        .HasColumnName("show_position")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnName("update_time")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id")
                        .HasName("pk_card_experiences");

                    b.HasIndex("CardId")
                        .HasName("ix_card_experiences_card_id");

                    b.HasIndex("ExperienceId")
                        .HasName("ix_card_experiences_experience_id");

                    b.ToTable("card_experiences");
                });

            modelBuilder.Entity("Infrastructure.Models.Experience", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int");

                    b.Property<string>("CoreAbilities")
                        .HasColumnName("core_abilities")
                        .HasColumnType("varchar(100) CHARACTER SET utf8mb4")
                        .HasMaxLength(100);

                    b.Property<DateTime>("CreateTime")
                        .HasColumnName("create_time")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DateEnd")
                        .HasColumnName("date_end")
                        .HasColumnType("Date");

                    b.Property<DateTime>("DateStart")
                        .HasColumnName("date_start")
                        .HasColumnType("Date");

                    b.Property<string>("Description")
                        .HasColumnName("description")
                        .HasColumnType("varchar(250) CHARACTER SET utf8mb4")
                        .HasMaxLength(250);

                    b.Property<int>("ExperienceType")
                        .HasColumnName("experience_type")
                        .HasColumnType("int");

                    b.Property<string>("Feedback")
                        .HasColumnName("feedback")
                        .HasColumnType("varchar(250) CHARACTER SET utf8mb4")
                        .HasMaxLength(250);

                    b.Property<string>("Link")
                        .HasColumnName("link")
                        .HasColumnType("Text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasColumnType("varchar(30) CHARACTER SET utf8mb4")
                        .HasMaxLength(30);

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnName("position")
                        .HasColumnType("varchar(10) CHARACTER SET utf8mb4")
                        .HasMaxLength(10);

                    b.Property<string>("Semester")
                        .IsRequired()
                        .HasColumnName("semester")
                        .HasColumnType("varchar(5) CHARACTER SET utf8mb4")
                        .HasMaxLength(5);

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnName("update_time")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UserId")
                        .HasColumnName("user_id")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("pk_experiences");

                    b.HasIndex("UserId")
                        .HasName("ix_experiences_user_id");

                    b.ToTable("experiences");
                });

            modelBuilder.Entity("Infrastructure.Models.Experience_Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnName("create_time")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("ExperienceId")
                        .HasColumnName("experience_id")
                        .HasColumnType("int");

                    b.Property<int>("TagId")
                        .HasColumnName("tag_id")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnName("update_time")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id")
                        .HasName("pk_experience_tags");

                    b.HasIndex("ExperienceId")
                        .HasName("ix_experience_tags_experience_id");

                    b.HasIndex("TagId")
                        .HasName("ix_experience_tags_tag_id");

                    b.ToTable("experience_tags");
                });

            modelBuilder.Entity("Infrastructure.Models.Resume", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnName("create_time")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasColumnType("varchar(30) CHARACTER SET utf8mb4")
                        .HasMaxLength(30);

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnName("update_time")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UserId")
                        .HasColumnName("user_id")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("pk_resumes");

                    b.HasIndex("UserId")
                        .HasName("ix_resumes_user_id");

                    b.ToTable("resumes");
                });

            modelBuilder.Entity("Infrastructure.Models.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnName("create_time")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasColumnType("varchar(30) CHARACTER SET utf8mb4")
                        .HasMaxLength(30);

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnName("update_time")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UserId")
                        .HasColumnName("user_id")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("pk_tags");

                    b.HasIndex("UserId")
                        .HasName("ix_tags_user_id");

                    b.ToTable("tags");
                });

            modelBuilder.Entity("Infrastructure.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int");

                    b.Property<Guid>("AspNetId")
                        .HasColumnName("asp_net_id")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnName("create_time")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .HasColumnName("email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("EnrollmentYear")
                        .HasColumnName("enrollment_year")
                        .HasColumnType("int");

                    b.Property<int>("GraduationYear")
                        .HasColumnName("graduation_year")
                        .HasColumnType("int");

                    b.Property<string>("Major")
                        .IsRequired()
                        .HasColumnName("major")
                        .HasColumnType("varchar(20) CHARACTER SET utf8mb4")
                        .HasMaxLength(20);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasColumnType("varchar(30) CHARACTER SET utf8mb4")
                        .HasMaxLength(30);

                    b.Property<string>("StudentId")
                        .IsRequired()
                        .HasColumnName("student_id")
                        .HasColumnType("varchar(9) CHARACTER SET utf8mb4")
                        .HasMaxLength(9);

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnName("update_time")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.ToTable("users");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnName("id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnName("concurrency_stamp")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnName("normalized_name")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.HasKey("Id")
                        .HasName("pk_roles");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnName("claim_type")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ClaimValue")
                        .HasColumnName("claim_value")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnName("role_id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("Id")
                        .HasName("pk_role_claims");

                    b.HasIndex("RoleId")
                        .HasName("ix_role_claims_role_id");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnName("id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnName("access_failed_count")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnName("concurrency_stamp")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Email")
                        .HasColumnName("email")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnName("email_confirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnName("lockout_enabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnName("lockout_end")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnName("normalized_email")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnName("normalized_user_name")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnName("password_hash")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("PhoneNumber")
                        .HasColumnName("phone_number")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnName("phone_number_confirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnName("security_stamp")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnName("two_factor_enabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasColumnName("user_name")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnName("claim_type")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ClaimValue")
                        .HasColumnName("claim_value")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnName("user_id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("Id")
                        .HasName("pk_user_claims");

                    b.HasIndex("UserId")
                        .HasName("ix_user_claims_user_id");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnName("login_provider")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("ProviderKey")
                        .HasColumnName("provider_key")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnName("provider_display_name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnName("user_id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("LoginProvider", "ProviderKey")
                        .HasName("pk_user_logins");

                    b.HasIndex("UserId")
                        .HasName("ix_user_logins_user_id");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnName("user_id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("RoleId")
                        .HasColumnName("role_id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("UserId", "RoleId")
                        .HasName("pk_user_roles");

                    b.HasIndex("RoleId")
                        .HasName("ix_user_roles_role_id");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnName("user_id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("LoginProvider")
                        .HasColumnName("login_provider")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("Value")
                        .HasColumnName("value")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("UserId", "LoginProvider", "Name")
                        .HasName("pk_user_tokens");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Infrastructure.Models.Card", b =>
                {
                    b.HasOne("Infrastructure.Models.Resume", "Resume")
                        .WithMany()
                        .HasForeignKey("ResumeId")
                        .HasConstraintName("fk_cards_resumes_resume_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Infrastructure.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_cards_users_user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Infrastructure.Models.Card_Experience", b =>
                {
                    b.HasOne("Infrastructure.Models.Card", "Card")
                        .WithMany()
                        .HasForeignKey("CardId")
                        .HasConstraintName("fk_card_experiences_cards_card_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Infrastructure.Models.Experience", "Experience")
                        .WithMany()
                        .HasForeignKey("ExperienceId")
                        .HasConstraintName("fk_card_experiences_experiences_experience_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Infrastructure.Models.Experience", b =>
                {
                    b.HasOne("Infrastructure.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_experiences_users_user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Infrastructure.Models.Experience_Tag", b =>
                {
                    b.HasOne("Infrastructure.Models.Experience", "Experience")
                        .WithMany()
                        .HasForeignKey("ExperienceId")
                        .HasConstraintName("fk_experience_tags_experiences_experience_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Infrastructure.Models.Tag", "Tag")
                        .WithMany()
                        .HasForeignKey("TagId")
                        .HasConstraintName("fk_experience_tags_tags_tag_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Infrastructure.Models.Resume", b =>
                {
                    b.HasOne("Infrastructure.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_resumes_users_user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Infrastructure.Models.Tag", b =>
                {
                    b.HasOne("Infrastructure.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_tags_users_user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("fk_role_claims_asp_net_roles_identity_role_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_user_claims_asp_net_users_identity_user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_user_logins_asp_net_users_identity_user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("fk_user_roles_asp_net_roles_identity_role_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_user_roles_asp_net_users_identity_user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_user_tokens_asp_net_users_identity_user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
