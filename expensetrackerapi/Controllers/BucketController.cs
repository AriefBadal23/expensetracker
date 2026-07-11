using expensetrackerapi.Contracts;
using expensetrackerapi.DTO;
using expensetrackerapi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace expensetrackerapi.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class BucketsController : ControllerBase
{
    private readonly IBucketService _bucketService;
    private readonly UserManager<ApplicationUser> _manager;

    public BucketsController(IBucketService bucketService, UserManager<ApplicationUser> userManager)
    {
        // constructor DI 
        _bucketService = bucketService;
        _manager = userManager;
    }

    [HttpPost]
    public async Task<ActionResult> CreateBucket([FromBody] BucketRequestDto bucket)
    {
        var userId = _manager.GetUserId(User);
        var newBucketCreated = await _bucketService.CreateBucket(userId, bucket);

        if (newBucketCreated.IsSuccess)
        {
            return Ok(newBucketCreated);
        }
        return BadRequest("Failed to create bucket.");



    }


    [HttpGet("summary")]
    public async Task<ActionResult> GetBucketSummary([FromQuery] int month, [FromQuery] int year)
    {
        var userId = _manager.GetUserId(User);
        if (userId is null) return BadRequest("Invalid userId provided.");

        var transactions = await _bucketService.GetSummary(userId, month, year);
        if (transactions.IsSuccess)
        {

            return Ok(transactions);
        }
        return BadRequest("No transactions found.");
    }


    [HttpGet("user")]
    public async Task<ActionResult> GetBucketsByUserId()
    {
        var id = _manager.GetUserId(User);
        var buckets = await _bucketService.GetBucketsByUserId(id);
        if (buckets.IsSuccess)
        {
            return Ok(buckets);
        }
        return BadRequest("No buckets found.");
    }

    [HttpDelete("{bucketId:int}")]
    public async Task<ActionResult> DeleteBucket(int bucketId)
    {
        var id = _manager.GetUserId(User);
        var bucketIsDeleted = await _bucketService.DeleteBucket(id,bucketId);
        if (bucketIsDeleted.IsSuccess)
        {
            return Ok($"Bucket with id {bucketId} is deleted.");
        }

        return BadRequest("Failed to delete bucket.");
    }

    [HttpPut("{bucketId:int}")]
    public async Task<ActionResult> UpdateBucket(int bucketId, [FromBody] BucketRequestDto bucket)
    {
        var userId = _manager.GetUserId(User);
        var updateBucket = await _bucketService.UpdateBucket(bucketId,userId, bucket);
        if (updateBucket.IsSuccess)
        {
            return Ok(updateBucket);
        }

        return BadRequest("Failed to update bucket");

    }
    
}