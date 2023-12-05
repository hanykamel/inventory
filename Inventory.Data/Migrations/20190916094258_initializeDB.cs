using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class initializeDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdditionDocumentType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionDocumentType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AttachmentType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Budget",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budget", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExaminationStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExaminationStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExchangeOrderStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeOrderStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExternalEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HistoryAction",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryAction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Id1 = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobTitle",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobTitle", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Operation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StoreItemStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreItemStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StoreType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Supplier",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplier", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TechnicalDepartment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    TechnicianId = table.Column<string>(maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicalDepartment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransformationStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransformationStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransformationStoreItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    StoreItemId = table.Column<Guid>(nullable: false),
                    TransformationId = table.Column<Guid>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    NoteCreatorId = table.Column<string>(maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransformationStoreItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Unit",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Attachment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    FileName = table.Column<string>(nullable: false),
                    FileUrl = table.Column<string>(nullable: false),
                    FileSize = table.Column<long>(nullable: false),
                    FileExtention = table.Column<string>(nullable: false),
                    AttachmentTypeId = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachment_AttachmentType1",
                        column: x => x.AttachmentTypeId,
                        principalTable: "AttachmentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    CardCode = table.Column<string>(nullable: true),
                    DepartmentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Department",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OperationAttachmentType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    OperationId = table.Column<int>(nullable: false),
                    BudgetId = table.Column<int>(nullable: true),
                    AttachmentTypeId = table.Column<int>(nullable: false),
                    AdditionDocumentTypeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationAttachmentType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperationAttachmentType_AdditionDocumentType_AdditionDocumentTypeId",
                        column: x => x.AdditionDocumentTypeId,
                        principalTable: "AdditionDocumentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OperationAttachmentType_AttachmentType",
                        column: x => x.AttachmentTypeId,
                        principalTable: "AttachmentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OperationAttachmentType_Budget_BudgetId",
                        column: x => x.BudgetId,
                        principalTable: "Budget",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OperationAttachmentType_Operation",
                        column: x => x.OperationId,
                        principalTable: "Operation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Store",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    AdminId = table.Column<string>(maxLength: 450, nullable: true),
                    StoreTypeId = table.Column<int>(nullable: false),
                    TechnicalDepartmentId = table.Column<int>(nullable: true),
                    RobbingBudgetId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(maxLength: 150, nullable: false),
                    TenantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Store", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Store_Budget",
                        column: x => x.RobbingBudgetId,
                        principalTable: "Budget",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Store_StoreType",
                        column: x => x.StoreTypeId,
                        principalTable: "StoreType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Store_TechnicalDepartment",
                        column: x => x.TechnicalDepartmentId,
                        principalTable: "TechnicalDepartment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BaseItem",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    ShortName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Consumed = table.Column<bool>(nullable: false),
                    DefaultUnitId = table.Column<int>(nullable: false),
                    WarningLevel = table.Column<int>(nullable: true),
                    ItemCategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseItem_Unit",
                        column: x => x.DefaultUnitId,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseItem_ItemCategory",
                        column: x => x.ItemCategoryId,
                        principalTable: "ItemCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExchangeOrder",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    Code = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    BugetId = table.Column<int>(nullable: false),
                    OperationId = table.Column<int>(nullable: false),
                    ForEmployeeId = table.Column<int>(nullable: false),
                    DirectOrderNotes = table.Column<string>(nullable: false),
                    ExchangeOrderStatusId = table.Column<int>(nullable: true),
                    IsDirectOrder = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExchangeOrder_Budget",
                        column: x => x.BugetId,
                        principalTable: "Budget",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExchangeOrder_ExchangeOrderStatus",
                        column: x => x.ExchangeOrderStatusId,
                        principalTable: "ExchangeOrderStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExchangeOrder_Employees",
                        column: x => x.ForEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExchangeOrder_Operation",
                        column: x => x.OperationId,
                        principalTable: "Operation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Invoice",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    Code = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    ReceivedEmployeeId = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoice_Employees",
                        column: x => x.ReceivedEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Book",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    Number = table.Column<int>(nullable: false),
                    Consumed = table.Column<bool>(nullable: false),
                    StoreId = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Book_Store",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExaminationCommitte",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    Code = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    BudgetId = table.Column<int>(nullable: false),
                    StoreId = table.Column<int>(nullable: false),
                    DecisionNumber = table.Column<string>(nullable: true),
                    DecisionDate = table.Column<DateTime>(type: "date", nullable: true),
                    SupplyOrderNumber = table.Column<string>(nullable: true),
                    SupplyOrderDate = table.Column<DateTime>(type: "date", nullable: true),
                    SupplierId = table.Column<int>(nullable: true),
                    ContractNumber = table.Column<string>(nullable: true),
                    ContractDate = table.Column<DateTime>(type: "date", nullable: true),
                    ForConsumedItems = table.Column<bool>(nullable: false),
                    OperationId = table.Column<int>(nullable: false),
                    ExternalEntityId = table.Column<int>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    ExaminationStatusId = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    Serial = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExaminationCommitte", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExaminationCommitte_Budget",
                        column: x => x.BudgetId,
                        principalTable: "Budget",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExaminationCommitte_ExaminationStatus",
                        column: x => x.ExaminationStatusId,
                        principalTable: "ExaminationStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExaminationCommitte_ExternalEntity",
                        column: x => x.ExternalEntityId,
                        principalTable: "ExternalEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExaminationCommitte_Operation",
                        column: x => x.OperationId,
                        principalTable: "Operation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExaminationCommitte_Store",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExaminationCommitte_Supplier",
                        column: x => x.SupplierId,
                        principalTable: "Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transformation",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    FromStoreId = table.Column<int>(nullable: false),
                    ToStoreId = table.Column<int>(nullable: true),
                    ToExternalEntityId = table.Column<int>(nullable: true),
                    OperationId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(nullable: false),
                    BudgetId = table.Column<int>(nullable: true),
                    TransformationStatusId = table.Column<int>(nullable: true),
                    Date = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transformation_Budget",
                        column: x => x.BudgetId,
                        principalTable: "Budget",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transformation_Store",
                        column: x => x.FromStoreId,
                        principalTable: "Store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transformation_Operation",
                        column: x => x.OperationId,
                        principalTable: "Operation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transformation_ExternalEntity",
                        column: x => x.ToExternalEntityId,
                        principalTable: "ExternalEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transformation_TransformationStatus",
                        column: x => x.TransformationStatusId,
                        principalTable: "TransformationStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CommitteeAttachment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    AttachmentId = table.Column<Guid>(nullable: false),
                    CommitteeId = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommitteeAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommitteeAttachment_Attachment",
                        column: x => x.AttachmentId,
                        principalTable: "Attachment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommitteeAttachment_ExaminationCommitte",
                        column: x => x.CommitteeId,
                        principalTable: "ExaminationCommitte",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CommitteeEmployee",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    EmplyeeId = table.Column<int>(nullable: false),
                    JobTitleId = table.Column<int>(nullable: false),
                    ExaminationCommitteId = table.Column<Guid>(nullable: false),
                    IsHead = table.Column<bool>(nullable: false),
                    TenantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommitteeEmployee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommitteeEmployee_Employees",
                        column: x => x.EmplyeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommitteeEmployee_ExaminationCommitte",
                        column: x => x.ExaminationCommitteId,
                        principalTable: "ExaminationCommitte",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommitteeEmployee_JobTitle",
                        column: x => x.JobTitleId,
                        principalTable: "JobTitle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CommitteeItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    BaseItemId = table.Column<long>(nullable: false),
                    ExaminationCommitteId = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    UnitId = table.Column<int>(nullable: false),
                    ExaminationPercentage = table.Column<int>(nullable: false),
                    Accepted = table.Column<int>(nullable: false),
                    Rejected = table.Column<int>(nullable: true),
                    Reasons = table.Column<string>(nullable: true),
                    AdditionNotes = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommitteeItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommitteeItem_BaseItem",
                        column: x => x.BaseItemId,
                        principalTable: "BaseItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommitteeItem_ExaminationCommitte",
                        column: x => x.ExaminationCommitteId,
                        principalTable: "ExaminationCommitte",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommitteeItem_Unit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Addition",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    ExaminationCommitteId = table.Column<Guid>(nullable: true),
                    TransformationId = table.Column<Guid>(nullable: true),
                    Code = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    BudgetId = table.Column<int>(nullable: false),
                    OperationId = table.Column<int>(nullable: false),
                    AdditionDocumentTypeId = table.Column<int>(nullable: false),
                    AdditionDocumentNumber = table.Column<string>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    RequesterName = table.Column<string>(nullable: true),
                    RequestDate = table.Column<DateTime>(nullable: false),
                    AdditionDocumentDate = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    Serial = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addition_AdditionDocumentType",
                        column: x => x.AdditionDocumentTypeId,
                        principalTable: "AdditionDocumentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Addition_Budget",
                        column: x => x.BudgetId,
                        principalTable: "Budget",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Addition_ExaminationCommitte",
                        column: x => x.ExaminationCommitteId,
                        principalTable: "ExaminationCommitte",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Addition_Operation",
                        column: x => x.OperationId,
                        principalTable: "Operation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Addition_Transformation",
                        column: x => x.TransformationId,
                        principalTable: "Transformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransformationAttachment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    TransformationId = table.Column<Guid>(nullable: false),
                    AttachmentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransformationAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransformationAttachment_Attachment",
                        column: x => x.AttachmentId,
                        principalTable: "Attachment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransformationAttachment_Transformation",
                        column: x => x.TransformationId,
                        principalTable: "Transformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CommitteeItemHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    BaseItemId = table.Column<long>(nullable: false),
                    ExaminationCommitteId = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    ExaminationPercentage = table.Column<int>(nullable: false),
                    Accepted = table.Column<int>(nullable: false),
                    Rejected = table.Column<int>(nullable: true),
                    Reasons = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    CommitteeItemId = table.Column<Guid>(nullable: false),
                    HistoryActionId = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommitteeItemHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommitteeItemHistory_CommitteeItem",
                        column: x => x.CommitteeItemId,
                        principalTable: "CommitteeItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommitteeItemHistory_HistoryAction",
                        column: x => x.HistoryActionId,
                        principalTable: "HistoryAction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AdditionAttachment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    AdditionId = table.Column<Guid>(nullable: false),
                    AttachmentId = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdditionAttachment_Addition",
                        column: x => x.AdditionId,
                        principalTable: "Addition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdditionAttachment_Attachment",
                        column: x => x.AttachmentId,
                        principalTable: "Attachment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StoreItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    AdditionId = table.Column<Guid>(nullable: false),
                    BaseItemId = table.Column<long>(nullable: false),
                    Code = table.Column<string>(nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18, 0)", nullable: false),
                    BookId = table.Column<long>(nullable: false),
                    BookPageNumber = table.Column<int>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    NoteCreatorId = table.Column<string>(maxLength: 450, nullable: true),
                    StoreId = table.Column<int>(nullable: false),
                    StoreItemStatusId = table.Column<int>(nullable: false),
                    CurrentItemStatusId = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreItem_Addition",
                        column: x => x.AdditionId,
                        principalTable: "Addition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoreItem_BaseItem",
                        column: x => x.BaseItemId,
                        principalTable: "BaseItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoreItem_Book",
                        column: x => x.BookId,
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoreItem_ItemStatusId",
                        column: x => x.CurrentItemStatusId,
                        principalTable: "ItemStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoreItem_Store",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoreItem_StoreItemStatus",
                        column: x => x.StoreItemStatusId,
                        principalTable: "StoreItemStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExchangeOrderStoreItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    ExchangeOrderId = table.Column<Guid>(nullable: false),
                    StoreItemId = table.Column<Guid>(nullable: false),
                    Notes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeOrderStoreItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExchangeOrderStoreItem_ExchangeOrder",
                        column: x => x.ExchangeOrderId,
                        principalTable: "ExchangeOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExchangeOrderStoreItem_StoreItem",
                        column: x => x.StoreItemId,
                        principalTable: "StoreItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceStoreItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    InvoiceId = table.Column<Guid>(nullable: false),
                    StoreItemId = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceStoreItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceStoreItem_Invoice",
                        column: x => x.InvoiceId,
                        principalTable: "Invoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceStoreItem_StoreItem",
                        column: x => x.StoreItemId,
                        principalTable: "StoreItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StoreItemHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    StoreItemId = table.Column<Guid>(nullable: false),
                    OperationId = table.Column<int>(nullable: false),
                    OperationCode = table.Column<string>(nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18, 0)", nullable: false),
                    BookNumber = table.Column<int>(nullable: false),
                    BookPageNumber = table.Column<int>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    NoteCreatorName = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreItemHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreItemHistory_Operation",
                        column: x => x.OperationId,
                        principalTable: "Operation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoreItemHistory_StoreItem",
                        column: x => x.StoreItemId,
                        principalTable: "StoreItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addition_AdditionDocumentTypeId",
                table: "Addition",
                column: "AdditionDocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Addition_BudgetId",
                table: "Addition",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_Addition_ExaminationCommitteId",
                table: "Addition",
                column: "ExaminationCommitteId");

            migrationBuilder.CreateIndex(
                name: "IX_Addition_OperationId",
                table: "Addition",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_Addition_TransformationId",
                table: "Addition",
                column: "TransformationId");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionAttachment_AdditionId",
                table: "AdditionAttachment",
                column: "AdditionId");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionAttachment_AttachmentId",
                table: "AdditionAttachment",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_AttachmentTypeId",
                table: "Attachment",
                column: "AttachmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseItem_DefaultUnitId",
                table: "BaseItem",
                column: "DefaultUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseItem_ItemCategoryId",
                table: "BaseItem",
                column: "ItemCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Book_StoreId",
                table: "Book",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_CommitteeAttachment_AttachmentId",
                table: "CommitteeAttachment",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_CommitteeAttachment_CommitteeId",
                table: "CommitteeAttachment",
                column: "CommitteeId");

            migrationBuilder.CreateIndex(
                name: "IX_CommitteeEmployee_EmplyeeId",
                table: "CommitteeEmployee",
                column: "EmplyeeId");

            migrationBuilder.CreateIndex(
                name: "IX_CommitteeEmployee_ExaminationCommitteId",
                table: "CommitteeEmployee",
                column: "ExaminationCommitteId");

            migrationBuilder.CreateIndex(
                name: "IX_CommitteeEmployee_JobTitleId",
                table: "CommitteeEmployee",
                column: "JobTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_CommitteeItem_BaseItemId",
                table: "CommitteeItem",
                column: "BaseItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CommitteeItem_ExaminationCommitteId",
                table: "CommitteeItem",
                column: "ExaminationCommitteId");

            migrationBuilder.CreateIndex(
                name: "IX_CommitteeItem_UnitId",
                table: "CommitteeItem",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_CommitteeItemHistory_CommitteeItemId",
                table: "CommitteeItemHistory",
                column: "CommitteeItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CommitteeItemHistory_HistoryActionId",
                table: "CommitteeItemHistory",
                column: "HistoryActionId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ExaminationCommitte_BudgetId",
                table: "ExaminationCommitte",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_ExaminationCommitte_ExaminationStatusId",
                table: "ExaminationCommitte",
                column: "ExaminationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ExaminationCommitte_ExternalEntityId",
                table: "ExaminationCommitte",
                column: "ExternalEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ExaminationCommitte_OperationId",
                table: "ExaminationCommitte",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_ExaminationCommitte_StoreId",
                table: "ExaminationCommitte",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_ExaminationCommitte_SupplierId",
                table: "ExaminationCommitte",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeOrder_BugetId",
                table: "ExchangeOrder",
                column: "BugetId");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeOrder_ExchangeOrderStatusId",
                table: "ExchangeOrder",
                column: "ExchangeOrderStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeOrder_ForEmployeeId",
                table: "ExchangeOrder",
                column: "ForEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeOrder_OperationId",
                table: "ExchangeOrder",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeOrderStoreItem_ExchangeOrderId",
                table: "ExchangeOrderStoreItem",
                column: "ExchangeOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeOrderStoreItem_StoreItemId",
                table: "ExchangeOrderStoreItem",
                column: "StoreItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_ReceivedEmployeeId",
                table: "Invoice",
                column: "ReceivedEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceStoreItem_InvoiceId",
                table: "InvoiceStoreItem",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceStoreItem_StoreItemId",
                table: "InvoiceStoreItem",
                column: "StoreItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationAttachmentType_AdditionDocumentTypeId",
                table: "OperationAttachmentType",
                column: "AdditionDocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationAttachmentType_AttachmentTypeId",
                table: "OperationAttachmentType",
                column: "AttachmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationAttachmentType_BudgetId",
                table: "OperationAttachmentType",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationAttachmentType_OperationId",
                table: "OperationAttachmentType",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_Store_RobbingBudgetId",
                table: "Store",
                column: "RobbingBudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_Store_StoreTypeId",
                table: "Store",
                column: "StoreTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Store_TechnicalDepartmentId",
                table: "Store",
                column: "TechnicalDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreItem_AdditionId",
                table: "StoreItem",
                column: "AdditionId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreItem_BaseItemId",
                table: "StoreItem",
                column: "BaseItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreItem_BookId",
                table: "StoreItem",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreItem_CurrentItemStatusId",
                table: "StoreItem",
                column: "CurrentItemStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreItem_StoreId",
                table: "StoreItem",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreItem_StoreItemStatusId",
                table: "StoreItem",
                column: "StoreItemStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreItemHistory_OperationId",
                table: "StoreItemHistory",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreItemHistory_StoreItemId",
                table: "StoreItemHistory",
                column: "StoreItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Transformation_BudgetId",
                table: "Transformation",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_Transformation_FromStoreId",
                table: "Transformation",
                column: "FromStoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Transformation_OperationId",
                table: "Transformation",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_Transformation_ToExternalEntityId",
                table: "Transformation",
                column: "ToExternalEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Transformation_TransformationStatusId",
                table: "Transformation",
                column: "TransformationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TransformationAttachment_AttachmentId",
                table: "TransformationAttachment",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TransformationAttachment_TransformationId",
                table: "TransformationAttachment",
                column: "TransformationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdditionAttachment");

            migrationBuilder.DropTable(
                name: "CommitteeAttachment");

            migrationBuilder.DropTable(
                name: "CommitteeEmployee");

            migrationBuilder.DropTable(
                name: "CommitteeItemHistory");

            migrationBuilder.DropTable(
                name: "ExchangeOrderStoreItem");

            migrationBuilder.DropTable(
                name: "InvoiceStoreItem");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "OperationAttachmentType");

            migrationBuilder.DropTable(
                name: "StoreItemHistory");

            migrationBuilder.DropTable(
                name: "TransformationAttachment");

            migrationBuilder.DropTable(
                name: "TransformationStoreItem");

            migrationBuilder.DropTable(
                name: "JobTitle");

            migrationBuilder.DropTable(
                name: "CommitteeItem");

            migrationBuilder.DropTable(
                name: "HistoryAction");

            migrationBuilder.DropTable(
                name: "ExchangeOrder");

            migrationBuilder.DropTable(
                name: "Invoice");

            migrationBuilder.DropTable(
                name: "StoreItem");

            migrationBuilder.DropTable(
                name: "Attachment");

            migrationBuilder.DropTable(
                name: "ExchangeOrderStatus");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Addition");

            migrationBuilder.DropTable(
                name: "BaseItem");

            migrationBuilder.DropTable(
                name: "Book");

            migrationBuilder.DropTable(
                name: "ItemStatus");

            migrationBuilder.DropTable(
                name: "StoreItemStatus");

            migrationBuilder.DropTable(
                name: "AttachmentType");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropTable(
                name: "AdditionDocumentType");

            migrationBuilder.DropTable(
                name: "ExaminationCommitte");

            migrationBuilder.DropTable(
                name: "Transformation");

            migrationBuilder.DropTable(
                name: "Unit");

            migrationBuilder.DropTable(
                name: "ItemCategory");

            migrationBuilder.DropTable(
                name: "ExaminationStatus");

            migrationBuilder.DropTable(
                name: "Supplier");

            migrationBuilder.DropTable(
                name: "Store");

            migrationBuilder.DropTable(
                name: "Operation");

            migrationBuilder.DropTable(
                name: "ExternalEntity");

            migrationBuilder.DropTable(
                name: "TransformationStatus");

            migrationBuilder.DropTable(
                name: "Budget");

            migrationBuilder.DropTable(
                name: "StoreType");

            migrationBuilder.DropTable(
                name: "TechnicalDepartment");
        }
    }
}
