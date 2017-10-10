﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using NUnit.Framework;
using NHibernate.Test.Immutable.EntityWithMutableCollection;

namespace NHibernate.Test.Immutable.EntityWithMutableCollection.Inverse
{
	using System.Threading.Tasks;
	[TestFixture]
	public class VersionedEntityWithInverseOneToManyFailureExpectedTestAsync : AbstractEntityWithOneToManyTestAsync
	{
		protected override System.Collections.IList Mappings
		{
			get
			{
				return new string[] { "Immutable.EntityWithMutableCollection.Inverse.ContractVariationVersioned.hbm.xml" };
			}
		}
		
		[Test]
		[Ignore("known to fail with versioned entity with inverse collection")]
		public override Task AddExistingOneToManyElementToPersistentEntityAsync()
		{
			return Task.CompletedTask;
		}

		[Test]
		[Ignore("known to fail with versioned entity with inverse collection")]
		public override Task CreateWithEmptyOneToManyCollectionMergeWithExistingElementAsync()
		{
			return Task.CompletedTask;
		}

		[Test]
		[Ignore("known to fail with versioned entity with inverse collection")]
		public override Task CreateWithEmptyOneToManyCollectionUpdateWithExistingElementAsync()
		{
			return Task.CompletedTask;
		}
		
		[Test]
		[Ignore("known to fail with versioned entity with inverse collection")]
		public override Task RemoveOneToManyElementUsingUpdateAsync()
		{
			return Task.CompletedTask;
		}

		[Test]
		[Ignore("known to fail with versioned entity with inverse collection")]
		public override Task RemoveOneToManyElementUsingMergeAsync()
		{
			return Task.CompletedTask;
		}
	}
}