﻿using AdeDl.BlazorApp.Models.Database;
using AdeDl.BlazorApp.Models.Operations;
using AdeDl.BlazorApp.Services;

namespace AdeDl.BlazorApp.Strategies.DownloadSrategy;

public class F24DownloadStrategy : IDownloadStrategy
{
    private readonly IF24Service _f24Service;

    public F24DownloadStrategy(IF24Service f24Service)
    {
        _f24Service = f24Service;
    }
    
    public bool CanHandle(BaseOperation operation) => operation is F24;

    public async Task DownloadAsync(Customer customer, BaseOperation operation, CancellationToken cancellationToken)
    {
        await _f24Service.DownloadF24Async(customer, (F24)operation, cancellationToken);
    }
}