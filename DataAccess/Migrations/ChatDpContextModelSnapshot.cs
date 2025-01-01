﻿// <auto-generated />
using System;
using Business.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccess.Migrations
{
    [DbContext(typeof(ChatDpContext))]
    partial class ChatDpContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Business.Entities.Conversation", b =>
                {
                    b.Property<int>("ConversationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ConversationID"));

                    b.Property<string>("ConversationName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ConversationType")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("LastMessageID")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("datetime2");

                    b.HasKey("ConversationID");

                    b.HasIndex("LastMessageID");

                    b.ToTable("Conversations");
                });

            modelBuilder.Entity("Business.Entities.ConversationUser", b =>
                {
                    b.Property<int>("ConversationUserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ConversationUserID"));

                    b.Property<int>("ConversationID")
                        .HasColumnType("int");

                    b.Property<DateTime>("JoinedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("ConversationUserID");

                    b.HasIndex("ConversationID");

                    b.HasIndex("UserID");

                    b.ToTable("ConversationUsers");
                });

            modelBuilder.Entity("Business.Entities.Media", b =>
                {
                    b.Property<int>("MediaID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MediaID"));

                    b.Property<string>("MediaType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("MessageID")
                        .HasColumnType("int");

                    b.Property<string>("URL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UploadedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("MediaID");

                    b.HasIndex("MessageID");

                    b.ToTable("Media");
                });

            modelBuilder.Entity("Business.Entities.Message", b =>
                {
                    b.Property<int>("MessageID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MessageID"));

                    b.Property<int>("ConversationID")
                        .HasColumnType("int");

                    b.Property<string>("MessageText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("ReadStatus")
                        .HasColumnType("bit");

                    b.Property<int>("SenderID")
                        .HasColumnType("int");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("MessageID");

                    b.HasIndex("ConversationID");

                    b.HasIndex("SenderID");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Business.Entities.Notification", b =>
                {
                    b.Property<int>("NotificationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NotificationID"));

                    b.Property<int>("MessageID")
                        .HasColumnType("int");

                    b.Property<int>("UnreadCount")
                        .HasColumnType("int");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("NotificationID");

                    b.HasIndex("MessageID");

                    b.HasIndex("UserID");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("Business.Entities.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserID"));

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastTimeConnected")
                        .HasColumnType("datetime2");

                    b.Property<bool>("OnlineStatus")
                        .HasColumnType("bit");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("RefreshTokenExpiryTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserType")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Business.Entities.Conversation", b =>
                {
                    b.HasOne("Business.Entities.Message", "LastMessage")
                        .WithMany()
                        .HasForeignKey("LastMessageID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("LastMessage");
                });

            modelBuilder.Entity("Business.Entities.ConversationUser", b =>
                {
                    b.HasOne("Business.Entities.Conversation", "Conversation")
                        .WithMany("ConversationUsers")
                        .HasForeignKey("ConversationID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Business.Entities.User", "User")
                        .WithMany("ConversationUsers")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Conversation");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Business.Entities.Media", b =>
                {
                    b.HasOne("Business.Entities.Message", "Message")
                        .WithMany("Media")
                        .HasForeignKey("MessageID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Message");
                });

            modelBuilder.Entity("Business.Entities.Message", b =>
                {
                    b.HasOne("Business.Entities.Conversation", "Conversation")
                        .WithMany("Messages")
                        .HasForeignKey("ConversationID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Business.Entities.User", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Conversation");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("Business.Entities.Notification", b =>
                {
                    b.HasOne("Business.Entities.Message", "Message")
                        .WithMany()
                        .HasForeignKey("MessageID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Business.Entities.User", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Message");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Business.Entities.Conversation", b =>
                {
                    b.Navigation("ConversationUsers");

                    b.Navigation("Messages");
                });

            modelBuilder.Entity("Business.Entities.Message", b =>
                {
                    b.Navigation("Media");
                });

            modelBuilder.Entity("Business.Entities.User", b =>
                {
                    b.Navigation("ConversationUsers");

                    b.Navigation("Notifications");
                });
#pragma warning restore 612, 618
        }
    }
}
