namespace PoolGuy.Mobile.Models
{
    public class ActionSheetModel
    {
        public string Title { get; set; }
        public string Cancel { get; set; }
        public string[] Buttons { get; set; }
        public eContentType ContentType { get; set; }
    }

    public enum eContentType
    { 
      Buttons,
      ImageUrl
    }
}