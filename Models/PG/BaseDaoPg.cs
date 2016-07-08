using Microsoft.Extensions.Configuration;

namespace DcaLag.AspNet.Models.PG{
    public class BaseDaoPg{
        public string ConnectionString { get; set; }

        public BaseDaoPg(){
        }
    }
}