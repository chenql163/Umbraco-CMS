﻿using System;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Umbraco.Core;
using Umbraco.Core.Configuration;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;
using Umbraco.Tests.TestHelpers;
using GlobalSettings = Umbraco.Core.Configuration.GlobalSettings;

namespace Umbraco.Tests.Migrations.Upgrades
{
    [TestFixture]
    public abstract class BaseUpgradeTest
    {
        /// <summary>Regular expression that finds multiline block comments.</summary>
        private static readonly Regex m_findComments = new Regex(@"\/\*.*?\*\/", RegexOptions.Singleline | RegexOptions.Compiled);

        [SetUp]
        public virtual void Initialize()
        {
            TestHelper.SetupLog4NetForTests();
            TestHelper.InitializeContentDirectories();

            Path = TestHelper.CurrentAssemblyDirectory;
            AppDomain.CurrentDomain.SetData("DataDirectory", Path);

            UmbracoSettings.UseLegacyXmlSchema = false;

            //this ensures its reset
            PluginManager.Current = new PluginManager(false);

            //for testing, we'll specify which assemblies are scanned for the PluginTypeResolver
            PluginManager.Current.AssembliesToScan = new[]
                                                         {
                                                             typeof (MigrationRunner).Assembly
                                                         };

            DatabaseSpecificSetUp();

            SyntaxConfig.SqlSyntaxProvider = GetSyntaxProvider();
        }

        [Test]
        public void Can_Upgrade_From_470_To_600()
        {
            var configuredVersion = new Version("4.7.0");
            var targetVersion = new Version("6.0.0");
            var provider = GetDatabaseProvider();
            var db = GetConfiguredDatabase();

            //Create db schema and data from old Total.sql file for Sql Ce
            string statements = GetDatabaseSpecificSqlScript();
            // replace block comments by whitespace
            statements = m_findComments.Replace(statements, " ");
            // execute all non-empty statements
            foreach (string statement in statements.Split(";".ToCharArray()))
            {
                string rawStatement = statement.Trim();
                if (rawStatement.Length > 0)
                    db.Execute(rawStatement);
            }

            //Setup the MigrationRunner
            var migrationRunner = new MigrationRunner(configuredVersion, targetVersion, GlobalSettings.UmbracoMigrationName);
            bool upgraded = migrationRunner.Execute(db, provider, true);

            Assert.That(upgraded, Is.True);

            bool hasTabTable = db.TableExist("cmsTab");
            bool hasPropertyTypeGroupTable = db.TableExist("cmsPropertyTypeGroup");
            bool hasAppTreeTable = db.TableExist("umbracoAppTree");

            Assert.That(hasTabTable, Is.False);
            Assert.That(hasPropertyTypeGroupTable, Is.True);
            Assert.That(hasAppTreeTable, Is.False);
        }

        [TearDown]
        public virtual void TearDown()
        {
            PluginManager.Current = null;
            SyntaxConfig.SqlSyntaxProvider = null;

            TestHelper.CleanContentDirectories();

            Path = TestHelper.CurrentAssemblyDirectory;
            AppDomain.CurrentDomain.SetData("DataDirectory", null);

            DatabaseSpecificTearDown();
        }

        public string Path { get; set; }
        public abstract void DatabaseSpecificSetUp();
        public abstract void DatabaseSpecificTearDown();
        public abstract ISqlSyntaxProvider GetSyntaxProvider();
        public abstract UmbracoDatabase GetConfiguredDatabase();
        public abstract DatabaseProviders GetDatabaseProvider();
        public abstract string GetDatabaseSpecificSqlScript();
    }
}