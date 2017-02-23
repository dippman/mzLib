﻿// Copyright 2012, 2013, 2014 Derek J. Bailey
// Modified work Copyright 2016 Stefan Solntsev
//
// This file (TestRange.cs) is part of MassSpectrometry.Tests.
//
// MassSpectrometry.Tests is free software: you can redistribute it and/or modify it
// under the terms of the GNU Lesser General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// MassSpectrometry.Tests is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
// FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public
// License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with MassSpectrometry.Tests. If not, see <http://www.gnu.org/licenses/>.

using NUnit.Framework;
using Proteomics;
using System.Collections.Generic;
using System.IO;
using UsefulProteomicsDatabases;
using System.Linq;

namespace Test
{
    [TestFixture]
    public sealed class TestProteinReader
    {

        #region Public Methods

        [Test]
        public void XmlTest()
        {
            var nice = new List<Modification>
            {
                new ModificationWithLocation("fayk",null, null,ModificationSites.A,null,  null)
            };

            Dictionary<string, Modification> un;
            var ok = ProteinDbLoader.LoadProteinDb(Path.Combine(TestContext.CurrentContext.TestDirectory, @"xml.xml"), true, nice, false, out un);

            Assert.AreEqual('M', ok[0][0]);
            Assert.AreEqual('M', ok[1][0]);

            Assert.AreEqual("P62805|H4_HUMAN|Histone H4", ok[0].FullDescription);
            Assert.AreEqual("DECOY_P62805|H4_HUMAN|Histone H4", ok[1].FullDescription);
            Assert.AreEqual("0070062", ok[0].GoTerms[0].id);
            Assert.AreEqual("extracellular exosome", ok[0].GoTerms[0].description);
            Assert.AreEqual(Aspect.cellularComponent, ok[0].GoTerms[0].aspect);
        }

        [Test]
        public void XmlTest_2entry()
        {
            var nice = new List<Modification>
            {
                new ModificationWithLocation("fayk",null, null,ModificationSites.A,null,  null)
            };

            Dictionary<string, Modification> un;
            var ok = ProteinDbLoader.LoadProteinDb(Path.Combine(TestContext.CurrentContext.TestDirectory, @"xml2.xml"), true, nice, false, out un);

            Assert.True(ok.All(p => p.OneBasedBeginPositions.All(begin => begin == null || begin > 0 && begin <= p.Length)));
            Assert.True(ok.All(p => p.OneBasedEndPositions.All(end => end == null || end > 0 && end <= p.Length)));
        }

        [Test]
        public void XmlGzTest()
        {
            var nice = new List<Modification>
            {
                new ModificationWithLocation("fayk",null, null,ModificationSites.A,null,  null)
            };

            Dictionary<string, Modification> un;
            var ok = ProteinDbLoader.LoadProteinDb(Path.Combine(TestContext.CurrentContext.TestDirectory, @"xml.xml.gz"), true, nice, false, out un);

            Assert.AreEqual('M', ok[0][0]);
            Assert.AreEqual('M', ok[1][0]);

            Assert.AreEqual("P62805|H4_HUMAN|Histone H4", ok[0].FullDescription);
            Assert.AreEqual("DECOY_P62805|H4_HUMAN|Histone H4", ok[1].FullDescription);
            Assert.AreEqual("0070062", ok[0].GoTerms[0].id);
            Assert.AreEqual("extracellular exosome", ok[0].GoTerms[0].description);
            Assert.AreEqual(Aspect.cellularComponent, ok[0].GoTerms[0].aspect);
        }

        [Test]
        public void XmlFunkySequenceTest()
        {
            var nice = new List<Modification>
            {
                new ModificationWithLocation("fayk",null, null,ModificationSites.A,null,  null)
            };

            Dictionary<string, Modification> un;
            var ok = ProteinDbLoader.LoadProteinDb(Path.Combine(TestContext.CurrentContext.TestDirectory, @"fake_h4.xml"), true, nice, false, out un);

            Assert.AreEqual('S', ok[0][0]);
            Assert.AreEqual('G', ok[1][0]);
        }

        [Test]
        public void XmlModifiedStartTest()
        {
            var nice = new List<Modification>
            {
                new ModificationWithLocation("fayk",null, null,ModificationSites.A,null,  null)
            };

            Dictionary<string, Modification> un;
            var ok = ProteinDbLoader.LoadProteinDb(Path.Combine(TestContext.CurrentContext.TestDirectory, @"modified_start.xml"), true, nice, false, out un);

            Assert.AreEqual('M', ok[0][0]);
            Assert.AreEqual('M', ok[1][0]);
            Assert.AreEqual(1, ok[1].OneBasedPossibleLocalizedModifications[1].Count);
        }

        [Test]
        public void FastaTest()
        {
            Dictionary<string, Modification> un;
            ProteinDbLoader.LoadProteinDb(Path.Combine(TestContext.CurrentContext.TestDirectory, @"fasta.fasta"), true, new List<Modification>(), false, out un);
        }

        [Test]
        public void bad_fasta_header_test()
        {
            Dictionary<string, Modification> un;
            ProteinDbLoader.LoadProteinDb(Path.Combine(TestContext.CurrentContext.TestDirectory, @"bad.fasta"), true, new List<Modification>(), false, out un);
        }

        #endregion Public Methods

    }
}