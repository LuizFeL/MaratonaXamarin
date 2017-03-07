using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PCLStorage;
using PurchaseOrderManager.Model;
using PurchaseOrderManager.Services;
using SQLite;
using Xamarin.Forms;
/*
namespace PurchaseOrderManager.Storage
{
    public class DatabaseManager
    {
        private string _dbName = "dbPo.db";
        private string _dbPath = "";
        public DatabaseManager()
        {
            TreatDbPath();
            database = new SQLiteConnection(_dbPath);
            CreateTables();
        }

        private void CreateTables()
        {
            if (database.TableMappings.All(x => x.MappedType != typeof(PurchaseOrder)))
                database.CreateTable<PurchaseOrder>();

            if (database.TableMappings.All(x => x.MappedType != typeof(PurchaseOrderItem)))
                database.CreateTable<PurchaseOrderItem>();

            if (database.TableMappings.All(x => x.MappedType != typeof(Login)))
                database.CreateTable<Login>();
#if DEBUG
            Save(new Login { Key = "LuizFel", Password = LoginService.Encripty("123") });
#endif
        }

        private SQLiteConnection database;

        private void TreatDbPath()
        {
            _dbPath = Path.Combine(FileSystem.Current.LocalStorage.Path, _dbName);
            if (Device.OS != TargetPlatform.Android) return;
            _dbPath = _dbPath.Replace("\\", "/");
        }

        public void Save<T>(T value) where T : IKeyObject, new()
        {
            var itemDb = database.Table<T>().AsEnumerable().FirstOrDefault(x => x.Key == value.Key);
            if (itemDb == null) database.Insert(value);
            else database.Update(value);
        }

        public void Delete<T>(T value) where T : IKeyObject, new()
        {
            var itemDb = database.Table<T>().AsEnumerable().SingleOrDefault(x => x.Key == value.Key);
            if (itemDb == null) return;
            database.Delete(value);
        }

        public List<T> GetAll<T>() where T : IKeyObject, new()
        {
            return database.Table<T>().AsEnumerable().ToList();
        }

        public List<T> Retrieve<T>(Func<T, bool> where) where T : IKeyObject, new()
        {
            return database.Table<T>().AsEnumerable().Where(where).ToList();
        }

        public T Get<T>(string key) where T : IKeyObject, new()
        {
            return database.Table<T>().AsEnumerable().FirstOrDefault(x => x.Key == key);
        }
    }
}
*/