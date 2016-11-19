using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using System.Reflection;

namespace SQL.NoSQL.Library.SQL
{
    public class NHIbernateContext
    {
        /// <summary>
        /// Get a configuration using settings in web.config and map entities in this assembly
        /// </summary>
        /// <returns></returns>
        public static Configuration GetConfiguration()
        {

            Configuration cfg = new Configuration();

            ModelMapper mapper = new ModelMapper();
            
            mapper.AddMappings(Assembly.Load("SQL.NoSQL.BLL").GetExportedTypes());

            HbmMapping domainMapping =
              mapper.CompileMappingForAllExplicitlyAddedEntities();
            cfg.AddMapping(domainMapping);
            cfg.Configure();

            return cfg;
        }

        public Configuration Configuration { get; set; }
        public ISessionFactory SessionFactory { get; set; }

        private static NHIbernateContext CreateNewContext()
        {
            NHIbernateContext ctx = new NHIbernateContext();
            ctx.Configuration = GetConfiguration();
            ctx.SessionFactory = ctx.Configuration.BuildSessionFactory();
            return ctx;
        }

        public static void ApplySchemaChanges()
        {
            Configuration cfg = GetConfiguration();

            SchemaMetadataUpdater.QuoteTableAndColumns(cfg);
            var update = new SchemaUpdate(cfg);

            update.Execute(true, true);

        }


        private static NHIbernateContext _Current;
        public static NHIbernateContext Current
        {
            get
            {
                //ApplySchemaChanges();
                return _Current ?? (_Current = CreateNewContext());
            }
        }

    }
}
