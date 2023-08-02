﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Scribble.Content.Infrastructure;

#nullable disable

namespace Scribble.Content.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "uuid-ossp");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GroupsCategory", b =>
                {
                    b.Property<Guid>("GroupId")
                        .HasColumnType("uuid")
                        .HasColumnName("group_id");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid")
                        .HasColumnName("category_id");

                    b.HasKey("GroupId", "CategoryId")
                        .HasName("pk_groups_categories");

                    b.HasIndex("CategoryId");

                    b.ToTable("groups_categories", (string)null);
                });

            modelBuilder.Entity("PostsTag", b =>
                {
                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid")
                        .HasColumnName("post_id");

                    b.Property<Guid>("TagId")
                        .HasColumnType("uuid")
                        .HasColumnName("tag_id");

                    b.HasKey("PostId", "TagId")
                        .HasName("pk_posts_tags");

                    b.HasIndex("TagId");

                    b.ToTable("posts_tags", (string)null);
                });

            modelBuilder.Entity("Scribble.Content.Models.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("category_name");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_categories_id");

                    b.HasIndex("UserId");

                    b.HasIndex(new[] { "Name" }, "categories_category_name_key")
                        .IsUnique();

                    b.ToTable("categories", (string)null);
                });

            modelBuilder.Entity("Scribble.Content.Models.Entities.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime>("CreatedOnUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_on_utc")
                        .HasDefaultValueSql("LOCALTIMESTAMP(0)");

                    b.Property<DateTime?>("ModifiedOnUtc")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("modified_on_utc");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid")
                        .HasColumnName("post_id");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("comment_text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_comments_id");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("comments", (string)null);
                });

            modelBuilder.Entity("Scribble.Content.Models.Entities.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime>("CreatedOnUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_on_utc")
                        .HasDefaultValueSql("LOCALTIMESTAMP(0)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("character varying(400)")
                        .HasColumnName("group_description");

                    b.Property<DateTime?>("ModifiedOnUtc")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("modified_on_utc");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(75)
                        .HasColumnType("character varying(75)")
                        .HasColumnName("group_name");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("character varying(45)")
                        .HasColumnName("group_short_name");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_groups_id");

                    b.HasIndex("UserId");

                    b.ToTable("groups", (string)null);
                });

            modelBuilder.Entity("Scribble.Content.Models.Entities.Like", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("ModifiedOnUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid")
                        .HasColumnName("post_id");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_like_id");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("likes", (string)null);
                });

            modelBuilder.Entity("Scribble.Content.Models.Entities.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("post_content");

                    b.Property<DateTime>("CreatedOnUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_on_utc")
                        .HasDefaultValueSql("LOCALTIMESTAMP(0)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("post_description");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uuid")
                        .HasColumnName("group_id");

                    b.Property<DateTime?>("ModifiedOnUtc")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("modified_on_utc");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("post_title");

                    b.HasKey("Id")
                        .HasName("pk_posts_id");

                    b.HasIndex("GroupId");

                    b.ToTable("posts", (string)null);
                });

            modelBuilder.Entity("Scribble.Content.Models.Entities.Subscription", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uuid")
                        .HasColumnName("group_id");

                    b.Property<DateTime?>("ModifiedOnUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_subscription_id");

                    b.HasIndex("GroupId");

                    b.ToTable("subscriptions", (string)null);
                });

            modelBuilder.Entity("Scribble.Content.Models.Entities.Tag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("ModifiedOnUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("tag_name");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_tags_id");

                    b.HasIndex("UserId");

                    b.HasIndex(new[] { "Name" }, "tags_tag_name_key")
                        .IsUnique();

                    b.ToTable("tags", (string)null);
                });

            modelBuilder.Entity("Scribble.Content.Models.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.HasKey("Id")
                        .HasName("pk_users_id");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("GroupsCategory", b =>
                {
                    b.HasOne("Scribble.Content.Models.Entities.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_groups_categories_category_id");

                    b.HasOne("Scribble.Content.Models.Entities.Group", null)
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_groups_categories_group_id");
                });

            modelBuilder.Entity("PostsTag", b =>
                {
                    b.HasOne("Scribble.Content.Models.Entities.Post", null)
                        .WithMany()
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_posts_tags_post_id");

                    b.HasOne("Scribble.Content.Models.Entities.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_posts_tags_tag_id");
                });

            modelBuilder.Entity("Scribble.Content.Models.Entities.Category", b =>
                {
                    b.HasOne("Scribble.Content.Models.Entities.User", "User")
                        .WithMany("Categories")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_users_category_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Scribble.Content.Models.Entities.Comment", b =>
                {
                    b.HasOne("Scribble.Content.Models.Entities.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_comments_post_id");

                    b.HasOne("Scribble.Content.Models.Entities.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_users_comment_id");

                    b.Navigation("Post");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Scribble.Content.Models.Entities.Group", b =>
                {
                    b.HasOne("Scribble.Content.Models.Entities.User", "User")
                        .WithMany("Groups")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_users_group_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Scribble.Content.Models.Entities.Like", b =>
                {
                    b.HasOne("Scribble.Content.Models.Entities.Post", null)
                        .WithMany("Likes")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Scribble.Content.Models.Entities.User", "User")
                        .WithMany("Likes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_users_like_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Scribble.Content.Models.Entities.Post", b =>
                {
                    b.HasOne("Scribble.Content.Models.Entities.Group", "Group")
                        .WithMany("Posts")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_posts_group_id");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("Scribble.Content.Models.Entities.Subscription", b =>
                {
                    b.HasOne("Scribble.Content.Models.Entities.Group", null)
                        .WithMany("Subscriptions")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Scribble.Content.Models.Entities.Tag", b =>
                {
                    b.HasOne("Scribble.Content.Models.Entities.User", "User")
                        .WithMany("Tags")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_users_tag_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Scribble.Content.Models.Entities.Group", b =>
                {
                    b.Navigation("Posts");

                    b.Navigation("Subscriptions");
                });

            modelBuilder.Entity("Scribble.Content.Models.Entities.Post", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Likes");
                });

            modelBuilder.Entity("Scribble.Content.Models.Entities.User", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("Comments");

                    b.Navigation("Groups");

                    b.Navigation("Likes");

                    b.Navigation("Tags");
                });
#pragma warning restore 612, 618
        }
    }
}
