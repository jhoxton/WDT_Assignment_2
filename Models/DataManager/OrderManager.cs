//using System.Collections.Generic;
//using System.Linq;
//using WDT_Assignment_2.Data;
//using WDT_Assignment_2.Models.Repository;

//namespace WDT_Assignment_2.Models.DataManager
//{
//    public class OrderManager : IDataRepository<Order, int>
//    {
//        private readonly Context _context;

//        public OrderManager(Context context)
//        {
//            _context = context;
//        }

//        public Order Get(int id)
//        {
//            return _context.Order.Find(id);
//        }

//        public IEnumerable<Order> GetAll()
//        {
//            return _context.Order.ToList();
//        }

//        public int Add(Order order)
//        {
//            _context.Order.Add(order);
//            _context.SaveChanges();

//            return order.ID;
//        }

//        public int Delete(int id)
//        {
//            _context.Order.Remove(_context.Order.Find(id));
//            _context.SaveChanges();

//            return id;
//        }

//        public int Update(int id, Order order)
//        {
//            _context.Update(order);
//            _context.SaveChanges();

//            return id;
//        }
//    }
//}
