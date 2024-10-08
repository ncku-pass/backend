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
    [Migration("20210304152240_initMigration")]
    partial class initMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Infrastructure.Models.Experience", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnName("create_time")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnName("description")
                        .HasColumnType("varchar(100) CHARACTER SET utf8mb4")
                        .HasMaxLength(100);

                    b.Property<string>("Feedback")
                        .HasColumnName("feedback")
                        .HasColumnType("varchar(150) CHARACTER SET utf8mb4")
                        .HasMaxLength(150);

                    b.Property<string>("Link")
                        .HasColumnName("link")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

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

            modelBuilder.Entity("Infrastructure.Models.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasColumnType("varchar(30) CHARACTER SET utf8mb4")
                        .HasMaxLength(30);

                    b.Property<int>("UserId")
                        .HasColumnName("user_id")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("pk_tags");

                    b.HasIndex("UserId")
                        .HasName("ix_tags_user_id");

                    b.ToTable("tags");
                });

            modelBuilder.Entity("Infrastructure.Models.Tag_Experience", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int");

                    b.Property<int>("ExperienceId")
                        .HasColumnName("experience_id")
                        .HasColumnType("int");

                    b.Property<int>("TagId")
                        .HasColumnName("tag_id")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("pk_tag_experiences");

                    b.HasIndex("ExperienceId")
                        .HasName("ix_tag_experiences_experience_id");

                    b.HasIndex("TagId")
                        .HasName("ix_tag_experiences_tag_id");

                    b.ToTable("tag_experiences");
                });

            modelBuilder.Entity("Infrastructure.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnName("create_time")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .HasColumnName("email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

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
                        .HasName("pk_user");

                    b.ToTable("user");
                });

            modelBuilder.Entity("Infrastructure.Models.Experience", b =>
                {
                    b.HasOne("Infrastructure.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_experiences_user_user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Infrastructure.Models.Tag", b =>
                {
                    b.HasOne("Infrastructure.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_tags_user_user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Infrastructure.Models.Tag_Experience", b =>
                {
                    b.HasOne("Infrastructure.Models.Experience", "Experience")
                        .WithMany()
                        .HasForeignKey("ExperienceId")
                        .HasConstraintName("fk_tag_experiences_experiences_experience_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Infrastructure.Models.Tag", "Tag")
                        .WithMany()
                        .HasForeignKey("TagId")
                        .HasConstraintName("fk_tag_experiences_tags_tag_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
