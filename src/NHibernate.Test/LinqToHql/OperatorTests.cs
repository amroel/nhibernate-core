using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.DomainModel.Northwind.Entities;
using NHibernate.LinqToHql;
using NUnit.Framework;

namespace NHibernate.Test.LinqToHql
{
    [TestFixture]
    public class OperatorTests : LinqTestCase
    {
        [Test]
        public void Mod()
        {
            Assert.AreEqual(2, session.Query<TimesheetEntry>().Where(a => a.NumberOfHours % 7 == 0).Count());
        }
    }
}
