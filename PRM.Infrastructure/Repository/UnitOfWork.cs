﻿using Microsoft.EntityFrameworkCore;
using PRM.Infrastructure;
using System;
using System.Collections;
using System.Threading.Tasks;

public class UnitOfWork : IUnitOfWork
{
    private readonly PRMDbContext _context;
    private Hashtable _repositories;

    public UnitOfWork(PRMDbContext context)
    {
        _context = context;
        _repositories = new Hashtable();
    }

    public IGenericRepository<T> Repository<T>() where T : class
    {
        var type = typeof(T).Name;

        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(GenericRepository<>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);
            _repositories.Add(type, repositoryInstance);
        }

        return (IGenericRepository<T>)_repositories[type];
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
