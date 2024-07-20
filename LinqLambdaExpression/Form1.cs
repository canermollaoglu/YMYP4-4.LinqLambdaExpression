using LinqLambdaExpression.Contexts;
using LinqLambdaExpression.Entities;

namespace LinqLambdaExpression
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        NorthwndContext db = new NorthwndContext();
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        List<int> numbers = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        private void button1_Click(object sender, EventArgs e)
        {
            //Çift sayýlarý bul, Linq
            var ciftSayilar = from num in numbers
                              where num % 2 == 0
                              select num;

            foreach (var item in ciftSayilar)
            {
                lstListe.Items.Add(item);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var ciftSayilar = numbers.Where(x => x % 2 == 0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Tüm ürünleri listeleyiniz.
            var urunListesi = db.Products.ToList();

            urunListesi.ForEach(x => lstListe.Items.Add(x.ProductName));

            var productNames = from p in db.Products
                               select p.ProductName;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Ürün Adý, Birim Fiyatý, Kategori Adý 

            var productInfo = from p in db.Products
                              select new { p.ProductName, p.Category.CategoryName, p.UnitPrice };

            lstListe.DataSource = productInfo.ToList();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var orders = from o in db.Orders
                         where o.OrderDetails.Sum(x => x.Quantity) > 20
                         select new { o.OrderId, o.Customer.CompanyName };
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //1998 yýlýndan sonra verilen tüm sipariþler

            //CustomerID, OrderDate, OrderID

            //Cross Join
            //var list = from c in db.Customers
            //           from o in db.Orders
            //           where o.OrderDate >= new DateTime(1998, 1, 1)
            //           select new { c.CustomerId, o.OrderDate, o.OrderId };

            var list1 = from o in db.Orders
                        where o.OrderDate >= new DateTime(1998, 1, 1)
                        select new { o.CustomerId, o.Customer.CompanyName, o.OrderDate, o.OrderId };

            lstListe.DataSource = list1.ToList();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            //Washington da oturan müþterilerin 1997 yýlýndan sonra yapmýþ olduklarý sipariþler hangileridir.
            //Region="WA"

            var orders = from c in db.Customers
                         where c.Region == "WA"
                         join o in db.Orders on c.CustomerId equals o.CustomerId
                         where o.OrderDate >= new DateTime(1997, 1, 1)
                         select new { MusteriID = c.CustomerId, SiparisID = o.OrderId };

            lstListe.DataSource = orders.ToList();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //Stok miktarý 0 dan büyük olan ve ayný zamanda birim fiyatý 15 dolardan büyük olan ürünleri listeleyiniz.

            var productList = from p in db.Products
                              where p.UnitsInStock > 0 && p.UnitPrice > 15
                              select new
                              {
                                  UrunAdi = p.ProductName,
                                  BirimFiyati = p.UnitPrice
                              };
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //Hangi kategoriden kaç adet ürünüm vardýr.

            var data = from p in db.Products
                       group p by p.Category.CategoryName into g
                       select new
                       {
                           KategoriAdi=g.Key,
                           UrunSayisi=g.Count()
                       };

            lstListe.DataSource = data.ToList();
        }
    }
}
