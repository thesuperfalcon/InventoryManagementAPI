﻿using InventoryManagementAPI.Data;
using InventoryManagementAPI.Models;
using System.Data;

namespace InventoryManagementAPI.DAL
{
    public class StorageManager
    {
        private readonly InventoryManagementAPIContext _context;

        public StorageManager(InventoryManagementAPIContext context)
        {
            _context = context;
        }

        public async Task UpdateStoragesAsync(int id, Storage storage)
        {
            var existingStorage = await _context.Storages.FindAsync(id);

            if (existingStorage != null)
            {
                existingStorage.Name = storage.Name;
                existingStorage.MaxCapacity = storage.MaxCapacity;             
                existingStorage.CurrentStock = storage.CurrentStock;
                existingStorage.Created = storage.Created;
                existingStorage.Updated = DateTime.Now;
                existingStorage.IsDeleted = storage.IsDeleted;
                existingStorage.InventoryTrackers = storage.InventoryTrackers;

                _context.Storages.Update(existingStorage);
                await _context.SaveChangesAsync();
            }
        }
    }
}
