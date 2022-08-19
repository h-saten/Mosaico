using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Wallet
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "wlt");

            migrationBuilder.CreateTable(
                name: "CompanyWallets",
                schema: "wlt",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrivateKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Network = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublicKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastSyncBlockHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyWallets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExchangeRates",
                schema: "wlt",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ticker = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BaseCurrency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCrypto = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeRates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExternalExchanges",
                schema: "wlt",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalExchanges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentCurrencies",
                schema: "wlt",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Ticker = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NativeChainCurrency = table.Column<bool>(type: "bit", nullable: false),
                    ContractAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Chain = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DecimalPlaces = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentCurrencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TokenTypes",
                schema: "wlt",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokenTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionStatuses",
                schema: "wlt",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionTypes",
                schema: "wlt",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wallets",
                schema: "wlt",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivateKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Network = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublicKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastSyncBlockHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tokens",
                schema: "wlt",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameNormalized = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SymbolNormalized = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnerAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalSupply = table.Column<decimal>(type: "decimal(36,18)", precision: 36, scale: 18, nullable: false),
                    TokensLeft = table.Column<decimal>(type: "decimal(36,18)", precision: 36, scale: 18, nullable: false),
                    Network = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsBurnable = table.Column<bool>(type: "bit", nullable: false),
                    IsMintable = table.Column<bool>(type: "bit", nullable: false),
                    StakingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LegacyId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractVersion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tokens_TokenTypes_TypeId",
                        column: x => x.TypeId,
                        principalSchema: "wlt",
                        principalTable: "TokenTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                schema: "wlt",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CorrelationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TokenAmount = table.Column<decimal>(type: "decimal(36,18)", precision: 36, scale: 18, nullable: true),
                    PayedAmount = table.Column<decimal>(type: "decimal(36,18)", precision: 36, scale: 18, nullable: true),
                    PaymentProcessor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentCurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InitiatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    FinishedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    FailureReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TokenId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WalletAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Network = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    From = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    To = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConfirmationAttemptsCounter = table.Column<int>(type: "int", nullable: false),
                    LastConfirmationAttemptedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    NextConfirmationAttemptAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ToDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FromDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_PaymentCurrencies_PaymentCurrencyId",
                        column: x => x.PaymentCurrencyId,
                        principalSchema: "wlt",
                        principalTable: "PaymentCurrencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_TransactionStatuses_StatusId",
                        column: x => x.StatusId,
                        principalSchema: "wlt",
                        principalTable: "TransactionStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_TransactionTypes_TypeId",
                        column: x => x.TypeId,
                        principalSchema: "wlt",
                        principalTable: "TransactionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyWalletToToken",
                schema: "wlt",
                columns: table => new
                {
                    TokenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyWalletId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyWalletToToken", x => new { x.TokenId, x.CompanyWalletId });
                    table.ForeignKey(
                        name: "FK_CompanyWalletToToken_CompanyWallets_CompanyWalletId",
                        column: x => x.CompanyWalletId,
                        principalSchema: "wlt",
                        principalTable: "CompanyWallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyWalletToToken_Tokens_TokenId",
                        column: x => x.TokenId,
                        principalSchema: "wlt",
                        principalTable: "Tokens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stakings",
                schema: "wlt",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TokenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContractAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxReward = table.Column<decimal>(type: "decimal(38,18)", nullable: false),
                    MaxParticipants = table.Column<long>(type: "bigint", nullable: false),
                    RewardCycle = table.Column<int>(type: "int", nullable: false),
                    CycleLimit = table.Column<decimal>(type: "decimal(3,2)", nullable: false),
                    ContractVersion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RewardTokenId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RewardCurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stakings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stakings_PaymentCurrencies_RewardCurrencyId",
                        column: x => x.RewardCurrencyId,
                        principalSchema: "wlt",
                        principalTable: "PaymentCurrencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Stakings_Tokens_RewardTokenId",
                        column: x => x.RewardTokenId,
                        principalSchema: "wlt",
                        principalTable: "Tokens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Stakings_Tokens_TokenId",
                        column: x => x.TokenId,
                        principalSchema: "wlt",
                        principalTable: "Tokens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TokenDistributions",
                schema: "wlt",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TokenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TokenAmount = table.Column<decimal>(type: "decimal(36,18)", precision: 36, scale: 18, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokenDistributions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TokenDistributions_Tokens_TokenId",
                        column: x => x.TokenId,
                        principalSchema: "wlt",
                        principalTable: "Tokens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TokenToExternalExchanges",
                schema: "wlt",
                columns: table => new
                {
                    TokenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExternalExchangeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokenToExternalExchanges", x => new { x.TokenId, x.ExternalExchangeId });
                    table.ForeignKey(
                        name: "FK_TokenToExternalExchanges_ExternalExchanges_ExternalExchangeId",
                        column: x => x.ExternalExchangeId,
                        principalSchema: "wlt",
                        principalTable: "ExternalExchanges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TokenToExternalExchanges_Tokens_TokenId",
                        column: x => x.TokenId,
                        principalSchema: "wlt",
                        principalTable: "Tokens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vestings",
                schema: "wlt",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TokenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vestings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vestings_Tokens_TokenId",
                        column: x => x.TokenId,
                        principalSchema: "wlt",
                        principalTable: "Tokens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VestingWallets",
                schema: "wlt",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VestingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FundId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TokenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrivateKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Network = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublicKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VestingWallets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VestingWallets_Tokens_TokenId",
                        column: x => x.TokenId,
                        principalSchema: "wlt",
                        principalTable: "Tokens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WalletToToken",
                schema: "wlt",
                columns: table => new
                {
                    TokenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WalletId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletToToken", x => new { x.TokenId, x.WalletId });
                    table.ForeignKey(
                        name: "FK_WalletToToken_Tokens_TokenId",
                        column: x => x.TokenId,
                        principalSchema: "wlt",
                        principalTable: "Tokens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WalletToToken_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalSchema: "wlt",
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VestingFunds",
                schema: "wlt",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Distribution = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DistributionPerPerson = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Days = table.Column<long>(type: "bigint", nullable: false),
                    StartAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    EndAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CanWithdrawEarly = table.Column<bool>(type: "bit", nullable: false),
                    ReleaseCycleInDays = table.Column<long>(type: "bigint", nullable: true),
                    SubtractedPercent = table.Column<long>(type: "bigint", nullable: true),
                    VestingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VestingWalletId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VestingFunds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VestingFunds_Vestings_VestingId",
                        column: x => x.VestingId,
                        principalSchema: "wlt",
                        principalTable: "Vestings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WalletToVesting",
                schema: "wlt",
                columns: table => new
                {
                    WalletId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VestingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Claimed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletToVesting", x => new { x.WalletId, x.VestingId });
                    table.ForeignKey(
                        name: "FK_WalletToVesting_VestingWallets_VestingId",
                        column: x => x.VestingId,
                        principalSchema: "wlt",
                        principalTable: "VestingWallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WalletToVesting_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalSchema: "wlt",
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VestingInvitations",
                schema: "wlt",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WalletAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsInvitationSent = table.Column<bool>(type: "bit", nullable: false),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: false),
                    AcceptedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ExpiresAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    VestingFundId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VestingInvitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VestingInvitations_VestingFunds_VestingFundId",
                        column: x => x.VestingFundId,
                        principalSchema: "wlt",
                        principalTable: "VestingFunds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "wlt",
                table: "ExternalExchanges",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "IsDisabled", "LogoUrl", "ModifiedAt", "ModifiedById", "Name", "Type", "Url", "Version" },
                values: new object[] { new Guid("bc3b535f-59db-4512-ba2c-52243cf4790d"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, false, "/assets/media/logos/kanga_logo.svg", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Kanga Exchange", 1, "https://kanga.exchange", 0L });

            migrationBuilder.InsertData(
                schema: "wlt",
                table: "PaymentCurrencies",
                columns: new[] { "Id", "Chain", "ContractAddress", "CreatedAt", "CreatedById", "DecimalPlaces", "LogoUrl", "ModifiedAt", "ModifiedById", "Name", "NativeChainCurrency", "Ticker", "Version" },
                values: new object[,]
                {
                    { new Guid("8c53a7ba-0d71-47f7-8a80-d1534656be0c"), "Ethereum", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 18, null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Ethereum", true, "ETH", 0L },
                    { new Guid("13fb17b8-5979-4258-af02-423a58c79878"), "Ethereum", "0xdAC17F958D2ee523a2206206994597C13D831ec7", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 6, null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Tether USD", false, "USDT", 0L },
                    { new Guid("f0e5097d-383d-420f-91f0-0fc7a9d2770e"), "Ethereum", "0xA0b86991c6218b36c1d19D4a2e9Eb0cE3606eB48", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 6, null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "USD Coin", false, "USDC", 0L }
                });

            migrationBuilder.InsertData(
                schema: "wlt",
                table: "TokenTypes",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "Key", "ModifiedAt", "ModifiedById", "Title", "Version" },
                values: new object[,]
                {
                    { new Guid("cddc16c2-5969-4160-ac94-67e57fe8c181"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "UTILITY", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Utility", 0L },
                    { new Guid("062ef44f-592c-4cdc-a1e8-6b2b2521ed16"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "SECURITY", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Security", 0L },
                    { new Guid("1c12c473-d4f7-4906-9700-b6ec8d2b7437"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "NFT", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "NFT", 0L }
                });

            migrationBuilder.InsertData(
                schema: "wlt",
                table: "TransactionStatuses",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "Key", "ModifiedAt", "ModifiedById", "Title", "Version" },
                values: new object[,]
                {
                    { new Guid("313f94fb-dc91-4013-9a0b-53dd94f133ec"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "PENDING", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Pending", 0L },
                    { new Guid("770de2f5-6d2f-4bcf-9b18-73a8eed114ed"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "CONFIRMED", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Confirmed", 0L },
                    { new Guid("c8605b82-a71a-4c9c-8019-a71154fd103c"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "CANCELED", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Canceled", 0L },
                    { new Guid("c154cd29-8d6b-48df-86a9-a8c979e68a25"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "FAILED", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Failed", 0L }
                });

            migrationBuilder.InsertData(
                schema: "wlt",
                table: "TransactionTypes",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "Key", "ModifiedAt", "ModifiedById", "Title", "Version" },
                values: new object[,]
                {
                    { new Guid("b0398047-84cf-4264-9cc5-4bd2c839eaed"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "DEPOSIT", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Deposit", 0L },
                    { new Guid("077cc88b-5c2a-4fce-9f41-9c6a3fea38f7"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "PURCHASE", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Purchase", 0L },
                    { new Guid("7fcd586d-10b8-4e5b-a897-ceedc91510e6"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "EXCHANGE", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Exchange", 0L },
                    { new Guid("06b797db-a303-4cfe-954a-9d18d55a4f3a"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "WITHDRAWAL", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Withdrawal", 0L },
                    { new Guid("799cac01-4e82-4b82-9a22-2d633c55df6b"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "TRANSFER", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Transfer", 0L }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyWalletToToken_CompanyWalletId",
                schema: "wlt",
                table: "CompanyWalletToToken",
                column: "CompanyWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_CreatedAt",
                schema: "wlt",
                table: "ExchangeRates",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalExchanges_Name",
                schema: "wlt",
                table: "ExternalExchanges",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentCurrencies_Name_Ticker_Chain",
                schema: "wlt",
                table: "PaymentCurrencies",
                columns: new[] { "Name", "Ticker", "Chain" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stakings_RewardCurrencyId",
                schema: "wlt",
                table: "Stakings",
                column: "RewardCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Stakings_RewardTokenId",
                schema: "wlt",
                table: "Stakings",
                column: "RewardTokenId");

            migrationBuilder.CreateIndex(
                name: "IX_Stakings_TokenId",
                schema: "wlt",
                table: "Stakings",
                column: "TokenId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TokenDistributions_TokenId",
                schema: "wlt",
                table: "TokenDistributions",
                column: "TokenId");

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_TypeId",
                schema: "wlt",
                table: "Tokens",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TokenToExternalExchanges_ExternalExchangeId",
                schema: "wlt",
                table: "TokenToExternalExchanges",
                column: "ExternalExchangeId");

            migrationBuilder.CreateIndex(
                name: "IX_TokenTypes_Key",
                schema: "wlt",
                table: "TokenTypes",
                column: "Key",
                unique: true,
                filter: "[Key] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CorrelationId",
                schema: "wlt",
                table: "Transactions",
                column: "CorrelationId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_PaymentCurrencyId",
                schema: "wlt",
                table: "Transactions",
                column: "PaymentCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_StatusId",
                schema: "wlt",
                table: "Transactions",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TypeId",
                schema: "wlt",
                table: "Transactions",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStatuses_Key",
                schema: "wlt",
                table: "TransactionStatuses",
                column: "Key",
                unique: true,
                filter: "[Key] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTypes_Key",
                schema: "wlt",
                table: "TransactionTypes",
                column: "Key",
                unique: true,
                filter: "[Key] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_VestingFunds_VestingId",
                schema: "wlt",
                table: "VestingFunds",
                column: "VestingId");

            migrationBuilder.CreateIndex(
                name: "IX_VestingInvitations_VestingFundId",
                schema: "wlt",
                table: "VestingInvitations",
                column: "VestingFundId");

            migrationBuilder.CreateIndex(
                name: "IX_Vestings_TokenId",
                schema: "wlt",
                table: "Vestings",
                column: "TokenId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VestingWallets_TokenId",
                schema: "wlt",
                table: "VestingWallets",
                column: "TokenId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletToToken_WalletId",
                schema: "wlt",
                table: "WalletToToken",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletToVesting_VestingId",
                schema: "wlt",
                table: "WalletToVesting",
                column: "VestingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyWalletToToken",
                schema: "wlt");

            migrationBuilder.DropTable(
                name: "ExchangeRates",
                schema: "wlt");

            migrationBuilder.DropTable(
                name: "Stakings",
                schema: "wlt");

            migrationBuilder.DropTable(
                name: "TokenDistributions",
                schema: "wlt");

            migrationBuilder.DropTable(
                name: "TokenToExternalExchanges",
                schema: "wlt");

            migrationBuilder.DropTable(
                name: "Transactions",
                schema: "wlt");

            migrationBuilder.DropTable(
                name: "VestingInvitations",
                schema: "wlt");

            migrationBuilder.DropTable(
                name: "WalletToToken",
                schema: "wlt");

            migrationBuilder.DropTable(
                name: "WalletToVesting",
                schema: "wlt");

            migrationBuilder.DropTable(
                name: "CompanyWallets",
                schema: "wlt");

            migrationBuilder.DropTable(
                name: "ExternalExchanges",
                schema: "wlt");

            migrationBuilder.DropTable(
                name: "PaymentCurrencies",
                schema: "wlt");

            migrationBuilder.DropTable(
                name: "TransactionStatuses",
                schema: "wlt");

            migrationBuilder.DropTable(
                name: "TransactionTypes",
                schema: "wlt");

            migrationBuilder.DropTable(
                name: "VestingFunds",
                schema: "wlt");

            migrationBuilder.DropTable(
                name: "VestingWallets",
                schema: "wlt");

            migrationBuilder.DropTable(
                name: "Wallets",
                schema: "wlt");

            migrationBuilder.DropTable(
                name: "Vestings",
                schema: "wlt");

            migrationBuilder.DropTable(
                name: "Tokens",
                schema: "wlt");

            migrationBuilder.DropTable(
                name: "TokenTypes",
                schema: "wlt");
        }
    }
}
