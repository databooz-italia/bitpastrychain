using System;
using System.Numerics;
using BitPastry.Backend.Core.Interfaces;
using BitPastry.Backend.Data;
using BitPastry.Backend.DTO.Configurations;

namespace BitPastry.Backend.Core.Services
{
    public class BaseService : IDisposable
    {
        protected BitPastryDB _db;
        protected Configuration _conf;
        protected IAuthenticationProvider _auth;

        private readonly bool transactionOwner = false;
        private bool complete = false;

        public BaseService(
            BitPastryDB db,
            Configuration conf,
            IAuthenticationProvider authProvider
        ) {
            this._db = db;
            this._conf = conf;
            this._auth = authProvider;
            
            if (_db.Database.CurrentTransaction == null)
            {
                _db.Database.AutoTransactionsEnabled = false;
                _db.Database.BeginTransaction();
                transactionOwner = true;
            }
        }

        public BaseService(BaseService b) : this(b._db, b._conf, b._auth) { }

        /* ------------------------------------------------------------------------------------- */
        // Public Props
        public Configuration BitPastryConfiguration => _conf;
        public BitPastryDB Database => _db;

        /* ------------------------------------------------------------------------------------- */
        public void Complete()
        {
            if (transactionOwner)
                complete = true;
        }

        public void Dispose()
        {
            if (transactionOwner && _db.Database?.CurrentTransaction != null)
            {
                if (complete)
                    _db.Database.CurrentTransaction.Commit();
                else
                    _db.Database.CurrentTransaction.Rollback();
            }
        }

    }
}
