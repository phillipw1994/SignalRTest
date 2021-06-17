using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SignalR.Api.Model.Base;
using SignalR.Api.Model.Interface;

namespace SignalR.Api.Database
{
    public interface ISignalRDbContext : IDisposable
    {
        Task EnsureSeededAsync();
        Task<bool> CanConnect();
        Guid SystemUid { get; }

        #region DbSets
        #region Schema [dbo]
        DbSet<ItemSection> ItemSectons { get; set; }
        DbSet<Address> Addresses { get; set; }
        DbSet<AddressType> AddressTypes { get; set; }
        DbSet<Barcode> Barcodes { get; set; }
        DbSet<BarcodeCharacterType> BarcodeCharacterTypes { get; set; }
        DbSet<BarcodeFormat> BarcodeFormats { get; set; }
        DbSet<BarcodeLookupField> BarcodeLookupFields { get; set; }
        DbSet<BarcodeType> BarcodeTypes { get; set; }
        DbSet<Batch> Batches { get; set; }
        DbSet<Button> Buttons { get; set; }
        DbSet<CustomerOuterLabel> CustomerOuterLabels { get; set; }
        DbSet<DateFormat> DateFormats { get; set; }
        DbSet<LabelFormat> LabelFormats { get; set; }
        DbSet<LabelType> LabelTypes { get; set; }
        DbSet<Locale> Locales { get; set; }
        DbSet<OuterPackaging> OuterPackagings { get; set; }
        DbSet<OuterPackagingType> OuterPackagingTypes { get; set; }
        DbSet<OuterProperty> OuterProperties { get; set; }
        DbSet<PackagingType> PackagingTypes { get; set; }
        DbSet<PackagingWeightFactor> PackagingWeightFactors { get; set; }
        DbSet<Page> Pages { get; set; }
        DbSet<Plant> Plants { get; set; }
        DbSet<Preservation> Preservations { get; set; }
        DbSet<PromoPricing> PromoPricings { get; set; }
        DbSet<Property> Properties { get; set; }
        DbSet<PropertyFormatOverride> PropertyFormatOverrides { get; set; }
        DbSet<PropertyType> PropertyTypes { get; set; }
        DbSet<Stock> Stocks { get; set; }
        DbSet<StockReservation> StockReservations { get; set; }
        DbSet<SystemSetting> SystemSettings { get; set; }
        DbSet<Unit> Units { get; set; }
        DbSet<UnitConversion> UnitConversions { get; set; }
        DbSet<UnitOfMeasure> UnitsOfMeasure { get; set; }
        #endregion

        #region Accounting [acc]
        DbSet<Account> Accounts { get; set; }
        DbSet<Country> Countries { get; set; }
        DbSet<CountryCurrency> CountryCurrencies { get; set; }
        DbSet<Currency> Currencies { get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<CustomerAccount> CustomerAccountings { get; set; }
        DbSet<CustomerAddress> CustomerAddresses { get; set; }
        DbSet<CustomerType> CustomerTypes { get; set; }
        DbSet<FreightCalculation> FreightCalculations { get; set; }
        DbSet<FreightChargeType> FreightChargeTypes { get; set; }
        DbSet<Invoice> Invoices { get; set; }
        DbSet<InvoiceDefault> InvoiceDefaults { get; set; }
        DbSet<InvoiceLine> InvoiceLines { get; set; }
        DbSet<InvoiceReference> InvoiceReferences { get; set; }
        DbSet<ItemPrice> ItemPrices { get; set; }
        DbSet<Price> Prices { get; set; }
        DbSet<PriceType> PriceTypes { get; set; }
        DbSet<Term> Terms { get; set; }
        DbSet<TermType> TermTypes { get; set; }
        #endregion

        #region Hardware [hdw]
        DbSet<ComPort> ComPorts { get; set; }
        DbSet<Device> Devices { get; set; }
        DbSet<DeviceConfiguration> DeviceConfigurations { get; set; }
        DbSet<DeviceType> DeviceTypes { get; set; }
        DbSet<Dock> Docks { get; set; }
        DbSet<Handshake> Handshakes { get; set; }
        DbSet<IoLine> IoLines { get; set; }
        DbSet<Machine> Machines { get; set; }
        DbSet<MakeModel> MakeModels { get; set; }
        DbSet<MakeModelDetail> MakeModelDetails { get; set; }
        DbSet<Message> Messages { get; set; }
        DbSet<MessageType> MessageTypes { get; set; }
        DbSet<Parity> Parities { get; set; }
        DbSet<RegularExpression> RegularExpressions { get; set; }
        DbSet<Scan> Scans { get; set; }
        DbSet<StopBits> StopBits { get; set; }
        DbSet<TcpIpConfiguration> TcpIpConfigurations { get; set; }
        DbSet<Driver> Drivers { get; set; }
        DbSet<PrintJob> PrintJobs { get; set; }
        DbSet<PrintJobStatus> PrintJobStatuses { get; set; }
        #endregion

        #region Inventory [inv]
        DbSet<Area> Areas { get; set; }
        DbSet<Brand> Brands { get; set; }
        DbSet<Building> Buildings { get; set; }
        DbSet<Category> Categories { get; set; }
        // ReSharper disable once InconsistentNaming
        DbSet<Category2> Category2s { get; set; }
        // ReSharper disable once InconsistentNaming
        DbSet<Category3> Category3s { get; set; }
        DbSet<Form> Forms { get; set; }
        DbSet<MediaTypeNames.Image> Images { get; set; }
        DbSet<Inner> Inners { get; set; }
        DbSet<Location> Locations { get; set; }
        DbSet<Outer> Outers { get; set; }
        DbSet<OuterCategory> OuterCategories { get; set; }
        DbSet<OuterImage> OuterImages { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<ProductCategory> ProductCategories { get; set; }
        DbSet<ProductImage> ProductImages { get; set; }
        DbSet<Production> Productions { get; set; }
        DbSet<ProductProperty> ProductProperties { get; set; }
        DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        DbSet<PurchaseOrderLine> PurchaseOrderLines { get; set; }
        DbSet<PurchaseOrderLineReceiptLine> PurchaseOrderLineReceiptLines { get; set; }
        DbSet<PurchaseOrderReference> PurchaseOrderReferences { get; set; }
        DbSet<PurchaseOrderStatus> PurchaseOrderStatuses { get; set; }
        DbSet<Receipt> Receipts { get; set; }
        DbSet<ReceiptLine> ReceiptLines { get; set; }
        DbSet<ReceiptReference> ReceiptReferences { get; set; }
        DbSet<ReceiptStatus> ReceiptStatuses { get; set; }
        DbSet<ReceiptingScan> ReceiptingScans { get; set; }
        DbSet<ReservedItem> ReservedItems { get; set; }
        DbSet<BitVector32.Section> Sections { get; set; }
        DbSet<SectionStock> SectionStocks { get; set; }
        DbSet<ShippingContainer> ShippingContainers { get; set; }
        DbSet<Sku> Skus { get; set; }
        DbSet<StockAdjustment> StockAdjustments { get; set; }
        DbSet<StockAdjustmentType> StockAdjustmentTypes { get; set; }
        DbSet<Supplier> Suppliers { get; set; }
        DbSet<SupplierAddress> SupplierAddresses { get; set; }
        DbSet<SupplierType> SupplierTypes { get; set; }
        DbSet<WarehouseContainer> WarehouseContainers { get; set; }
        DbSet<WarehouseContainerType> WarehouseContainerTypes { get; set; }
        #endregion

        #region Orders [ord]
        DbSet<Group> Groups { get; set; }
        DbSet<GroupLine> GroupLines { get; set; }
        DbSet<GroupType> GroupTypes { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<OrderReference> OrderReferences { get; set; }
        DbSet<OrderLine> OrderLines { get; set; }
        DbSet<OrderLinePackingSlipLine> OrderLinePackingSlipLines { get; set; }
        DbSet<OrderGroup> OrderGroups { get; set; }
        DbSet<OrderStatus> OrderStatuses { get; set; }
        DbSet<PackingScan> PackingScans { get; set; }
        DbSet<PackingSlip> PackingSlips { get; set; }
        DbSet<PackingSlipReference> PackingSlipReferences { get; set; }
        DbSet<PackingSlipLine> PackingSlipLines { get; set; }
        DbSet<PackingSlipStatus> PackingSlipStatuses { get; set; }
        DbSet<Shipment> Shipments { get; set; }
        DbSet<ShipmentReference> ShipmentReferences { get; set; }
        DbSet<ShipmentStatus> ShipmentStatuses { get; set; }
        #endregion

        #region Management [mgt]
        DbSet<SystemUser> SystemUsers { get; set; }
        DbSet<SystemUserCustomer> SystemUserCustomers { get; set; }
        #endregion

        #region Printng [prt]
        DbSet<PackingSlipPrintFile> PackingSlipPrintFiles { get; set; }
        DbSet<PrintFile> PrintFiles { get; set; }
        DbSet<PrintObject> PrintObjects { get; set; }
        DbSet<PrintTemplate> PrintTemplates { get; set; }
        #endregion

        #region Production [pdn]
        DbSet<Creation> Creations { get; set; }
        DbSet<CreationLine> CreationLines { get; set; }
        DbSet<CreationLineOuter> CreationLineOuters { get; set; }
        DbSet<Defect> Defects { get; set; }
        DbSet<Grade> Grades { get; set; }
        DbSet<Item> Items { get; set; }
        DbSet<ItemCategory> ItemCategories { get; set; }
        DbSet<ItemDefect> ItemDefects { get; set; }
        DbSet<ItemInner> ItemInners { get; set; }
        DbSet<ItemSupplier> ItemSuppliers { get; set; }
        DbSet<Recipe> Recipes { get; set; }
        DbSet<RecipeItem> RecipeItems { get; set; }
        #endregion

        #region Settings [set]
        DbSet<MediaTypeNames.Application> Applications { get; set; }
        DbSet<Jargon> Jargons { get; set; }
        DbSet<Setting> Settings { get; set; }
        #endregion

        #region Stocktake [stk]
        DbSet<Stocktake> Stocktakes { get; set; }
        DbSet<StocktakeAdjustment> StocktakeAdjustments { get; set; }
        DbSet<StocktakeCount> StocktakeCounts { get; set; }
        DbSet<StocktakeCountType> StocktakeCountTypes { get; set; }
        DbSet<StocktakeException> StocktakeExceptions { get; set; }
        DbSet<StocktakeLine> StocktakeLines { get; set; }
        DbSet<StocktakeSectionStock> StocktakeSectionStocks { get; set; }
        DbSet<StocktakeStatus> StocktakeStatuses { get; set; }
        DbSet<StocktakeType> StocktakeTypes { get; set; }
        #endregion
        #endregion

        #region ef stuff
        EntityState GetState<TEntity>(TEntity entity) where TEntity : class, IBaseEntity;

        void SetState<TEntity>(TEntity entity, EntityState newState) where TEntity : class, IBaseEntity;

        void RollBackAll<TEntity>() where TEntity : class, IBaseEntity;

        void DetachAll<TEntity>() where TEntity : class, IBaseEntity;

        void RollBack<TEntity>(TEntity entity) where TEntity : class, IBaseEntity;

        void RollBackAllEntities();

        TEntity Find<TEntity>(string name) where TEntity : BaseEntity, INamedObject;
        Task<TEntity> FindAsync<TEntity>(string name) where TEntity : BaseEntity, INamedObject;

        TEntity Find<TEntity>(Guid uid) where TEntity : BaseEntity, IGuidUid;
        Task<TEntity> FindAsync<TEntity>(Guid uid) where TEntity : BaseEntity, IGuidUid;

        TEntity Find<TEntity>(int id) where TEntity : Lookup;

        Task<TEntity> FindAsync<TEntity>(int id) where TEntity : Lookup;

        void ReloadEntity<TEntity>(TEntity entity) where TEntity : BaseEntity;

        void ReloadNavigationProperty<TEntity, TElement>(TEntity entity, Expression<Func<TEntity, IEnumerable<TElement>>> navigationProperty)
            where TEntity : BaseEntity
            where TElement : BaseEntity;
        #endregion

        #region general ef stuff
        DatabaseFacade Database { get; }
        ChangeTracker ChangeTracker { get; }
        //DbContextConfiguration Configuration { get; }
        DbSet<TEntity> Set<TEntity>() where TEntity : class, IBaseEntity;

        EntityEntry Attach(object entity);
        // DbSet<TEntity> Set<TEntity>(TEntity entity) where TEntity : class, IRaceEntity;
        //DbSet Set(Type entityType);
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<int> SaveChangesAsync();

        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
        //IEnumerable<DbEntityValidationResult> GetValidationErrors();
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class, IBaseEntity;
        EntityEntry Entry(object entity);
        string ToString();
        bool Equals(object obj);
        int GetHashCode();
        Type GetType();
        bool EntityHasChanged<TEntity>(TEntity entity) where TEntity : class, IBaseEntity;
        #endregion
    }
}
