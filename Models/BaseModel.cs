namespace DcaLag.AspNet{
    public class BaseModel{
        public string Mensaje { get; set; }

        public string Error { get; set; }

        public BaseModel(){
            Mensaje = "";
            Error = "";
        }
    }
}