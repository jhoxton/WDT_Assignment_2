//using System.Collections.Generic;
//using Microsoft.AspNetCore.Mvc;
//using WDT_Assignment_2.Models;
//using WDT_Assignment_2.Models.DataManager;

//namespace WDT_Assignment_2.Controllers
//{
//    [Route("api/[controller]")]
//    public class OrderController : Controller
//    {
//        private readonly OrderManager _repo;

//        public OrderController(OrderManager repo)
//        {
//            _repo = repo;
//        }

//        // GET: api/movies
//        [HttpGet]
//        public IEnumerable<Order> Get()
//        {
//            return _repo.GetAll();
//        }

//        // GET api/movies/1
//        [HttpGet("{id}")]
//        public Order Get(int id)
//        {
//            return _repo.Get(id);
//        }

//        // POST api/movies
//        [HttpPost]
//        public void Post([FromBody] Order order)
//        {
//            _repo.Add(order);
//        }

//        // PUT api/movies
//        [HttpPut]
//        public void Put([FromBody] Order order)
//        {
//            _repo.Update(order.ID, order);
//        }

//        // DELETE api/movies/1
//        [HttpDelete("{id}")]
//        public long Delete(int id)
//        {
//            return _repo.Delete(id);
//        }
//    }
//}
