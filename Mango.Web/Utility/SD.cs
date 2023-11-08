namespace Mango.Web.Utility
{
    public class SD
    {
        public static string CouponAPIBase {  get; set; }
        public enum ApiType 
        {
            GET, 
            POST, 
            PUT, 
            DELETE
        }

		public enum ContentType
		{
			Json,
			MultipartFormData,
		}
	}
}
