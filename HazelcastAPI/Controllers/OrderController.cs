﻿using Cache.Services;
using Microsoft.AspNetCore.Mvc;

namespace HazelcastAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Route("/load/customer")]
        public async Task<IActionResult> LoadCustomer(CancellationToken token = default)
        {
            try
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                var customerList = await _orderService.LoadCustomerAsync(token).ConfigureAwait(false);
                watch.Stop();
                return Ok($"{customerList.Count()} Records Load Time: {watch.ElapsedMilliseconds} milliseconds, {TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds).TotalSeconds} seconds and {TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds).TotalMinutes} minutes");
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        [Route("/load/order")]
        public async Task<IActionResult> LoadOrder(CancellationToken token = default)
        {
            try
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                var customerList = await _orderService.LoadOrderAsync(token).ConfigureAwait(false);
                watch.Stop();
                return Ok($"{customerList.Count()} Records Load Time: {watch.ElapsedMilliseconds} milliseconds, {TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds).TotalSeconds} seconds and {TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds).TotalMinutes} minutes");
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        [Route("/load/order-with-customer")]
        public async Task<IActionResult> LoadOrderWithCustomer(CancellationToken token = default)
        {
            try
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                var customerList = await _orderService.LoadOrderWithCustomerAsync(token).ConfigureAwait(false);
                watch.Stop();
                return Ok($"{customerList.Count()} Records Load Time: {watch.ElapsedMilliseconds} milliseconds, {TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds).TotalSeconds} seconds and {TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds).TotalMinutes} minutes");
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        [Route("/seed-data")]
        public async Task<IActionResult> SeedData(CancellationToken token = default)
        {
            try
            {
                //5 million
                //await _orderService.SeedDataAsync(0, 5000000, token).ConfigureAwait(false);
                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
