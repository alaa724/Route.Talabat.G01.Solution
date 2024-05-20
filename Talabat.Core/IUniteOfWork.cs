﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repository.Contract;

namespace Talabat.Core
{
	public interface IUniteOfWork : IAsyncDisposable
	{
		public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;

		public Task<int> CompleteAsync();

	}
}
