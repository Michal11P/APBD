﻿using Tutorial9.Model;

namespace Tutorial9.Services;

public interface IWarehouseService
{
    Task<int> AddProductToWarehouseAsync(WarehouseRequestDTO request);
    Task<int>AddProductToWarehouseProcedure(WarehouseRequestDTO request);
}