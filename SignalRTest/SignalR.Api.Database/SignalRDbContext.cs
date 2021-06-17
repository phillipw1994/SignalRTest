using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SignalR.Api.Database
{
    public class SignalRDbContext : DbContext, ISignalRDbContext
    {
        #region constructors
        public SignalRDbContext(string connectionString, Guid systemUid, IContextFactory contextFactory)
        {
            ConnectionString = connectionString;
            SystemUid = systemUid;
            ContextFactory = contextFactory;
        }
        #endregion

        #region private members

        private string ConnectionString { get; }
        private IContextFactory ContextFactory { get; }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(ConnectionString);
            // o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
        }

        public Task EnsureSeededAsync()
        {
            return this.EnsureSeedDataAsync();
        }

        public Task<bool> CanConnect()
        {
            return Database.CanConnectAsync();
        }

        public Guid SystemUid { get; }

        #region DbSets
        #region Schema [dbo]
        public DbSet<ItemSection> ItemSectons { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<AddressType> AddressTypes { get; set; }
        public DbSet<Barcode> Barcodes { get; set; }
        public DbSet<BarcodeCharacterType> BarcodeCharacterTypes { get; set; }
        public DbSet<BarcodeFormat> BarcodeFormats { get; set; }
        public DbSet<BarcodeLookupField> BarcodeLookupFields { get; set; }
        public DbSet<BarcodeType> BarcodeTypes { get; set; }
        public DbSet<Batch> Batches { get; set; }
        public DbSet<Button> Buttons { get; set; }
        public DbSet<CustomerOuterLabel> CustomerOuterLabels { get; set; }
        public DbSet<DateFormat> DateFormats { get; set; }
        public DbSet<LabelFormat> LabelFormats { get; set; }
        public DbSet<LabelType> LabelTypes { get; set; }
        public DbSet<Locale> Locales { get; set; }
        public DbSet<OuterPackaging> OuterPackagings { get; set; }
        public DbSet<OuterPackagingType> OuterPackagingTypes { get; set; }
        public DbSet<OuterProperty> OuterProperties { get; set; }
        public DbSet<PackagingType> PackagingTypes { get; set; }
        public DbSet<PackagingWeightFactor> PackagingWeightFactors { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<Preservation> Preservations { get; set; }
        public DbSet<PromoPricing> PromoPricings { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyFormatOverride> PropertyFormatOverrides { get; set; }
        public DbSet<PropertyType> PropertyTypes { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<StockReservation> StockReservations { get; set; }
        public DbSet<SystemSetting> SystemSettings { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<UnitConversion> UnitConversions { get; set; }
        public DbSet<UnitOfMeasure> UnitsOfMeasure { get; set; }
        #endregion

        #region Accounting [acc]
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<CountryCurrency> CountryCurrencies { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerAccount> CustomerAccountings { get; set; }
        public DbSet<CustomerAddress> CustomerAddresses { get; set; }
        public DbSet<CustomerType> CustomerTypes { get; set; }
        public DbSet<FreightCalculation> FreightCalculations { get; set; }
        public DbSet<FreightChargeType> FreightChargeTypes { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDefault> InvoiceDefaults { get; set; }
        public DbSet<InvoiceLine> InvoiceLines { get; set; }
        public DbSet<InvoiceReference> InvoiceReferences { get; set; }
        public DbSet<ItemPrice> ItemPrices { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<PriceType> PriceTypes { get; set; }
        public DbSet<Term> Terms { get; set; }
        public DbSet<TermType> TermTypes { get; set; }
        #endregion

        #region Hardware [hdw]
        public DbSet<ComPort> ComPorts { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceConfiguration> DeviceConfigurations { get; set; }
        public DbSet<DeviceType> DeviceTypes { get; set; }
        public DbSet<Dock> Docks { get; set; }
        public DbSet<Handshake> Handshakes { get; set; }
        public DbSet<IoLine> IoLines { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<MakeModel> MakeModels { get; set; }
        public DbSet<MakeModelDetail> MakeModelDetails { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MessageType> MessageTypes { get; set; }
        public DbSet<Parity> Parities { get; set; }
        public DbSet<RegularExpression> RegularExpressions { get; set; }
        public DbSet<Scan> Scans { get; set; }
        public DbSet<StopBits> StopBits { get; set; }
        public DbSet<TcpIpConfiguration> TcpIpConfigurations { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<PrintJob> PrintJobs { get; set; }
        public DbSet<PrintJobStatus> PrintJobStatuses { get; set; }
        #endregion

        #region Inventory [inv]
        public DbSet<Area> Areas { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Category> Categories { get; set; }
        // ReSharper disable once InconsistentNaming
        public DbSet<Category2> Category2s { get; set; }
        // ReSharper disable once InconsistentNaming
        public DbSet<Category3> Category3s { get; set; }
        public DbSet<Form> Forms { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Inner> Inners { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Outer> Outers { get; set; }
        public DbSet<OuterCategory> OuterCategories { get; set; }
        public DbSet<OuterImage> OuterImages { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Production> Productions { get; set; }
        public DbSet<ProductProperty> ProductProperties { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderLine> PurchaseOrderLines { get; set; }
        public DbSet<PurchaseOrderLineReceiptLine> PurchaseOrderLineReceiptLines { get; set; }
        public DbSet<PurchaseOrderReference> PurchaseOrderReferences { get; set; }
        public DbSet<PurchaseOrderStatus> PurchaseOrderStatuses { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<ReceiptLine> ReceiptLines { get; set; }
        public DbSet<ReceiptReference> ReceiptReferences { get; set; }
        public DbSet<ReceiptStatus> ReceiptStatuses { get; set; }
        public DbSet<ReceiptingScan> ReceiptingScans { get; set; }
        public DbSet<ReservedItem> ReservedItems { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<SectionStock> SectionStocks { get; set; }
        public DbSet<ShippingContainer> ShippingContainers { get; set; }
        public DbSet<Sku> Skus { get; set; }
        public DbSet<StockAdjustment> StockAdjustments { get; set; }
        public DbSet<StockAdjustmentType> StockAdjustmentTypes { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierAddress> SupplierAddresses { get; set; }
        public DbSet<SupplierType> SupplierTypes { get; set; }
        public DbSet<WarehouseContainer> WarehouseContainers { get; set; }
        public DbSet<WarehouseContainerType> WarehouseContainerTypes { get; set; }
        #endregion

        #region Orders [ord]
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupLine> GroupLines { get; set; }
        public DbSet<GroupType> GroupTypes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderReference> OrderReferences { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<OrderLinePackingSlipLine> OrderLinePackingSlipLines { get; set; }
        public DbSet<OrderGroup> OrderGroups { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<PackingScan> PackingScans { get; set; }
        public DbSet<PackingSlip> PackingSlips { get; set; }
        public DbSet<PackingSlipReference> PackingSlipReferences { get; set; }
        public DbSet<PackingSlipLine> PackingSlipLines { get; set; }
        public DbSet<PackingSlipStatus> PackingSlipStatuses { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<ShipmentReference> ShipmentReferences { get; set; }
        public DbSet<ShipmentStatus> ShipmentStatuses { get; set; }
        #endregion

        #region Management [mgt]
        public DbSet<SystemUser> SystemUsers { get; set; }
        public DbSet<SystemUserCustomer> SystemUserCustomers { get; set; }
        #endregion

        #region Printng [prt]
        public DbSet<PackingSlipPrintFile> PackingSlipPrintFiles { get; set; }
        public DbSet<PrintFile> PrintFiles { get; set; }
        public DbSet<PrintObject> PrintObjects { get; set; }
        public DbSet<PrintTemplate> PrintTemplates { get; set; }
        #endregion

        #region Production [pdn]
        public DbSet<Creation> Creations { get; set; }
        public DbSet<CreationLine> CreationLines { get; set; }
        public DbSet<CreationLineOuter> CreationLineOuters { get; set; }
        public DbSet<Defect> Defects { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<ItemDefect> ItemDefects { get; set; }
        public DbSet<ItemInner> ItemInners { get; set; }
        public DbSet<ItemSupplier> ItemSuppliers { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeItem> RecipeItems { get; set; }
        #endregion

        #region Settings [set]
        public DbSet<Application> Applications { get; set; }
        public DbSet<Jargon> Jargons { get; set; }
        public DbSet<Setting> Settings { get; set; }
        #endregion

        #region Stocktake [stk]
        public DbSet<Stocktake> Stocktakes { get; set; }
        public DbSet<StocktakeAdjustment> StocktakeAdjustments { get; set; }
        public DbSet<StocktakeCount> StocktakeCounts { get; set; }
        public DbSet<StocktakeCountType> StocktakeCountTypes { get; set; }
        public DbSet<StocktakeException> StocktakeExceptions { get; set; }
        public DbSet<StocktakeLine> StocktakeLines { get; set; }
        public DbSet<StocktakeSectionStock> StocktakeSectionStocks { get; set; }
        public DbSet<StocktakeStatus> StocktakeStatuses { get; set; }
        public DbSet<StocktakeType> StocktakeTypes { get; set; }
        #endregion
        #endregion

        #region General EF Methods
        //public DbSet<TEntity> Set<TEntity>(TEntity entity) where TEntity : class, IRaceEntity
        //{
        //    return base.Set<TEntity>();
        //}



        public new DbSet<TEntity> Set<TEntity>() where TEntity : class, IRaceEntity
        {
            return base.Set<TEntity>();
        }

        public new EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class, IRaceEntity
        {
            return base.Entry(entity);
        }

        public bool EntityHasChanged<TEntity>(TEntity entity) where TEntity : class, IRaceEntity
        {
            var entityState = Entry(entity).State;
            return entityState == EntityState.Added ||
                   entityState == EntityState.Deleted ||
                   entityState == EntityState.Modified;
        }

        public void RollBackAll<TEntity>() where TEntity : class, IRaceEntity
        {
            var changedEntries = ChangeTracker.Entries<TEntity>()
                .Where(x => x.State != EntityState.Unchanged).ToList();

            foreach (var entry in changedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Detached:
                        break;
                    case EntityState.Unchanged:
                        // do nothing
                        break;
                }
            }
        }

        public EntityState GetState<TEntity>(TEntity entity) where TEntity : class, IRaceEntity
        {
            var entry = ChangeTracker.Entries<TEntity>().SingleOrDefault(e => e.Entity.Equals(entity));
            return entry?.State ?? EntityState.Detached;
        }

        public void SetState<TEntity>(TEntity entity, EntityState newState) where TEntity : class, IRaceEntity
        {
            var entry = ChangeTracker.Entries<TEntity>().SingleOrDefault(e => e.Entity.Equals(entity));
            if (entry != null)
            {
                entry.State = newState;
            }
        }

        public void RollBack<TEntity>(TEntity entity) where TEntity : class, IRaceEntity
        {
            var changedEntry = ChangeTracker.Entries<TEntity>().SingleOrDefault(e => e.Entity.Equals(entity));
            if (changedEntry == null) return;

            switch (changedEntry.State)
            {
                case EntityState.Modified:
                    changedEntry.CurrentValues.SetValues(changedEntry.OriginalValues);
                    changedEntry.State = EntityState.Unchanged;
                    break;
                case EntityState.Added:
                    changedEntry.State = EntityState.Detached;
                    break;
                case EntityState.Deleted:
                    changedEntry.State = EntityState.Unchanged;
                    break;
                case EntityState.Detached:
                    break;
                case EntityState.Unchanged:
                    // do nothing
                    break;
            }
        }

        public void RollBackAllEntities()
        {
            var changedEntries = ChangeTracker.Entries()
                .Where(x => x.State != EntityState.Unchanged).ToList();

            foreach (var entry in changedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Detached:
                        break;
                    case EntityState.Unchanged:
                        // do nothing
                        break;
                }
            }
        }

        public override int SaveChanges()
        {
            var changedEntries = ChangeTracker.Entries<RaceObject>()
                .Where(x => x.State != EntityState.Unchanged).ToList();

            foreach (var entry in changedEntries)
            {
                if (entry.State.Equals(EntityState.Deleted))
                {
                    if (entry.Entity is RaceGuidActiveDelete deletableEntity)
                    {
                        deletableEntity.Deleted = true;
                        entry.State = EntityState.Modified;
                    }
                }

                if (entry.State.Equals(EntityState.Modified))
                {
                    entry.Entity.UpdatedUtc = DateTime.UtcNow;
                }
            }

            return base.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return SaveChangesAsync(true);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var changedEntries = ChangeTracker.Entries<RaceObject>()
                .Where(x => x.State != EntityState.Unchanged).ToList();

            foreach (var entry in changedEntries)
            {
                if (entry.State.Equals(EntityState.Deleted))
                {
                    if (entry.Entity is RaceGuidActiveDelete deletableEntity)
                    {
                        deletableEntity.Deleted = true;
                        entry.State = EntityState.Modified;
                    }
                }

                if (entry.State.Equals(EntityState.Modified))
                {
                    entry.Entity.UpdatedUtc = DateTime.UtcNow;
                }
            }

            await TryBranchSyncAsync();

            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }















        public void DetachAll<TEntity>() where TEntity : class, IRaceEntity
        {
            RollBackAll<TEntity>();
            var allEntries = ChangeTracker.Entries<TEntity>().ToList();

            foreach (var entry in allEntries)
            {
                entry.State = EntityState.Detached;
            }
        }

        public TEntity Find<TEntity>(string name) where TEntity : RaceEntity, INamedObject
        {
            return Set<TEntity>().FirstOrDefault(e => e.Name == name);
        }

        public async Task<TEntity> FindAsync<TEntity>(string name) where TEntity : RaceEntity, INamedObject
        {
            return await Set<TEntity>().FirstOrDefaultAsync(e => e.Name == name);
        }

        public TEntity Find<TEntity>(Guid uid) where TEntity : RaceEntity, IGuidUid
        {
            return Set<TEntity>().SingleOrDefault(e => e.Uid == uid);
        }

        public async Task<TEntity> FindAsync<TEntity>(Guid uid) where TEntity : RaceEntity, IGuidUid
        {
            return await Set<TEntity>().SingleOrDefaultAsync(e => e.Uid == uid);
        }

        public TEntity Find<TEntity>(int id) where TEntity : Lookup
        {
            return Set<TEntity>().SingleOrDefault(e => e.Id == id);
        }

        public async Task<TEntity> FindAsync<TEntity>(int id) where TEntity : Lookup
        {
            return await Set<TEntity>().SingleOrDefaultAsync(e => e.Id == id);
        }

        public void ReloadEntity<TEntity>(TEntity entity) where TEntity : RaceEntity
        {
            var entry = ChangeTracker.Entries<TEntity>().SingleOrDefault(e => e.Entity.Equals(entity));
            entry?.Reload();
        }

        public void ReloadNavigationProperty<TEntity, TElement>(TEntity entity, Expression<Func<TEntity, IEnumerable<TElement>>> navigationProperty)
            where TEntity : RaceEntity
            where TElement : RaceEntity
        {
            var entry = ChangeTracker.Entries<TEntity>().SingleOrDefault(e => e.Entity.Equals(entity));
            entry?.Collection(navigationProperty).Query();
        }
        #endregion

        #region fluent Api table relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ForeignKeyRegistrar.RegisterForeignKeys(modelBuilder);
            IndexRegistrar.RegisterIndices(modelBuilder);
            IndexRegistrar.RegisterCompositeKeys(modelBuilder);
            ReferenceEntityRegistrar.RegisterReferenceEntities(modelBuilder);
            ColumnTypeRegistrar.RegisterColumnTypes(modelBuilder);
            TableSchemaRegistrar.RegisterTablesInSchemas(modelBuilder);
        }
        #endregion
    }
}
