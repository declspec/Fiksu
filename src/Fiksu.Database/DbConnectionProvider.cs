﻿using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Fiksu.Database {
    public interface IDbConnectionProvider {
        Task<IDbConnection> GetConnectionAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}