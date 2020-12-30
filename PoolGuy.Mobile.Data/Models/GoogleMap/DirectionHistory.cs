using Newtonsoft.Json;
using System.Diagnostics;

namespace PoolGuy.Mobile.Data.Models.GoogleMap
{
    public class DirectionHistory :  EntityBase
    {
        public string Json { get; set; }
        public Direction Direction
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(Json))
                    {
                        return null;
                    }

                    return JsonConvert.DeserializeObject<Direction>(Json);

                }
                catch (System.Exception e)
                {
                    Debug.WriteLine(e);
                    return null;
                }
            }
        }
    }
}
