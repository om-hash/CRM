using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pal.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActionType = table.Column<int>(type: "int", nullable: false),
                    ActionName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TransType = table.Column<int>(type: "int", nullable: false),
                    TransReferenceId = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    UserType = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserTypeReferenceId = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    IsCustomer = table.Column<bool>(type: "bit", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserPhone = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    UserEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OldData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RelatedTo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetPhoneVerificationTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    VerificationTokenType = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    SendDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetPhoneVerificationTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    WhatsappNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PhoneNumber2 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ReferenceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserType = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RegisterdByAdmin = table.Column<bool>(type: "bit", nullable: false),
                    LastLogInDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FCMToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReferenceId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FileExtension = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Alt = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    ReferenceType = table.Column<int>(type: "int", nullable: false),
                    MediaType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChatInbox",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChatType = table.Column<int>(type: "int", nullable: false),
                    ReferenceId1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReferenceId2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReferenceName1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReferenceName2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastMsgContent = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    LastMsgDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatInbox", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerSavedSearches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsForSale = table.Column<bool>(type: "bit", nullable: true),
                    IsForRent = table.Column<bool>(type: "bit", nullable: true),
                    RealestateTypeId = table.Column<int>(type: "int", nullable: true),
                    CountryIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegionIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    neighborhoodIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: true),
                    RoomsCountIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BathRoomsCountIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FeaturesIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinPrice = table.Column<float>(type: "real", nullable: true),
                    MaxPrice = table.Column<float>(type: "real", nullable: true),
                    MinArea = table.Column<float>(type: "real", nullable: true),
                    MaxArea = table.Column<float>(type: "real", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    AddressNamesAsString = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerSavedSearches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EditableTables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tableName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EditableTables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Shortcut = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Culture = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    IsRtl = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SmsMsgs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Msg = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsMsgs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SmsTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemplateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysCallPurposes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysCallPurposes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysCallResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysCallResults", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysCallStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysCallStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysCallTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysCallTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysCompanyCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysCompanyCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysCountries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysCountries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysDealStages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysDealStages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysDealTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysDealTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysJobTitles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysJobTitles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysLeadRatings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysLeadRatings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysLeadSources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysLeadSources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysLeadStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysLeadStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysNationalities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysNationalities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysPaymentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysPaymentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysTaskStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysTaskStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemErrors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LogDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActionName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExceptionMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Importance = table.Column<int>(type: "int", nullable: false),
                    UserType = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemErrors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysViewTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysViewTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WebsiteConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsUseIFrameTemplateInControlPanel = table.Column<bool>(type: "bit", nullable: false),
                    GoogleMapsAPIKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebsiteConfigurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InboxId = table.Column<int>(type: "int", nullable: false),
                    SenderType = table.Column<int>(type: "int", nullable: false),
                    ReceiverType = table.Column<int>(type: "int", nullable: false),
                    MsgType = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MsgDateLocal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MsgDateUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatMessages_ChatInbox_InboxId",
                        column: x => x.InboxId,
                        principalTable: "ChatInbox",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ColumnDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ColId = table.Column<int>(type: "int", nullable: false),
                    TableReferenceId = table.Column<int>(type: "int", nullable: false),
                    Val = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColumnDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ColumnDetails_EditableTables_ColId",
                        column: x => x.ColId,
                        principalTable: "EditableTables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LanguageStringResources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    ResourceName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ResourceValue = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StringResourceGroup = table.Column<int>(type: "int", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageStringResources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LanguageStringResources_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationTypeId = table.Column<int>(type: "int", nullable: false),
                    NotifyDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NotificationFor = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroupId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeenUserIdsList = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_NotificationTypes_NotificationTypeId",
                        column: x => x.NotificationTypeId,
                        principalTable: "NotificationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificationTypeTranslates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationTypeId = table.Column<int>(type: "int", nullable: false),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTypeTranslates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationTypeTranslates_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificationTypeTranslates_NotificationTypes_NotificationTypeId",
                        column: x => x.NotificationTypeId,
                        principalTable: "NotificationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SmsTemplateTranslates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemplateId = table.Column<int>(type: "int", nullable: false),
                    MessageTemplate = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsTemplateTranslates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmsTemplateTranslates_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SmsTemplateTranslates_SmsTemplates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "SmsTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SysCallPurposeTranslates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CallPurposeId = table.Column<int>(type: "int", nullable: false),
                    CallPurposeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysCallPurposeTranslates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SysCallPurposeTranslates_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SysCallPurposeTranslates_SysCallPurposes_CallPurposeId",
                        column: x => x.CallPurposeId,
                        principalTable: "SysCallPurposes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SysCallResultTranslates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CallResultId = table.Column<int>(type: "int", nullable: false),
                    CallResultName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysCallResultTranslates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SysCallResultTranslates_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SysCallResultTranslates_SysCallResults_CallResultId",
                        column: x => x.CallResultId,
                        principalTable: "SysCallResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SysCallStatusTranslates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CallStatusId = table.Column<int>(type: "int", nullable: false),
                    CallStatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysCallStatusTranslates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SysCallStatusTranslates_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SysCallStatusTranslates_SysCallStatus_CallStatusId",
                        column: x => x.CallStatusId,
                        principalTable: "SysCallStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SysCallTypeTranslates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CallTypeId = table.Column<int>(type: "int", nullable: false),
                    CallTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysCallTypeTranslates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SysCallTypeTranslates_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SysCallTypeTranslates_SysCallTypes_CallTypeId",
                        column: x => x.CallTypeId,
                        principalTable: "SysCallTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SysCompanyCategoryTranslates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyCategoryId = table.Column<int>(type: "int", nullable: false),
                    CompanyCategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysCompanyCategoryTranslates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SysCompanyCategoryTranslates_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SysCompanyCategoryTranslates_SysCompanyCategories_CompanyCategoryId",
                        column: x => x.CompanyCategoryId,
                        principalTable: "SysCompanyCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SysCities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysCities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SysCities_SysCountries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "SysCountries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SysCountryTranslates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    CountryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysCountryTranslates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SysCountryTranslates_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SysCountryTranslates_SysCountries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "SysCountries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SysDealStageTranslates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StageId = table.Column<int>(type: "int", nullable: false),
                    StageName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysDealStageTranslates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SysDealStageTranslates_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SysDealStageTranslates_SysDealStages_StageId",
                        column: x => x.StageId,
                        principalTable: "SysDealStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SysDealTypeTranslates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealTypeId = table.Column<int>(type: "int", nullable: false),
                    TypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysDealTypeTranslates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SysDealTypeTranslates_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SysDealTypeTranslates_SysDealTypes_DealTypeId",
                        column: x => x.DealTypeId,
                        principalTable: "SysDealTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserType = table.Column<int>(type: "int", nullable: false),
                    JobTitleId = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_SysJobTitles_JobTitleId",
                        column: x => x.JobTitleId,
                        principalTable: "SysJobTitles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SysJobTitleTranslates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobTitleId = table.Column<int>(type: "int", nullable: false),
                    JobTitleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysJobTitleTranslates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SysJobTitleTranslates_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SysJobTitleTranslates_SysJobTitles_JobTitleId",
                        column: x => x.JobTitleId,
                        principalTable: "SysJobTitles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SysLeadRatingTranslates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeadRatingId = table.Column<int>(type: "int", nullable: false),
                    RateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysLeadRatingTranslates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SysLeadRatingTranslates_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SysLeadRatingTranslates_SysLeadRatings_LeadRatingId",
                        column: x => x.LeadRatingId,
                        principalTable: "SysLeadRatings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SysLeadSourceTranslates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeadSourceId = table.Column<int>(type: "int", nullable: false),
                    SourceName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysLeadSourceTranslates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SysLeadSourceTranslates_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SysLeadSourceTranslates_SysLeadSources_LeadSourceId",
                        column: x => x.LeadSourceId,
                        principalTable: "SysLeadSources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SysLeadStatusTranslates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeadStatusId = table.Column<int>(type: "int", nullable: false),
                    StatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysLeadStatusTranslates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SysLeadStatusTranslates_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SysLeadStatusTranslates_SysLeadStatus_LeadStatusId",
                        column: x => x.LeadStatusId,
                        principalTable: "SysLeadStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerStatus = table.Column<int>(type: "int", nullable: false),
                    NationalityId = table.Column<int>(type: "int", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    WhatsappNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsLead = table.Column<bool>(type: "bit", nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    LeadSourceId = table.Column<int>(type: "int", nullable: true),
                    LeadStatusId = table.Column<int>(type: "int", nullable: true),
                    LeadRatingId = table.Column<int>(type: "int", nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    RegionId = table.Column<int>(type: "int", nullable: true),
                    FollowedByEmpId = table.Column<int>(type: "int", maxLength: 100, nullable: false),
                    FullAddress = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    TwitterId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Facebook = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Instagram = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsRegisterdByAdmin = table.Column<bool>(type: "bit", nullable: false),
                    MainImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_SysCountries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "SysCountries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Customers_SysLeadRatings_LeadRatingId",
                        column: x => x.LeadRatingId,
                        principalTable: "SysLeadRatings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Customers_SysLeadSources_LeadSourceId",
                        column: x => x.LeadSourceId,
                        principalTable: "SysLeadSources",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Customers_SysLeadStatus_LeadStatusId",
                        column: x => x.LeadStatusId,
                        principalTable: "SysLeadStatus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Customers_SysNationalities_NationalityId",
                        column: x => x.NationalityId,
                        principalTable: "SysNationalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SysNationalityTranslates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NationalityId = table.Column<int>(type: "int", nullable: false),
                    NationalityName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysNationalityTranslates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SysNationalityTranslates_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SysNationalityTranslates_SysNationalities_NationalityId",
                        column: x => x.NationalityId,
                        principalTable: "SysNationalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SysPaymentTypeTranslates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentTypeId = table.Column<int>(type: "int", nullable: false),
                    PaymentTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysPaymentTypeTranslates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SysPaymentTypeTranslates_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SysPaymentTypeTranslates_SysPaymentTypes_PaymentTypeId",
                        column: x => x.PaymentTypeId,
                        principalTable: "SysPaymentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SysTaskStatusTranslates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskStatusId = table.Column<int>(type: "int", nullable: false),
                    StatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysTaskStatusTranslates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SysTaskStatusTranslates_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SysTaskStatusTranslates_SysTaskStatus_TaskStatusId",
                        column: x => x.TaskStatusId,
                        principalTable: "SysTaskStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SysViewTypeTranslates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ViewTypeId = table.Column<int>(type: "int", nullable: false),
                    ViewTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysViewTypeTranslates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SysViewTypeTranslates_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SysViewTypeTranslates_SysViewTypes_ViewTypeId",
                        column: x => x.ViewTypeId,
                        principalTable: "SysViewTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificationParams",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationId = table.Column<long>(type: "bigint", nullable: false),
                    Param = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationParams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationParams_Notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SysCityTranslates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    CityName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysCityTranslates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SysCityTranslates_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SysCityTranslates_SysCities_CityId",
                        column: x => x.CityId,
                        principalTable: "SysCities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SysRegions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    MainImg = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Population = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    FullAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lat = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Lng = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    LocationUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsOnlyLookup = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysRegions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SysRegions_SysCities_CityId",
                        column: x => x.CityId,
                        principalTable: "SysCities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Calls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    CallToType = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    DealId = table.Column<int>(type: "int", nullable: true),
                    CallTypeId = table.Column<int>(type: "int", nullable: false),
                    CallStatusId = table.Column<int>(type: "int", nullable: true),
                    CallPurposeId = table.Column<int>(type: "int", nullable: true),
                    CallResultId = table.Column<int>(type: "int", nullable: true),
                    CallStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CallDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsScheduled = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Calls_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Calls_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Calls_SysCallPurposes_CallPurposeId",
                        column: x => x.CallPurposeId,
                        principalTable: "SysCallPurposes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Calls_SysCallResults_CallResultId",
                        column: x => x.CallResultId,
                        principalTable: "SysCallResults",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Calls_SysCallStatus_CallStatusId",
                        column: x => x.CallStatusId,
                        principalTable: "SysCallStatus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Calls_SysCallTypes_CallTypeId",
                        column: x => x.CallTypeId,
                        principalTable: "SysCallTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerFavorites",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FavoriteType = table.Column<int>(type: "int", nullable: false),
                    FavoriteReference = table.Column<int>(type: "int", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerFavorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerFavorites_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerNotes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddedByEmpId = table.Column<int>(type: "int", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerNotes_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerNotes_Employees_AddedByEmpId",
                        column: x => x.AddedByEmpId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Deals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    LeadSourceId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    StageId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    DealName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NextStep = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<float>(type: "real", nullable: false),
                    ClosingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SuccessProbability = table.Column<float>(type: "real", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deals_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Deals_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Deals_SysDealStages_StageId",
                        column: x => x.StageId,
                        principalTable: "SysDealStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Deals_SysDealTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "SysDealTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Deals_SysLeadSources_LeadSourceId",
                        column: x => x.LeadSourceId,
                        principalTable: "SysLeadSources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Meetings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsOnline = table.Column<bool>(type: "bit", nullable: false),
                    From = table.Column<DateTime>(type: "datetime2", nullable: false),
                    To = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meetings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Meetings_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Meetings_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    TaskPriority = table.Column<int>(type: "int", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tasks_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tasks_SysTaskStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "SysTaskStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SysNeighborhoods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysNeighborhoods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SysNeighborhoods_SysRegions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "SysRegions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SysRegionTranslates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegionId = table.Column<int>(type: "int", nullable: false),
                    RegionName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SubTitle = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysRegionTranslates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SysRegionTranslates_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SysRegionTranslates_SysRegions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "SysRegions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SysNeighborhoodTranslates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NeighborhoodId = table.Column<int>(type: "int", nullable: false),
                    NeighborhoodName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysNeighborhoodTranslates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SysNeighborhoodTranslates_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SysNeighborhoodTranslates_SysNeighborhoods_NeighborhoodId",
                        column: x => x.NeighborhoodId,
                        principalTable: "SysNeighborhoods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Calls_CallPurposeId",
                table: "Calls",
                column: "CallPurposeId");

            migrationBuilder.CreateIndex(
                name: "IX_Calls_CallResultId",
                table: "Calls",
                column: "CallResultId");

            migrationBuilder.CreateIndex(
                name: "IX_Calls_CallStatusId",
                table: "Calls",
                column: "CallStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Calls_CallTypeId",
                table: "Calls",
                column: "CallTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Calls_CustomerId",
                table: "Calls",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Calls_EmployeeId",
                table: "Calls",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_InboxId",
                table: "ChatMessages",
                column: "InboxId");

            migrationBuilder.CreateIndex(
                name: "IX_ColumnDetails_ColId",
                table: "ColumnDetails",
                column: "ColId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFavorites_CustomerId",
                table: "CustomerFavorites",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerNotes_AddedByEmpId",
                table: "CustomerNotes",
                column: "AddedByEmpId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerNotes_CustomerId",
                table: "CustomerNotes",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CountryId",
                table: "Customers",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_LeadRatingId",
                table: "Customers",
                column: "LeadRatingId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_LeadSourceId",
                table: "Customers",
                column: "LeadSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_LeadStatusId",
                table: "Customers",
                column: "LeadStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_NationalityId",
                table: "Customers",
                column: "NationalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_CustomerId",
                table: "Deals",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_EmployeeId",
                table: "Deals",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_LeadSourceId",
                table: "Deals",
                column: "LeadSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_StageId",
                table: "Deals",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_TypeId",
                table: "Deals",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_JobTitleId",
                table: "Employees",
                column: "JobTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageStringResources_LanguageId",
                table: "LanguageStringResources",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_CustomerId",
                table: "Meetings",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_EmployeeId",
                table: "Meetings",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationParams_NotificationId",
                table: "NotificationParams",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_NotificationTypeId",
                table: "Notifications",
                column: "NotificationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTypeTranslates_LanguageId",
                table: "NotificationTypeTranslates",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTypeTranslates_NotificationTypeId",
                table: "NotificationTypeTranslates",
                column: "NotificationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SmsTemplateTranslates_LanguageId",
                table: "SmsTemplateTranslates",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_SmsTemplateTranslates_TemplateId",
                table: "SmsTemplateTranslates",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_SysCallPurposeTranslates_CallPurposeId",
                table: "SysCallPurposeTranslates",
                column: "CallPurposeId");

            migrationBuilder.CreateIndex(
                name: "IX_SysCallPurposeTranslates_LanguageId",
                table: "SysCallPurposeTranslates",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_SysCallResultTranslates_CallResultId",
                table: "SysCallResultTranslates",
                column: "CallResultId");

            migrationBuilder.CreateIndex(
                name: "IX_SysCallResultTranslates_LanguageId",
                table: "SysCallResultTranslates",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_SysCallStatusTranslates_CallStatusId",
                table: "SysCallStatusTranslates",
                column: "CallStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_SysCallStatusTranslates_LanguageId",
                table: "SysCallStatusTranslates",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_SysCallTypeTranslates_CallTypeId",
                table: "SysCallTypeTranslates",
                column: "CallTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SysCallTypeTranslates_LanguageId",
                table: "SysCallTypeTranslates",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_SysCities_CountryId",
                table: "SysCities",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_SysCityTranslates_CityId",
                table: "SysCityTranslates",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_SysCityTranslates_LanguageId",
                table: "SysCityTranslates",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_SysCompanyCategoryTranslates_CompanyCategoryId",
                table: "SysCompanyCategoryTranslates",
                column: "CompanyCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SysCompanyCategoryTranslates_LanguageId",
                table: "SysCompanyCategoryTranslates",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_SysCountryTranslates_CountryId",
                table: "SysCountryTranslates",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_SysCountryTranslates_LanguageId",
                table: "SysCountryTranslates",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_SysDealStageTranslates_LanguageId",
                table: "SysDealStageTranslates",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_SysDealStageTranslates_StageId",
                table: "SysDealStageTranslates",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_SysDealTypeTranslates_DealTypeId",
                table: "SysDealTypeTranslates",
                column: "DealTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SysDealTypeTranslates_LanguageId",
                table: "SysDealTypeTranslates",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_SysJobTitleTranslates_JobTitleId",
                table: "SysJobTitleTranslates",
                column: "JobTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_SysJobTitleTranslates_LanguageId",
                table: "SysJobTitleTranslates",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_SysLeadRatingTranslates_LanguageId",
                table: "SysLeadRatingTranslates",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_SysLeadRatingTranslates_LeadRatingId",
                table: "SysLeadRatingTranslates",
                column: "LeadRatingId");

            migrationBuilder.CreateIndex(
                name: "IX_SysLeadSourceTranslates_LanguageId",
                table: "SysLeadSourceTranslates",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_SysLeadSourceTranslates_LeadSourceId",
                table: "SysLeadSourceTranslates",
                column: "LeadSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_SysLeadStatusTranslates_LanguageId",
                table: "SysLeadStatusTranslates",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_SysLeadStatusTranslates_LeadStatusId",
                table: "SysLeadStatusTranslates",
                column: "LeadStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_SysNationalityTranslates_LanguageId",
                table: "SysNationalityTranslates",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_SysNationalityTranslates_NationalityId",
                table: "SysNationalityTranslates",
                column: "NationalityId");

            migrationBuilder.CreateIndex(
                name: "IX_SysNeighborhoods_RegionId",
                table: "SysNeighborhoods",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_SysNeighborhoodTranslates_LanguageId",
                table: "SysNeighborhoodTranslates",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_SysNeighborhoodTranslates_NeighborhoodId",
                table: "SysNeighborhoodTranslates",
                column: "NeighborhoodId");

            migrationBuilder.CreateIndex(
                name: "IX_SysPaymentTypeTranslates_LanguageId",
                table: "SysPaymentTypeTranslates",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_SysPaymentTypeTranslates_PaymentTypeId",
                table: "SysPaymentTypeTranslates",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SysRegions_CityId",
                table: "SysRegions",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_SysRegionTranslates_LanguageId",
                table: "SysRegionTranslates",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_SysRegionTranslates_RegionId",
                table: "SysRegionTranslates",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_SysTaskStatusTranslates_LanguageId",
                table: "SysTaskStatusTranslates",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_SysTaskStatusTranslates_TaskStatusId",
                table: "SysTaskStatusTranslates",
                column: "TaskStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_SysViewTypeTranslates_LanguageId",
                table: "SysViewTypeTranslates",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_SysViewTypeTranslates_ViewTypeId",
                table: "SysViewTypeTranslates",
                column: "ViewTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_CustomerId",
                table: "Tasks",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_EmployeeId",
                table: "Tasks",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_StatusId",
                table: "Tasks",
                column: "StatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityLogs");

            migrationBuilder.DropTable(
                name: "AspNetPhoneVerificationTokens");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "Calls");

            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropTable(
                name: "ColumnDetails");

            migrationBuilder.DropTable(
                name: "CustomerFavorites");

            migrationBuilder.DropTable(
                name: "CustomerNotes");

            migrationBuilder.DropTable(
                name: "CustomerSavedSearches");

            migrationBuilder.DropTable(
                name: "Deals");

            migrationBuilder.DropTable(
                name: "LanguageStringResources");

            migrationBuilder.DropTable(
                name: "Meetings");

            migrationBuilder.DropTable(
                name: "NotificationParams");

            migrationBuilder.DropTable(
                name: "NotificationTypeTranslates");

            migrationBuilder.DropTable(
                name: "SmsMsgs");

            migrationBuilder.DropTable(
                name: "SmsTemplateTranslates");

            migrationBuilder.DropTable(
                name: "SysCallPurposeTranslates");

            migrationBuilder.DropTable(
                name: "SysCallResultTranslates");

            migrationBuilder.DropTable(
                name: "SysCallStatusTranslates");

            migrationBuilder.DropTable(
                name: "SysCallTypeTranslates");

            migrationBuilder.DropTable(
                name: "SysCityTranslates");

            migrationBuilder.DropTable(
                name: "SysCompanyCategoryTranslates");

            migrationBuilder.DropTable(
                name: "SysCountryTranslates");

            migrationBuilder.DropTable(
                name: "SysDealStageTranslates");

            migrationBuilder.DropTable(
                name: "SysDealTypeTranslates");

            migrationBuilder.DropTable(
                name: "SysJobTitleTranslates");

            migrationBuilder.DropTable(
                name: "SysLeadRatingTranslates");

            migrationBuilder.DropTable(
                name: "SysLeadSourceTranslates");

            migrationBuilder.DropTable(
                name: "SysLeadStatusTranslates");

            migrationBuilder.DropTable(
                name: "SysNationalityTranslates");

            migrationBuilder.DropTable(
                name: "SysNeighborhoodTranslates");

            migrationBuilder.DropTable(
                name: "SysPaymentTypeTranslates");

            migrationBuilder.DropTable(
                name: "SysRegionTranslates");

            migrationBuilder.DropTable(
                name: "SysTaskStatusTranslates");

            migrationBuilder.DropTable(
                name: "SystemErrors");

            migrationBuilder.DropTable(
                name: "SysViewTypeTranslates");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "WebsiteConfigurations");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ChatInbox");

            migrationBuilder.DropTable(
                name: "EditableTables");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "SmsTemplates");

            migrationBuilder.DropTable(
                name: "SysCallPurposes");

            migrationBuilder.DropTable(
                name: "SysCallResults");

            migrationBuilder.DropTable(
                name: "SysCallStatus");

            migrationBuilder.DropTable(
                name: "SysCallTypes");

            migrationBuilder.DropTable(
                name: "SysCompanyCategories");

            migrationBuilder.DropTable(
                name: "SysDealStages");

            migrationBuilder.DropTable(
                name: "SysDealTypes");

            migrationBuilder.DropTable(
                name: "SysNeighborhoods");

            migrationBuilder.DropTable(
                name: "SysPaymentTypes");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "SysViewTypes");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "SysTaskStatus");

            migrationBuilder.DropTable(
                name: "NotificationTypes");

            migrationBuilder.DropTable(
                name: "SysRegions");

            migrationBuilder.DropTable(
                name: "SysLeadRatings");

            migrationBuilder.DropTable(
                name: "SysLeadSources");

            migrationBuilder.DropTable(
                name: "SysLeadStatus");

            migrationBuilder.DropTable(
                name: "SysNationalities");

            migrationBuilder.DropTable(
                name: "SysJobTitles");

            migrationBuilder.DropTable(
                name: "SysCities");

            migrationBuilder.DropTable(
                name: "SysCountries");
        }
    }
}
