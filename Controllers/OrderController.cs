using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantFavoritesAPI.Models;

namespace RestaurantFavoritesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private RestaurantFavesDbContext _dbContext = new RestaurantFavesDbContext();

        [HttpGet()]
        public IActionResult GetAllOrders(string? restaurant = null, bool? fave = false)
        {

            List<Order> result = _dbContext.Orders.ToList();
            if (restaurant != null && fave == true)
            {
                result = result
                    .Where(o => o.Restaurant == restaurant && o.Favorite == fave).ToList();
            }
            else if (restaurant == null && fave == true) {
                result = result
                    .Where(o => o.Favorite == fave).ToList();
            }
            else if (restaurant != null && fave == false)
            {
                result = result
                    .Where(o => o.Restaurant == restaurant).ToList();
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetOrderById(int id)
        {
            Order result = _dbContext.Orders.FirstOrDefault(o => o.Id == id);

            if (result == null)
            {
                return NotFound("Order not found");
            }
            return Ok(result);
        }

        [HttpPost()]
        public IActionResult AddOrder([FromBody] Order newOrder)
        {
            _dbContext.Orders.Add(newOrder);
            _dbContext.SaveChanges();
            return Created($"/api/Order/{newOrder.Id}", newOrder);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateOrder([FromBody] Order changeOrder, int id)
        {
            if (id != changeOrder.Id) { return BadRequest("Please enter valid id"); }
            if (!_dbContext.Orders.Any(o => o.Id == id)) { return NotFound("Order not found"); }
            _dbContext.Orders.Update(changeOrder);
            _dbContext.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult RemoveOrder(int id)
        {
            Order order = _dbContext.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }
            _dbContext.Orders.Remove(order);
            _dbContext.SaveChanges();
            return Ok("Order deleted successfully");
        }

    }
}
