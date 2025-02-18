﻿using Module = RegistaMaster.Domain.Entities.Module;

namespace RegistaMaster.Application.Repositories;

public interface IModuleRepository : IRepository
{
    public Task<IQueryable<Module>> GetModule();
    public Task<string> CreateModule(Module model);
    public Task<string> UpdateModule(Module model);
    public string DeleteModule(int ID);
}
