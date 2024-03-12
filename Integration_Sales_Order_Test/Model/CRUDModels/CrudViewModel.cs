namespace Integration_Sales_Order_Test.Model.CRUDModels
{
    public class CrudViewModel<T> where T : class
    {
        public string actions { get; set; }

        public object key { get; set; }

        public string  anitForgery {  get; set; }   

        public T value { get; set; }

    }
}
