using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        public ProductController(IMemoryCache memoryCache)
        {
           _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            // 1. yol  zaman value'lu cache değeri yoksa set ediliyor.
            if(String.IsNullOrEmpty(_memoryCache.Get<string>("zaman")))
            {
                _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
            }

            // 2. yol  zaman value'lu değerine göre bool tipi dönüyor ve zamancache isimli değişkene değer varsa atanıyor.
            if (!_memoryCache.TryGetValue("zaman", out string zamancache))
            {
                _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
            }



            return View();
        }

        public IActionResult Show()
        {
            _memoryCache.Remove("zaman"); // memoryden data silinir.

            //İlgili değer varsa getirir yoksa oluşturur.
            //function almasının bir sebebide key attributeleri ekleyebiliriz.
            _memoryCache.GetOrCreate<string>("zaman", entry =>
            {
                return DateTime.Now.ToString();
            });

            ViewBag.zaman = _memoryCache.Get<string>("zaman");
            return View();
        }
    }
}
