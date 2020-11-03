
namespace PoolGuy.Mobile.Data.Models
{
    public class EquipmentTypeModel : EntityBase
    {
        private string _name;
        public string Name { get { return _name; } set { _name = value; OnPropertyChanged("Name"); } }
        private string _description;
        public string Description { get { return _description; } set { _description = value; OnPropertyChanged("Description"); } }
        private string _imageUrl;
        public string ImageUrl { get { return _imageUrl; } set { _imageUrl = value; OnPropertyChanged("ImageUrl"); } }

        private bool _selected;
        public bool Selected 
        {
            get 
            { 
                return  _selected;
            }
            set { _selected = value; OnPropertyChanged("Selected"); } 
        }

        public void NotifyAll()
        {
            OnPropertyChanged("Name");
            OnPropertyChanged("Description");
            OnPropertyChanged("ImageUrl");
            OnPropertyChanged("Selected");
        }
    }
}